using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Test
{
    //Overseer class to manage the game, load and switch levels and handle generalized overhead
    public class GameOverseer
    {

        Block[,] blocks;
        Player player;
        Camera camera;
        MapPortal map;
        //ParticleGenerator particle_generator;

        KeyboardState keyboard;
        
        public Texture2D level;
        int current_level = 0, level_width, level_height;

        //Enemy objects
        List<Enemy> enemies = new List<Enemy>();
        Random random = new Random();
        float spawn = 0;

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

            player = new Player(new Vector2(100, 100));
            //particle_generator = new ParticleGenerator(screen_width, 100f, Particle.ParticleType.RAIN);
            this.current_level = test_level;
            if (test_level == 0)
            {
                //Debug mode
            }

            map = new MapPortal(Vector2.Zero);
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

        public void update(GameTime gameTime, GraphicsDevice graphics)
        {
            player_level_collision();
            player.update(gameTime);
            camera_updates();
            spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(graphics);
            }
            LoadEnemies();
            //particle_generator.update(gameTime, graphics);

            keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.R))
            {
                apply_screen_shake();
            }

            map.update(gameTime, player.get_base_position());
        }

        private void camera_updates()
        {
            //Add rotation limit scale for the camera
            if (player.get_rotation() <= 10 && player.get_rotation() >= -10)
            {
                camera.Rotation = (float)((player.get_rotation() * Math.PI) / 180f);
            }
            camera.Update(player.position);
        }

        private void apply_screen_shake()
        {
            float radius = 30f;
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
            int randY = random.Next(100, 1000);
            if (spawn >= 1)
            {
                spawn = 0;
                if (enemies.Count < 4)
                {

                    enemies.Add(new Enemy(Constant.enemy_tex, new Vector2(1500, randY)));

                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].isVisible)
                {
                    enemies.RemoveAt(i);
                    i--;
                }

            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //Begin spritebatch
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);
            
            //Increase the x
            for (int x = 0; x < blocks.GetLength(0); x++)
            {
                //Decrease the y
                for (int y = blocks.GetLength(1); y >= 0; y--)
                {
                    //Do not draw outside the world
                    if (x >= 0 && y >= 0 && x < level_width && y < level_height)
                    {
                        //Draw the blocks
                        blocks[x, y].draw(spriteBatch);
                    }
                }
            }

            spawnBackground(spriteBatch);

            player.draw(spriteBatch);
            //particle_generator.draw(spriteBatch);

            foreach (Enemy enemy in enemies)
                enemy.Draw(spriteBatch);

            map.draw(spriteBatch);

            //End spriteBatch
            spriteBatch.End();
        }

        public void spawnBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Constant.background, new Rectangle(-500, 0, 1000, 800), Color.White);
            spriteBatch.Draw(Constant.background, new Rectangle(500, 0, 1500, 800), Color.White);
            spriteBatch.Draw(Constant.background, new Rectangle(-500, -500, 1000, 800), Color.White);
            spriteBatch.Draw(Constant.background, new Rectangle(500, -500, 1500, 800), Color.White);

        }
    }
}
