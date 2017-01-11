using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    //Particle gen
    public class ParticleGenerator
    {
        //Define the width of the area of the particles, density and timing
        float spawn_width, density, timer;
        List<Particle> particles = new List<Particle>();
        Random r1, r2;

        //Type of particle
        Particle.ParticleType particles_type;

        //Constructor
        public ParticleGenerator(float spawn_width, float density, Particle.ParticleType particles_type)
        {
            this.spawn_width = spawn_width;
            this.density = density;
            this.particles_type = particles_type;
            r1 = new Random();
            r2 = new Random();
        }

        public void create_particle()
        {
            //Flush the first random value
            double anything  = r1.Next();
            //Add a randomized particle to the list
            particles.Add(new Particle(new Vector2(-50 + (float)r1.NextDouble() * spawn_width, 0), new Vector2(1, r2.Next(5, 8)), particles_type));
        }

        public void update(GameTime gameTime, GraphicsDevice graphics)
        {
            //Calulate the time
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //If we are still running the game generate particles
            while(timer > 0)
            {
                timer -= 1f / density;
                create_particle();
            }

            //Loop over particles
            for (int i = 0; i < particles.Count; i++)
            {
                //update them
                particles[i].update(gameTime);

                //Once they're off the screen remove them
                if (particles[i].Position.Y > graphics.Viewport.Height)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }

        //draw
        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
            {
                particle.draw(spriteBatch);
            }
        }
    }
}
