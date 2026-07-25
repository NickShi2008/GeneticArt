namespace GeneticArt
{
    public partial class screen : Form
    {
        GeneticArtTrainer trainer;
        int populationSize = 10;
        int maxTriangles = 100;
        int epochs = 1000;
        public screen()
        {
            InitializeComponent();
            Graphics gfx = Graphics.FromImage(picture.Image);
            trainer = new GeneticArtTrainer(new Bitmap(picture.Image), maxTriangles, populationSize);
            
        }

        private void screen_Load(object sender, EventArgs e)
        {
            for(int i = 0; i < epochs; i++)
            {
                trainer.Train(new Random());
                picture.Image = trainer.GetBestImage(picture.Width, picture.Height);
                picture.Refresh();
            }
        }

        private void picture_Click(object sender, EventArgs e)
        {

        }
    }
}
