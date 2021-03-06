﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test
{
    public class MapPortal
    {
        public Vector2 position;
        public float scale_factor;

        bool show_map = false;
        bool map_key_down = false;

        public MapPortal(Vector2 position)
        {
            this.position = position;
            scale_factor = 1.9f;
        }

        public void showmap() { show_map = true; }
        public void endmap() { show_map = false; }
        public void checkMapKey(KeyboardState keyboardState, GamePadState gamePadState)
        {
            bool mapKeyDownThisFrame = (keyboardState.IsKeyDown(Keys.E) ||
                (gamePadState.Buttons.Back == ButtonState.Pressed));
            // If key was not down before, but is down now, we toggle the
            // pause setting
            if (!map_key_down && mapKeyDownThisFrame)
            {
                if (!show_map)
                    showmap();
                else
                    endmap();
            }
            map_key_down = mapKeyDownThisFrame;
        }

        public void update(GameTime gameTime, Vector2 position)
        {
            checkMapKey(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One));
            
            this.position = position;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (show_map)
            {
                spriteBatch.Draw(Constant.symbol_circle, position, null, Color.White, 0f, new Vector2(Constant.symbol_circle.Width / 2, Constant.symbol_circle.Height / 2), 1f * scale_factor, SpriteEffects.None, 0f);
                spriteBatch.Draw(Constant.symbol_spritesheet, new Vector2(position.X + 25, position.Y + 25), new Rectangle(0 * 32, 0 * 32, 32, 32), Color.White, -1, new Vector2(16, 16), 1f * scale_factor, SpriteEffects.None, 0f);
                spriteBatch.Draw(Constant.symbol_spritesheet, new Vector2(position.X - 150, position.Y - 100), new Rectangle(1 * 32, 0 * 32, 32, 32), Color.White, 0.5f, new Vector2(16, 16), 1f * scale_factor, SpriteEffects.None, 0f);
                spriteBatch.Draw(Constant.symbol_spritesheet, new Vector2(position.X + 200, position.Y + 50), new Rectangle(3 * 32, 0 * 32, 32, 32), Color.White, 100, new Vector2(16, 16), 1f * scale_factor, SpriteEffects.None, 0f);
            }
        }
    }
}
