using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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


        public static readonly Vector2 gravity = new Vector2(0, 9.8f);

        public static int tile_size = 32;

        public static int[] test_block = { 2, 0 };
        public static int[] other_test_block = { 4, 0 };

        public static bool debug = true;

        public static bool shake = false;
    }
}
