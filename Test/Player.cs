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
        private float rotation;
        public Rectangle other_collision_rect;
		public Rectangle healthDestRect;
		public Rectangle healthSourceRect;


        private int ship_frame_count = 20;
		int healthFrames = 0;
        private float ship_sep = 1;

        public bool grounded;

        public float friction = 0.08f;
        public static int width = 32, height = 32;
        public static int ship_width = 28, ship_height = 82;

        public bool player_debug = false;
		public int health;

        //private Animation test_animation;

        //Constructor
        public Player(Vector2 position, int health)
        {
            //Initialize animation
            //test_animation = new Animation(100.0f, 4-1, 0, 0, width, height);

            this.position = position;
			this.health = health;
            base_position = new Vector2(position.X, position.Y + (height/2));
            left_side_pt = new Vector2(position.X + (width/2), position.Y);
            right_side_pt = new Vector2(position.X - (width/2), position.Y);
            top_side_pt = new Vector2(position.X, position.Y - (height/2));
            velocity = Vector2.Zero;
            rotation = 0f;
            other_collision_rect = new Rectangle((int)this.position.X, (int)this.position.Y, width, height);
			healthDestRect = new Rectangle(-400, -200, 158, 152);
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

        public float get_rotation()
        {
            return rotation;
        }

		public int getHealth() 
		{
			return health;
		}

		public void playerHit()
		{
			
			health--;
			healthFrames++;
		}


        public void update(GameTime gameTime, SpriteBatch spriteBatch)
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

            //Handle friction
            float i = velocity.X;
            float j = velocity.Y;
            velocity.X = i -= friction * i;
            velocity.Y = j -= friction * j;

			if (healthFrames <= 4)
			{			
				healthSourceRect = new Rectangle(157 * healthFrames, 0, 157, 152);

			}

           // Console.WriteLine("Player rotation:" + rotation);
            //Console.WriteLine("Radians:" + to_radians(rotation));
        }

        public void poll_input()
        {
            keyboard = Keyboard.GetState();

            //Check input across WASD and the Arrow Keys
            //Adjust velocity being added to the players position accordingly
            
            //Control rotation
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
            {
                rotation += 0.02f;
            }
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
            {
                rotation += -0.02f;
            }
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
            {
                Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                direction.Normalize();
                velocity += direction * 0.2f;
            }
        }

        public float to_radians(float angle)
        {
            if (angle == 360 || angle == -360)
            {
                angle = 0;
            }

            return (float)(angle * Math.PI / 180);
        }

        public void draw(SpriteBatch spriteBatch)
        {
			//Draw animation
			//spriteBatch.Draw(Constant.spritesheet, position, test_animation.source_rect, Color.White);
			//draw other
			spriteBatch.Draw(Constant.health_bar,healthDestRect, healthSourceRect, Color.White);
            for (int i = (ship_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.ship_tex, new Vector2(position.X, position.Y + i * ship_sep), new Rectangle((ship_frame_count - i) * ship_width, 0, ship_width, ship_height), Color.White, rotation + 180 + 0.6f, new Vector2((float)(ship_width / 2), (float)(ship_height / 2)), 1f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(Constant.ship_tex, new Vector2(position.X, position.Y + i * ship_sep), new Rectangle((ship_frame_count - i) * ship_width, 0, ship_width, ship_height), Color.White, to_radians(rotation) - to_radians(90), new Vector2((float)(ship_width / 2), (float)(ship_height / 2)), 1f, SpriteEffects.None, 0f);
            }
            //spriteBatch.Draw(Constant.bird, position, null, Color.White, rotation, new Vector2(Constant.bird.Width / 2, Constant.bird.Height / 2), 1f, SpriteEffects.None, 0f);
            //Other collision rect
            if (Constant.debug && player_debug)
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
