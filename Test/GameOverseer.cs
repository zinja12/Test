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
    //Overseer class to manage the game, load and switch levels and handle generalized overhead
    public class GameOverseer
    {
        MouseState mouse;
        Vector2 mouse_position;
        Rectangle mouse_collision_rect;
        bool mouse_left_click = false;
        Object mouse_parent = null;

        Player player;
        ParticleGenerator particle_generator;
        List<Card> cards;

        int test_level = 0;

        public GameOverseer(int test_level, int screen_width, int screen_height)
        {
            mouse_collision_rect = new Rectangle(0, 0, 5, 5);

            player = new Player(new Vector2(100, 100));
            particle_generator = new ParticleGenerator(screen_width, 100f, Particle.ParticleType.RAIN);
            this.test_level = test_level;
            if (test_level == 0)
            {
                //Debug mode
            }
            cards = new List<Card>();
            cards.Add(new Card(new Vector2(300, 150)));
        }

        public void update(GameTime gameTime, GraphicsDevice graphics)
        {
            get_mouse_input();
            player.update(gameTime);
            particle_generator.update(gameTime, graphics);
            foreach (Card c in cards){
                if (mouse_collision_rect.Intersects(c.card_collision_rect) && mouse_left_click)
                {
                    mouse_parent = c;
                } else
                {
                    mouse_parent = null;
                }
                c.update(gameTime);
            }

            if (mouse_parent != null)
            {
                Card c = (Card)mouse_parent;
                c.position = new Vector2(mouse_position.X - (Card.card_width / 2), mouse_position.Y - (Card.card_height / 2));
            }
        }

        private void get_mouse_input()
        {
            mouse = Mouse.GetState();
            mouse_position = new Vector2(mouse.X, mouse.Y);
            mouse_collision_rect = new Rectangle((int)mouse_position.X, (int)mouse_position.Y, 5, 5);

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                mouse_left_click = true;
            }
            else
            {
                mouse_left_click = false;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Card c in cards)
            {
                c.draw(spriteBatch);
            }
            player.draw(spriteBatch);
            spriteBatch.Draw(Constant.particle, mouse_position, Color.Red);
            particle_generator.draw(spriteBatch);
        }
    }
}
