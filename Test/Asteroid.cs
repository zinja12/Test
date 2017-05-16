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
        public static int asteroid_width = 40, asteroid_height = 40;
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
            origin = new Vector2(asteroid_width/2, asteroid_height/2);
            player = newPlayer;
            player_bullets = player.get_bullets();

        }

        public void update(GraphicsDevice graphics, GameTime gameTime)
        {
            // Set collision for asteroid
            boundingBox = new Rectangle((int)position.X - 20, (int)position.Y, asteroid_width, asteroid_height);
            player_bullets = player.get_bullets();


            // Update movement for asteroid
            position.Y = position.Y + speed;

            if (position.Y >= 950)
            {
                position.Y = -400;
                position.X = player.position.X + 200;
                destroyed = false;
            }

            foreach (Bullets bullet in player_bullets)
            {
                if (boundingBox.Intersects(bullet.boundingBox))
                {
                    destroyed = true;
                    //Play explosion sound
                    Constant.explosion_sound.Play();
                    GameOverseer.particle_manager.create_explosion(position, Constant.particle);
                }
            }

            //Collision for player hitbox
            if (player.collision_circle.intersects_rectangle(boundingBox))
            {
                destroyed = true;
                player.playerHit();
                Constant.damage_sound.Play();
                GameOverseer.particle_manager.create_explosion(position, Constant.particle);
                position.Y = -400;
                position.X = player.position.X + 200;
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
            //Renderer.FillRectangle(spriteBatch, new Vector2(boundingBox.X, boundingBox.Y), boundingBox.Width, boundingBox.Height, Color.CornflowerBlue);

        }

    }
}
