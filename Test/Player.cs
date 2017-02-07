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
        private KeyboardState keyboard;

        public Vector2 position, velocity, base_position;
        public Vector2 left_side_pt, right_side_pt, top_side_pt;
        private float other_rotation;
        public Rectangle other_collision_rect;

        public float friction = 0.1f;
        public static int width = 32, height = 32;
        private int leg_bone_length = (width / 2);

        //private Animation test_animation;

        //Constructor
        public Player(Vector2 position)
        {
            //Initialize animation
            //test_animation = new Animation(100.0f, 4-1, 0, 0, width, height);

            this.position = position;
            base_position = new Vector2(position.X, position.Y + (height/2));
            left_side_pt = new Vector2(position.X + (width/2), position.Y);
            right_side_pt = new Vector2(position.X - (width/2), position.Y);
            top_side_pt = new Vector2(position.X, position.Y - (height/2));
            velocity = Vector2.Zero;
            other_rotation = 0f;
            other_collision_rect = new Rectangle((int)this.position.X, (int)this.position.Y, width, height);
        }

        //Getters
        public Vector2 get_base_position()
        {
            return base_position;
        }

        public Rectangle get_collision_rect()
        {
            return other_collision_rect;
        }

        public void update(GameTime gameTime)
        {
            //Handle polling input and velocity
            poll_input();

            //Update points and collision rectangle
            position += velocity;
            base_position += velocity;
            left_side_pt += velocity;
            right_side_pt += velocity;
            top_side_pt += velocity;
            other_collision_rect.X = (int)(position.X - (width/2));
            other_collision_rect.Y = (int)(position.Y - (height/2));
        }

        public void poll_input()
        {
            keyboard = Keyboard.GetState();

            //Check input across WASD and the Arrow Keys
            //Adjust velocity being added to the players position accordingly
            
            //Control rotation
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
            {
                other_rotation += 0.08f;
            }
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
            {
                other_rotation += -0.08f;
            }
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
            {
                Vector2 direction = new Vector2((float)Math.Cos(other_rotation), (float)Math.Sin(other_rotation));
                direction.Normalize();
                velocity += direction * 0.3f;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            //Draw animation
            //spriteBatch.Draw(Constant.spritesheet, position, test_animation.source_rect, Color.White);
            //draw other
            spriteBatch.Draw(Constant.bird, position, null, Color.White, other_rotation, new Vector2(Constant.bird.Width / 2, Constant.bird.Height / 2), 1f, SpriteEffects.None, 0f);
            //Other collision rect
            if (Constant.debug)
            {
                //Draw position point
                Renderer.FillRectangle(spriteBatch, position, 5, 5, Color.Purple);
                Renderer.FillRectangle(spriteBatch, new Vector2(other_collision_rect.X, other_collision_rect.Y), 5, 5, Color.Red);
                //Draw Rectangle
                Renderer.DrawALine(spriteBatch, Constant.pixel, 1, Color.Purple, new Vector2(other_collision_rect.X, other_collision_rect.Y), new Vector2(other_collision_rect.X + width, other_collision_rect.Y));
                Renderer.DrawALine(spriteBatch, Constant.pixel, 1, Color.Purple, new Vector2(other_collision_rect.X, other_collision_rect.Y), new Vector2(other_collision_rect.X, other_collision_rect.Y + height));
                Renderer.DrawALine(spriteBatch, Constant.pixel, 1, Color.Purple, new Vector2(other_collision_rect.X + width, other_collision_rect.Y), new Vector2(other_collision_rect.X + width, other_collision_rect.Y + height));
                Renderer.DrawALine(spriteBatch, Constant.pixel, 1, Color.Purple, new Vector2(other_collision_rect.X, other_collision_rect.Y + height), new Vector2(other_collision_rect.X + width, other_collision_rect.Y + height));
                //Draw points
                Renderer.FillRectangle(spriteBatch, top_side_pt, 5, 5, Color.Orange);
                Renderer.FillRectangle(spriteBatch, left_side_pt, 5, 5, Color.Orange);
                Renderer.FillRectangle(spriteBatch, right_side_pt, 5, 5, Color.Orange);
                Renderer.FillRectangle(spriteBatch, base_position, 5, 5, Color.Orange);
            }
        }
    }
}
