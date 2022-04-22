using System;
using System.Windows.Forms;

namespace Memory_Game
{
    public partial class Form3 : Form
    {
        private Player player { get; set; }
        public Form3(Player player)
        {
            InitializeComponent();
            this.player = player;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            player.Username = textBox1.Text;
            Close();
        }
    }
}
