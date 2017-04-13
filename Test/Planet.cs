﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class Planet
    {
        public Vector2 position;

        int planet_frame_count = 40;
        float planet_sep = 1f;
        int planet_width = 40, planet_height = 40;
        float rotation = 0;

        public Planet(Vector2 position)
        {
            this.position = position;
        }

        public void update(GameTime gameTime)
        {
            rotation += 0.01f;
            if (rotation * (180/Math.PI) >= 360)
            {
                rotation = 0;
            }
            //Console.WriteLine("Planet rotation: " + rotation * (180 / Math.PI));
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Color col = Color.Red;
            for (int i = (planet_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.planet_tex, new Vector2(position.X, position.Y + i * planet_sep), new Rectangle((planet_frame_count - i) * planet_width, 0, planet_width, planet_height), col, rotation + 180 + 0.6f, new Vector2((float)(planet_width / 2), (float)(planet_height / 2)), 2f, SpriteEffects.None, 0f);
            }
        }
    }
}
