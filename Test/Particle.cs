using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class Particle
    {
        public enum ParticleType
        {
            RAIN = 0,
            SNOW = 1,
            LEAVES = 3,
        }

        Vector2 position;
        Vector2 velocity;
        ParticleType particle_type;

        public Particle(Vector2 position, Vector2 velocity, ParticleType particle_type)
        {
            this.position = position;
            this.velocity = velocity;
            this.particle_type = particle_type;
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public void update(GameTime gameTime)
        {
            position += velocity;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            switch (particle_type)
            {
                case ParticleType.RAIN:
                    spriteBatch.Draw(Constant.particle, position, Color.Blue);
                    break;
                case ParticleType.SNOW:
                    spriteBatch.Draw(Constant.particle, position, Color.White);
                    break;
                case ParticleType.LEAVES:
                    spriteBatch.Draw(Constant.particle, position, Color.LightGreen);
                    break;
            }
        }
    }
}
