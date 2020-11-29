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

        public const int NUMBEROFBLOCKSINLINE = 4;
        public const int NUMBEROFBLOCKS = NUMBEROFBLOCKSINLINE * NUMBEROFBLOCKSINLINE;
        int countSteps = 0;

        List<int> tabelGame = new List<int>(NUMBEROFBLOCKS);
        public MainForm()
        {
            InitializeComponent();
            GenerateTabel();
            ImageTabel();
            //Algoritm algoritm = new Algoritm();
            //Algoritm.TabelGame = tabelGame;
            //algoritm.GenerateTree();

        }

        private void GenerateTabel() // Генерирует игровое поле
        {
            int index;
            List<int> numbers = new List<int>();
            do
            {
                tabelGame.Clear();
                for (int i = 1; i <= NUMBEROFBLOCKS; i++)
                    numbers.Add(i);
                for (int i = 0; i < NUMBEROFBLOCKS; i++)
                {
                    index = random.Next(numbers.Count());
                    tabelGame.Add(numbers[index]);
                    numbers.RemoveAt(index);
                }
            } while (!CheckState(tabelGame));
        }
        private bool CheckState(List<int> tabel) //Проверка сгенерированного поля на решаемость
        {
            int n = 0; //для каждого числа количество чисел, меньших его и стоящих после него, если смотреть справа налево сверху вниз
            int e = 0; //ряд, в котором находится пустое поле
            for (int i = 0; i < NUMBEROFBLOCKS; i++)
            {
                if (tabel[i] == NUMBEROFBLOCKS)
                    e = (i / NUMBEROFBLOCKSINLINE) + 1;
                else
                {
                    for (int j = i + 1; j < NUMBEROFBLOCKS; j++)
                    {
                        if (tabel[j] < tabel[i])
                            n++;
                    }
                }
            }
            return (n + e) % 2 == 0; //если сумма чётная, то существует решение
        }
        private void ImageTabel()
        {
            int count = 1;
            List<Image> image = new List<Image>();
            for (int i = 1; i <= NUMBEROFBLOCKS; i++)
                image.Add(((PictureBox)Controls.Find("picture" + i, true)[0]).Image);

            for (int i = 0; i < NUMBEROFBLOCKS; i++)
                //if (i == NUMBEROFBLOCKS)
                //    ((PictureBox)Controls.Find("picture" + count++, true)[0]).Image = null;
                //else
                    ((PictureBox)Controls.Find("picture" + count++, true)[0]).Image = image[tabelGame[i] - 1];
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
                    countSteps++;
                }
            }
            if (IsGameOver())
            {
                if (MessageBox.Show("Поздравляю, вы выйграли. Хотите продолжить ?" + countSteps, "Поздравляю", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    GenerateTabel();
                    ImageTabel();
                    countSteps = 0;
                }
                else
                    Close();
            }
        }

        private bool IsGameOver()
        {
            int count = 1;
            for (int i = 0; i < NUMBEROFBLOCKS; i++)
                if (tabelGame[i] != count++)
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
            int temp = tabelGame[indexNull - 1];
            tabelGame[indexNull - 1] = tabelGame[tag - 1];
            tabelGame[tag - 1] = temp;
        }
    }
}