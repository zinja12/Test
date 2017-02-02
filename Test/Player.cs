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
        private Vector2 l_foot_anchor_point, r_foot_anchor_point;
        private KeyboardState keyboard;
        private bool is_moving;
        private volatile bool shine;
        private float top_speed = 2.0f, friction = 0.1f;

        private Vector2 sin_point = new Vector2(200, 200);
        private float n = 0;

        private int pref_rotation = -1;

        public static int width = 32, height = 32;
        private int leg_bone_length = (width / 2);

        //private Animation test_animation;

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
            l_foot_anchor_point = new Vector2(position.X, position.Y + height * 2 - 5);
            right_foot = new Vector2(position.X + width, position.Y + height * 2 - 5);
            r_foot_anchor_point = new Vector2(position.X + width, position.Y + height * 2 - 5);
            l_hip = new Vector2(position.X, position.Y + height);
            r_hip = new Vector2(position.X + width, position.Y + height);
            l_knee = new Vector2(position.X + 5, position.Y + height - (height / 2));
            r_knee = new Vector2(position.X + width + 5, position.Y + height - (height / 2));
            //Initialize animation
            //test_animation = new Animation(100.0f, 4-1, 0, 0, width, height);
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
            l_foot_anchor_point = new Vector2(position.X, position.Y + height * 2 - 5);
            r_foot_anchor_point = new Vector2(position.X + width, position.Y + height * 2 - 5);
            l_hip = new Vector2(position.X, position.Y + height);
            r_hip = new Vector2(position.X + width, position.Y + height);
            //l_knee = new Vector2(position.X, position.Y + height*2 - (height / 2));
            //r_knee = new Vector2(position.X + width, position.Y + height*2 - (height / 2));
            //test_animation.update(gameTime);

            if (is_moving)
            {
                //Need to add to not do anything to the foot position if the value the sin gives back is negative
                //This will help the foot look like it is staying on the ground
                float sin_value = (float)Math.Sin(n) * 0.5f;
                float sin_a_value = (float)Math.Sin(n+1f) * 0.5f;
                float cos_value = (float)Math.Cos(n) * 0.5f;

                sin_point += new Vector2((float)Math.Cos(n), (float)Math.Sin(n));
                left_foot.X = l_foot_anchor_point.X + cos_value;
                left_foot.Y += sin_value;
                right_foot.X = r_foot_anchor_point.X + cos_value;
                right_foot.Y += sin_a_value;
                n += 0.1f;
            } else
            {
                float sin_value = (float)Math.Sin(n) * 0.5f;
                float sin_a_value = (float)Math.Sin(n + 1f) * 0.5f;
                float cos_value = (float)Math.Cos(n) * 0.5f;
                if (Vector2.Distance(left_foot, l_foot_anchor_point) >= 0.5f)
                {
                    left_foot.Y += sin_a_value;

                    n += 0.1f;
                }

                if (Vector2.Distance(right_foot, r_foot_anchor_point) >= 0.5f)
                {
                    right_foot.Y += sin_a_value;

                    n += 0.1f;
                }

                n = 0f;
            }
        }

        public void poll_input()
        {
            keyboard = Keyboard.GetState();

            //Check input across WASD and the Arrow Keys
            //Adjust velocity being added to the players position accordingly
            if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
            {
                velocity.X = -top_speed;
                pref_rotation = 1;
            }
            else if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
            {
                velocity.X = top_speed;
                pref_rotation = -1;
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
            float l_leg_direction_x = left_foot.X - l_hip.X;
            float l_leg_direction_y = left_foot.Y - l_hip.Y;
            float l_leg_length = (float)Math.Sqrt(l_leg_direction_x * l_leg_direction_x + l_leg_direction_y * l_leg_direction_y);
            l_leg_direction_x = (l_leg_direction_x / l_leg_length);
            l_leg_direction_y = (l_leg_direction_y / l_leg_length);

            float l_leg_disc = leg_bone_length * leg_bone_length - l_leg_length * l_leg_length / 4;
            if (l_leg_disc < 0)
            {
                l_knee.X = l_hip.X + l_leg_direction_x * leg_bone_length;
                l_knee.Y = l_hip.Y + l_leg_direction_y * leg_bone_length;
                left_foot.X = l_hip.X + l_leg_direction_x * leg_bone_length * 2;
                left_foot.Y = l_hip.Y + l_leg_direction_y * leg_bone_length * 2;
            } else
            {
                l_knee.X = l_hip.X + l_leg_direction_x * l_leg_length / 2;
                l_knee.Y = l_hip.Y + l_leg_direction_y * l_leg_length / 2;
                l_leg_disc = (float)Math.Sqrt(l_leg_disc);
                if (pref_rotation < 0)
                {
                    l_leg_disc = -l_leg_disc;
                }
                l_knee.X -= l_leg_direction_y * l_leg_disc;
                l_knee.Y += l_leg_direction_x * l_leg_disc;
            }

            //Inverse kinematics for the right leg
            float r_leg_direction_x = right_foot.X - r_hip.X;
            float r_leg_direction_y = right_foot.Y - r_hip.Y;
            float r_leg_length = (float)Math.Sqrt(r_leg_direction_x * r_leg_direction_x + r_leg_direction_y * r_leg_direction_y);
            r_leg_direction_x = (r_leg_direction_x / r_leg_length);
            r_leg_direction_y = (r_leg_direction_y / r_leg_length);

            float r_leg_disc = leg_bone_length * leg_bone_length - r_leg_length * r_leg_length / 4;
            if (r_leg_disc < 0)
            {
                r_knee.X = r_hip.X + r_leg_direction_x * leg_bone_length;
                r_knee.Y = r_hip.Y + r_leg_direction_y * leg_bone_length;
                right_foot.X = r_hip.X + r_leg_direction_x * leg_bone_length * 2;
                right_foot.Y = r_hip.Y + r_leg_direction_y * leg_bone_length * 2;
            }
            else
            {
                r_knee.X = r_hip.X + r_leg_direction_x * r_leg_length / 2;
                r_knee.Y = r_hip.Y + r_leg_direction_y * r_leg_length / 2;
                r_leg_disc = (float)Math.Sqrt(r_leg_disc);
                if (pref_rotation < 0)
                {
                    r_leg_disc = -r_leg_disc;
                }
                r_knee.X -= r_leg_direction_y * r_leg_disc;
                r_knee.Y += r_leg_direction_x * r_leg_disc;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //Renderer.FillRectangle(spriteBatch, position, width, height, Color.Purple);
            //Renderer.FillRectangle(spriteBatch, position, 5, 5, Color.Red);
            //Renderer.FillRectangle(spriteBatch, point_orbit, 5, 5, Color.Green);
            //Renderer.FillRectangle(spriteBatch, origin, 5, 5, Color.Blue);
            //Draw hitbox
            
            //Draw animation
            //spriteBatch.Draw(Constant.spritesheet, position, test_animation.source_rect, Color.White);
            //Draw body
            spriteBatch.Draw(Constant.spritesheet, position, new Rectangle(2 * width, 1 * height, width, height), Color.White);
            //Draw eyes
            Renderer.FillRectangle(spriteBatch, new Vector2(position.X + 11, position.Y + 10), 5, 5, Color.Black);
            Renderer.FillRectangle(spriteBatch, new Vector2(position.X + width - 11, position.Y + 10), 5, 5, Color.Black);
            //Draw debug points depending on flag
            if (Constant.debug)
            {
                Renderer.FillRectangle(spriteBatch, left_foot, 5, 5, Color.Red);
                Renderer.FillRectangle(spriteBatch, right_foot, 5, 5, Color.Red);
                Renderer.FillRectangle(spriteBatch, l_hip, 5, 5, Color.Red);
                Renderer.FillRectangle(spriteBatch, r_hip, 5, 5, Color.Red);
                Renderer.FillRectangle(spriteBatch, l_knee, 5, 5, Color.Red);
                Renderer.FillRectangle(spriteBatch, r_knee, 5, 5, Color.Red);
                Renderer.FillRectangle(spriteBatch, l_foot_anchor_point, 5, 5, Color.Orange);
                Renderer.FillRectangle(spriteBatch, r_foot_anchor_point, 5, 5, Color.Orange);
            }

            //Draw legs?
            Renderer.DrawALine(spriteBatch, Constant.pixel, 1, Color.Blue, l_hip, l_knee);
            Renderer.DrawALine(spriteBatch, Constant.pixel, 1, Color.Blue, l_knee, left_foot);
            Renderer.DrawALine(spriteBatch, Constant.pixel, 1, Color.Blue, r_hip, r_knee);
            Renderer.DrawALine(spriteBatch, Constant.pixel, 1, Color.Blue, r_knee, right_foot);

            //Draw sin point
            Renderer.FillRectangle(spriteBatch, sin_point, 5, 5, Color.Purple);
        }
    }
}
