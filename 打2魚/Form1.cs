using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
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
        private Image[] crocEsc = new Image[3];
        private Random rnd = new Random();
        private int size = 50;
        private int score;
        private bool[] Crocodiles = new bool[100];
        private int[] Crocodile_OnMap = new int[5];
        private TableLayoutPanel tableLayoutPanel;


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
        private void CustomizeLabel(Label lbl, int fontSize, Image backgroundImage)
        {
            lbl.Font = new Font("Segoe UI", fontSize, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(231, 76, 60);  // 設定字體顏色
            lbl.BackgroundImage = backgroundImage; // 設置背景圖片
            lbl.BackgroundImageLayout = ImageLayout.Stretch; // 讓圖片適應 Label 大小
        }

        private void CustomizeButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(52, 152, 219);  // 按鈕背景顏色
            btn.ForeColor = Color.White;  // 字體顏色
            btn.FlatStyle = FlatStyle.Flat;  // 平面樣式
            btn.FlatAppearance.BorderSize = 0;  // 移除邊框
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);  // 改變字體和大小
            btn.Size = new Size(size + 5, size + 5);  // 調整按鈕大小，讓按鈕更醒目
            btn.Cursor = Cursors.Hand; // 改變滑鼠圖示

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
            DisplayCharts();

        }

        private void InitializeUI()
        {
            // 初始化 TableLayoutPanel
            tableLayoutPanel = new TableLayoutPanel
            {
                ColumnCount = 2,
                AutoSize = true,
                Location = new Point(1111, 12),
                BackColor = Color.LightGray
            };
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // 添加表頭
            tableLayoutPanel.Controls.Add(new Label { Text = "Name", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "Time", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleRight }, 1, 0);

            this.Controls.Add(tableLayoutPanel);

            // 添加 Crocodiles 遊戲的其他 UI
            pictureBox.Location = new Point(550, 123);
            pictureBox.Size = new Size(557, 404);
            pictureBox.Image = croco;
            //pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

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

            for(int i=0;i<3;i++)
            {
                crocDie[i] = Image.FromFile($"D:\\2fishDie{i + 1}.png");
                crocEsc[i] = Image.FromFile($"D:\\EscapeCroco{i + 1}.png");
            }

            CustomizeLabelBackgroundColor(score_Label, Color.LightGray, Color.Red, 14);
            CustomizeLabelBackgroundColor(DateTime_Label, Color.LightGray, Color.Red, 12);
            CustomizeLabelBackgroundColor(NowTime_Label, Color.LightGray, Color.Red, 12);
            
        }
        private void DisplayCharts()
        {
            // 清空舊資料，保留表頭
            tableLayoutPanel.Controls.Clear();

            // 添加表頭
            tableLayoutPanel.Controls.Add(new Label { Text = "Name", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            tableLayoutPanel.Controls.Add(new Label { Text = "Time", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleRight }, 1, 0);

            // 讀取排行榜並整理輸出
            var chartData = ReadCharts();
            int row = 1;  // 從第 1 行開始填充資料
            foreach (var entry in chartData)
            {
                tableLayoutPanel.Controls.Add(new Label { Text = entry.Name, Font = new Font("Segoe UI", 10F), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
                tableLayoutPanel.Controls.Add(new Label { Text = entry.Time, Font = new Font("Segoe UI", 10F), ForeColor = Color.Red, TextAlign = ContentAlignment.MiddleRight }, 1, row);
                row++;
            }
        }

        private List<(string Name, string Time)> ReadCharts()
        {
            var charts = new List<(string Name, string Time)>();

            if (File.Exists("charts.txt"))
            {
                var lines = File.ReadAllLines("charts.txt");
                foreach (var line in lines)
                {
                    // 假設文件的格式是 "Name: playerName, Time: elapsedTime"
                    var parts = line.Split(new[] { "Name: ", ", Time: " }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 2)
                    {
                        // 處理空名字的情況
                        string name = string.IsNullOrWhiteSpace(parts[0]) ? "Unknown" : parts[0];
                        charts.Add((name, parts[1]));
                    }
                }
            }
            return charts;
        }

        private void CustomizeLabelBackgroundColor(Label lbl, Color backgroundColor, Color textColor, int fontSize)
        {
            lbl.BackColor = backgroundColor;  // 設定背景顏色
            lbl.ForeColor = textColor;        // 設定字體顏色
            lbl.Font = new Font("Segoe UI", fontSize, FontStyle.Bold);  // 設定字體樣式和大小
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
                    CustomizeButton(btns[i * 10 + j]);  // 套用自定義的按鈕樣式

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
                pictureBox.Image = crocEsc[rnd.Next(0, 3)];
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
            File.AppendAllText("charts.txt", $"Name: {playerName}, Time: {elapsedTime:hh\\:mm\\:ss}\n");

            // 更新排行榜
            DisplayCharts();

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
