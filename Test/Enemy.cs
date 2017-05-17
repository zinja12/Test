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
        public bool dead = false;

        public Vector2 target_position;
        public float rotation;

        Random random = new Random();

        int randX, randY;

        //Bullets 
        List<Bullets> bullets = new List<Bullets>();
        Texture2D bulletTexture;
        public static int enemy_width = 40, enemy_height = 40;
        private float enemy_sep = 1;


        public Enemy(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture, Player newPlayer, Vector2 target_position) {

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
            this.target_position = target_position;
            rotation = 0f;
            Console.WriteLine("Added enemy at:" + position);
        }

        public Vector2 get_position()
        {
            return position;
        }

        public bool isEnemyDead()
        {
            return dead;
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
                if (player.collision_circle.intersects_rectangle(bullet.boundingBox))
                {
                    //Player hit, play hit sound
                    Constant.shake = true;
                    bullet.isVisible = false;
                    player.playerHit();
                    Constant.damage_sound.Play();
                    GameOverseer.particle_manager.create_explosion(player.position, Constant.particle);
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
            newbullet.velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * -5f + velocity;
            newbullet.velocity = -newbullet.velocity;
            newbullet.position = position + newbullet.velocity * 5;
            newbullet.isVisible = true;
            if (bullets.Count < 20)
            {
                bullets.Add(newbullet);
            }
        }
        public void Update(GraphicsDevice graphics, GameTime gameTime)
        {
            /*boundingBox = new Rectangle((int)position.X + 15, (int)position.Y + 15, enemy_width, enemy_height);
            player_bullets = player.get_bullets();
            float distance = Vector2.Distance(player.position, this.position);
            if (distance <= 200)
            {
                foundPlayer = true;
                

            }

            foreach (Bullets bullet in player_bullets)
            {
                if (boundingBox.Intersects(bullet.boundingBox))
                {
                    //Player hit something
                    isVisible = false;
                    Constant.explosion_sound.Play();
                    dead = true;
                    GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (enemy_width / 2), boundingBox.Y + (enemy_height / 2)), Constant.particle);
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

                //Close enough to player to hear laser sound
                if (Vector2.Distance(player.position, position) <= 200)
                {
                    Constant.laser_sound.Play();
                }
            }
            UpdateBullets();*/
            update_enemy(gameTime);
        }

        public void update_enemy(GameTime gameTime)
        {
            boundingBox = new Rectangle((int)position.X + 15, (int)position.Y + 15, enemy_width, enemy_height);
            player_bullets = player.get_bullets();

            Vector2 distance;
            float dist = Vector2.Distance(position, target_position);
            if (dist > 200)
            {
                distance = target_position - position;
                distance.Normalize();
                position += distance * 1.5f;
                rotation = (float)Math.Atan2(target_position.Y - position.Y, target_position.X - position.X);
                //Keep collision circle updated with position
            }

            foreach (Bullets bullet in player_bullets)
            {
                if (boundingBox.Intersects(bullet.boundingBox))
                {
                    //Player hit something
                    isVisible = false;
                    Constant.explosion_sound.Play();
                    dead = true;
                    GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (enemy_width / 2), boundingBox.Y + (enemy_height / 2)), Constant.particle);
                }
            }

            shoot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shoot > 1)
            {
                shoot = 0;
                ShootBullets();

                //Close enough to player to hear laser sound
                if (Vector2.Distance(player.position, position) <= 200)
                {
                    Constant.laser_sound.Play();
                }
            }
            UpdateBullets();

        }

        public void Draw(SpriteBatch spriteBatch) {

            
            for (int i = (enemy_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.enemy_tex, new Vector2(position.X, position.Y + i * enemy_sep), new Rectangle((enemy_frame_count - i) * enemy_width, 0, enemy_width, 
                    enemy_height), Color.White, rotation + (float)(Math.PI), new Vector2(enemy_width/2,enemy_height/2), 2f, SpriteEffects.None, 0f);
            }
            //Renderer.FillRectangle(spriteBatch, new Vector2((float)boundingBox.X, (float)boundingBox.Y), boundingBox.Width, boundingBox.Height, Color.CornflowerBlue);
            foreach (Bullets bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }

        }


    }
}
