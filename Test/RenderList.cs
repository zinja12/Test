using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class RenderList
    {
        List<IEntity> entities;

        public RenderList()
        {
            entities = new List<IEntity>();
        }

        public void add(IEntity e)
        {
            entities.Add(e);
        }

        public void delete(IEntity e)
        {
            entities.Remove(e);
        }

        /*public void update(GameTime gameTime)
        {
            //Sort the list?
            //Probably not efficient at all
            //if there is player movement, sort the list on the fly, insert the player at the right position
            if (Player.is_moving) 
            {
                sort_list();
            }
        }*/

        public void sort_list()
        {
            entities.Sort((p, q) => p.get_base_position().Y.CompareTo(q.get_base_position().Y));
        }

        public void print_entities()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Console.WriteLine("Entity: " + i + ", position: " + entities[i].get_base_position().ToString());
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].draw(spriteBatch);
            }
        }
    }
}
