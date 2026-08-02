using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;

namespace GeneticArt
{

    public struct Pixel
    {
        public byte B;
        public byte G;
        public byte R;
        public byte A;
    }

    public class TriangleArtMod : IDisposable
    {
        int maxTriangles;
        List<Triangle> triangles;

        private Bitmap bp;

        private Bitmap oldBp;
        
        private Graphics graphics;

        double currentError = double.MaxValue;
        long totalError = 0;

        int lastSearchedIndex;

        int width => bp.Width;
        int height => bp.Height;

        Rectangle BoxToSearch;

        long[] pixelErrors;

        Triangle previousTriangle;
        //still need to draw only inside image area and then recompute error in 2d array int of errors
        //recompute total error
        //try to avoid using opy to if expensive
        
        public TriangleArtMod(int maxTriangles, Bitmap bitmap, Random random)
        {
            //this.bitmap = bitmap;
            bp = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);
            graphics = Graphics.FromImage(bp);
            graphics.Clear(Color.White);
            //OldBox = new Rectangle(0, 0, bp.Width, bp.Height);

            this.maxTriangles = maxTriangles;
            triangles = new List<Triangle>();
            pixelErrors = new long[bitmap.Height * bitmap.Width];


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
                Rectangle OldBox = Rectangle.Empty;
                if (triangles.Count >= maxTriangles)
                {
                    OldBox = triangles[0].GetBoundingBox(width, height);
                    triangles.RemoveAt(0);
                }
                triangles.Add(Triangle.RandomTriangle(random));
                Rectangle NewBox = triangles[triangles.Count - 1].GetBoundingBox(width, height);
                if (!OldBox.IsEmpty)
                {
                    BoxToSearch = Rectangle.Union(OldBox, NewBox);
                }
                else
                {
                    BoxToSearch = NewBox;
                }

                index = triangles.Count - 1;
                
            }
            else if (removeAddOrmutate < TriangleArtConstants.AddChance + TriangleArtConstants.RemoveChance)
            {
                index = random.Next(triangles.Count);
                BoxToSearch = triangles[index].GetBoundingBox(width, height);
                previousTriangle = triangles[index];
                triangles.RemoveAt(index);
            }
            else
            {
                index = random.Next(triangles.Count);
                previousTriangle = triangles[index].Copy();

                Rectangle before = triangles[index].GetBoundingBox(width, height);
                triangles[index].Mutate(random);
                Rectangle after = triangles[index].GetBoundingBox(width, height);

                BoxToSearch = Rectangle.Union(before, after);
            }
            lastSearchedIndex = index;

            BoxToSearch = Rectangle.Intersect(BoxToSearch, new Rectangle(0, 0, width, height));
        }

        public Bitmap DrawImageSmall()
        {
           
            int xCoef = bp.Width;
            int yCoef = bp.Height;

            graphics = Graphics.FromImage(bp);
            graphics.SetClip(BoxToSearch);
            graphics.Clear(Color.White);


            for(int i = 0; i < triangles.Count; i++)
            {
                if (BoxToSearch.IntersectsWith(triangles[i].BoundingBox))
                {
                    triangles[i].DrawTriangle(graphics, xCoef, yCoef);
                }
            }

            return bp;
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
            totalError = 0;
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

                            pixelErrors[i] += r * r + g * g + b * b + a * a;
                            totalError += pixelErrors[i];
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
            double newError = (double) (pixelErrors.Sum() / (height * width));
            if (newError < currentError)
            {
                currentError = newError;
                oldBp = (Bitmap)bp.Clone();
            }
            return currentError;

        }
        public double GetErrorTest(Pixel[] sourcePixels)
        {
            if(BoxToSearch.IsEmpty) return GetError(sourcePixels);
            long totalError = 0;
            int width = bp.Width;
            int height = bp.Height;
            bp = DrawImageSmall();

            Rectangle rect;



            rect = BoxToSearch;
            double newError;
            BitmapData newBPData = bp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unchecked
            {
                unsafe
                {


                    //BitmapData originalBPData = originalImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    fixed (Pixel* psourcePixels = sourcePixels)
                    {
                        Pixel* p1 = (Pixel*) newBPData.Scan0;
                        Pixel* p2 = psourcePixels + rect.Y * width + rect.X;
                        int count = rect.Left;
                        for (int y = 0; y < rect.Height; y++)
                        {
                            for (int x = 0; x < rect.Width; x++)
                            {
                                int r = p1->R - p2->R;
                                int g = p1->G - p2->G;
                                int b = p1->B - p2->B;
                                int a = p1->A - p2->A;

                                pixelErrors[count] = r * r + g * g + b * b + a * a;
                                p1++;
                                p2++;
                            }
                            //basically since array, think about subtracting total pixels in a row mby the width of rect giving amount to skip over
                            //4byte so no padding 
                            count += width - rect.Width;
                            p1 += width - rect.Width;
                            p2 += width - rect.Width;
                        }

                    }

                }
                newError = pixelErrors.Sum() / (height * width);
            }
            bp.UnlockBits(newBPData);

            if (newError < currentError)
            {
                currentError = newError;
                oldBp = (Bitmap)bp.Clone();
            }
            return currentError;

        }






        public void CopyTo(TriangleArtMod triangleArt)
        {
            triangleArt.triangles.Clear();
            for(int i = 0; i < triangles.Count; i++)
            {
                triangleArt.triangles.Add(triangles[i].Copy());
            }
            triangleArt.currentError = this.currentError;
        }

    }
}
