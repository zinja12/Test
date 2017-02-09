﻿using System;
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
        //ParticleGenerator particle_generator;

        public Texture2D level;
        int current_level = 0, level_width, level_height;

        //Enemy objects
        /*List<Enemy> enemies = new List<Enemy>();
        Random random = new Random();
        float spawn = 0;*/

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
            apply_gravity();
            player_level_collision();
            player.update(gameTime);
            camera_updates();
            //particle_generator.update(gameTime, graphics);
            //enemy_updates(gameTime, graphics);
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

        private void player_level_collision()
        {
            //Calculate tile positions of the points
            int left_pt_tile_x = (int)(player.left_side_pt.X / Constant.tile_size);
            int left_pt_tile_y = (int)(player.left_side_pt.Y / Constant.tile_size);
            int top_pt_tile_x = (int)(player.top_side_pt.X / Constant.tile_size);
            int top_pt_tile_y = (int)(player.top_side_pt.Y / Constant.tile_size);
            int right_pt_tile_x = (int)(player.right_side_pt.X / Constant.tile_size);
            int right_pt_tile_y = (int)(player.right_side_pt.Y / Constant.tile_size);

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

        }

        private void apply_gravity()
        {
            int tile_x = (int)(player.get_base_position().X / Constant.tile_size);
            int tile_y = (int)(player.get_base_position().Y / Constant.tile_size);

            if (blocks[tile_x, tile_y].collision_rect.Contains((int)player.get_base_position().X, (int)player.get_base_position().Y) && blocks[tile_x, tile_y].id == Constant.test_block)
            {
                player.velocity.Y = -0.1f;
                float i = player.velocity.X;
                //Handle friction on the ground
                player.velocity.X = i -= player.friction * i;
                player.grounded = true;
            }
            else
            {
                float i = 1;
                player.velocity.Y += 0.10f * i;
                player.grounded = false;
            }
        }

        /*private void enemy_updates(GameTime gameTime, GraphicsDevice graphics_device)
        {
            foreach(Enemy enemy in enemies)
                enemy.Update(graphics_device, gameTime);

            LoadEnemies();
        }*/

        /*public void LoadEnemies()
        {

            int randY = random.Next(100, 400);
            // enemy spawns every second
            if (spawn >= 1)
            {
                spawn = 0;
                // number of enemies
                if (enemies.Count < 1)
                {
                    enemies.Add(new Enemy(Constant.enemy_tex, new Vector2(600, randY), Constant.bullet_tex));
                    Console.WriteLine("Created enemy at:" + enemies[enemies.Count-1].get_position());
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
        }*/

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

            player.draw(spriteBatch);
            //particle_generator.draw(spriteBatch);

            //Draw enemies
            /*foreach (Enemy enemy in enemies)
                enemy.Draw(spriteBatch);*/

            //End spriteBatch
            spriteBatch.End();
        }
    }
}
