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
        Random random = new Random();

        const int NUMBEROFBLOCKS = 16;
        const int NUMBEROFBLOCKSINLINE = 4;

        int[,] tabelGame = new int[NUMBEROFBLOCKSINLINE, NUMBEROFBLOCKSINLINE];
        public MainForm()
        {
            InitializeComponent();
            GenerateTabel();
            ImageTabel();
            //MessageBox.Show(picture1.Tag.ToString());
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
            } while (CheckState(tabelGame));
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
                for (int j = i + 1; j < a.Count(); j++)
                {
                    if (a[j] < a[i])
                        n++;
                }
            }
            return (n + e) % 2 == 0;
        }
        private void ImageTabel()
        {
            int count = 1;
            for (int i = 0; i < NUMBEROFBLOCKSINLINE; i++)
            {
                for (int j = 0; j < NUMBEROFBLOCKSINLINE; j++)
                {
                    Image temp = ((PictureBox)Controls.Find("picture" + count, true)[0]).Image;
                    //((PictureBox)(Controls.Find("picture" + count++, true)[0]))
                    ((PictureBox)Controls.Find("picture" + count++, true)[0]).Image = ((PictureBox)(Controls.Find("picture" + tabelGame[i, j], true)[0])).Image;
                    //((PictureBox)Controls.Find("picture" + count, true)[0]).Tag = "" + i + j;
                    ((PictureBox)Controls.Find("picture" + tabelGame[i, j], true)[0]).Image = temp;
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
                    i = NUMBEROFBLOCKS;
                }
            }
            //if (picture5.Image == null)
            //{
            //    Image temp = start.Image;
            //    start.Image = picture5.Image;
            //    picture5.Image = temp;
            //}
            //else if (picture2.Image == null)
            //{
            //    Image temp = start.Image;
            //    picture1.Image = picture2.Image;
            //    start.Image = temp;
            //}
        }
        //private void Check
        private bool CheckVoidPlace(int i, int tag)
        {
            if (tag - NUMBEROFBLOCKSINLINE == i || tag + NUMBEROFBLOCKSINLINE == i || (tag + 1 == i && tag % NUMBEROFBLOCKSINLINE != 0) || (tag - 1 == i && tag % NUMBEROFBLOCKSINLINE != 1))
                return true;
            return false;
        }
    }
}