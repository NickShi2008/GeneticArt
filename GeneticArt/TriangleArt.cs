using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace GeneticArt
{

    public class TriangleArt : IDisposable
    {
        int maxTriangles;
        public List<Triangle> triangles;

        private Bitmap bp;
        
        private Graphics graphics;

        //still need to draw only inside image area and then recompute error in 2d array int of errors
        //recompute total error
        //try to avoid using opy to if expensive
        
        public TriangleArt(int maxTriangles, Bitmap bitmap, Random random)
        {
            //this.bitmap = bitmap;
            bp = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
            graphics = Graphics.FromImage(bp);
           

            this.maxTriangles = maxTriangles;
            triangles = new List<Triangle>();



            //for (int i = 0; i < maxTriangles; i++)
            //{
            //    triangles.Add(Triangle.RandomTriangle(random));
            //}

        }

        public void Dispose()
        {
            graphics.Dispose();
            bp.Dispose();

        }

        
        public void Mutate(Random random)
        {
            int removeAddOrmutate = random.Next(TriangleArtConstants.AddChance 
                + TriangleArtConstants.RemoveChance + TriangleArtConstants.MutateChance);
            int index = 0;

            if(removeAddOrmutate < TriangleArtConstants.AddChance || triangles.Count == 0)
            {
                if (triangles.Count >= maxTriangles)
                {
                    triangles.RemoveAt(0);
                }
                triangles.Add(Triangle.RandomTriangle(random));
                index = triangles.Count - 1;
                
            }
            else if (removeAddOrmutate < TriangleArtConstants.AddChance + TriangleArtConstants.RemoveChance)
            {
                index = random.Next(triangles.Count);

                triangles.RemoveAt(index);
                if (index == triangles.Count && triangles.Count != 1) index--;
                else if (index == 0) ;

            }
            else
            {
                index = random.Next(triangles.Count);

                triangles[index].Mutate(random);
            }

        }

        public Bitmap DrawImage()
        {

            int xCoef = bp.Width;
            int yCoef = bp.Height;

            graphics.Clear(Color.White);

            for (int i = 0; i < triangles.Count; i++)
            {
                triangles[i].DrawTriangle(graphics, xCoef, yCoef);
            }
            return bp;
        }

        public double GetError(Pixel[] sourcePixels)
        {
            long totalError = 0;
            int width = bp.Width;
            int height = bp.Height;
            bp = DrawImage();

            Rectangle rect;
            rect = new Rectangle(0, 0, width, height);

            BitmapData newBPData = bp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unchecked
            {
                unsafe
                {


                    //BitmapData originalBPData = originalImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    fixed (Pixel* psourcePixels = sourcePixels) 
                    {
                        Pixel* p1 = (Pixel*)newBPData.Scan0.ToPointer();
                        Pixel* p2 = psourcePixels;

                        for (int i = rect.Left; i < rect.Height * rect.Width; i++)
                        {
                            int r = p1->R - p2->R;
                            int g = p1->G - p2->G;
                            int b = p1->B - p2->B;
                            int a = p1->A - p2->A;

                            totalError += r * r + g * g + b * b + a * a;
                            p1++;
                            p2++;
                            //if(i > rect.Width)
                            //{
                            //    p1 += 
                            //}
                        }

                    }
                   
                }
            }
            bp.UnlockBits(newBPData);
            double newError = totalError / (height * width);
            return newError;

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
