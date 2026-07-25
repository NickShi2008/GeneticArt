using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace GeneticArt
{
    public class Triangle
    {
        Color color;
        PointF[] points;   

        public Triangle(PointF point0, PointF point1, PointF point2, Color color)
        {
            this.color = color;
            points = new PointF[3];
            points[0] = point0;
            points[1] = point1;
            points[2] = point2;
        }

        public Triangle(PointF[] points, Color color)
        {
            this.color = color;
            points = new PointF[3];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new PointF(points[i].X, points[i].Y);
            }
        }

        public void DrawTriangle(Graphics gfx, float xCoef, float yCoef)
        {
            PointF[] scaledPoints = new PointF[3];
            for(int i = 0; i < 3; i++)
            {
                scaledPoints[i] = new PointF(points[i].X * xCoef, points[i].Y * yCoef);
            }
            gfx.FillPolygon(new SolidBrush(color), scaledPoints);
        }

        public void Mutate(Random random)
        {
            int PointOrColor = random.Next(TriangleArtConstants.PointChangeChance + TriangleArtConstants.ColorChangeChance);

            if (PointOrColor < TriangleArtConstants.PointChangeChance)
            {
                int XorY = random.Next(TriangleArtConstants.PointXChance + TriangleArtConstants.PointYChance);
                int AddOrSub = random.Next(TriangleArtConstants.PointAddChance + TriangleArtConstants.PointSubChance);

                bool canAdd = AddOrSub < TriangleArtConstants.PointAddChance;
                float pointChangeAmount = (float) (random.NextDouble() * TriangleArtConstants.PointChangeAmount + 1);
                if (XorY < TriangleArtConstants.PointXChance)
                {
                    for(int i =0; i < points.Length; i++)
                    {
                        if(canAdd)
                            points[i].X += pointChangeAmount;
                        else
                            points[i].X -= pointChangeAmount;
                    }
                }
                else
                {
                    for (int i = 0; i < points.Length; i++)
                    {
                        if (canAdd)
                            points[i].Y += pointChangeAmount;
                        else
                            points[i].Y -= pointChangeAmount;
                    }
                }
            }
            else
            {
                int RGBorA = random.Next(TriangleArtConstants.RedChance + TriangleArtConstants.GreenChance 
                    + TriangleArtConstants.BlueChance + TriangleArtConstants.AlphaChance);
                int AddOrSub = random.Next(TriangleArtConstants.ColorAddChance + TriangleArtConstants.ColorSubChance);
                Color newColor;

                bool canAdd = AddOrSub < TriangleArtConstants.ColorAddChance;
                int changeAmount = random.Next(TriangleArtConstants.ColorChangeAmount) + 1;
                if (RGBorA < TriangleArtConstants.RedChance)
                {
                    int r = color.R;
                    if (canAdd)
                        r += changeAmount;
                    else
                        r -= changeAmount;

                    newColor = Color.FromArgb(color.A, r, color.G, color.B);
                }
                else if(RGBorA < TriangleArtConstants.RedChance + TriangleArtConstants.GreenChance)
                {
                    int g = color.G;
                    if (canAdd)
                        g += changeAmount;
                    else
                        g -= changeAmount;

                    newColor = Color.FromArgb(color.A, color.R, g, color.B);
                }
                else if (RGBorA < TriangleArtConstants.RedChance + TriangleArtConstants.GreenChance + TriangleArtConstants.BlueChance)
                {
                    int b = color.B;
                    if (canAdd)
                        b += changeAmount;
                    else
                        b -= changeAmount;

                    newColor = Color.FromArgb(color.A, color.R, color.G, b);
                }
                else
                {
                    int a = color.A;
                    if (canAdd)
                        a += changeAmount;
                    else
                        a -= changeAmount;

                    newColor = Color.FromArgb(a, color.R, color.G, color.B);
                }
            }
        }

        public Triangle Copy()
        {
            PointF[] newPoints = new PointF[3];
            for (int i = 0; i < newPoints.Length; i++)
            {
                newPoints[i] = points[i];
            }

            Color newColor = color;
            return new Triangle(newPoints[0], newPoints[1], newPoints[2], newColor);
        }

        public static Triangle RandomTriangle(Random random)
        {
            int a = random.Next(TriangleArtConstants.AlphaMin, TriangleArtConstants.AlphaMax);
            Color randomColor = Color.FromArgb(a, random.Next(256), random.Next(256), random.Next(256));

            PointF[] randomPoints = new PointF[3];
            int triangleTranslationX = random.Next(1);
            int triangleTranslationY = random.Next(1);
            for(int i = 0; i < 3; i++)
            {
                randomPoints[i].X = random.Next(TriangleArtConstants.PointXMin + triangleTranslationX,
                    TriangleArtConstants.PointXMax + triangleTranslationX);
                randomPoints[i].Y = random.Next(TriangleArtConstants.PointYMin + triangleTranslationY, 
                    TriangleArtConstants.PointYMax + triangleTranslationY);
            }

            return new Triangle(randomPoints, randomColor);
        }
    }
}
