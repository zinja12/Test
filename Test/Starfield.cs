﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class Starfield
    {
        int screen_width, screen_height;
        int total_stars = 250;
        List<Vector2> stars;
        List<int> depth_layers;
        Random rnd;

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

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                spriteBatch.Draw(Constant.particle, stars[i], null, Color.White, 0f, Vector2.Zero, depth_layers[i] - 0.7f, SpriteEffects.None, 0f);
            }
        }
    }
}
