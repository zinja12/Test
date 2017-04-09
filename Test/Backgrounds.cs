using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class Backgrounds
    {
        //Positions
        Vector2[,] back_positions;
        int background_size;
        int image_width = 1500, image_height = 800;
        int ptx = 0, pty = 0;

        public Backgrounds()
        {
            background_size = 3;
            back_positions = new Vector2[3, 3];
            back_positions[0, 0] = new Vector2(-1 * image_width, -1 * image_height);
            back_positions[0, 1] = new Vector2(-1 * image_width, 0 * image_height);
            back_positions[0, 2] = new Vector2(-1 * image_width, 1 * image_height);
            back_positions[1, 0] = new Vector2(0 * image_width, -1 * image_height);
            back_positions[1, 1] = new Vector2(0 * image_width, 0 * image_height);
            back_positions[1, 2] = new Vector2(0 * image_width, 1 * image_height);
            back_positions[2, 0] = new Vector2(1 * image_width, -1 * image_height);
            back_positions[2, 1] = new Vector2(1 * image_width, 0 * image_height);
            back_positions[2, 2] = new Vector2(1 * image_width, 1 * image_height);
        }

        public void update(GameTime gameTime, Vector2 position)
        {
            int tx = (int)position.X / image_width, ty = (int)position.Y / image_height;

            if (ptx == tx && pty == ty)
            {

            }
            else
            {
                if (tx - 1 == ptx && ty == pty)
                {
                    //Shift right
                    back_positions[0, 0].X += image_width * 3;
                    back_positions[0, 1].X += image_width * 3;
                    back_positions[0, 2].X += image_width * 3;
                    //Rotate tiles
                    Vector2 tmp0 = back_positions[0, 0];
                    Vector2 tmp1 = back_positions[0, 1];
                    Vector2 tmp2 = back_positions[0, 2];
                    back_positions[0, 0] = back_positions[1, 0];
                    back_positions[0, 1] = back_positions[1, 1];
                    back_positions[0, 2] = back_positions[1, 2];
                    back_positions[1, 0] = back_positions[2, 0];
                    back_positions[1, 1] = back_positions[2, 1];
                    back_positions[1, 2] = back_positions[2, 2];
                    back_positions[2, 0] = tmp0;
                    back_positions[2, 1] = tmp1;
                    back_positions[2, 2] = tmp2;
                }
                else if (tx + 1 == ptx && ty == pty)
                {
                    //Shift left
                    back_positions[2, 0].X -= image_width * 3;
                    back_positions[2, 1].X -= image_width * 3;
                    back_positions[2, 2].X -= image_width * 3;
                    //Rotate tiles
                    Vector2 tmp0 = back_positions[2, 0];
                    Vector2 tmp1 = back_positions[2, 1];
                    Vector2 tmp2 = back_positions[2, 2];
                    back_positions[2, 0] = back_positions[1, 0];
                    back_positions[2, 1] = back_positions[1, 1];
                    back_positions[2, 2] = back_positions[1, 2];
                    back_positions[1, 0] = back_positions[0, 0];
                    back_positions[1, 1] = back_positions[0, 1];
                    back_positions[1, 2] = back_positions[0, 2];
                    back_positions[0, 0] = tmp0;
                    back_positions[0, 1] = tmp1;
                    back_positions[0, 2] = tmp2;
                }
                else if (tx == ptx && ty - 1 == pty)
                {
                    //Shift up
                    back_positions[0, 2].Y -= image_height * 2;
                    back_positions[1, 2].Y -= image_height * 2;
                    back_positions[2, 2].Y -= image_height * 2;
                }
                else if (tx == ptx - 1 && ty + 1 == pty)
                {
                    //Shift down
                    back_positions[0, 0].Y += image_height * 2;
                    back_positions[1, 0].Y += image_height * 2;
                    back_positions[2, 0].Y += image_height * 2;
                }
            }

            Console.WriteLine("TX: " + tx + "TY: " + ty);
            Console.WriteLine("PTX: " + ptx + "PTY: " + pty);
            Console.WriteLine(position);

            ptx = tx;
            pty = ty;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //Draw 9 rectangles at their respective positions
            for (int x = 0; x < background_size; x++)
            {
                for (int y = 0; y < background_size; y++)
                {
                    spriteBatch.Draw(Constant.background, back_positions[x, y], Color.White);
                }
            }
        }
    }
}
