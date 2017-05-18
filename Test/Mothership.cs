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

        int randX, randY;

        //Bullets
        List<Bullets> bullets = new List<Bullets>();
        Texture2D bulletTexture;
        public static int enemy_width = 40, enemy_height = 40;
        private float enemy_sep = 1;


        public MotherShip(Texture2D newTexture, Vector2 newPosition, Texture2D newBulletTexture, Player newPlayer)
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
            bulletTexture = Constant.particle;
            texture = Constant.enemy_tex;

            Bullets bullet1 = new Bullets(bulletTexture, Color.Lavender, 0f, 4f);
            Bullets bullet2 = new Bullets(bulletTexture, Color.Lavender, 0f, 4f);
            Bullets bullet3 = new Bullets(bulletTexture, Color.Lavender, 0f, 4f);


            bullet1.velocity.X = velocity.X - 2f;
            bullet1.position = new Vector2((position.X + bullet1.velocity.X + 30) *3 + 100, position.Y + 500);
            bullet1.isVisible = true;

            bullet2.velocity.X = velocity.X - 2f;
            bullet2.velocity.Y = -2f;
            bullet2.position = new Vector2((position.X + bullet2.velocity.X + 30) *3 +100, position.Y + 500);
            bullet2.isVisible = true;

            bullet3.velocity.X = velocity.X - 2f;
            bullet3.velocity.Y = 2f;
            bullet3.position = new Vector2((position.X + bullet3.velocity.X + 30)*3 +100, position.Y+500);
            bullet3.isVisible = true;

            bullets.Add(bullet1);
            bullets.Add(bullet2);
            bullets.Add(bullet3);

        }
        public void Update(GraphicsDevice graphics, GameTime gameTime)
        {
            if (position.X > 200)
            {

                motherXCollider += (int)velocity.X;


            }
            boundingBox = new Rectangle(motherXCollider, -100, 450, 200);
            player_bullets = player.get_bullets();

            foreach (Bullets bullet in player_bullets)
            {
                if (boundingBox.Intersects(bullet.boundingBox))
                {
                    //Player hit something
                    Constant.explosion_sound.Play();
                    bullet.isVisible = false;

                    if (motherShipArrived)
                    {
                        motherHealth--;
                    }
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

            if (position.X > 200)
            {

                position += velocity;

            }
            else
            {
                motherShipArrived = true;
                shoot += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (shoot > 5)
                {
                    shoot = 0;
                    ShootBullets();

                    //Close enough to player to hear laser sound
                    if (Vector2.Distance(player.position, position) <= 200)
                    {
                        Constant.laser_sound.Play();
                    }
                }
                UpdateBullets(gameTime);

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            if (!dead)
            {
                for (int i = (enemy_frame_count - 1); i >= 0; i--)
                {
                    spriteBatch.Draw(Constant.enemy_tex, new Vector2(position.X, position.Y + i * enemy_sep), new Rectangle((enemy_frame_count - i) * enemy_width, 0, enemy_width,
                        enemy_height), Color.Purple, 0, new Vector2(0, 0), 30f, SpriteEffects.None, 0f);
                }
                //Renderer.FillRectangle(spriteBatch, new Vector2((float)boundingBox.X, (float)boundingBox.Y), boundingBox.Width, boundingBox.Height, Color.CornflowerBlue);
                foreach (Bullets bullet in bullets)
                {
                    bullet.Draw(spriteBatch);
                }
            }
            

        }


    }
}
