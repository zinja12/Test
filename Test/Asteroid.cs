using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Test.Content
{
    public class Asteroid
    {

        public Texture2D texture;
        public Vector2 position, velocity, origin;
        public float rotationAngle;
        public int speed;
        public bool isColliding, destroyed;
        public Rectangle boundingBox;
        public static int asteroid_width = 40, asteroid_height = 73;
        private int asteroid_frame_count = 22;
        private float asteroid_sep = 1;
        Player player;
        public List<Bullets> player_bullets;

        public Asteroid(Texture2D newTexture, Player newPlayer)
        {
            this.position = new Vector2(newPlayer.position.X+ 150, -200);
            texture = newTexture;
            speed = 4;
            isColliding = false;
            destroyed = false;
            texture = newTexture;
            if (newTexture != null) {
                origin.X = (newTexture.Width / 22) / 2;
                origin.Y = newTexture.Height / 2;
            }
            player = newPlayer;
            player_bullets = player.get_bullets();

        }

        public void update(GraphicsDevice graphics, GameTime gameTime)
        {
            // Set collision for asteroid
            boundingBox = new Rectangle((int) position.X, (int) position.Y, asteroid_width, asteroid_height);
            player_bullets = player.get_bullets();


            // Update movement for asteroid
            position.Y = position.Y + speed;

            if (position.Y >= 950)
            {
                position.Y = -200;
                position.X = player.position.X + 150;
            }

            foreach (Bullets bullet in player_bullets)
            {
                if (boundingBox.Contains(bullet.position))
                {
                    destroyed = true;
                    //Play explosion sound
                    Constant.explosion_sound.Play();
                    GameOverseer.particle_manager.create_explosion(position, Constant.particle);
                }
            }

            //Rotate Asteroid
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            rotationAngle += elapsed;
            float circle = MathHelper.Pi * 2;
            rotationAngle = rotationAngle % circle;


        }
        public void draw(SpriteBatch spriteBatch)
        {

            if (!destroyed) {
                for (int i = (asteroid_frame_count - 1); i >= 0; i--)
                {
                    spriteBatch.Draw(Constant.asteroid, new Vector2(position.X, position.Y + i * asteroid_sep), new Rectangle((asteroid_frame_count - i) * asteroid_width, 0, asteroid_width,
                        asteroid_height), Color.White, rotationAngle, origin, 2f, SpriteEffects.None, 0f);
                }
            }
            

        }

    }
}
