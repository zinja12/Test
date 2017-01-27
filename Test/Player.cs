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
        //Variables
        private Vector2 position, velocity, base_position, left_foot, right_foot, l_hip, r_hip, l_knee, r_knee;
        private KeyboardState keyboard;
        private bool is_moving;
        private volatile bool shine;
        private float top_speed = 2.0f, friction = 0.1f;

        int pref_rotation = -1;

        public static int width = 32, height = 32;
        private int leg_bone_length = (width / 2);

        private Animation test_animation;

        //Constructor
        public Player(Vector2 position)
        {
            //Null check
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
            //Initial left and right foot positions and leg positions
            left_foot = new Vector2(position.X, position.Y + height * 2 - 5);
            right_foot = new Vector2(position.X + width, position.Y + height * 2 - 5);
            l_hip = new Vector2(position.X, position.Y + height);
            r_hip = new Vector2(position.X + width, position.Y + height);
            l_knee = new Vector2(position.X + 5, position.Y + height - (height / 2));
            r_knee = new Vector2(position.X + width + 5, position.Y + height - (height / 2));
            //Initialize animation
            test_animation = new Animation(100.0f, 4-1, 0, 0, width, height);
        }

        //Getters
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
            handle_ik();
            //Combine position with velocity, make sure to keep the constraints in mine
            if (velocity.X > top_speed) velocity.X = top_speed;
            if (velocity.Y > top_speed) velocity.Y = top_speed;

            position += velocity;
            //Keep base position and feet constant
            base_position = new Vector2(position.X + (width / 2), position.Y + height);
            //left_foot = new Vector2(position.X, position.Y + height * 2);
            right_foot = new Vector2(position.X + width, position.Y + height * 2);
            l_hip = new Vector2(position.X, position.Y + height);
            r_hip = new Vector2(position.X + width, position.Y + height);
            //l_knee = new Vector2(position.X, position.Y + height*2 - (height / 2));
            r_knee = new Vector2(position.X + width, position.Y + height*2 - (height / 2));
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

            if (keyboard.IsKeyDown(Keys.K))
            {
                left_foot.Y += 1.5f;
            } else if (keyboard.IsKeyDown(Keys.I))
            {
                left_foot.Y -= 1.5f;
            } else if (keyboard.IsKeyDown(Keys.J))
            {
                left_foot.X -= 1.5f;
            } else if (keyboard.IsKeyDown(Keys.L))
            {
                left_foot.X += 1.5f;
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

        //Handle inverse kinematics with a lot of maths lol
        private void handle_ik()
        {
            //Inverse kinematics for the left leg
            float direction_x = left_foot.X - l_hip.X;
            float direction_y = left_foot.Y - l_hip.Y;
            float length = (float)Math.Sqrt(direction_x * direction_x + direction_y * direction_y);
            direction_x = (direction_x / length);
            direction_y = (direction_y / length);

            float disc = leg_bone_length * leg_bone_length - length * length / 4;
            if (disc < 0)
            {
                l_knee.X = l_hip.X + direction_x * leg_bone_length;
                l_knee.Y = l_hip.Y + direction_y * leg_bone_length;
                left_foot.X = l_hip.X + direction_x * leg_bone_length * 2;
                left_foot.Y = l_hip.Y + direction_y * leg_bone_length * 2;
            } else
            {
                l_knee.X = l_hip.X + direction_x * length / 2;
                l_knee.Y = l_hip.Y + direction_y * length / 2;
                disc = (float)Math.Sqrt(disc);
                if (pref_rotation < 0)
                {
                    disc = -disc;
                }
                l_knee.X -= direction_y * disc;
                l_knee.Y += direction_x * disc;
            }
            
            //TODO: Implement inverse kinematics for the right leg
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //Renderer.FillRectangle(spriteBatch, position, width, height, Color.Purple);
            //Renderer.FillRectangle(spriteBatch, position, 5, 5, Color.Red);
            //Renderer.FillRectangle(spriteBatch, point_orbit, 5, 5, Color.Green);
            //Renderer.FillRectangle(spriteBatch, origin, 5, 5, Color.Blue);
            //Draw hitbox
            spriteBatch.Draw(Constant.spritesheet, position, test_animation.source_rect, Color.White);
            Renderer.FillRectangle(spriteBatch, left_foot, 5, 5, Color.Cyan);
            Renderer.FillRectangle(spriteBatch, right_foot, 5, 5, Color.Cyan);
            Renderer.FillRectangle(spriteBatch, l_hip, 5, 5, Color.Cyan);
            Renderer.FillRectangle(spriteBatch, r_hip, 5, 5, Color.Cyan);
            Renderer.FillRectangle(spriteBatch, l_knee, 5, 5, Color.Cyan);
            Renderer.FillRectangle(spriteBatch, r_knee, 5, 5, Color.Cyan);
            //Draw legs?
            Renderer.DrawALine(spriteBatch, Constant.pixel, 1, Color.LightCyan, l_hip, l_knee);
            Renderer.DrawALine(spriteBatch, Constant.pixel, 1, Color.LightCyan, l_knee, left_foot);
        }
    }
}
