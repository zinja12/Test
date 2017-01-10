using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class GameOverseer
    {
        Player player;

        int test_level = 0;

        public GameOverseer(int test_level)
        {
            player = new Player(new Vector2(100, 100));
            this.test_level = test_level;
            if (test_level == 0)
            {
                //Debug mode
            }
        }

        public void update(GameTime gameTime)
        {
            player.update(gameTime);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            player.draw(spriteBatch);
        }
    }
}
