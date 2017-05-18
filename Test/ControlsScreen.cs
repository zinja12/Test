using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class ControlsScreen
    {
        Starfield starfield;

        public ControlsScreen()
        {
            starfield = new Starfield(1000, 800);
        }

        public void update(GameTime gameTime)
        {
            starfield.update(gameTime, new Vector2(1, -1));
        }

        public void draw(SpriteBatch spriteBatch)
        {
            starfield.draw(spriteBatch);
        }
    }
}
