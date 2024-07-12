using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace 打2魚
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private DateTime startTime;
        private TimeSpan elapsedTime;
        private int times = 0;
        private Button[] btns = new Button[100];
        private Label score_Label;
        private Label DateTime_Label;
        private Label NowTime_Label;
        private Stopwatch stopwatch = new Stopwatch();
        private PictureBox pictureBox = new PictureBox();
        private Image croco = Image.FromFile("D:\\crocoIcon.png");
        private Image[] crocDie = new Image[3];
        private Random rnd = new Random();
        private int size = 50;
        private int score;
        private int CrocoNO;
        private bool[] Crocodiles = new bool[100];
        private int[] Crocodile_OnMap = new int[5];
        private Label charts_Label;

        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new Timer
            {
                Interval = 1000
            };
            timer.Tick += Timer_Tick;
            startTime = DateTime.Now;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - startTime;
            NowTime_Label.Text = $"已執行時間: {elapsedTime:hh\\:mm\\:ss}";
            times++;
            if (times >= 2)
            {
                times = 0;
                Random_Crocodile();
            }
        }

        private void Random_Crocodile()
        {
            for (int i = 0; i < 5; i++)
            {
                if (Crocodiles[Crocodile_OnMap[i]])
                {
                    Crocodiles[Crocodile_OnMap[i]] = false;
                    btns[Crocodile_OnMap[i]].Image = null;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                int boss;
                do
                {
                    boss = rnd.Next(0, 100);
                } while (Crocodiles[boss] || !btns[boss].Enabled);

                Crocodiles[boss] = true;
                Crocodile_OnMap[i] = boss;
                btns[boss].Image = croco;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Array.Fill(Crocodiles, false);

            score = 0;
            stopwatch.Start();

            InitializeUI();
            CreateButton();
            this.Controls.AddRange(btns);
            Random_Crocodile();
        }

        private void InitializeUI()
        {
            pictureBox.Location = new Point(550, 150);
            pictureBox.Size = new Size(200, 200);
            pictureBox.Image = croco;
            this.Controls.Add(pictureBox);

            score_Label = new Label
            {
                Location = new Point(550, 12),
                Text = "score_Label:"
            };
            this.Controls.Add(score_Label);

            DateTime_Label = new Label
            {
                AutoSize = true,
                Location = new Point(550, 50),
                Text = "遊玩開始時間:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
            };
            this.Controls.Add(DateTime_Label);

            NowTime_Label = new Label
            {
                AutoSize = true,
                Location = new Point(550, 88)
            };
            this.Controls.Add(NowTime_Label);

            charts_Label = new Label
            {
                AutoSize = true,
                Location = new Point(1111, 12),
                Text = "排行榜:\n" + ReadCharts()
            };
            this.Controls.Add(charts_Label);

            for (int i = 0; i < 3; i++)
            {
                crocDie[i] = Image.FromFile($"D:\\2fishDie{i + 1}.png");
            }
        }

        private string ReadCharts()
        {
            return File.Exists("charts.txt") ? File.ReadAllText("charts.txt") : "No records";
        }

        private void CreateButton()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    btns[i * 10 + j] = new Button
                    {
                        Name = "button" + (i * 10 + j),
                        Size = new Size(size, size),
                        Location = new Point(12 + j * (size + 1), 12 + i * (size + 1))
                    };
                    btns[i * 10 + j].Click += btns_Click;
                }
            }
        }

        private void btns_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int buttonIndex = int.Parse(clickedButton.Name.Remove(0, 6));

            if (Crocodiles[buttonIndex])
            {
                clickedButton.Enabled = false;
                score += 10;
                pictureBox.Image = crocDie[rnd.Next(0, 3)];
            }
            else
            {
                score -= 5;
            }
            score_display();

            if (score >= 100)
            {
                EndGame();
            }
        }

        private void score_display()
        {
            score_Label.Text = "score:" + score;
        }

        private void EndGame()
        {
            timer.Stop();
            NowTime_Label.Text = $"花費時間: {elapsedTime:hh\\:mm\\:ss}";

            string playerName = Prompt.ShowDialog("請輸入你的名字", "遊戲結束");
            File.AppendAllText("charts.txt", $"Name: {playerName}, Score: {score}, Time: {elapsedTime:hh\\:mm\\:ss}\n");

            charts_Label.Text = "排行榜:\n" + ReadCharts();
            MessageBox.Show("EndGame!");
            Environment.Exit(0);
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button { Text = "Ok", Left = 350, Width = 100, Top = 75, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
