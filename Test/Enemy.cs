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

        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 velocity;

        public bool isVisible = true;

        Random random = new Random();

        int randX, randY;

        //Bullets 
        List<Bullets> bullets = new List<Bullets>();
        Texture2D bulletTexture;

        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture) {

            texture = newTexture;
            position = newPosition;
            bulletTexture = newBulletTexture;

            randY = random.Next(-4, 4);
            //speed across the screen
            randX = random.Next(-4, 1);

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
                bullet.position += bullet.velocity;
                if (bullet.position.X < 0)
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
            newbullet.position = new Vector2(position.X + newbullet.velocity.X, position.Y + (texture.Height/2) - bulletTexture.Height/2);

            newbullet.isVisible = true;
            if (bullets.Count <3)
            {
                bullets.Add(newbullet);
            }
        }

        float shoot = 0;

        public void Update(GraphicsDevice graphics, GameTime gameTime)
        {
            /*
            position += velocity;
            if (position.Y <= 0 || position.Y >= graphics.Viewport.Height - texture.Height)
            {
                velocity.Y = -velocity.Y;
            }
            
            if (position.X < 0 - texture.Width)
            {
                isVisible = false;
            }
            */
            shoot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shoot > 1)
            {
                shoot = 0;
                ShootBullets();
            }
            UpdateBullets();
        }
        public void Draw(SpriteBatch spriteBatch) {

            foreach (Bullets bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
            spriteBatch.Draw(texture, position, Color.White);    

        }


    }
}
