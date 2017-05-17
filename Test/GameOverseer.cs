using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Test.Content;

namespace Test
{
    //Overseer class to manage the game, load and switch levels and handle generalized overhead
    public class GameOverseer
    {

        Block[,] blocks;
        Player player;
        Asteroid asteroid;
        Camera camera;
        MapPortal map;
        Starfield starfield;
        List<SolarSystem> solar_systems;
        MotherShip mother;
        public static ParticleManager particle_manager;
        Rogue rogue;
        private int score;
        private int counter = 0;
        private int counter1 = 0;

        public bool lose = false;

        KeyboardState keyboard;

        public Texture2D level;
        public Texture2D healthTexture;
        int current_level = 0, level_width, level_height;

        //Enemy objects
        List<Enemy> enemies = new List<Enemy>();
        Random random = new Random();
        float spawn = 0;

        Vector2 past_player_position;

        //Constructor
        public GameOverseer(int test_level, int screen_width, int screen_height, ContentManager content, Viewport viewport)
        {
            //Set up camera
            camera = new Camera(viewport);

            //Load a level
            level = content.Load<Texture2D>("Levels/lvl" + current_level + ".png");
            level_width = level.Width;
            level_height = level.Height;
            blocks = new Block[level_width, level_height];
            generate_level();

            player = new Player(new Vector2(350, 100));
            particle_manager = new ParticleManager();
            this.current_level = test_level;
            if (test_level == 0)
            {
                //Debug mode
            }

            map = new MapPortal(Vector2.Zero);
            starfield = new Starfield(1000, 800);
            asteroid = new Asteroid(Constant.asteroid, player);
            past_player_position = Vector2.Zero;
            
            solar_systems = new List<SolarSystem>();
            generate_planetary_systems();

            rogue = new Rogue(new Vector2(0, 0), player);
            mother = new MotherShip(Constant.enemy_tex, new Vector2(1500, -500), Constant.particle, player);
            score = 0;
        }

        public void generate_level()
        {
            //Define a color array
            Color[,] colors2d = new Color[level_width, level_height];
            //Define and set a 1d color array
            Color[] colors1d = new Color[level_width * level_height];
            //get colors out of texture
            level.GetData(colors1d);

            //Convert the 1D array into a 2D array
            for (int x = 0; x < level.Width; x++)
            {
                for (int y = 0; y < level.Height; y++)
                {
                    colors2d[x, y] = colors1d[x + y * level.Width];
                }
            }

            //Map the blocks to the color of the pixels
            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                for (int y = 0; y < blocks.GetLength(1); y++)
                {
                    //Black
                    if (colors2d[x, y].R == 0 && colors2d[x, y].G == 0 && colors2d[x, y].B == 0)
                    {
                        blocks[x, y] = new Block(new Vector2(x * 32, y * 32), Constant.test_block);
                    }
                    //True pink
                    if (colors2d[x, y].R == 255 && colors2d[x, y].G == 0 && colors2d[x, y].B == 255)
                    {
                        blocks[x, y] = new Block(new Vector2(x * 32, y * 32), Constant.other_test_block);
                    }
                }
            }
        }

        private void generate_planetary_systems()
        {
            SolarSystem tmp_ss = null;

            char[] delimiters = { ',' };
            string[] lines = File.ReadAllLines("Content/Levels/test_planet_file.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] create_params = lines[i].Split(delimiters);
                if (create_params.Length == 3)
                {
                    if (create_params[0].Equals("SolarSystem"))
                    {
                        //Creation of solar system
                        Console.WriteLine("SolarSystem");
                        tmp_ss = new SolarSystem(new Vector2(Int32.Parse(create_params[1]), Int32.Parse(create_params[2])));
                        solar_systems.Add(tmp_ss);
                    }
                }
                else
                {
                    //Set color
                    Color col;
                    if (create_params[3].Equals("Y")) col = Color.Yellow;
                    else if (create_params[3].Equals("R")) col = Color.Red;
                    else if (create_params[3].Equals("G")) col = Color.Green;
                    else if (create_params[3].Equals("B")) col = Color.Blue;
                    else col = Color.White;

                    if (create_params[0].Equals("Sun"))
                    {
                        //Creation of planet
                        Console.WriteLine("Sun");
                        tmp_ss.add_sun(new Planet(new Vector2(Int32.Parse(create_params[1]), Int32.Parse(create_params[2])), tmp_ss.system_center, col, 5f, 0f));
                    }
                    else if (create_params[0].Equals("Planet"))
                    {
                        //Creation of planet
                        Console.WriteLine("Planet");
                        Console.WriteLine("Planet orbit speed: " + string_to_float(create_params[4]));
                        tmp_ss.add_planet(new Planet(new Vector2(Int32.Parse(create_params[1]), Int32.Parse(create_params[2])), tmp_ss.system_center, col, 2f, string_to_float(create_params[4])));
                    }
                }
            }
        }

        //Function to convert string to float
        public static float string_to_float(string s)
        {
            float conversion = 0f;
            bool minus = false;
            int ten_c = 1;
            int ten_c_predec = 0;

            int dec_idx = -1;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Equals('-')) minus = true;

                if (s[i].Equals('.'))
                {
                    dec_idx = i;
                    break;
                }
            }

            int c_to_int;
            //Pre dec
            for (int i = dec_idx - 1; i >= 0; i--)
            {
                if (s[i].Equals('-')) break;

                c_to_int = (int)Char.GetNumericValue(s[i]);
                conversion += (float)(c_to_int * Math.Pow(10, ten_c_predec));
                ten_c_predec++;
            }

            //Post dec
            for (int i = dec_idx + 1; i < s.Length; i++)
            {
                if (s[i].Equals('f'))
                {
                    break;
                }
                else
                {
                    c_to_int = (int)Char.GetNumericValue(s[i]);
                    float c_to_fl = ((float)c_to_int / (float)(Math.Pow(10, ten_c)));
                    ten_c++;
                    conversion += c_to_fl;
                }
            }

            if (minus) return (conversion * -1);
            else return conversion;
        }

        public void update(GameTime gameTime, GraphicsDevice graphics, SpriteBatch spriteBatch)
        {
            //player_level_collision();
            player.update(gameTime, spriteBatch);
            asteroid.update(graphics, gameTime);
            camera_updates();
            spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(graphics, gameTime);
                if (enemy.isEnemyDead() == true)
                {
                    score += 7;
                    enemy.dead = false;
                }
            }
            LoadEnemies();
            particle_manager.update(gameTime);
            mother.Update(graphics, gameTime);

            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.R))
            {
                apply_screen_shake();
            }

            map.update(gameTime, player.get_base_position());

            //If hit apply shake
            if (Constant.shake)
            {
                apply_screen_shake();
                Constant.shake = false;
            }
            
            Vector2 diff = player.position - past_player_position;
            starfield.update(gameTime, diff);
            past_player_position = player.position;
            
            for (int i = 0; i < solar_systems.Count; i++)
            {
                solar_systems[i].update(gameTime);
            }

            rogue.update(gameTime, player.position);
            if (rogue.dead == true)
            {
                addRougeScore();
            }
            
        }

        public void addRougeScore()
        {
            if (counter < 1)
            {
                score += 50;
                counter++;
            }
        }
        public void addMotherShipScore()
        {
            if (counter1 < 1)
            {
                score += 1500;
                counter1++;
            }
        }

        private void camera_updates()
        {
            //Add rotation limit scale for the camera
            /*if (player.get_rotation() <= 10 && player.get_rotation() >= -10)
            {
                camera.Rotation = (float)((player.get_rotation() * Math.PI) / 180f);
            }*/
            camera.Update(player.position);
        }

        private void apply_screen_shake()
        {
            float radius = 400f;
            Random random = new Random();
            float random_angle = random.Next(0, 360);
            Vector2 offset = new Vector2((float)Math.Sin(random_angle) * radius, (float)Math.Cos(random_angle) * radius);

            while (radius >= 2)
            {
                radius *= 0.9f;
                if (random.Next(0, 100) > 50)
                {
                    random_angle += (180 + random.Next(0, 60));
                }
                else
                {
                    random_angle += (180 - random.Next(0, 60));
                }
                offset = new Vector2((float)Math.Sin(random_angle) * radius, (float)Math.Cos(random_angle) * radius);
                camera.Update(player.position + offset);
            }
        }

        private void player_level_collision()
        {
            //Calculate tile positions of the points
            int left_pt_tile_x = (int)(player.left_side_pt.X / Constant.tile_size);
            int left_pt_tile_y = (int)(player.left_side_pt.Y / Constant.tile_size);
            int top_pt_tile_x = (int)(player.top_side_pt.X / Constant.tile_size);
            int top_pt_tile_y = (int)(player.top_side_pt.Y / Constant.tile_size);
            int right_pt_tile_x = (int)(player.right_side_pt.X / Constant.tile_size);
            int right_pt_tile_y = (int)(player.right_side_pt.Y / Constant.tile_size);
            int bottom_pt_tile_x = (int)(player.get_base_position().X / Constant.tile_size);
            int bottom_pt_tile_y = (int)(player.get_base_position().Y / Constant.tile_size);

            //Left blocks check
            if (blocks[left_pt_tile_x, left_pt_tile_y].collision_rect.Contains((int)player.left_side_pt.X, (int)player.left_side_pt.Y) && blocks[left_pt_tile_x, left_pt_tile_y].id == Constant.test_block)
            {
                player.velocity.X = -0.1f;
            }
            //Right blocks check
            if (blocks[right_pt_tile_x, right_pt_tile_y].collision_rect.Contains((int)player.right_side_pt.X, (int)player.right_side_pt.Y) && blocks[right_pt_tile_x, right_pt_tile_y].id == Constant.test_block)
            {
                player.velocity.X = 0.1f;
            }
            //Top blocks check
            if (blocks[top_pt_tile_x, top_pt_tile_y].collision_rect.Contains((int)player.top_side_pt.X, (int)player.top_side_pt.Y) && blocks[top_pt_tile_x, top_pt_tile_y].id == Constant.test_block)
            {
                player.velocity.Y = 0.1f;
            }
            //Bottom blocks check
            //Top blocks check
            if (blocks[bottom_pt_tile_x, bottom_pt_tile_y].collision_rect.Contains((int)player.get_base_position().X, (int)player.get_base_position().Y) && blocks[bottom_pt_tile_x, bottom_pt_tile_y].id == Constant.test_block)
            {
                player.velocity.Y = -0.1f;
            }
        }

        /*private void enemy_updates(GameTime gameTime, GraphicsDevice graphics_device)
        {
            foreach(Enemy enemy in enemies)
                enemy.Update(graphics_device, gameTime);

            LoadEnemies();
        }*/

        public void LoadEnemies()
        {
            int randY = random.Next(100, 200);
            if (spawn >= 1)
            {
                spawn = 0;
                if (enemies.Count < 4)
                {

                    enemies.Add(new Enemy(Constant.enemy_tex, new Vector2(1000, randY), Constant.particle, player));

                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].isVisible || mother.motherShipArrived == true)
                {
                    enemies.RemoveAt(i);
                    i--;
                }

            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            starfield.draw(spriteBatch);
            spriteBatch.End();

            //Begin spritebatch
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);

            //spawnBackground(spriteBatch);
            //backgrounds.draw(spriteBatch);
            //starfield.draw(spriteBatch, camera);

            //Draw planet(s)
            for (int i = 0; i < solar_systems.Count; i++)
            {
                solar_systems[i].draw(spriteBatch);
            }

            player.draw(spriteBatch);
            asteroid.draw(spriteBatch);

            foreach (Enemy enemy in enemies)
                enemy.Draw(spriteBatch);
            mother.Draw(spriteBatch);
            rogue.draw(spriteBatch);

            particle_manager.draw(spriteBatch);

            map.draw(spriteBatch);

            //End spriteBatch
            spriteBatch.End();

            //Draw heads up display
            spriteBatch.Begin();

            spriteBatch.Draw(Constant.health_bar, Vector2.Zero, player.healthSourceRect, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Constant.score_font, "Score: " + score.ToString(), new Vector2(860, 0), Color.White);

            if (player.isDestroyed)
            {
                spriteBatch.DrawString(Constant.score_font, "YOU LOSE!", new Vector2(450, 200), Color.Red);

            } else if (mother.dead)
            {
                addMotherShipScore();
                spriteBatch.DrawString(Constant.score_font, "YOU WIN!", new Vector2(450, 200), Color.Green);
            }
            spriteBatch.End();
        }
    }
}