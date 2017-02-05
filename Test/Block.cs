using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class Block
    {
        public Vector2 position;
        public Rectangle source_rect;
        public Rectangle collision_rect;

        public int[] id = { -1, -1 };

        public Block(Vector2 position, int[] id)
        {
            this.id = id;
            this.position = position;
            source_rect = new Rectangle(id[0] * Constant.tile_size, id[1] * Constant.tile_size, Constant.tile_size, Constant.tile_size);
            collision_rect = new Rectangle((int)position.X, (int)position.Y, Constant.tile_size, Constant.tile_size);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Constant.spritesheet, position, source_rect, Color.White);
        }
    }
}