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
    public class Bullets
    {
        Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;
        Color color;
        float rotation;
        Vector2 origin;
        float scale;

        public bool isVisible = true;
        public bool isReflected = true;
        public static int bullet_width = 40, bullet_height = 178;

        public Rectangle boundingBox;




        public Bullets(Texture2D newTexture, Color color, float rotation, float scale)
        {

            texture = newTexture;
            isVisible = false;
            isReflected = false;
            this.color = color;
            this.rotation = rotation;
            origin = new Vector2(newTexture.Width/2, newTexture.Height/2);
            this.scale = scale;
            boundingBox = new Rectangle((int)position.X - 15, (int)position.Y - 15, 30, 30);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            boundingBox.X = (int)position.X - 15;
            boundingBox.Y = (int)position.Y - 15;
            spriteBatch.Draw(texture, position, null, color, rotation, origin, scale, SpriteEffects.None, 0f);
            /*for (int i = (bullet_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(texture, new Vector2(position.X, position.Y + i * bullet_sep), new Rectangle((bullet_frame_count - i) * bullet_width, 0, bullet_width,
                    bullet_height), Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            }*/


        }

    }
}
