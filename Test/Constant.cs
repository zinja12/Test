﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Test
{
    public class Constant
    {
        //A constant class to reference textures and non changing variables from
        //Easier in my mind to keep all the variables in one place and reference them individually at the needed time
        //instead of have one texture in every class
        public static Texture2D pixel;
        public static Texture2D particle;
        public static Texture2D spritesheet;
        public static Texture2D card;
        public static Texture2D bird;
        public static Texture2D enemy_tex;
        public static Texture2D bullet_tex;
        public static Texture2D ship_tex;
        public static Texture2D background;
        public static Texture2D symbol_spritesheet;
        public static Texture2D symbol_circle;
        public static Texture2D fire;
        public static Texture2D health_bar;
        public static Texture2D planet_tex;
        public static Texture2D laser_tex;
        public static Texture2D asteroid;
        public static Texture2D pause_tex;
        public static Texture2D start_button;
        public static Texture2D controls_button;



        public static SoundEffect laser_sound;
        public static SoundEffect explosion_sound;
        public static SoundEffect damage_sound;
        public static Song background_music;
        public static bool background_music_started = false;
        public static SpriteFont score_font;




        public static readonly Vector2 gravity = new Vector2(0, 9.8f);

        public static int tile_size = 32;

        public static int[] test_block = { 2, 0 };
        public static int[] other_test_block = { 4, 0 };

        public static bool debug = true;

        public static bool shake = false;

        public static bool paused = false;
        public static bool pause_key_down = false;

        public static void start_pause() { paused = true; }
        public static void end_pause() { paused = false; }
        public static void checkPauseKey(KeyboardState keyboardState, GamePadState gamePadState)
        {
            bool pauseKeyDownThisFrame = (keyboardState.IsKeyDown(Keys.P) ||
                (gamePadState.Buttons.Start == ButtonState.Pressed));
            // If key was not down before, but is down now, we toggle the
            // pause setting
            if (!pause_key_down && pauseKeyDownThisFrame)
            {
                if (!paused)
                    start_pause();
                else
                    end_pause();
            }
            pause_key_down = pauseKeyDownThisFrame;
        }

        public static float angle_between_vectors(Vector2 a, Vector2 b)
        {
            return (float)Math.Atan2(b.Y - a.Y, b.X - a.X);
        }
    }
}
