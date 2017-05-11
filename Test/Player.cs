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

        public Circle collision_circle;
        private float collision_circle_radius = 35;
        
        private int ship_frame_count = 20;
        int healthFrames = 0;
        private float ship_sep = 1;

        public bool grounded;
        List<Bullets> bullets = new List<Bullets>();
        public float friction = 0.03f;
        public float player_rotation_speed = 0.03f;
        public static int width = 32, height = 32;
        public static int ship_width = 28, ship_height = 82;

        public bool player_debug = false;
        public bool isDestroyed = false;


        //private Animation test_animation;

        //Constructor
        public Player(Vector2 position)
        {
            //Initialize animation
            //test_animation = new Animation(100.0f, 4-1, 0, 0, width, height);

            this.position = position;
            base_position = new Vector2(position.X, position.Y + (height / 2));
            left_side_pt = new Vector2(position.X + (width / 2), position.Y);
            right_side_pt = new Vector2(position.X - (width / 2), position.Y);
            top_side_pt = new Vector2(position.X, position.Y - (height / 2));
            velocity = Vector2.Zero;
            rotation = 0f;
            other_collision_rect = new Rectangle((int)this.position.X, (int)this.position.Y, width, height);
            healthDestRect = new Rectangle(-400, -200, 158, 152);
            healthSourceRect = new Rectangle(0, 0, 157, 152);

            //Set up collision geometry
            collision_circle = new Circle(position, collision_circle_radius);

        }

        //Getters
        public Vector2 get_base_position()
        {
            return base_position;
        }

        public List<Bullets> get_bullets()
        {
            return bullets;
        }

        public Rectangle get_collision_rect()
        {
            return other_collision_rect;
        }

        public float get_rotation()
        {
            return rotation;
        }

        public void playerHit()
        {
            if (healthFrames < 4) {
                healthFrames++;
                healthSourceRect = new Rectangle(157 * healthFrames, 0, 157, 152);
            }
            else if (healthFrames >=4) {
                isDestroyed = true;
            }
        }

        public void update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Handle polling input and velocity
            poll_input(gameTime);

            //Update points and collision rectangle
            position += velocity;

            base_position += velocity;
            left_side_pt += velocity;
            right_side_pt += velocity;
            top_side_pt += velocity;
            other_collision_rect.X = (int)(position.X - (width / 2));
            other_collision_rect.Y = (int)(position.Y - (height / 2));
            //Update circle position
            collision_circle.center = position;

            //Handle friction
            float i = velocity.X;
            float j = velocity.Y;
            velocity.X = i -= friction * i;
            velocity.Y = j -= friction * j;

            //Console.WriteLine("Player rotation:" + rotation);
            //Console.WriteLine("Radians:" + to_radians(rotation));
        }
        float elapsedTime = 0;
        public void poll_input(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();

            //Check input across WASD and the Arrow Keys
            //Adjust velocity being added to the players position accordingly

            //Control rotation
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
            {
                rotation += player_rotation_speed;
            }
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
            {
                rotation -= player_rotation_speed;
            }
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
            {
                Vector2 direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
                direction.Normalize();
                velocity += direction * 0.2f;
            }
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboard.IsKeyDown(Keys.Space))
            {
                if (elapsedTime > 0.5)
                {
                    ShootBullets();
                    elapsedTime = 0;
                }
                Constant.shake = true;
                Starfield.shake_stars = true;

            }
            UpdateBullets();

        }

        public float to_radians(float angle)
        {
            if (angle == 360 || angle == -360)
            {
                angle = 0;
            }

            return (float)(angle * Math.PI / 180);
        }

        public void UpdateBullets()
        {
            foreach (Bullets bullet in bullets)
            {
                bullet.position += bullet.velocity;
                if (Vector2.Distance(bullet.position, position) > 500)
                {
                    bullet.isVisible = false;
                }
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public void ShootBullets()
        {
            Bullets newbullet = new Bullets(Constant.laser_tex, Color.White, rotation, 1f);
            newbullet.velocity = new Vector2((float)Math.Cos(rotation), (float) Math.Sin(rotation)) * 5f +velocity;
            newbullet.position = position + newbullet.velocity *5;
            newbullet.isVisible = true;
            if (bullets.Count < 20)
            {
                bullets.Add(newbullet);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (!isDestroyed)
            {
                //Draw animation
                //spriteBatch.Draw(Constant.spritesheet, position, test_animation.source_rect, Color.White);
                //draw other
                for (int i = (ship_frame_count - 1); i >= 0; i--)
                {
                    spriteBatch.Draw(Constant.ship_tex, new Vector2(position.X, position.Y + i * ship_sep), new Rectangle((ship_frame_count - i) * ship_width, 0, ship_width, ship_height), Color.White, rotation + 180 + 0.6f, new Vector2((float)(ship_width / 2), (float)(ship_height / 2)), 1f, SpriteEffects.None, 0f);
                    //spriteBatch.Draw(Constant.ship_tex, new Vector2(position.X, position.Y + i * ship_sep), new Rectangle((ship_frame_count - i) * ship_width, 0, ship_width, ship_height), Color.White, to_radians(rotation) - to_radians(90), new Vector2((float)(ship_width / 2), (float)(ship_height / 2)), 1f, SpriteEffects.None, 0f);
                }
                foreach (Bullets bullet in bullets)
                {
                    bullet.Draw(spriteBatch);
                }
                //spriteBatch.Draw(Constant.bird, position, null, Color.White, rotation, new Vector2(Constant.bird.Width / 2, Constant.bird.Height / 2), 1f, SpriteEffects.None, 0f);
                //Other collision rect
                //Renderer.FillRectangle(spriteBatch, collision_circle.center, 5, 5, Color.Purple);
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
}
