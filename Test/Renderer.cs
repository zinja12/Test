using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    //Renderer class to draw rectangles and lines without the need to create individual textures for each of them
    public class Renderer
    {
        //Line angle and length
        public static float lineAngle;
        public static float lineLength;

        //Function to draw the line
        public static void DrawALine(SpriteBatch batch, Texture2D blank,
              float width, Color color, Vector2 point1, Vector2 point2)
        {
            //Calculate the angle of the start and end points
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            //Calculate the length
            float length = Vector2.Distance(point1, point2);
            //Set to variables
            lineAngle = angle;
            lineLength = length;

            //Draw line from one point to another
            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        //Draw and fill a rectangle
        public static void FillRectangle(SpriteBatch spriteBatch, Vector2 rect_position, int width, int height, Color color)
        {
            //Create a new texture
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, width, height);

            //Create it's color data array
            Color[] color_data = new Color[width * height];
            //Set its color array to a solid predefined color
            for (int i = 0; i < color_data.Length; i++)
                color_data[i] = color;
            //Set it
            rect.SetData(color_data);

            //Draw it at position
            Vector2 position = rect_position;
            spriteBatch.Draw(rect, position, Color.White);
        }
    }
}
