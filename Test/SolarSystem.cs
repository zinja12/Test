using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class SolarSystem
    {
        public Vector2 system_center;
        public List<Planet> planets_in_system;
        public Planet sun = null;

        public SolarSystem(Vector2 system_center)
        {
            this.system_center = system_center;
            planets_in_system = new List<Planet>();
        }

        public bool add_sun(Planet planet)
        {
            if (sun == null)
            {
                sun = planet;
                return true;
            }
            return false;
        }

        public void add_planet(Planet planet)
        {
            planets_in_system.Add(planet);
        }

        public void update(GameTime gameTime)
        {
            //Update planets
            for (int i = 0; i < planets_in_system.Count; i++)
            {
                planets_in_system[i].update(gameTime);
            }

            //Update sun
            if (sun != null)
            {
                sun.update(gameTime);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //Draw Plants
            for (int i = 0; i < planets_in_system.Count; i++)
            {
                planets_in_system[i].draw(spriteBatch);
            }
            //Update sun
            if (sun != null)
            {
                sun.draw(spriteBatch);
            }
        }
    }
}
