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
        MouseState mouse;
        Vector2 mouse_position;
        Rectangle mouse_collision_rect;

        Block[,] blocks;
        Player player;
        //ParticleGenerator particle_generator;

        public Texture2D level;
        int current_level = 0, level_width, level_height;

        public GameOverseer(int test_level, int screen_width, int screen_height, ContentManager content)
        {
            mouse_collision_rect = new Rectangle(0, 0, 5, 5);

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
            get_mouse_input();
            apply_gravity(gameTime);
            player.update(gameTime);
            //particle_generator.update(gameTime, graphics);
        }

        public void apply_gravity(GameTime gameTime)
        {
            int tile_x = (int)(player.get_base_position().X / Constant.tile_size);
            int tile_y = (int)(player.get_base_position().Y / Constant.tile_size);

            if (blocks[tile_x, tile_y].collision_rect.Contains((int)player.get_base_position().X, (int)player.get_base_position().Y) && blocks[tile_x, tile_y].id == Constant.test_block)
            {
                player.has_jumped = false;
                player.velocity.Y = 0f;
            }
            else
            {
                float i = 1;
                player.velocity.Y += 0.15f * i;
            }
        }

        private void get_mouse_input()
        {
            mouse = Mouse.GetState();
            mouse_position = new Vector2(mouse.X, mouse.Y);
            mouse_collision_rect = new Rectangle((int)mouse_position.X, (int)mouse_position.Y, 5, 5);
        }

        public void draw(SpriteBatch spriteBatch)
        {
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
            spriteBatch.Draw(Constant.particle, mouse_position, Color.Red);
            //particle_generator.draw(spriteBatch);
        }
    }
}
