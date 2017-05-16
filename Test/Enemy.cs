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
        public bool foundPlayer = false;
        public bool behindPlayer = false;
        public Rectangle boundingBox;
        public List<Bullets> player_bullets;

        Random random = new Random();

        int randX, randY;

        //Bullets 
        List<Bullets> bullets = new List<Bullets>();
        Texture2D bulletTexture;
        public static int enemy_width = 40, enemy_height = 40;
        private float enemy_sep = 1;


        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture, Player newPlayer) {

            texture = newTexture;
            position = newPosition;
            bulletTexture = newBulletTexture;
            player = newPlayer;
            randY = random.Next(-2, 2);
            //speed across the screen
            randX = -1;
            boundingBox = new Rectangle((int)position.X + 15, (int)position.Y + 15, enemy_width, enemy_height);
            player_bullets = player.get_bullets();
            velocity = new Vector2(randX, randY);
        }

        public Vector2 get_position()
        {
            return position;
        }
        public void UpdateBullets(GameTime gameTime)
        {
            foreach (Bullets bullet in bullets)
            {
                Vector2 bulletPos = new Vector2(bullet.position.X, bullet.position.Y);
                if (player.collision_circle.contains_point(bullet.position))
                {
                    //Player hit, play hit sound
                    Constant.shake = true;
                    bullet.isVisible = false;
                    player.playerHit();
                    Constant.damage_sound.Play();
                }
                bullet.position += bullet.velocity;
                if (bullet.position.X < -500)
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
            Bullets newbullet = new Bullets(bulletTexture, Color.Red, 0f, 4f);
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
            boundingBox = new Rectangle((int)position.X + 15, (int)position.Y + 15, enemy_width, enemy_height);
            player_bullets = player.get_bullets();
            float distance = Vector2.Distance(player.position, this.position);
            if (distance <= 200)
            {
                foundPlayer = true;
                

            }

            foreach (Bullets bullet in player_bullets)
            {
                if (boundingBox.Contains(bullet.position))
                {
                    //Player hit something
                    isVisible = false;
                    Constant.explosion_sound.Play();
                }
            }

            if (foundPlayer && !behindPlayer)
            {
                position += velocity;

                if (position.Y == player.top_side_pt.Y-5)
                {
                    velocity.Y = 0;
                } else if (position.Y > player.top_side_pt.Y-5) {
                    velocity.Y = -2;

                }
                else if (position.Y < player.top_side_pt.Y-5)
                {
                    velocity.Y = 2;

                }



                if (position.Equals(player.position)) { isVisible = false; }
                if (position.X < player.left_side_pt.X-10) { behindPlayer = true; }

            }
            else {
                position += velocity;
                if (position.Y <= -225 || position.Y >= 350)
                {
                    velocity.Y = -velocity.Y;
                }
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
            UpdateBullets(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch) {

            
            for (int i = (enemy_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.enemy_tex, new Vector2(position.X, position.Y + i * enemy_sep), new Rectangle((enemy_frame_count - i) * enemy_width, 0, enemy_width, 
                    enemy_height), Color.White, 0, new Vector2(0,0), 2f, SpriteEffects.None, 0f);
            }
            //Renderer.FillRectangle(spriteBatch, new Vector2((float)boundingBox.X, (float)boundingBox.Y), boundingBox.Width, boundingBox.Height, Color.CornflowerBlue);
            foreach (Bullets bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }

        }


    }
}
