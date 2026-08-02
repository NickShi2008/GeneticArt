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
 
        public PointF[] extremas;

        public Rectangle BoundingBox;

        
        

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
            this.points = new PointF[3];
            for (int i = 0; i < points.Length; i++)
            {
                this.points[i] = new PointF(points[i].X, points[i].Y);
            }
        }

        public Triangle(PointF[] points, Color color, Rectangle rect, PointF[] extremas)
        {
            this.color = color;
            this.points = new PointF[3];
            for (int i = 0; i < points.Length; i++)
            {
                this.points[i] = new PointF(points[i].X, points[i].Y);
            }
            BoundingBox = rect;
            this.extremas = extremas;
        }

        public void DrawTriangle(Graphics gfx, float xCoef, float yCoef)
        {
            PointF[] scaledPoints = new PointF[3];
            for(int i = 0; i < 3; i++)
            {
                scaledPoints[i] = new PointF(points[i].X * xCoef, points[i].Y * yCoef);
            }
            using var brush = new SolidBrush(color);
            gfx.FillPolygon(brush, scaledPoints);
        }

        //maybe figure out faster fill polygon if possible later?
        public void DrawTriangleSpecial(Graphics gfx, float xCoef, float yCoef)
        {
            PointF[] scaledPoints = new PointF[3];
            for (int i = 0; i < 3; i++)
            {
                scaledPoints[i] = new PointF(points[i].X * xCoef, points[i].Y * yCoef);
            }
            using var brush = new SolidBrush(color);
            //for(int x = (int) extremas[0].X; y < (int) extremas[1].X; x++)
            //{
            //    for (int y = (int)extremas[0].Y; y < (int)extremas[1].Y; y++)
            //    {
            //        if (BoundingBox.Contains(x, y))
            //        {
                        
            //        }
            //    }
            //}
        }

        public void Mutate(Random random)
        {
            int PointOrColor = random.Next(TriangleArtConstants.PointChangeChance + TriangleArtConstants.ColorChangeChance);

            if (PointOrColor < TriangleArtConstants.PointChangeChance)
            {
                int XorY = random.Next(TriangleArtConstants.PointXChance + TriangleArtConstants.PointYChance);
                float pointChangeAmount = (float) (random.NextDouble() * TriangleArtConstants.PointChangeAmount * 2 - TriangleArtConstants.PointChangeAmount);

                int point = random.Next(3);
                if (XorY < TriangleArtConstants.PointXChance)
                {

                    points[point].X += pointChangeAmount;
                    points[point].X = Math.Clamp(points[point].X, 0f, 1f);
                }
                else
                {
                    points[point].Y += pointChangeAmount;
                    points[point].Y = Math.Clamp(points[point].Y, 0f, 1f);
                }
            }
            else
            {
                int RGBorA = random.Next(TriangleArtConstants.RedChance + TriangleArtConstants.GreenChance 
                    + TriangleArtConstants.BlueChance + TriangleArtConstants.AlphaChance);

                Color newColor;
                int changeAmount = random.Next(TriangleArtConstants.ColorChangeAmount) * 2 - TriangleArtConstants.ColorChangeAmount + 1;

                if (RGBorA < TriangleArtConstants.RedChance)
                {
                    int r = color.R;
                    r += changeAmount;

                    r = Math.Clamp(r, 0, 255);

                    newColor = Color.FromArgb(color.A, r, color.G, color.B);
                }
                else if(RGBorA < TriangleArtConstants.RedChance + TriangleArtConstants.GreenChance)
                {
                    int g = color.G;
                    g += changeAmount;
                  
                    g = Math.Clamp(g, 0, 255);

                    newColor = Color.FromArgb(color.A, color.R, g, color.B);
                }
                else if (RGBorA < TriangleArtConstants.RedChance + TriangleArtConstants.GreenChance + TriangleArtConstants.BlueChance)
                {
                    int b = color.B;
                    
                    b += changeAmount;

                    b = Math.Clamp(b, 0, 255);

                    newColor = Color.FromArgb(color.A, color.R, color.G, b);
                }
                else
                {
                    int a = color.A;
                    a += changeAmount;

                    a = Math.Clamp(a, 0, 255);

                    newColor = Color.FromArgb(a, color.R, color.G, color.B);
                }
                color = newColor;
            }
        }

        public Triangle Copy()
        {
            return new Triangle(points, color, BoundingBox, extremas);
        }

        public static Triangle RandomTriangle(Random random)
        {
            int a = random.Next(TriangleArtConstants.AlphaMin, TriangleArtConstants.AlphaMax);
            Color randomColor = Color.FromArgb(a, random.Next(256), random.Next(256), random.Next(256));

            PointF[] randomPoints = new PointF[3];
            double triangleTranslationX = random.NextDouble();
            double triangleTranslationY = random.NextDouble();
            for(int i = 0; i < 3; i++)
            {
                randomPoints[i].X = (float) (random.NextDouble() * TriangleArtConstants.PointXMin * 2 
                    - TriangleArtConstants.PointXMin + triangleTranslationX);
                randomPoints[i].X = Math.Clamp(randomPoints[i].X, 0, 1);
                randomPoints[i].Y = (float)(random.NextDouble() * TriangleArtConstants.PointYMin * 2
                   - TriangleArtConstants.PointYMin + triangleTranslationY);
                randomPoints[i].Y = Math.Clamp(randomPoints[i].Y, 0, 1);


            }

            Triangle randomTriangle = new Triangle(randomPoints, randomColor);
            return randomTriangle;
        }

        public Rectangle GetBoundingBox(int width, int height)
        {
            FindExtremas();
            int minX = (int) MathF.Round(extremas[0].X * width) ;
            int minY = (int) MathF.Round(extremas[0].Y * height);
            int maxX = (int) MathF.Round(extremas[1].X * width);
            int maxY = (int) MathF.Round(extremas[1].Y * height);

            minX = Math.Clamp(minX, 0, width - 1);
            minY = Math.Clamp(minY, 0, height - 1);
            maxX = Math.Clamp(maxX, minX, width - 1);
            maxY = Math.Clamp(maxY, minY, height - 1);

            int boxWidth = maxX - minX + 1;
            int boxHeight = maxY - minY + 1;

            boxWidth = Math.Max(1, boxWidth);
            boxHeight = Math.Max(1, boxHeight);
            //window forms origin is top left
            BoundingBox = new Rectangle(minX, minY, boxWidth, boxHeight);
            return BoundingBox ;
        }

        public PointF[] FindExtremas()
        {
            extremas = new PointF[2];
            float xMax = float.MinValue;
            float yMax = float.MinValue;
            float xMin = float.MaxValue;
            float yMin = float.MaxValue;

            foreach (PointF point in points)
            {
                if(point.X > xMax)
                {
                    xMax = point.X;
                }
                if (point.Y > yMax)
                {
                    yMax = point.Y;
                }


                if(point.X < xMin)
                {
                    xMin = point.X; 
                }
                if (point.Y < yMin)
                {
                    yMin = point.Y;
                }

            }
            extremas[0] = new PointF(xMin, yMin);
            extremas[1] = new PointF(xMax, yMax);
            return extremas;
        }
    }
}
