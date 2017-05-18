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
    public class GameOverScreen
    {
        Starfield starfield;

        public static int score;

        int planet_frame_count = 40;
        int planet_width = 40, planet_height = 40;
        float planet_sep = 1f;

        float rotation;

        public GameOverScreen()
        {
            starfield = new Starfield(1000, 800);
            rotation = 0;
        }

        public void update(GameTime gameTime)
        {
            starfield.update(gameTime, new Vector2(1, -1));

            rotation += 0.01f;
            if (rotation * (180 / Math.PI) >= 360)
            {
                rotation = 0;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                Game1.current_game_state = Game1.GameState.MainMenu;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            starfield.draw(spriteBatch);

            for (int i = (planet_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.planet_tex, new Vector2(500, 300 + i * planet_sep), new Rectangle((planet_frame_count - i) * planet_width, 0, planet_width, planet_height), Color.Yellow, rotation + 180 + 0.6f, new Vector2((float)(planet_width / 2), (float)(planet_height / 2)), 4f, SpriteEffects.None, 0f);
            }

            spriteBatch.DrawString(Constant.score_font, "Game Over!", new Vector2(350, 30), Color.Red, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(Constant.score_font, "Score: " + score, new Vector2(400, 100), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(Constant.score_font, "B/Back - Main Menu", new Vector2(50, 550), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
