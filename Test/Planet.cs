using System;
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
        public Vector2 orbit_target;

        int planet_frame_count = 40;
        float planet_sep = 1f;
        int planet_width = 40, planet_height = 40;
        float rotation = 0;

        Color draw_color;

        public Planet(Vector2 position, Vector2 orbit_target, Color color)
        {
            this.position = position;
            this.orbit_target = orbit_target;
            draw_color = color;
        }

        public void update(GameTime gameTime)
        {
            rotation += 0.01f;
            if (rotation * (180/Math.PI) >= 360)
            {
                rotation = 0;
            }

            position = RotateAboutOrigin(position, orbit_target, 0.01f);
            //Console.WriteLine("Planet rotation: " + rotation * (180 / Math.PI));
        }

        public Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = (planet_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.planet_tex, new Vector2(position.X, position.Y + i * planet_sep), new Rectangle((planet_frame_count - i) * planet_width, 0, planet_width, planet_height), draw_color, rotation + 180 + 0.6f, new Vector2((float)(planet_width / 2), (float)(planet_height / 2)), 2f, SpriteEffects.None, 0f);
            }
        }
    }
}
