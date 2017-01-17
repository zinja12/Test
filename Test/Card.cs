using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class Card
    {
        public Vector2 position;
        public Rectangle card_collision_rect;

        public static int card_width = 200, card_height = 300;

        public Card(Vector2 position)
        {
            this.position = position;
            card_collision_rect = new Rectangle((int)position.X, (int)position.Y, card_width, card_height);
        }

        public void update(GameTime gameTime)
        {
            //Update the card's collision rectangle every 
            card_collision_rect = new Rectangle((int)position.X, (int)position.Y, card_width, card_height);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //Draw card
            spriteBatch.Draw(Constant.card, position, Color.White);
        }
    }
}
