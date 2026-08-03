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

        bool hasRemoved = false;

        enum MutationType
        {
            Add,
            Remove,
            Mutate
        }

        MutationType change;

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
            hasRemoved = false;
            if(removeAddOrmutate < TriangleArtConstants.AddChance || triangles.Count == 0)
            {
                Rectangle OldBox = Rectangle.Empty;
                if (triangles.Count >= maxTriangles)
                {
                    OldBox = triangles[0].GetBoundingBox(width, height);
                    previousTriangle = triangles[0];
                    triangles.RemoveAt(0);
                    hasRemoved = true;
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
                change = MutationType.Add;
            }
            else if (removeAddOrmutate < TriangleArtConstants.AddChance + TriangleArtConstants.RemoveChance)
            {
                index = random.Next(triangles.Count);
                BoxToSearch = triangles[index].GetBoundingBox(width, height);
                previousTriangle = triangles[index];
                triangles.RemoveAt(index);
                change = MutationType.Remove;
            }
            else
            {
                index = random.Next(triangles.Count);
                previousTriangle = triangles[index].Copy();

                Rectangle before = triangles[index].GetBoundingBox(width, height);
                triangles[index].Mutate(random);
                Rectangle after = triangles[index].GetBoundingBox(width, height);

                BoxToSearch = Rectangle.Union(before, after);
                change = MutationType.Mutate;
            }
            lastSearchedIndex = index;

            BoxToSearch = Rectangle.Intersect(BoxToSearch, new Rectangle(0, 0, width, height));
        }

        public void DrawImageSmall()
        {
           
            int xCoef = bp.Width;
            int yCoef = bp.Height;

            
            graphics.SetClip(BoxToSearch);
            graphics.Clear(Color.White);


            for(int i = 0; i < triangles.Count; i++)
            {
                if (BoxToSearch.IntersectsWith(triangles[i].BoundingBox))
                {
                    triangles[i].DrawTriangle(graphics, xCoef, yCoef);
                }
            }

            graphics.ResetClip();

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

        public void StartError(Pixel[] sourcePixels)
        {
            //totalError = 0;
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

                        for (int i = 0; i < sourcePixels.Length; i++)
                        {
                            int r = p1->R - p2->R;
                            int g = p1->G - p2->G;
                            int b = p1->B - p2->B;
                            int a = p1->A - p2->A;

                            pixelErrors[i] = r * r + g * g + b * b + a * a;
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
            currentError = (double)totalError;

        }
        public double GetError(Pixel[] sourcePixels)
        {
            if(BoxToSearch.Width == 0 || BoxToSearch.Height == 0)
            {
                return currentError;
            }
            //long totalError = 0;
            int width = bp.Width;
            int height = bp.Height;
            DrawImageSmall();

            Rectangle rect;



            rect = BoxToSearch;
            BitmapData newBPData = bp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            unchecked
            {
                unsafe
                {
                    
                    //gotta be something here because the error is completely wack, the error difference shouldn't be this large I don't think?

                    //BitmapData originalBPData = originalImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                    fixed (Pixel* psourcePixels = sourcePixels)
                    {
                        //Pixel* p1 = (Pixel*)newBPData.Scan0.ToPointer();

                        //Pixel* p2 = psourcePixels + rect.Y * width + rect.X;
                        //int count = rect.Y * rect.Width + rect.X;
                        

                        for (int y = 0; y < rect.Height; y++)
                        {
                            Pixel* p1 = (Pixel*)(newBPData.Scan0 + y * newBPData.Stride);
                            //compiler automatically converts the width values * size of Pixel to move the bytes meaning since 32 bpp stride == width
                            Pixel* p2 = psourcePixels + (rect.Y + y) * width + rect.X;
                            int count = (rect.Y + y) * width + rect.X;
                            for (int x = 0; x < rect.Width; x++)
                            {
                                int r = p1->R - p2->R;
                                int g = p1->G - p2->G;
                                int b = p1->B - p2->B;
                                int a = p1->A - p2->A;
                                int error = r * r + g * g + b * b + a * a;
                                totalError += error - pixelErrors[count];
                                pixelErrors[count] = error;
                                p1++;
                                p2++;
                                count++;
                            }
                            //basically since array, think about subtracting total pixels in a row mby the width of rect giving amount to skip over
                            //4byte so no padding 
                            //count += width - rect.Width;
                            //p1 += width - rect.Width;
                            //p2 += width - rect.Width;
                        }

                    }

                }
            }
            bp.UnlockBits(newBPData);

            return totalError;

        }

        //works as whole here to compare difference
        //most likely draw but 
        public double GetErrorWorking(Pixel[] sourcePixels)
        {
            //totalError = 0;
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

                        int count = rect.Left;
                        for (int y = 0; y < rect.Height; y++)
                        {
                            for (int x = 0; x < rect.Width; x++)
                            {
                                int r = p1->R - p2->R;
                                int g = p1->G - p2->G;
                                int b = p1->B - p2->B;
                                int a = p1->A - p2->A;
                                int error = r * r + g * g + b * b + a * a;
                                totalError += error - pixelErrors[count];
                                pixelErrors[count] = error;
                                p1++;
                                p2++;
                                count++;
                            }
                            //basically since array, think about subtracting total pixels in a row mby the width of rect giving amount to skip over
                            //4byte so no padding 
                            count += width - rect.Width;
                            p1 += width - rect.Width;
                            p2 += width - rect.Width;
                        }

                    }

                }
            }
            bp.UnlockBits(newBPData);
            //currentError = (double)totalError;
            return totalError;
        }

        void Undo()
        {
            switch (change)
            {
                case MutationType.Add:
                    triangles.RemoveAt(lastSearchedIndex);
                    if (hasRemoved) triangles.Insert(0, previousTriangle);
                    break;
                case MutationType.Remove:
                    triangles.Insert(lastSearchedIndex, previousTriangle);
                    break;
                case MutationType.Mutate:
                    triangles[lastSearchedIndex] = previousTriangle;
                    break;
            }
        }

        public double ErrorFunction(Pixel[] sourcePixels)
        {
            double error = GetError(sourcePixels);

            if (error < currentError)
            {
                currentError = error;
                return error;
            }

            Undo();
            //DrawImageSmall();
            GetError(sourcePixels);
            return currentError;
        }

    }
}
