using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class EnemyManager
    {

        float spawn = 0;
        List<Enemy> enemies = new List<Enemy>();
        Random random = new Random();

        public void Update(GraphicsDeviceManager graphics, GameTime gameTime)
        {


            // TODO: Add your update logic here
            spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(graphics.GraphicsDevice);
            }
            LoadEnemies();
        }

        public void LoadEnemies()
        {
            int randY = random.Next(100, 400);
            if (spawn >= 1)
            {
                spawn = 0;
                if (enemies.Count < 1)
                {

                    enemies.Add(new Enemy(Constant.enemy_tex, new Vector2(1100, randY)));

                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].isVisible)
                {
                    enemies.RemoveAt(i);
                    i--;
                }

            }
        }
        public void Draw(SpriteBatch spritebatch)
        {
            foreach (Enemy enemy in enemies)
                enemy.Draw(spritebatch);
        }
    }
}
