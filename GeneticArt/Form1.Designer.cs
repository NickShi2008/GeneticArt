namespace GeneticArt
{
    partial class screen
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(screen));
            picture = new PictureBox();
            geneticPicture = new PictureBox();
            hillPicture = new PictureBox();
            textBox1 = new TextBox();
            Genetic = new TextBox();
            HillText = new TextBox();
            TimerLabel = new Label();
            StartStopButton = new Button();
            GeneticButton = new Button();
            HillButton = new Button();
            GeneticIterationText = new Label();
            GeneticErrorText = new Label();
            HillErrorText = new Label();
            HillIterationText = new Label();
            ClickLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)picture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)geneticPicture).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hillPicture).BeginInit();
            SuspendLayout();
            // 
            // picture
            // 
            picture.Image = (Image)resources.GetObject("picture.Image");
            picture.Location = new Point(13, 3);
            picture.Margin = new Padding(4, 5, 4, 5);
            picture.Name = "picture";
            picture.Size = new Size(751, 565);
            picture.TabIndex = 0;
            picture.TabStop = false;
            picture.Click += picture_Click;
            // 
            // geneticPicture
            // 
            geneticPicture.Image = (Image)resources.GetObject("geneticPicture.Image");
            geneticPicture.Location = new Point(772, 3);
            geneticPicture.Margin = new Padding(4, 5, 4, 5);
            geneticPicture.Name = "geneticPicture";
            geneticPicture.Size = new Size(751, 565);
            geneticPicture.TabIndex = 1;
            geneticPicture.TabStop = false;
            // 
            // hillPicture
            // 
            hillPicture.Image = (Image)resources.GetObject("hillPicture.Image");
            hillPicture.Location = new Point(756, 612);
            hillPicture.Margin = new Padding(4, 5, 4, 5);
            hillPicture.Name = "hillPicture";
            hillPicture.Size = new Size(751, 565);
            hillPicture.TabIndex = 2;
            hillPicture.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(279, 596);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(150, 31);
            textBox1.TabIndex = 3;
            textBox1.Text = "Original";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // Genetic
            // 
            Genetic.Location = new Point(1109, 561);
            Genetic.Name = "Genetic";
            Genetic.Size = new Size(150, 31);
            Genetic.TabIndex = 4;
            Genetic.Text = "Genetic";
            // 
            // HillText
            // 
            HillText.Location = new Point(1017, 1185);
            HillText.Name = "HillText";
            HillText.Size = new Size(150, 31);
            HillText.TabIndex = 5;
            HillText.Text = "Hill Climber";
            // 
            // TimerLabel
            // 
            TimerLabel.AutoSize = true;
            TimerLabel.Location = new Point(62, 828);
            TimerLabel.Name = "TimerLabel";
            TimerLabel.Size = new Size(493, 25);
            TimerLabel.TabIndex = 7;
            TimerLabel.Text = "Start stop both,              Only Genetic,          Only Hill Climber";
            // 
            // StartStopButton
            // 
            StartStopButton.Location = new Point(26, 912);
            StartStopButton.Name = "StartStopButton";
            StartStopButton.Size = new Size(182, 94);
            StartStopButton.TabIndex = 8;
            StartStopButton.Text = "Start/Stop";
            StartStopButton.UseVisualStyleBackColor = true;
            StartStopButton.Click += StartStopButton_Click;
            // 
            // GeneticButton
            // 
            GeneticButton.Location = new Point(227, 912);
            GeneticButton.Name = "GeneticButton";
            GeneticButton.Size = new Size(182, 94);
            GeneticButton.TabIndex = 9;
            GeneticButton.Text = "Start/Stop";
            GeneticButton.UseVisualStyleBackColor = true;
            GeneticButton.Click += GeneticButton_Click;
            // 
            // HillButton
            // 
            HillButton.Location = new Point(437, 912);
            HillButton.Name = "HillButton";
            HillButton.Size = new Size(182, 94);
            HillButton.TabIndex = 10;
            HillButton.Text = "Start/Stop";
            HillButton.UseVisualStyleBackColor = true;
            HillButton.Click += HillButton_Click;
            // 
            // GeneticIterationText
            // 
            GeneticIterationText.AutoSize = true;
            GeneticIterationText.Location = new Point(1530, 203);
            GeneticIterationText.Name = "GeneticIterationText";
            GeneticIterationText.Size = new Size(145, 25);
            GeneticIterationText.TabIndex = 11;
            GeneticIterationText.Text = "Genetic Iteration:";
            // 
            // GeneticErrorText
            // 
            GeneticErrorText.AutoSize = true;
            GeneticErrorText.Location = new Point(1530, 245);
            GeneticErrorText.Name = "GeneticErrorText";
            GeneticErrorText.Size = new Size(117, 25);
            GeneticErrorText.TabIndex = 12;
            GeneticErrorText.Text = "Genetic Error:";
            // 
            // HillErrorText
            // 
            HillErrorText.AutoSize = true;
            HillErrorText.Location = new Point(1530, 763);
            HillErrorText.Name = "HillErrorText";
            HillErrorText.Size = new Size(84, 25);
            HillErrorText.TabIndex = 14;
            HillErrorText.Text = "Hill Error:";
            // 
            // HillIterationText
            // 
            HillIterationText.AutoSize = true;
            HillIterationText.Location = new Point(1530, 721);
            HillIterationText.Name = "HillIterationText";
            HillIterationText.Size = new Size(112, 25);
            HillIterationText.TabIndex = 13;
            HillIterationText.Text = "Hill Iteration:";
            // 
            // ClickLabel
            // 
            ClickLabel.AutoSize = true;
            ClickLabel.Location = new Point(208, 642);
            ClickLabel.Name = "ClickLabel";
            ClickLabel.Size = new Size(296, 25);
            ClickLabel.TabIndex = 15;
            ClickLabel.Text = "Click Image above to put your own!";
            // 
            // screen
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1829, 1516);
            Controls.Add(ClickLabel);
            Controls.Add(HillErrorText);
            Controls.Add(HillIterationText);
            Controls.Add(GeneticErrorText);
            Controls.Add(GeneticIterationText);
            Controls.Add(HillButton);
            Controls.Add(GeneticButton);
            Controls.Add(StartStopButton);
            Controls.Add(TimerLabel);
            Controls.Add(HillText);
            Controls.Add(Genetic);
            Controls.Add(textBox1);
            Controls.Add(hillPicture);
            Controls.Add(geneticPicture);
            Controls.Add(picture);
            Margin = new Padding(4, 5, 4, 5);
            Name = "screen";
            Text = "Form1";
            Load += screen_Load;
            ((System.ComponentModel.ISupportInitialize)picture).EndInit();
            ((System.ComponentModel.ISupportInitialize)geneticPicture).EndInit();
            ((System.ComponentModel.ISupportInitialize)hillPicture).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picture;
        private PictureBox geneticPicture;
        private PictureBox hillPicture;
        private TextBox textBox1;
        private TextBox Genetic;
        private TextBox HillText;
        private Label TimerLabel;
        private Button StartStopButton;
        private Button GeneticButton;
        private Button HillButton;
        private Label GeneticIterationText;
        private Label GeneticErrorText;
        private Label HillErrorText;
        private Label HillIterationText;
        private Label ClickLabel;
    }
}
