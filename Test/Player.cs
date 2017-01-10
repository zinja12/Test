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
    public interface IEntity
    {
        Vector2 get_base_position();
        void draw(SpriteBatch spriteBatch);
    }

    public class Player : IEntity
    {
        private Vector2 position, velocity, base_position;
        private KeyboardState keyboard;
        private bool is_moving;
        private volatile bool shine;
        private float top_speed = 2.0f, friction = 0.1f;
        
        public static int width = 32, height = 32;

        private Animation test_animation;

        public Player(Vector2 position)
        {
            if (position == null)
            {
                position = Vector2.Zero;
            } else
            {
                this.position = position;
            }
            shine = false;
            is_moving = false;
            base_position = new Vector2(position.X + (width / 2), position.Y + height);
            //Initialize animation
            test_animation = new Animation(100.0f, 4-1, 0, 0, width, height);
        }

        public Vector2 get_base_position()
        {
            return base_position;
        }

        public bool is_player_moving()
        {
            return is_moving;
        }

        public bool is_shine()
        {
            return shine;
        }

        public void update(GameTime gameTime)
        {
            //Handle polling input and velocity
            poll_input();
            //Combine position with velocity, make sure to keep the constraints in mine
            if (velocity.X > top_speed) velocity.X = top_speed;
            if (velocity.Y > top_speed) velocity.Y = top_speed;

            position += velocity;
            //Keep base position constant
            base_position = new Vector2(position.X + (width / 2), position.Y + height);
            test_animation.update(gameTime);
        }

        public void poll_input()
        {
            keyboard = Keyboard.GetState();

            //Check input across WASD and the Arrow Keys
            //Adjust velocity being added to the players position accordingly
            if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
            {
                velocity.X = -top_speed;
            }
            else if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
            {
                velocity.X = top_speed;
            }
            else
            {
                float i = velocity.X;
                velocity.X = i -= friction * i;
            }

            if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
            {
                velocity.Y = -top_speed;
            }
            else if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
            {
                velocity.Y = top_speed;
            }
            else
            {
                float i = velocity.Y;
                velocity.Y = i -= friction * i;
            }

            //Check if the player is moving
            //If none of the input keys are being depressed then there is no movement
            if (keyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyUp(Keys.Down) && keyboard.IsKeyUp(Keys.Right) && keyboard.IsKeyUp(Keys.Left) &&
                keyboard.IsKeyUp(Keys.W) && keyboard.IsKeyUp(Keys.A) && keyboard.IsKeyUp(Keys.S) && keyboard.IsKeyUp(Keys.D))
            {
                is_moving = false;
            }
            else
            {
                is_moving = true;
            }

            if (keyboard.IsKeyDown(Keys.Space))
            {
                if (shine)
                {
                    //Jump cancel the shine
                    shine = false;
                }
                else
                {
                    //Do a normal jump
                    //If on the ground
                }
            }

            //Shine has to be active at least one frame so we check it last and then we are able to jump cancel it
            if (keyboard.IsKeyDown(Keys.LeftShift) || keyboard.IsKeyDown(Keys.RightShift))
            {
                shine = true;
            }
            else
            {
                shine = false;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //Renderer.FillRectangle(spriteBatch, position, width, height, Color.Purple);
            //Renderer.FillRectangle(spriteBatch, position, 5, 5, Color.Red);
            //Renderer.FillRectangle(spriteBatch, point_orbit, 5, 5, Color.Green);
            //Renderer.FillRectangle(spriteBatch, origin, 5, 5, Color.Blue);
            //Draw hitbox
            Renderer.FillRectangle(spriteBatch, position, width, width, Color.Purple);
            spriteBatch.Draw(Constant.spritesheet, position, test_animation.source_rect, Color.White);
        }
    }
}
