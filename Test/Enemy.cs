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
    public class Enemy
    {
        Player player;

        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 velocity;
        private int enemy_frame_count = 9;
        float shoot = 0;

        public bool isVisible = true;

        Random random = new Random();

        int randX, randY;

        //Bullets 
        List<Bullets> bullets = new List<Bullets>();
        Texture2D bulletTexture;
        public static int enemy_width = 40, enemy_height = 178;
        private float enemy_sep = 1;


        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture, Player newPlayer) {

            texture = newTexture;
            position = newPosition;
            bulletTexture = newBulletTexture;
            player = newPlayer;

            randY = random.Next(-4, 4);
            //speed across the screen
            randX = random.Next(-3, -1);

            velocity = new Vector2(randX, randY);
        }

        public Vector2 get_position()
        {
            return position;
        }

        public void UpdateBullets()
        {
            foreach (Bullets bullet in bullets)
            {
                int dx = Math.Max((int)Math.Abs(bullet.position.X - player.other_collision_rect.Center.X) -(player.other_collision_rect.Width) / 2, 0);
                int dy = Math.Max((int)Math.Abs(bullet.position.Y - player.other_collision_rect.Center.Y) - (player.other_collision_rect.Height) / 2, 0);

                if ((dx * dx + dy * dy) == 0) {
                    System.Console.WriteLine("Hello, World!");
                    Constant.shake = true;
                }
                bullet.position += bullet.velocity;
                if (bullet.position.X < -300)
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
            Bullets newbullet = new Bullets(bulletTexture);
            newbullet.velocity.X = velocity.X - 3f;
            newbullet.position = new Vector2(position.X + newbullet.velocity.X +30, position.Y + 
                (texture.Height/2) - (bulletTexture.Height /2) +20 );
            newbullet.isVisible = true;
            if (bullets.Count < 3)
            {
                bullets.Add(newbullet);
            }
        }
        public void Update(GraphicsDevice graphics, GameTime gameTime)
        {
            
            position += velocity;
            if (position.Y <= -225 || position.Y >= 350)
            {
                velocity.Y = -velocity.Y;
            }
            
            if (position.X < 0 - texture.Width)
            {
                isVisible = false;
            }
            
            shoot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shoot > 1)
            {
                shoot = 0;
                ShootBullets();
            }
            UpdateBullets();
        }
        public void Draw(SpriteBatch spriteBatch) {

            
            for (int i = (enemy_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.enemy_tex, new Vector2(position.X, position.Y + i * enemy_sep), new Rectangle((enemy_frame_count - i) * enemy_width, 0, enemy_width, 
                    enemy_height), Color.White, 0, new Vector2(0,0), 2f, SpriteEffects.None, 0f);
            }
            foreach (Bullets bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }

        }


    }
}
