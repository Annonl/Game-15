using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Игра_в_15
{
    public partial class MainForm : Form
    {
        readonly Random random = new Random();

        const int NUMBEROFBLOCKSINLINE = 4;
        const int NUMBEROFBLOCKS = NUMBEROFBLOCKSINLINE * NUMBEROFBLOCKSINLINE;

        int[,] tabelGame = new int[NUMBEROFBLOCKSINLINE, NUMBEROFBLOCKSINLINE];
        public MainForm()
        {
            InitializeComponent();
            GenerateTabel();
            ImageTabel();
        }

        private void GenerateTabel()
        {
            int index;
            List<int> numbers = new List<int>();
            do
            {
                numbers.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 });
                for (int i = 0; i < NUMBEROFBLOCKSINLINE; i++)
                {
                    for (int j = 0; j < NUMBEROFBLOCKSINLINE; j++)
                    {
                        index = random.Next(numbers.Count());
                        tabelGame[i, j] = numbers[index];
                        numbers.RemoveAt(index);
                    }
                }
            } while (!CheckState(tabelGame));
        }
        private bool CheckState(int[,] tabel)
        {
            int n = 0;
            int e = 0;
            List<int> a = new List<int>();
            for (int i = 0; i < NUMBEROFBLOCKSINLINE; i++)
            {
                for (int j = 0; j < NUMBEROFBLOCKSINLINE; j++)
                {
                    a.Add(tabel[i, j]);
                }
            }
            for (int i = 0; i < a.Count(); i++)
            {
                if (a[i] == 16)
                    e = (i / NUMBEROFBLOCKSINLINE) + 1;
                //else if (i == 0)
                //    continue;
                else
                {
                    for (int j = i + 1; j < a.Count(); j++)
                    {
                        if (a[j] < a[i] )//|| a[j] == 16)
                            n++;
                    }
                }
            }
            return (n + e) % 2 == 0;
        }
        private void ImageTabel()
        {
            int count = 1;
            List<Image> image = new List<Image>();
            for (int i = 1; i <= NUMBEROFBLOCKS; i++)
            {
                image.Add(((PictureBox)Controls.Find("picture" + i, true)[0]).Image);
            }
            for (int i = 0; i < NUMBEROFBLOCKSINLINE; i++)
            {
                for (int j = 0; j < NUMBEROFBLOCKSINLINE; j++)
                {
                    //Image temp = ((PictureBox)Controls.Find("picture" + count, true)[0]).Image;
                    ((PictureBox)Controls.Find("picture" + count++, true)[0]).Image = image[tabelGame[i, j] - 1];
                }
            }
        }

        private void picture1_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox start = (PictureBox)sender;
            int count = Convert.ToInt32(start.Tag);
            for (int i = 1; i <= NUMBEROFBLOCKS; i++)
            {
                if (((PictureBox)Controls.Find("picture" + i, true)[0]).Image == null && CheckVoidPlace(i, count))
                {
                    Image temp = start.Image;
                    start.Image = null;
                    ((PictureBox)Controls.Find("picture" + i, true)[0]).Image = temp;
                    MoveBlockInTabel(i, count);
                    i = NUMBEROFBLOCKS;
                }
            }
            if (IsGameOver())
            {
                if (MessageBox.Show("Поздравляю, вы выйграли. Хотите продолжить?", "Поздравляю", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    GenerateTabel();
                    ImageTabel();
                }
                else
                {
                    Close();
                }
            }
        }

        private bool IsGameOver()
        {
            int count = 1;
            for (int i = 0; i < NUMBEROFBLOCKSINLINE; i++)
                for (int j = 0; j < NUMBEROFBLOCKSINLINE; j++)
                    if (tabelGame[i, j] != count++)
                        return false;

            return true;
        }

        private bool CheckVoidPlace(int indexNull, int tag)
        {
            if (tag - NUMBEROFBLOCKSINLINE == indexNull || tag + NUMBEROFBLOCKSINLINE == indexNull || (tag + 1 == indexNull && tag % NUMBEROFBLOCKSINLINE != 0) || (tag - 1 == indexNull && tag % NUMBEROFBLOCKSINLINE != 1))
                return true;
            return false;
        }
        private void MoveBlockInTabel(int indexNull, int tag)
        {
            int indexForNull1 = indexNull % NUMBEROFBLOCKSINLINE == 0 ? (indexNull / NUMBEROFBLOCKSINLINE) - 1 : indexNull / NUMBEROFBLOCKSINLINE;
            int indexForNull2 = indexNull % NUMBEROFBLOCKSINLINE == 0 ? NUMBEROFBLOCKSINLINE - 1 : indexNull % NUMBEROFBLOCKSINLINE - 1;
            int indexForTag1 = tag % NUMBEROFBLOCKSINLINE == 0 ? (tag / NUMBEROFBLOCKSINLINE) - 1 : tag / NUMBEROFBLOCKSINLINE;
            int indexForTag2 = tag % NUMBEROFBLOCKSINLINE == 0 ? NUMBEROFBLOCKSINLINE - 1 : tag % NUMBEROFBLOCKSINLINE - 1;
            int temp = tabelGame[indexForNull1, indexForNull2];
            tabelGame[indexForNull1, indexForNull2] = tabelGame[indexForTag1, indexForTag2];
            tabelGame[indexForTag1, indexForTag2] = temp;
        }
    }
}