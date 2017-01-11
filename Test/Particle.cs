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
        //Particle types
        public enum ParticleType
        {
            RAIN = 0,
            SNOW = 1,
            LEAVES = 3,
        }

        //Variables
        Vector2 position;
        Vector2 velocity;
        ParticleType particle_type;

        //Constructor
        public Particle(Vector2 position, Vector2 velocity, ParticleType particle_type)
        {
            this.position = position;
            this.velocity = velocity;
            this.particle_type = particle_type;
        }

        //Getter
        public Vector2 Position
        {
            get { return position; }
        }

        //Update where we just move the particle along with its velocity
        public void update(GameTime gameTime)
        {
            position += velocity;
        }

        //Draw the right type of particle
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
