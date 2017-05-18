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
        Button btnControls;
        float rotation;

        int menu_option = 0;
        Vector2 selector_position;

        int planet_frame_count = 40;
        int planet_width = 40, planet_height = 40;
        float planet_sep = 1f;

        GraphicsDevice graphics;

        float elapsed;

        public int ship_width = 28, ship_height = 82;
        public int ship_sep = 1;
        public int ship_frame_count = 20;

        public TitleScreen(GraphicsDevice graphics)
        {
            starfield = new Starfield(1000, 800);
            rotation = 0;
            this.graphics = graphics;
            selector_position = new Vector2(300f, 300f);
            elapsed = 0;
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

            btnStart = new Button(Constant.start_button, graphics);
            btnControls = new Button(Constant.controls_button, graphics);
            btnStart.setPosition(new Vector2(300, 150));
            btnControls.setPosition(new Vector2(305, 250));

            btnStart.Update(Mouse.GetState());
            btnControls.Update(Mouse.GetState());

            if (btnStart.isClicked || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                if (menu_option == 0) Game1.current_game_state = Game1.GameState.Playing;
                else if (menu_option == 1) Game1.current_game_state = Game1.GameState.Controls;

            }

            if (elapsed > 0.1)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                {
                    if (menu_option < 1)
                    {
                        menu_option++;
                        selector_position.Y += 100f;
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                {
                    if (menu_option > 0)
                    {
                        menu_option--;
                        selector_position.Y -= 100f;
                    }
                }
                elapsed = 0;
            }

        }

        public void draw(SpriteBatch spriteBatch)
        {
            starfield.draw(spriteBatch);
            btnControls.Draw(spriteBatch);
            btnStart.Draw(spriteBatch);

            for (int i = (planet_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.planet_tex, new Vector2(150, 400 + i * planet_sep), new Rectangle((planet_frame_count - i) * planet_width, 0, planet_width, planet_height), Color.DeepSkyBlue, rotation + 180 + 0.6f, new Vector2((float)(planet_width / 2), (float)(planet_height / 2)), 4f, SpriteEffects.None, 0f);
            }
            for (int i = (planet_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.planet_tex, new Vector2(800, 100 + i * planet_sep), new Rectangle((planet_frame_count - i) * planet_width, 0, planet_width, planet_height), Color.Yellow, rotation + 180 + 0.6f, new Vector2((float)(planet_width / 2), (float)(planet_height / 2)), 4f, SpriteEffects.None, 0f);
            }

            //Renderer.FillRectangle(spriteBatch, selector_position, 50, 50, Color.CornflowerBlue);
            for (int i = (ship_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.ship_tex, new Vector2(selector_position.X, selector_position.Y + i * ship_sep), new Rectangle((ship_frame_count - i) * ship_width, 0, ship_width, ship_height), Color.White, 180 + 0.6f, new Vector2((float)(ship_width / 2), (float)(ship_height / 2)), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
