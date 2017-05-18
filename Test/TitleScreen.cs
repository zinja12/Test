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
    public class TitleScreen
    {
        Starfield starfield;
        Button btnStart;
        float rotation;

        int planet_frame_count = 40;
        int planet_width = 40, planet_height = 40;
        float planet_sep = 1f;

        GraphicsDevice graphics;

        public TitleScreen(GraphicsDevice graphics)
        {
            starfield = new Starfield(1000, 800);
            rotation = 0;
            this.graphics = graphics;
        }

        public void update(GameTime gameTime)
        {
            starfield.update(gameTime, new Vector2(1, -1));

            rotation += 0.01f;
            if (rotation * (180 / Math.PI) >= 360)
            {
                rotation = 0;
            }

            btnStart = new Button(Constant.start_button, graphics);
            btnStart.setPosition(new Vector2(300, 150));

            if (btnStart.isClicked || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Game1.current_game_state = Game1.GameState.Playing;
            }
            btnStart.Update(Mouse.GetState());
        }

        public void draw(SpriteBatch spriteBatch)
        {
            starfield.draw(spriteBatch);

            btnStart.Draw(spriteBatch);

            for (int i = (planet_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.planet_tex, new Vector2(200, 400 + i * planet_sep), new Rectangle((planet_frame_count - i) * planet_width, 0, planet_width, planet_height), Color.DeepSkyBlue, rotation + 180 + 0.6f, new Vector2((float)(planet_width / 2), (float)(planet_height / 2)), 4f, SpriteEffects.None, 0f);
            }
            for (int i = (planet_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.planet_tex, new Vector2(800, 100 + i * planet_sep), new Rectangle((planet_frame_count - i) * planet_width, 0, planet_width, planet_height), Color.Yellow, rotation + 180 + 0.6f, new Vector2((float)(planet_width / 2), (float)(planet_height / 2)), 4f, SpriteEffects.None, 0f);
            }
        }
    }
}
