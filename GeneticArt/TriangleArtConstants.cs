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
        public static int RedChance = 25;
        public static int GreenChance = 25;
        public static int BlueChance = 25;
        public static int AlphaChance = 25;

        public static int ColorAddChance = 50;
        public static int ColorSubChance = 50;

        //point change chances
        public static int PointAddChance = 50;
        public static int PointSubChance = 50;

        public static int PointXChance = 50;
        public static int PointYChance = 50;


        public static int ColorChangeAmount = 10;
        public static int PointChangeAmount = 10;


        //Alpha constraints
        public static int AlphaMin = 24;
        public static int AlphaMax = 232;

        //Point constraints
        public static int PointXMin = 0;
        public static int PointXMax = 1;
        public static int PointYMin = 0;
        public static int PointYMax = 1;

        //Triangle constraints
        public static int AddChance = 25;
        public static int RemoveChance = 25;
        public static int MutateChance = 50;
    }
}
