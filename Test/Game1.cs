using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameOverseer game_overseer;

        List<Enemy> enemies = new List<Enemy>();
        Random random = new Random();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            if (!Constant.debug)
            {
                graphics.IsFullScreen = true;
            } else
            {
                graphics.PreferredBackBufferWidth = 1000;
                graphics.PreferredBackBufferHeight = 600;
            }
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            game_overseer = new GameOverseer(0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Constant.pixel = Content.Load<Texture2D>("Sprites/pixel.png");
            Constant.spritesheet = Content.Load<Texture2D>("Sprites/test_spritesheet.png");
            Constant.particle = Content.Load<Texture2D>("Sprites/particle.png");
            Constant.card = Content.Load<Texture2D>("Sprites/tmp_card.png");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        float spawn = 0;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Enemy enemy in enemies)
                enemy.Update(graphics.GraphicsDevice, gameTime);

            // TODO: Add your update logic here
            game_overseer.update(gameTime, graphics.GraphicsDevice);
            LoadEnemies();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public void LoadEnemies() {

            int randY = random.Next(100, 400);
            // enemy spawns every second
            if (spawn >= 1)
            {
                spawn = 0;
                // number of enemies
                if (enemies.Count < 1)
                {
                    enemies.Add(new Enemy(Content.Load<Texture2D>("Sprites/particle.png"), new Vector2(1100, randY), Content.Load<Texture2D>("Sprites/particle.png")));
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();
            game_overseer.draw(spriteBatch);
            foreach (Enemy enemy in enemies)
                enemy.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
