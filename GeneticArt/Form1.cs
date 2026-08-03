using System.Diagnostics;

namespace GeneticArt
{
    public partial class screen : Form
    {
        GeneticArtTrainer trainer;
        ArtTrainer trainerMod;
        int populationSize = 40;
        int maxTriangles = 100;
        int epochs = 60000;
        Random geneticRandom;
        Random hillRandom;

        bool canGeneticTrain = false;
        bool canHillTrain = false;

        double geneticScore = double.MaxValue;
        double hillScore = double.MaxValue; 
        private readonly object geneticLock = new object();
        private readonly object hillLock = new object();


        //before with simple lockbits popSize 30, maxTri 500, and epoch 500 time take for starry night image is 2 min 13 seconds
        //gave quite a bad image

        //looking at dan bystroms improvement (maybe parallel processing later?)
        //1min 53? 

        //60/
        //600
        //600
        // around 4 min 30 to 5 and 140 to 150 mb

        System.Threading.Thread t;
        Thread geneticThread;
        Thread hillThread;
        public screen()
        {
            InitializeComponent();
            Graphics gfx = Graphics.FromImage(picture.Image);

            geneticRandom = new Random();
            hillRandom = new Random();
            trainer = new GeneticArtTrainer(picture.Image, maxTriangles, populationSize, geneticRandom);
            trainerMod = new ArtTrainer(picture.Image, maxTriangles, hillRandom);

            geneticThread = new Thread(RunGenetic);
            hillThread = new Thread(RunHill);

            geneticThread.Start();
            hillThread.Start();

        }



        private void screen_Load(object sender, EventArgs e)
        {


        }

        private void picture_Click(object sender, EventArgs e)
        {


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void StartStopButton_Click(object sender, EventArgs e)
        {
            //Stopwatch sw = Stopwatch.StartNew();
            //double bestError = 0;
            //while (sw.Elapsed < TimeSpan.FromMinutes(5))
            //{
            //    bestError = trainer.Train(random);
            //}

            //Console.WriteLine(bestError);




            //hillPicture.Image = trainerMod.GetBestImage(picture.Width, picture.Height);
            //hillPicture.Refresh();
            //;

            if (canHillTrain && canGeneticTrain)
            {
                canGeneticTrain = false;
                canHillTrain = false;
            }
            else
            {
                canHillTrain = true;
                canGeneticTrain = true;
            }
        }


        private void RunGenetic()
        {
            int count = 0;

            while (true)
            {
                if (!canGeneticTrain)
                {
                    Thread.Sleep(10);
                    continue;
                }

                lock (geneticLock)
                    geneticScore = trainer.Train(geneticRandom);

                BeginInvoke(() =>
                {
                    GeneticIterationText.Text = $"Genetic Iteration: {count}";
                    GeneticErrorText.Text = $"Genetic Error: {geneticScore.ToString("F0")}";
                }
                );
                if (++count % 20 == 0)
                {
                    Bitmap snapshot;
                    lock (geneticLock)
                        snapshot = (Bitmap)trainer.GetBestImage(geneticPicture.Width, geneticPicture.Height).Clone();

                    BeginInvoke(() =>
                    {
                        var old = geneticPicture.Image;
                        geneticPicture.Image = snapshot;
                        old?.Dispose();
                    });
                }
            }
        }

        private void RunHill()
        {
            int count = 0;

            while (true)
            {
                if (!canHillTrain)
                {
                    Thread.Sleep(10);
                    continue;
                }

                lock(hillLock)
                    hillScore = trainerMod.Train(hillRandom)/ (picture.Image.Width * picture.Image.Height);

                BeginInvoke(() =>
                {
                    HillIterationText.Text = $"Hill Iteration: {count}";
                    HillErrorText.Text = $"Hill Error: {hillScore.ToString("F0")}";
                }
                );
                if (++count % 100 == 0)
                {
                    Bitmap snapshot;
                    lock (hillLock)
                        snapshot = (Bitmap)trainerMod.GetBestImage(hillPicture.Width, hillPicture.Height).Clone();

                    BeginInvoke(() =>
                    {
                        var old = hillPicture.Image;
                        hillPicture.Image = snapshot;
                        old?.Dispose();
                    });
                }
            }
        }


        private void GeneticButton_Click(object sender, EventArgs e)
        {
            canGeneticTrain = !canGeneticTrain;
        }

        private void HillButton_Click(object sender, EventArgs e)
        {
            canHillTrain = !canHillTrain;
        }
    }
}
