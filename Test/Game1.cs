﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int planet_frame_count = 40;
        int planet_width = 40, planet_height = 40;
        float rotation = 0;
        enum GameState
        {
            MainMenu,
            Playing,
        }

        GameState current_game_state = GameState.MainMenu;
        Button btnStart;
        float planet_sep = 1f;

        //Game object
        GameOverseer game_overseer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            if (!Constant.debug)
            {
                graphics.IsFullScreen = true;
            }
            else
            {
                graphics.PreferredBackBufferWidth = 1000;
                graphics.PreferredBackBufferHeight = 600;
            }
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            btnStart = new Button(Constant.start_button, graphics.GraphicsDevice);
            btnStart.setPosition(new Vector2(300, 150));
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
            Constant.health_bar = Content.Load<Texture2D>("Sprites/health.png");
            Constant.planet_tex = Content.Load<Texture2D>("Sprites/planet_sheet.png");
            Constant.laser_tex = Content.Load<Texture2D>("Sprites/laser.png");
            Constant.asteroid = Content.Load<Texture2D>("Sprites/asteroid.png");
            Constant.pause_tex = Content.Load<Texture2D>("Sprites/pause_icon.png");
            Constant.start_button = Content.Load<Texture2D>("Sprites/startbutton.png");


            Constant.laser_sound = Content.Load<SoundEffect>("Laser_Shoot2");
            Constant.explosion_sound = Content.Load<SoundEffect>("Explosion2");
            Constant.damage_sound = Content.Load<SoundEffect>("Hit_Hurt");
            Constant.background_music = Content.Load<Song>("song_of_earth");
            Constant.score_font = Content.Load<SpriteFont>("Score");



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
            MouseState mouse = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic heres
            switch (current_game_state)
            {
                case GameState.MainMenu:
                    if (btnStart.isClicked || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
                    {
                        current_game_state = GameState.Playing;
                    }
                    btnStart.Update(mouse);
                    rotation += 0.01f;
                    if (rotation * (180 / Math.PI) >= 360)
                    {
                        rotation = 0;
                    }
                    break;
                case GameState.Playing:
                    Constant.checkPauseKey(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One));
                    if (!Constant.paused)
                    {
                        game_overseer.update(gameTime, graphics.GraphicsDevice, spriteBatch);
                    }
                    else
                    {
                        GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
                    }

                    if (!Constant.background_music_started)
                    {
                        Constant.background_music_started = true;
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(Constant.background_music);
                    }
                    break;

            }
           
           
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            spriteBatch.Begin();

            switch (current_game_state)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Constant.background, new Rectangle(0, 0, 1000, 600), Color.White);
                    btnStart.Draw(spriteBatch);
                    for (int i = (planet_frame_count - 1); i >= 0; i--)
                    {
                        spriteBatch.Draw(Constant.planet_tex, new Vector2(200, 400 + i * planet_sep), new Rectangle((planet_frame_count - i) * planet_width, 0, planet_width, planet_height), Color.DeepSkyBlue, rotation + 180 + 0.6f, new Vector2((float)(planet_width / 2), (float)(planet_height / 2)), 4f, SpriteEffects.None, 0f);
                    }
                    for (int i = (planet_frame_count - 1); i >= 0; i--)
                    {
                        spriteBatch.Draw(Constant.planet_tex, new Vector2(800, 100 + i * planet_sep), new Rectangle((planet_frame_count - i) * planet_width, 0, planet_width, planet_height), Color.Yellow, rotation + 180 + 0.6f, new Vector2((float)(planet_width / 2), (float)(planet_height / 2)), 4f, SpriteEffects.None, 0f);
                    }
                    break;


                case GameState.Playing:
                    game_overseer.draw(spriteBatch);

                    break;

            }
            spriteBatch.End();


            if (Constant.paused)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Constant.pause_tex, new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 400 / 2, graphics.GraphicsDevice.Viewport.Height / 2 - 400 / 2), null, Color.White, 0f, Vector2.Zero, 2, SpriteEffects.None, 0f);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

    }
}