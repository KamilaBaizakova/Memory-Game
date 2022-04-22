using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Memory_Game
{
    public partial class Form1 : Form
    {
        public static List<Player> PlayerLeaderBoard = new List<Player>();

        public Settings configuration = new Settings() { Cards = 50, HideTimeout = 1, Height = 600, Width = 800, Columns = 10, PairInterval = 1000, ShrinkModifier = 2, FontSize = 30F };
        
        public Form1()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Player player = new Player();
            var nameFrm = new Form3(player);
            nameFrm.Location = Location;
            nameFrm.StartPosition = FormStartPosition.CenterScreen;
            nameFrm.FormClosing += delegate {
                var gameFrm = new Form2(configuration, player);
                gameFrm.Location = Location;
                gameFrm.StartPosition = FormStartPosition.Manual;
                gameFrm.FormClosing += delegate { Show(); };
                gameFrm.Show();
                Hide();
            };
            nameFrm.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            configuration.Cards = 50;
            configuration.Columns = 10;
            configuration.ShrinkModifier = 2;
            configuration.FontSize = 30F;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            configuration.Cards = 100;
            configuration.Columns = 10;
            configuration.ShrinkModifier = 3;
            configuration.FontSize = 20F;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            configuration.Cards = 150;
            configuration.Columns = 15;
            configuration.ShrinkModifier = 3;
            configuration.FontSize = 18F;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            configuration.HideTimeout = (int) numericUpDown1.Value * 60000;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            configuration.PairInterval = (int) numericUpDown4.Value * 1000;
        }
    }
}
