using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Игра_в_15
{
    public partial class MainForm : Form
    {
        Graphics grah;
        Font font = new Font("Arial", 26);
        Random random = new Random();

        const int NUMBEROFBLOCKS = 15;
        public MainForm()
        {
            InitializeComponent();
            grah = CreateGraphics();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            grah.DrawString("1", font, Brushes.White, pictureBox1.Size.Width / 2, pictureBox1.Size.Height / 2);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            grah.DrawString("1", font, Brushes.White, pictureBox1.Size.Width / 2, pictureBox1.Size.Height / 2);
            pictureBox1.Visible = false;
        }

        private void GenerateGameTabel() ?????????????????????????????????????????
        {
            do
            {
                int[] numbers = new int[NUMBEROFBLOCKS];
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = i + 1;
                }
                
            } while (true);
        }

        private void ImageBlocks(int[][] gameTabel)
        {

        }
    }
}
