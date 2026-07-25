using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticArt
{
    public class GeneticArtTrainer
    {
        TriangleArt[] population;
        double bestError = double.MaxValue;
        int bestIndex = 0;

        public GeneticArtTrainer(Bitmap originalImage, int maxTriangles, int populationSize)
        {
            population = new TriangleArt[populationSize];
            for(int i = 0; i < populationSize; i++)
            {
                population[i] = new TriangleArt(maxTriangles, originalImage);
            }
        }

        public double Train(Random random)
        {
            for (int i = 0; i < population.Length; i++)
            {
                if (i < population.Length - 1)
                    population[bestIndex].CopyTo(population[i + 1]);
                if(i != bestIndex)
                    population[i].Mutate(random);
            }

            return population[bestIndex].GetError();
        }

        public Bitmap GetBestImage(int x, int y)
        {
            int bestIndex = 0;
            for(int i =0; i < population.Length; i++)
            {
                double error = population[i].GetError();
                if (error < bestError)
                {
                    bestError = error;
                    bestIndex = i;
                    
                }
            }
            return population[bestIndex].DrawImage(x, y);
        }
    }
}
