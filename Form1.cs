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
using System.IO;
using System.Diagnostics;

namespace Игра_в_15
{
    public partial class MainForm : Form
    {
        readonly Random random = new Random();

        public const int NUMBEROFBLOCKSINLINE = 4;
        public const int NUMBEROFBLOCKS = NUMBEROFBLOCKSINLINE * NUMBEROFBLOCKSINLINE;
        int countSteps = 0;

        List<int> resh = new List<int>();
        int id;
        int algFind = 1; //переменная, отвечающая за выбор алгоритма поиска 1 - A* , 0 - жадный

        List<int> tabelGame = new List<int>(NUMBEROFBLOCKS);
        public MainForm()
        {
            InitializeComponent();
            GenerateTabel();
            ImageTabel();

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
                timer1.Stop();
                if (MessageBox.Show("Поздравляю, вы выйграли. \nКоличество ходов: " + countSteps + " \nХотите продолжить? ", "Поздравляю", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    tableLayoutPanel.Enabled = true;
                    GenerateTabel();
                    ImageTabel();
                    countSteps = 0;
                    ShowAlg();

                }
                else
                    Close();
            }
        }

        private void ShowAlg()
        {
            ToolStripMenuItemAlgA.Enabled = true;
            ToolStripMenuItemAlgG.Enabled = true;
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

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !timer1.Enabled)
            {
                ShowInfoTabel();
                HideAlghoritm();
                StartAlghoritm();


            }
            if (e.KeyCode == Keys.Tab)
            {
                Test.OutputResult();
            }
        }

        private void HideAlghoritm()
        {
            ToolStripMenuItemAlgA.Enabled = false;
            ToolStripMenuItemAlgG.Enabled = false;
        }

        private void ShowInfoTabel()
        {
            if (algFind == 1)
                MessageBox.Show("Алгоритм A* может находить решение игры в 15 долгое время.\n Пожалуйста, дождитесь конца работы алгоритма.", "Предупреждение");
            tableLayoutPanel.Enabled = false;
        }

        private void StartAlghoritm()
        {
            Algoritm algoritm = new Algoritm();
            algoritm.TabelGame = tabelGame;
            algoritm.ChooseAlgoritm = algFind;
            resh = algoritm.AStar();
            id = 0;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            PictureBox picture = (PictureBox)Controls.Find("picture" + resh[id++], true)[0];
            MouseEventArgs e1 = new MouseEventArgs(MouseButtons.Left, 1, 1, 1, 1);
            picture1_MouseClick(picture, e1);
        }

        private void ToolStripMenuItemMiddle_Click(object sender, EventArgs e)
        {
            timer1.Interval = 200;
            ToolStripMenuIteSlow.Checked = false;
            ToolStripMenuItemMiddle.Checked = true;
            ToolStripMenuItemFast.Checked = false;
            ToolStripMenuItemMomentum.Checked = false;
        }

        private void ToolStripMenuIteSlow_Click(object sender, EventArgs e)
        {
            timer1.Interval = 400;
            ToolStripMenuIteSlow.Checked = true;
            ToolStripMenuItemMiddle.Checked = false;
            ToolStripMenuItemFast.Checked = false;
            ToolStripMenuItemMomentum.Checked = false;
        }

        private void ToolStripMenuItemFast_Click(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            ToolStripMenuIteSlow.Checked = false;
            ToolStripMenuItemMiddle.Checked = false;
            ToolStripMenuItemFast.Checked = true;
            ToolStripMenuItemMomentum.Checked = false;
        }

        private void ToolStripMenuItemMomentum_Click(object sender, EventArgs e)
        {
            timer1.Interval = 50;
            ToolStripMenuIteSlow.Checked = false;
            ToolStripMenuItemMiddle.Checked = false;
            ToolStripMenuItemFast.Checked = false;
            ToolStripMenuItemMomentum.Checked = true;
        }

        private void ToolStripMenuItemAlgA_Click(object sender, EventArgs e)
        {
            algFind = 1;
            ToolStripMenuItemAlgA.Checked = true;
            ToolStripMenuItemAlgG.Checked = false;
        }

        private void ToolStripMenuItemAlgG_Click(object sender, EventArgs e)
        {
            algFind = 0;
            ToolStripMenuItemAlgA.Checked = false;
            ToolStripMenuItemAlgG.Checked = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Игра в 15.\n" +
                "Приложение предназначено для отоброжения выполнения алгоритма A* и жадного алгоритма.\n" +
                "Для начала работы выбранного алгоритма следует нажать Enter.\n" +
                "Так же программа имеет возможность ручного решения игры в 15.\n" +
                "Колпащиков Алексей. Студент ФИб-2301 Вятгу.", "Справка");
        }
    }
}