using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class GameOverseer
    {
        Player player;
        ParticleGenerator particle_generator;

        int test_level = 0;

        public GameOverseer(int test_level, int screen_width, int screen_height)
        {
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
            player.update(gameTime);
            particle_generator.update(gameTime, graphics);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            player.draw(spriteBatch);
            particle_generator.draw(spriteBatch);
        }
    }
}
