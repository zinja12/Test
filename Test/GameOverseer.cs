using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test
{
    //Overseer class to manage the game, load and switch levels and handle generalized overhead
    public class GameOverseer
    {
        MouseState mouse;
        Vector2 mouse_position;
        Rectangle mouse_collision_rect;

        Player player;
        ParticleGenerator particle_generator;

        int test_level = 0;

        public GameOverseer(int test_level, int screen_width, int screen_height)
        {
            mouse_collision_rect = new Rectangle(0, 0, 5, 5);

            player = new Player(new Vector2(100, 100));
            particle_generator = new ParticleGenerator(screen_width, 100f, Particle.ParticleType.RAIN);
            this.test_level = test_level;
            if (test_level == 0)
            {
                //Debug mode
            }
        }

        public void update(GameTime gameTime, GraphicsDevice graphics)
        {
            get_mouse_input();
            player.update(gameTime);
            particle_generator.update(gameTime, graphics);
        }

        private void get_mouse_input()
        {
            mouse = Mouse.GetState();
            mouse_position = new Vector2(mouse.X, mouse.Y);
            mouse_collision_rect = new Rectangle((int)mouse_position.X, (int)mouse_position.Y, 5, 5);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            player.draw(spriteBatch);
            spriteBatch.Draw(Constant.particle, mouse_position, Color.Red);
            particle_generator.draw(spriteBatch);
        }
    }
}
