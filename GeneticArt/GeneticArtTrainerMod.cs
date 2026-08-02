using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeneticArt
{
    public class GeneticArtTrainerMod
    {
        public TriangleArtMod[] population;
        double bestError = double.MaxValue;

        Rectangle rect;
        Pixel[] sourcePixels;

        bool hasRanOnce = false;

        public GeneticArtTrainerMod(Image originalImage, int maxTriangles, int populationSize, Random random)
        {
            Bitmap bp = new Bitmap(originalImage);
            population = new TriangleArtMod[populationSize];
            for(int i = 0; i < populationSize; i++)
            {
                population[i] = new TriangleArtMod(maxTriangles, bp, random);
            }
            ;
            rect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);
            sourcePixels = BitmapToPixels(bp);
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

        //errror same across all?
        public double Train(Random random)
        {
            int bestIndex = 0;
            double bestError = population[0].GetError(sourcePixels);

            for (int i = 1; i < population.Length; i++)
            {
                population[0].CopyTo(population[i]);
                population[i].Mutate(random);
                double error = hasRanOnce ? population[i].GetErrorTest(sourcePixels) : population[i].GetError(sourcePixels);
                if (error < bestError)
                {
                    bestError = error;
                    bestIndex = i;

                }
            }
            hasRanOnce = true;
            TriangleArtMod temp = population[0];
            population[0] = population[bestIndex];
            population[bestIndex] = temp;

            return bestError;
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
            return population[0].DrawImage();
        }
    }
}
