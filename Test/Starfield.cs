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
    public class Starfield
    {
        int screen_width, screen_height;
        int total_stars = 250;
        List<Vector2> stars;
        List<int> depth_layers;
        Random rnd;

        public static bool shake_stars = false;

        public Starfield(int screen_width, int screen_height)
        {
            this.screen_width = screen_width;
            this.screen_height = screen_height;

            stars = new List<Vector2>();
            depth_layers = new List<int>();
            rnd = new Random();

            for (int i = 0; i < total_stars; i++)
            {
                Vector2 star = new Vector2(rnd.Next(-100, screen_width - 1), rnd.Next(-100, screen_height - 1));
                stars.Add(star);
                depth_layers.Add(rnd.Next(0, 3));
            }
        }

        public void update(GameTime gameTime, Vector2 adjustment_direction)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i] -= (adjustment_direction * depth_layers[i]) * 0.5f;

                //Apply shake
                if (Constant.shake || Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Triggers.Right != 0)
                {
                    shake_stars = true;
                }

                if (shake_stars)
                {
                    star_shake(i);
                    shake_stars = false;
                }

                if (stars[i].X < -100)
                {
                    int tmp_depth = depth_layers[i];

                    //Generate a new star at the other side
                    Vector2 new_star = new Vector2(screen_width - 1, rnd.Next(-100, screen_height - 1));
                    stars.Add(new_star);
                    depth_layers.Add(tmp_depth);

                    //Remove
                    stars.RemoveAt(i);
                    depth_layers.RemoveAt(i);
                }
                else if (stars[i].X > screen_width)
                {
                    int tmp_depth = depth_layers[i];

                    //Generate a new star at the other side
                    Vector2 new_star = new Vector2(1, rnd.Next(-100, screen_height - 1));
                    stars.Add(new_star);
                    depth_layers.Add(tmp_depth);

                    //Remove
                    stars.RemoveAt(i);
                    depth_layers.RemoveAt(i);
                }
                else if (stars[i].Y < -100)
                {
                    int tmp_depth = depth_layers[i];

                    //Generate a new star at the other side
                    Vector2 new_star = new Vector2(rnd.Next(-100, screen_width - 1), screen_height - 1);
                    stars.Add(new_star);
                    depth_layers.Add(tmp_depth);

                    //Remove
                    stars.RemoveAt(i);
                    depth_layers.RemoveAt(i);
                }
                else if (stars[i].Y > screen_height)
                {
                    int tmp_depth = depth_layers[i];

                    //Generate a new star at the other side
                    Vector2 new_star = new Vector2(rnd.Next(-100, screen_width - 1), 1);
                    stars.Add(new_star);
                    depth_layers.Add(tmp_depth);

                    //Remove
                    stars.RemoveAt(i);
                    depth_layers.RemoveAt(i);
                }
            }
        }

        private void star_shake(int index)
        {
            float radius = 2f;
            Random random = new Random();
            float random_angle = random.Next(0, 360);
            Vector2 offset = new Vector2((float)Math.Sin(random_angle) * radius, (float)Math.Cos(random_angle) * radius);

            while (radius >= 2)
            {
                radius *= 0.9f;
                if (random.Next(0, 100) > 50)
                {
                    random_angle += (180 + random.Next(0, 60));
                }
                else
                {
                    random_angle += (180 - random.Next(0, 60));
                }
                offset = new Vector2((float)Math.Sin(random_angle) * radius, (float)Math.Cos(random_angle) * radius);
                stars[index] = stars[index] + offset;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                spriteBatch.Draw(Constant.particle, stars[i], null, Color.White, 0f, Vector2.Zero, depth_layers[i] - 0.5f, SpriteEffects.None, 0f);
            }
        }
    }
}
