using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace Memory_Game
{
    public partial class Form2 : Form
    {
        Stopwatch stopwatch = new Stopwatch();
        Random random = new Random();

        Player player;

        string icons = "qqwweerrttyyuuiioopp[[]]aassddffgghhjjkkll;;''zzxxccvvbbnnmm,,..//" +
            "!!@@##$$%%^^**(())__++AASSDDFFGGHHJJKKLLQQWWEERRTTYYUUIIOOPPZZXXCCVVBBNNMM" +
            "11223344556677889900";

        int mistakes = 0;

        Label firstClicked = null;

        Label secondClicked = null;

        public Form2(Settings configuration, Player player)
        {
            this.player = player;

            InitializeComponent();

            SetFormSize(configuration);

            icons = icons.Substring(0, configuration.Cards);

            BuildCards(configuration);

            AssignIconsToSquares();

            timer1.Interval = configuration.PairInterval;

            if (configuration.HideTimeout == 0)
            {
                timer2_Tick(new object(), new EventArgs());
                stopwatch.Start();
            } 
            else
            {
                timer2.Interval = configuration.HideTimeout;
                timer2.Start();
                stopwatch.Start();
            }
        }

        private void BuildCards(Settings configuration)
        {
            int cardsCount = configuration.Cards;
            tableLayoutPanel1.ColumnCount = configuration.Columns;
            while (cardsCount != 0)
            {
                if (cardsCount == Controls.Count - 3)
                {
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                }
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                Label card = new Label();
                card.BackColor = Color.LemonChiffon;
                card.Width /= configuration.ShrinkModifier;
                card.Height = card.Width;
                card.BorderStyle = BorderStyle.FixedSingle;
                card.Font = new Font("Webdings", configuration.FontSize, FontStyle.Bold, GraphicsUnit.Point, ((byte)(2)));
                card.Text = "e";
                card.TextAlign = ContentAlignment.MiddleCenter;
                card.Click += new EventHandler(label1_Click);
                tableLayoutPanel1.Controls.Add(card);
                cardsCount -= 1;
            }
            tableLayoutPanel1.RowCount += 1;
        }

        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label card = control as Label;
                if (card != null)
                {
                    int rndNum = random.Next(icons.Length);
                    card.Text = icons.Substring(rndNum, 1);
                    card.ForeColor = Color.Black;
                    icons = icons.Remove(rndNum, 1);
                }
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

            if (timer1.Enabled == true)
                return;

            Label clickedCard = sender as Label;

            if (clickedCard != null)
            {
                if (clickedCard.ForeColor == Color.Black)
                    return;

                if (firstClicked == null)
                {
                    firstClicked = clickedCard;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                secondClicked = clickedCard;
                secondClicked.ForeColor = Color.Black;

                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            if (firstClicked.Text == secondClicked.Text)
            {
                firstClicked.BorderStyle = BorderStyle.None;
                secondClicked.BorderStyle = BorderStyle.None;
                firstClicked.Text = "";
                secondClicked.Text = "";
                firstClicked.ForeColor = tableLayoutPanel1.BackColor;
                secondClicked.ForeColor = tableLayoutPanel1.BackColor;
                firstClicked.BackColor = tableLayoutPanel1.BackColor;
                secondClicked.BackColor = tableLayoutPanel1.BackColor;
                firstClicked.Enabled = false;
                secondClicked.Enabled = false;
                firstClicked = null;
                secondClicked = null;
                CheckForWinner();
                return;
            }
            ++mistakes;
            firstClicked = null;
            secondClicked = null;
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label card = control as Label;

                if (card != null)
                {
                    if (card.Enabled == true)
                        return;
                }
            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}",
            ts.Hours, ts.Minutes, ts.Seconds);

            player.Mistakes = mistakes;
            player.ElapsedTime = elapsedTime;
            player.EffectiveTime = ts + TimeSpan.FromSeconds(5 * mistakes);
            Form1.PlayerLeaderBoard.Add(player);
            string message = "";
            Form1.PlayerLeaderBoard = Form1.PlayerLeaderBoard.OrderBy(e => e.EffectiveTime).ToList();
            int count = 0;
            foreach (Player player in Form1.PlayerLeaderBoard)
            {
                ++count;
                message += $"{count}. {player.Username} - Mistakes: {player.Mistakes} - Time: {player.ElapsedTime}\n";
            }
            MessageBox.Show(message, "Leaders");
            Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label card = control as Label;
                if (card != null)
                {
                    card.ForeColor = card.BackColor;
                }
            }

            timer2.Stop();
        }

        private void SetFormSize(Settings configuration)
        {
            Size = new Size(configuration.Width, configuration.Height);
        }

        private void Form3_Resize(object sender, EventArgs e)
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label card = control as Label;
                card.Font = new Font("Webdings", ClientSize.Height / 20);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = (int) numericUpDown1.Value * 1000;
        }
    }
}
