using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test
{
    class Bullets
    {
        Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;

        public bool isVisible = true;


        public Bullets(Texture2D newTexture)
        {

            texture = newTexture;
            isVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(texture, position, Color.Yellow);

        }

    }
}
