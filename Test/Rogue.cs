using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test
{
    public class Rogue
    {

        Player player;
        public Vector2 position;
        private Vector2 start_position;
        private float rotation;
        public Circle collision_circle;

        public int health;
        public bool dead;

        public List<Bullets> player_bullets;

        int ship_sep = 1, ship_frame_count = 20, ship_width = 28, ship_height = 82;

        public Rogue(Vector2 position, Player player)
        {
            this.position = position;
            start_position = position;
            rotation = 0f;
            collision_circle = new Circle(position + new Vector2(ship_width/4, ship_height/4), 30);
            health = 2;
            dead = false;
            this.player = player;
        }

        public void update(GameTime gameTime, Vector2 target_position)
        {
            Vector2 distance;
            float dist = Vector2.Distance(position, target_position);
            if (dist > 5f && dist < 400 && !dead)
            {
                distance = target_position - position;
                distance.Normalize();
                position += distance * 3f;
                rotation = (float)Math.Atan2(target_position.Y - position.Y, target_position.X - position.X);
                //Keep collision circle updated with position
                collision_circle.center += distance * 3f;
            }

            if (collision_circle.intersects_circle(player.collision_circle) && !dead)
            {
                Constant.damage_sound.Play();
                player.playerHit();
                GameOverseer.particle_manager.create_explosion(player.collision_circle.center, Constant.particle);
                health--;
            }

            player_bullets = player.get_bullets();
            if (!dead) {
                for (int i = 0; i < player_bullets.Count; i++)
                {
                    if (collision_circle.contains_point(player_bullets[i].position))
                    {
                        //Player hit something
                        Constant.explosion_sound.Play();
                        GameOverseer.particle_manager.create_explosion(collision_circle.center, Constant.particle);
                        player_bullets.RemoveAt(i);
                        i--;
                        health--;
                    }
                }
            }

            if (health <= 0 && !dead)
            {
                dead = true;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            for (int i = (ship_frame_count - 1); i >= 0; i--)
            {
                spriteBatch.Draw(Constant.ship_tex, new Vector2(position.X, position.Y + i * ship_sep), new Rectangle((ship_frame_count - i) * ship_width, 0, ship_width, ship_height), Color.Red, rotation + 180 + 0.6f, new Vector2((float)(ship_width / 2), (float)(ship_height / 2)), 1f, SpriteEffects.None, 0f);
            }
            //Renderer.FillRectangle(spriteBatch, collision_circle.center, 5, 5, Color.CornflowerBlue);
            //Renderer.FillRectangle(spriteBatch, collision_circle.center + new Vector2(collision_circle.radius, 0), 5, 5, Color.CornflowerBlue);
            //Renderer.FillRectangle(spriteBatch, collision_circle.center + new Vector2(-collision_circle.radius, 0), 5, 5, Color.CornflowerBlue);
            //Renderer.FillRectangle(spriteBatch, collision_circle.center + new Vector2(0, collision_circle.radius), 5, 5, Color.CornflowerBlue);
            //Renderer.FillRectangle(spriteBatch, collision_circle.center + new Vector2(0, -collision_circle.radius), 5, 5, Color.CornflowerBlue);

        }
    }
}
