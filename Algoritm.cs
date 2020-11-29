using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Игра_в_15
{
    public class Algoritm
    {
        struct Tree
        {
            public List<int> tabel;
            public List<int> parent;
            public int index;
            public int countResh;
        }

        int numberOfBlocks = MainForm.NUMBEROFBLOCKS;
        int numberOfBlockInLine = MainForm.NUMBEROFBLOCKSINLINE;
        static public List<int> TabelGame { get; set; }

        public void GenerateTree()
        {
            List<Tree> allResul = new List<Tree>();
            Tree root;
            root.parent = null;
            root.index = -1;
            root.tabel = TabelGame;
            root.countResh = 0;
            allResul.Add(root);
            int indexNull;
            int indexBlock; // номер блока, который мы будем передвигать
            Tree element;
            for (int i = 0; !IsGameOver(allResul[i].tabel); i++)
            {
                indexNull = allResul[i].tabel.IndexOf(numberOfBlocks) + 1;
                indexBlock = indexNull + numberOfBlockInLine;
                if (indexBlock < numberOfBlocks)
                {
                    element.parent = allResul[i].tabel;
                    element.index = indexBlock;
                    element.tabel = MoveBlockInTabel(indexNull, indexBlock, allResul[i].tabel);
                    element.countResh = allResul[i].countResh + 1;
                    if (element.countResh < 81)
                        if (Realy(element, allResul))
                            allResul.Add(element);
                }
                indexBlock = indexNull - numberOfBlockInLine;
                if (indexBlock > 0)
                {
                    element.parent = allResul[i].tabel;
                    element.index = indexBlock;
                    element.tabel = MoveBlockInTabel(indexNull, indexBlock, allResul[i].tabel);
                    element.countResh = allResul[i].countResh + 1;
                    if (element.countResh < 81)
                        if (Realy(element, allResul))
                        allResul.Add(element);
                }
                if (indexNull % numberOfBlockInLine != 0)
                {
                    indexBlock = indexNull + 1;
                    element.parent = allResul[i].tabel;
                    element.index = indexBlock;
                    element.tabel = MoveBlockInTabel(indexNull, indexBlock, allResul[i].tabel);
                    element.countResh = allResul[i].countResh + 1;
                    if (element.countResh < 81)
                        if (Realy(element, allResul))
                        allResul.Add(element);
                }
                if (indexNull % numberOfBlockInLine != 1)
                {
                    indexBlock = indexNull - 1;
                    element.parent = allResul[i].tabel;
                    element.index = indexBlock;
                    element.tabel = MoveBlockInTabel(indexNull, indexBlock, allResul[i].tabel);
                    element.countResh = allResul[i].countResh + 1;
                    if (element.countResh < 81)
                        if (Realy(element, allResul))
                        allResul.Add(element);
                }
            }

        }
        private List<int> MoveBlockInTabel(int indexNull, int tag, List<int> tabelGame)
        {
            List<int> tabeGame = new List<int>();
            for (int i = 0; i < tabelGame.Count; i++)
            {
                tabeGame.Add(tabelGame[i]);
            }
            int temp = tabeGame[indexNull - 1];
            tabeGame[indexNull - 1] = tabeGame[tag - 1];
            tabeGame[tag - 1] = temp;
            return tabeGame;
        }

        private bool IsGameOver(List<int> tabelGame)
        {
            int count = 1;
            for (int i = 0; i < MainForm.NUMBEROFBLOCKS; i++)
                if (tabelGame[i] != count++)
                    return false;
            return true;
        }
        private bool Realy(Tree first, List<Tree> all)
        {
            for (int i = 0; i < all.Count; i++)
            {
                if (all[i].tabel == first.tabel)
                    return false;
            }
            return true;
        }
        //public void BFS(List<Tree> allResul)
        //{
        //    Queue<Tree> Q = new Queue<Tree>();
        //    List<int> result;
        //    Q.Enqueue(allResul[0]);
        //    while (!IsGameOver(Q.Peek().tabel))
        //    {

        //    }

        //}
    }
}
