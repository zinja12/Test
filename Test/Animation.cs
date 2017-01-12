using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class Animation
    {
        //Define a rectangle that will represent the current frame being drawn
        //Test to see if this goes to github.
        public Rectangle source_rect;
        //Variables
        private int x, y, rect_width, rect_height, frame_count, frames;

        private float elapsed, delay;

        public Animation(float delay, int frame_count, int x, int y, int rect_width, int rect_height)
        {
            this.delay = delay;
            this.frame_count = frame_count;
            this.x = x;
            this.y = y;
            this.rect_width = rect_width;
            this.rect_height = rect_height;
            frames = 0;
        }

        public void update(GameTime gameTime)
        {
            //Calulate how much time has passed
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //If the time exceeds the delay and the frames variable is greater than the number of frames of the animation, either
            //set it back to zero or increment the frame currently on
            if (elapsed >= delay)
            {
                if (frames >= frame_count)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }

            //Set the rectangle for the current frame of the animation based on the current frame we are at
            source_rect = new Rectangle(x + frames * rect_width, y, rect_width, rect_height);
        }

        //Getters
        public int X()
        {
            return x;
        }

        public int Y()
        {
            return y;
        }

        public int rectWidth()
        {
            return rect_width;
        }

        public int rectHeight()
        {
            return rect_height;
        }
    }
}
