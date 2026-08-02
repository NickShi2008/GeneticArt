using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace GeneticArt
{
    public static class TriangleArtConstants
    {
        //chance either color or point is changed
        public static int ColorChangeChance = 50;
        public static int PointChangeChance = 50;

        //color change chances
        public static int RedChance = 27;
        public static int GreenChance = 27;
        public static int BlueChance = 27;
        public static int AlphaChance = 19;

        public static int ColorAddChance = 50;
        public static int ColorSubChance = 50;

        //point change chances
        public static int PointAddChance = 50;
        public static int PointSubChance = 50;

        public static int PointXChance = 50;
        public static int PointYChance = 50;


        public static int ColorChangeAmount = 40;
        public static float PointChangeAmount = 0.1f;


        //Alpha constraints
        public static int AlphaMin = 30;
        public static int AlphaMax = 185;

        //Point constraints
        public static float PointXMin = 0.2f;
        public static float PointXMax = 0.8f;
        public static float PointYMin = 0.2f;
        public static float PointYMax = 0.8f;

        //Triangle constraints
        public static int AddChance = 20;
        public static int RemoveChance = 10;
        public static int MutateChance = 70;
    }
}
