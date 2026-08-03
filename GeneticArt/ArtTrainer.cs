using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeneticArt
{
    public class ArtTrainer
    {
        //public TriangleArtMod[] population;
        public TriangleArtMod art;
        Rectangle rect;
        Pixel[] sourcePixels;


        public ArtTrainer(Image originalImage, int maxTriangles, Random random)
        {
            Bitmap bp = new Bitmap(originalImage);
            art = new TriangleArtMod(maxTriangles, bp, random);
            //population = new TriangleArtMod[populationSize];
            //for(int i = 0; i < populationSize; i++)
            //{
            //    population[i] = new TriangleArtMod(maxTriangles, bp, random);
            //}
            ;
            //rect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);

            sourcePixels = BitmapToPixels(bp);
            art.StartError(sourcePixels);
        }

        public Pixel[] BitmapToPixels(Bitmap bp)
        {
            unsafe
            {
                BitmapData bpd = bp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                try
                {
                    int pixelCount = bp.Width * bp.Height;
                    Pixel[] pixels = new Pixel[pixelCount];

                    fixed(Pixel* stores = pixels)
                    {
                        Buffer.MemoryCopy(bpd.Scan0.ToPointer(), stores, pixels.Length * sizeof(Pixel), pixels.Length * sizeof(Pixel));
                    }

                    return pixels;
                }
                finally
                {
                    bp.UnlockBits(bpd);
                }
            }
        }

        public double Train(Random random)
        {
            art.Mutate(random);
            double error = art.ErrorFunction(sourcePixels);
            return error;
        }

        //public double TrainParallel(Random random)
        //{
        //    double[] errors = new double[population.Length];
        //    errors[0] = population[0].GetError();
        //    bestError = population[0].GetError();
        //    int bestIndex = 0;
        //    Parallel.For(1, population.Length, (i) =>
        //    {
        //        population[0].CopyTo(population[i]); 
        //        population[i].Mutate(random);
        //        errors[i] = population[i].GetError();
        //    });

        //    for(int i = 0; i < errors.Length; i++)
        //    {
        //        if (errors[i] < bestError)
        //        {
        //            bestError = errors[i];
        //            bestIndex = i;
        //        }
        //    }
        //    TriangleArt temp = population[0];
        //    population[0] = population[bestIndex];
        //    population[bestIndex] = temp;


        //    return bestError;
        //}

        public Bitmap GetBestImage(int x, int y)
        {
            return art.DrawImage();
        }
    }
}
