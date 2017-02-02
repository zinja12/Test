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
        //A constant class to reference textures from
        //Easier in my mind to keep all the textures in one place and reference them individually at the needed time
        //instead of have one texture in every class
        public static Texture2D pixel;
        public static Texture2D particle;
        public static Texture2D spritesheet;
        public static Texture2D card;

        public static int[] test_block = { 3, 3 };
        public static int[] other_test_block = { 0, 0 };

        public static bool debug = true;
    }
}
