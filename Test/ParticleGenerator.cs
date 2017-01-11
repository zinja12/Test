using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class ParticleGenerator
    {
        float spawn_width, density, timer;
        List<Particle> particles = new List<Particle>();
        Random r1, r2;

        Particle.ParticleType particles_type;

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
            double anything  = r1.Next();
            particles.Add(new Particle(new Vector2(-50 + (float)r1.NextDouble() * spawn_width, 0), new Vector2(1, r2.Next(5, 8)), particles_type));
        }

        public void update(GameTime gameTime, GraphicsDevice graphics)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while(timer > 0)
            {
                timer -= 1f / density;
                create_particle();
            }

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].update(gameTime);

                if (particles[i].Position.Y > graphics.Viewport.Height)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
            {
                particle.draw(spriteBatch);
            }
        }
    }
}
