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
    public class ControlsScreen
    {
        Starfield starfield;

        float elapsed;
        float rotation;

        int planet_frame_count = 40;
        int planet_width = 40, planet_height = 40;
        float planet_sep = 1f;

        public ControlsScreen()
        {
            starfield = new Starfield(1000, 800);
            elapsed = 0;
            rotation = 0;
        }

        public void update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            starfield.update(gameTime, new Vector2(1, -1));

            rotation += 0.01f;
            if (rotation * (180 / Math.PI) >= 360)
            {
                rotation = 0;
            }

            if (elapsed > 0.2)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.B) || Keyboard.GetState().IsKeyDown(Keys.Back) || Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
                {
                    Game1.current_game_state = Game1.GameState.MainMenu;
                }
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            starfield.draw(spriteBatch);

            for (int i = (planet_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.planet_tex, new Vector2(500, 300 + i * planet_sep), new Rectangle((planet_frame_count - i) * planet_width, 0, planet_width, planet_height), Color.Yellow, rotation + 180 + 0.6f, new Vector2((float)(planet_width / 2), (float)(planet_height / 2)), 4f, SpriteEffects.None, 0f);
            }

            spriteBatch.DrawString(Constant.score_font, "Controls!", new Vector2(350, 30), Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Constant.score_font, "Embark on your arcade-y mission pilot!", new Vector2(40, 100), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Constant.score_font, "Protect your solar system from invaders!", new Vector2(20, 150), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(Constant.score_font, "A - Selection", new Vector2(50, 300), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Constant.score_font, "B/Back - Back", new Vector2(50, 350), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Constant.score_font, "RT/Space - Fire", new Vector2(50, 400), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(Constant.score_font, "Start - Pause", new Vector2(600, 300), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Constant.score_font, "LS/W - Thrusters", new Vector2(600, 350), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Constant.score_font, "RS/A/D - Turn", new Vector2(600, 400), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(Constant.score_font, "It is highly recommended that you play with an Xbox360/XboxOne GamePad", new Vector2(80, 550), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
