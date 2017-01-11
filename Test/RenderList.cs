using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    //A class to handle depth sorting and rendering when the game gets to that point
    public class RenderList
    {
        //List of entities
        List<IEntity> entities;

        //Constructor
        public RenderList()
        {
            entities = new List<IEntity>();
        }

        //Add entities to list
        public void add(IEntity e)
        {
            entities.Add(e);
        }

        //delete entities from list
        public void delete(IEntity e)
        {
            entities.Remove(e);
        }

        //Do not need to sort on every update
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

        //Call sort when necessary
        public void sort_list()
        {
            entities.Sort((p, q) => p.get_base_position().Y.CompareTo(q.get_base_position().Y));
        }

        //Helper print function if needed
        public void print_entities()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Console.WriteLine("Entity: " + i + ", position: " + entities[i].get_base_position().ToString());
            }
        }

        //Simple draw function to draw the entities in order from the list
        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].draw(spriteBatch);
            }
        }
    }
}
