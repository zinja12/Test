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

        //Game object
        GameOverseer game_overseer;
        
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
            game_overseer = new GameOverseer(0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height, Content, graphics.GraphicsDevice.Viewport);

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
            Constant.bird = Content.Load<Texture2D>("Sprites/bird.png");
            Constant.bullet_tex = Constant.particle;
            Constant.enemy_tex = Content.Load<Texture2D>("Sprites/enemyspaceship.png");
            Constant.ship_tex = Content.Load<Texture2D>("Sprites/ship.png");
            Constant.background = Content.Load<Texture2D>("Sprites/stars");
            Constant.symbol_spritesheet = Content.Load<Texture2D>("Sprites/symb_spritesheet.png");
            Constant.symbol_circle = Content.Load<Texture2D>("Sprites/sphere.png");
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
        /// 
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic heres
            game_overseer.update(gameTime, graphics.GraphicsDevice);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            game_overseer.draw(spriteBatch);
            

            base.Draw(gameTime);
        }

    }
}
