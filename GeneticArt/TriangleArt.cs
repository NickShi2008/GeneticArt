using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace GeneticArt
{
    public class TriangleArt
    {
        int maxTriangles;
        List<Triangle> triangles;
        Bitmap originalImage;
        
        public TriangleArt(int maxTriangles, Bitmap bitmap)
        {
            originalImage = bitmap;
            this.maxTriangles = maxTriangles;
            triangles = new List<Triangle>();
            for (int i = 0; i < maxTriangles; i++)
            {
                triangles.Add(Triangle.RandomTriangle(new Random()));
            }
        }

        public void Mutate(Random random)
        {
            int removeAddOrmutate = new Random().Next(TriangleArtConstants.AddChance 
                + TriangleArtConstants.RemoveChance + TriangleArtConstants.MutateChance);

            if(removeAddOrmutate < TriangleArtConstants.AddChance || triangles.Capacity == 0)
            {
                if (triangles.Capacity >= maxTriangles)
                    triangles.Remove(triangles.First());
                triangles.Add(Triangle.RandomTriangle(random));
            }
            else if (removeAddOrmutate < TriangleArtConstants.AddChance + TriangleArtConstants.RemoveChance)
            {
                triangles.RemoveAt(random.Next(triangles.Count));
            }
            else
            {
                triangles[random.Next(triangles.Count)].Mutate(random);
            }
        }

        public Bitmap DrawImage(int width, int height)
        {
            Bitmap newBP = new Bitmap(width, height);

            int xCoef = width;
            int yCoef = height;

            Graphics graphics = Graphics.FromImage(newBP);

            for(int i = 0; i < triangles.Count; i++)
            {
                triangles[i].DrawTriangle(graphics, xCoef, yCoef);
            }

            return newBP;
        }

        public double GetError()
        {
            double totalError = 0;
            unsafe
            {
                Bitmap newBP = DrawImage(originalImage.Width, originalImage.Height);

                Rectangle rect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);

                BitmapData newBPData = newBP.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                BitmapData originalBPData = originalImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                try
                {
                    byte* newBPPtr = (byte*)newBPData.Scan0;
                    int stride = newBPData.Stride;

                    byte* bpp = (byte*)newBPData.Scan0;


                    for (int y = 0; y < newBP.Height; y++)
                    {
                        byte* row = (y * stride) + newBPPtr;
                        byte* originRow = (y * stride) + bpp;

                        for (int x = 0; x < newBP.Width; x++)
                        {
                            byte* pixel = row + (x * 4);

                            int b = pixel[0];
                            int g = pixel[1];
                            int r = pixel[2];
                            int a = pixel[3];

                            byte* originPixel = originRow + (x * 4);
                            int originB = originPixel[0];
                            int originG = originPixel[1];
                            int originR = originPixel[2];
                            int originA = originPixel[3];

                            double error = Math.Pow(Math.Abs(b - originB),2) + Math.Pow(Math.Abs(g - originG), 2)
                                + Math.Pow(Math.Abs(r - originR),2) + Math.Pow(Math.Abs(a - originA), 2);
                            totalError += error;
                        }
                    }
                }
                finally
                {
                    newBP.UnlockBits(newBPData);
                    originalImage.UnlockBits(originalBPData);
                }
            }

            return totalError/(originalImage.Height * originalImage.Width);
        }

        public void CopyTo(TriangleArt triangleArt)
        {
            triangleArt.triangles.Clear();
            for(int i = 0; i < triangles.Count; i++)
            {
                triangleArt.triangles.Add(triangles[i].Copy());
            }
        }

    }
}
