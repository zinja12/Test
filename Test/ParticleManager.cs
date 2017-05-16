using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class ParticleManager
    {
        public class Particle
        {
            public Vector2 position, direction;
            public Texture2D particle_texture;
            public int decay_time; //in seconds
            public float elapsed, speed, scale;
            public Color color;

            public Particle(Vector2 position, Vector2 direction, float speed, Texture2D texture, int decay_time, float scale, Color color)
            {
                this.position = position;
                this.direction = direction;
                this.speed = speed;
                particle_texture = texture;
                this.decay_time = decay_time;
                elapsed = 0;
                this.scale = scale;
                this.color = color;
            }
        }
        
        public List<Particle> particles;
        Random random;

        public ParticleManager()
        {
            particles = new List<Particle>();
            random = new Random();
        }

        //update all particles
        public void update(GameTime gameTime)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                //add elapsed time
                particles[i].elapsed += gameTime.ElapsedGameTime.Seconds;
                //check elapsed time against decay time
                if (particles[i].speed < 0.1f)
                {
                    //particle is over it's decay time, remove it
                    particles.RemoveAt(i);
                    i--;
                } else
                {
                    //update particle
                    update_particle(particles[i]);
                }
            }
        }

        public void update_particle(Particle particle)
        {
            particle.position += particle.direction * particle.speed;
            particle.speed -= 0.01f;
        }

        public void create_explosion(Vector2 position, Texture2D texture)
        {
            //Generate 120 particles
            for (int i = 0; i < 20; i++)
            {
                //Generate random variables
                int angle = random.Next(0, 360);
                Vector2 dir = new Vector2((float)Math.Cos(angle), -(float)Math.Sin(angle));
                particles.Add(new Particle(position, dir, random.Next(2, 5), texture, random.Next(2, 5), random.Next(1, 3), new Color(random.Next(65, 256), random.Next(0, 80), random.Next(0, 80))));
            }
            Console.WriteLine("Added particles");
        }

        //draw all particles
        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                spriteBatch.Draw(particles[i].particle_texture, particles[i].position, null, particles[i].color, 0f, Vector2.Zero, particles[i].scale, SpriteEffects.None, 0f);
            }
        }
    }
}
