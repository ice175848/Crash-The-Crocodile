namespace 打2魚
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Button[] btns = new Button[100];
        Label score_Label = new Label();
        PictureBox pictureBox = new PictureBox();
        Image croco = Image.FromFile("D:\\crocoIcon.png");
        Image[] crocDie = new Image[3];
        Image crocDie1 = Image.FromFile("D:\\2fishDie1.png");
        Image crocDie2 = Image.FromFile("D:\\2fishDie2.png");
        Image crocDie3 = Image.FromFile("D:\\2fishDie3.png");
        private static System.Timers.Timer Timer = new System.Timers.Timer();
        Random rnd = new Random();
        int size=50;
        int score;
        private void Form1_Load(object sender, EventArgs e)
        {
            score = 0;
            pictureBox.Location = new Point(600,50);
            pictureBox.Size = new Size(2000,2000);
            pictureBox.Image = croco;
            this.Controls.Add(pictureBox);
            score_Label = new Label();
            score_Label.Location = new Point(600, 12);
            score_Label.Text = "score_Label:";
            this.Controls.Add(score_Label);
            for (int i = 0; i < 3; i++)
            {
                string fileName = $"D:\\2fishDie{i + 1}.png";
                crocDie[i] = Image.FromFile(fileName);
            }
            for (int i=0;i<10;i++)
            {
                for(int j=0;j<10;j++)
                {
                    btns[i*10+j] = new Button();
                    btns[i * 10 + j].Size = new Size(size,size);
                    btns[i * 10 + j].Location = new Point(12+j*(size+1),12+i*(size+1));
                    btns[i * 10 + j].Click += new EventHandler(this.btns_Click);
                    //btns[i * 10 + j].Image = croco;
                }
            }
            this.Controls.AddRange(btns);
        }
        public void btns_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            clickedButton.Enabled = false;
            score++;
            score_Label.Text = "score:"+score.ToString();
            pictureBox.Image = crocDie[rnd.Next(0,3)];
        }
    }
}
