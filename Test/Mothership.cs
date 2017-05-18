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
    public class MotherShip
    {
        Player player;

        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 velocity;
        private int enemy_frame_count = 9;
        float shoot = 5;
        // Remove all enemies once mother shipped has arrived
        public bool motherShipArrived = false;
        public bool isVisible = true;
        public bool foundPlayer = false;
        public bool behindPlayer = false;
        public Rectangle boundingBox;
        public List<Bullets> player_bullets;
        public bool dead = false;

        public int motherHealth;
        public int motherXCollider = 1900;

        Random random = new Random();

        public float rotation;
        public Vector2 target_position;

        int randX, randY;

        //Bullets
        List<Bullets> bullets = new List<Bullets>();
        Texture2D bulletTexture;
        public static int enemy_width = 40, enemy_height = 40;
        private float enemy_sep = 1;


        public MotherShip(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture, Player newPlayer, Vector2 target_position)
        {

            texture = Constant.enemy_tex;
            position = newPosition;
            bulletTexture = newBulletTexture;
            player = newPlayer;
            randY = random.Next(-2, 2);
            //speed across the screen
            randX = -1;
            boundingBox = new Rectangle(motherXCollider, -100, 450, 200);
            ;
            player_bullets = player.get_bullets();
            motherHealth = 5;
            velocity = new Vector2(randX, 0);
            rotation = 0;
            this.target_position = target_position;
            Console.WriteLine("Mothership added");
        }

        public Vector2 get_position()
        {
            return position;
        }

        public bool isEnemyDead()
        {
            return dead;
        }


        public void UpdateBullets(GameTime gameTime)
        {
            foreach (Bullets bullet in bullets)
            {
                Vector2 bulletPos = new Vector2(bullet.position.X, bullet.position.Y);
                if (player.collision_circle.intersects_rectangle(bullet.boundingBox))
                {
                    //Player hit, play hit sound
                    Constant.shake = true;
                    bullet.isVisible = false;
                    player.playerHit();
                    Constant.damage_sound.Play();
                    GameOverseer.particle_manager.create_explosion(player.position, Constant.particle);
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
            Bullets newbullet = new Bullets(Constant.laser_tex, Color.White, rotation, 1f);
            newbullet.velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * -5f + velocity;
            newbullet.velocity = -newbullet.velocity;
            newbullet.position = position + newbullet.velocity * 5;
            newbullet.isVisible = true;
            if (bullets.Count < 500)
            {
                bullets.Add(newbullet);
            }
            Bullets newbullet1 = new Bullets(Constant.laser_tex, Color.White, rotation, 1f);
            newbullet1.velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * -3f + velocity;
            newbullet1.velocity = -newbullet1.velocity;
            newbullet1.position = position + newbullet1.velocity * 5;
            newbullet1.isVisible = true;
            if (bullets.Count < 500)
            {
                bullets.Add(newbullet1);
            }
            Bullets newbullet2 = new Bullets(Constant.laser_tex, Color.White, rotation, 1f);
            newbullet2.velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * -1f + velocity;
            newbullet2.velocity = -newbullet2.velocity;
            newbullet2.position = position + newbullet2.velocity * 5;
            newbullet2.isVisible = true;
            if (bullets.Count < 500)
            {
                bullets.Add(newbullet2);
            }
        }
        public void Update(GraphicsDevice graphics, GameTime gameTime)
        {
            boundingBox = new Rectangle((int)position.X - 250, (int)position.Y - 150, 450, 450);
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

            for (int i = 0; i < player_bullets.Count; i++)
            {
                if (boundingBox.Intersects(player_bullets[i].boundingBox))
                {
                    //Player hit something
                    Constant.explosion_sound.Play();
                    player_bullets[i].isVisible = false;

                    motherHealth--;

                    if (motherHealth <= 0)
                    {
                        dead = true;
                        isVisible = false;
                        GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);
                        GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);
                        GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);
                        GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);
                        GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);
                        GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);
                        GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);
                        GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);
                        GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);
                        GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);

                    }
                    GameOverseer.particle_manager.create_explosion(new Vector2(boundingBox.X + (boundingBox.Width / 2), boundingBox.Y + (boundingBox.Height / 2)), Constant.particle);
                }
            }

            shoot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shoot > 5)
            {
                shoot = 0;
                ShootBullets();

                //Close enough to player to hear laser sound
                if (Vector2.Distance(player.position, position) <= 400)
                {
                    Constant.laser_sound.Play();
                }
            }
            UpdateBullets(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            if (!dead)
            {
                for (int i = (enemy_frame_count - 1); i >= 0; i--)
                {
                    spriteBatch.Draw(Constant.enemy_tex, new Vector2(position.X, position.Y + i * enemy_sep), new Rectangle((enemy_frame_count - i) * enemy_width, 0, enemy_width,
                        enemy_height), Color.Purple, rotation + (float)Math.PI, new Vector2(enemy_width/2, enemy_height/2), 30f, SpriteEffects.None, 0f);
                }
                Renderer.FillRectangle(spriteBatch, new Vector2((float)boundingBox.X, (float)boundingBox.Y), boundingBox.Width, boundingBox.Height, Color.CornflowerBlue);
                Renderer.FillRectangle(spriteBatch, new Vector2((float)boundingBox.X, (float)boundingBox.Y), 5, 5, Color.Purple);
                foreach (Bullets bullet in bullets)
                {
                    bullet.Draw(spriteBatch);
                }
            }
            

        }


    }
}
