namespace GeneticArt
{
    public partial class screen : Form
    {
        GeneticArtTrainer trainer;
        int populationSize = 60;
        int maxTriangles = 600;
        int epochs = 600;
        Random random;
        //before with simple lockbits popSize 30, maxTri 500, and epoch 500 time take for starry night image is 2 min 13 seconds
        //gave quite a bad image

        //looking at dan bystroms improvement (maybe parallel processing later?)
        //1min 53? 

        //60/
        //600
        //600
        // around 4 min 30 to 5 and 140 to 150 mb
        public screen()
        {
            InitializeComponent();
            Graphics gfx = Graphics.FromImage(picture.Image);

            random = new Random();
            trainer = new GeneticArtTrainer(picture.Image, maxTriangles, populationSize, random);
            
        }

        private void screen_Load(object sender, EventArgs e)
        {
            double bestError = 0;
            for (int i = 0; i < epochs; i++)
            {
                bestError = trainer.Train(random);

                ;

            }
            picture.Image = trainer.GetBestImage(picture.Width, picture.Height); 
            picture.Refresh();
            ;

        }

        private void picture_Click(object sender, EventArgs e)
        {
            
            
        }
    }
}
