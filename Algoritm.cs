using System;
using System.Collections;
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
            public List<Tree> parent;
            public int index;
            //public int countResh;
        }

        static int numberOfBlocks = MainForm.NUMBEROFBLOCKS;
        int numberOfBlockInLine = MainForm.NUMBEROFBLOCKSINLINE;
        List<Tree> allResul = new List<Tree>();
        Tree element;
        int indexNull;
        int indexBlock; // номер блока, который мы будем передвигать
        int[] tabeGame = new int[numberOfBlocks];
        int temp;
        int count = 0;
        static public List<int> TabelGame { get; set; }

        //public void GenerateTree()
        //{
        //    List<Tree> allResul = new List<Tree>();
        //    Tree root;
        //    root.parent = null;
        //    root.index = -1;
        //    root.tabel = TabelGame;
        //    root.countResh = 0;
        //    allResul.Add(root);
        //    int indexNull;
        //    int indexBlock; // номер блока, который мы будем передвигать
        //    Tree element;
        //    for (int i = 0; !IsGameOver(allResul[i].tabel); i++)
        //    {
        //        indexNull = allResul[i].tabel.IndexOf(numberOfBlocks) + 1;
        //        indexBlock = indexNull + numberOfBlockInLine;
        //        if (indexBlock < numberOfBlocks)
        //        {
        //            element.parent = allResul[i].tabel;
        //            element.index = indexBlock;
        //            element.tabel = MoveBlockInTabel(indexNull, indexBlock, allResul[i].tabel);
        //            element.countResh = allResul[i].countResh + 1;
        //                if (Realy(element, allResul))
        //                    allResul.Add(element);
        //        }
        //        indexBlock = indexNull - numberOfBlockInLine;
        //        if (indexBlock > 0)
        //        {
        //            element.parent = allResul[i].tabel;
        //            element.index = indexBlock;
        //            element.tabel = MoveBlockInTabel(indexNull, indexBlock, allResul[i].tabel);
        //            element.countResh = allResul[i].countResh + 1;
        //                if (Realy(element, allResul))
        //                    allResul.Add(element);
        //        }
        //        if (indexNull % numberOfBlockInLine != 0)
        //        {
        //            indexBlock = indexNull + 1;
        //            element.parent = allResul[i].tabel;
        //            element.index = indexBlock;
        //            element.tabel = MoveBlockInTabel(indexNull, indexBlock, allResul[i].tabel);
        //            element.countResh = allResul[i].countResh + 1;
        //                if (Realy(element, allResul))
        //                    allResul.Add(element);
        //        }
        //        if (indexNull % numberOfBlockInLine != 1)
        //        {
        //            indexBlock = indexNull - 1;
        //            element.parent = allResul[i].tabel;
        //            element.index = indexBlock;
        //            element.tabel = MoveBlockInTabel(indexNull, indexBlock, allResul[i].tabel);
        //            element.countResh = allResul[i].countResh + 1;
        //                if (Realy(element, allResul))
        //                    allResul.Add(element);
        //        }
        //    }

        //}
        private List<Tree> GenerateTree(Tree tree)
        {
            allResul.Clear();
            indexNull = tree.tabel.IndexOf(numberOfBlocks) + 1;
            indexBlock = indexNull + numberOfBlockInLine;
            element.parent = new List<Tree>() { tree };
            //element.countResh = tree.countResh + 1;
            if (indexBlock <= numberOfBlocks)
            {
                element.index = indexBlock;
                element.tabel = MoveBlockInTabel(indexNull, indexBlock, tree.tabel);
                allResul.Add(element);
            }
            indexBlock = indexNull - numberOfBlockInLine;
            if (indexBlock > 0)
            {
                element.index = indexBlock;
                element.tabel = MoveBlockInTabel(indexNull, indexBlock, tree.tabel);
                allResul.Add(element);
            }
            if (indexNull % numberOfBlockInLine != 0)
            {
                indexBlock = indexNull + 1;
                element.index = indexBlock;
                element.tabel = MoveBlockInTabel(indexNull, indexBlock, tree.tabel);
                allResul.Add(element);
            }
            if (indexNull % numberOfBlockInLine != 1)
            {
                indexBlock = indexNull - 1;
                element.index = indexBlock;
                element.tabel = MoveBlockInTabel(indexNull, indexBlock, tree.tabel);
                allResul.Add(element);
            }
            return allResul;

        }
        private List<int> MoveBlockInTabel(int indexNull, int tag, List<int> tabelGame)
        {
            for (int i = 0; i < tabelGame.Count; i++)
            {
                tabeGame[i] = tabelGame[i];
            }
            temp = tabeGame[indexNull - 1];
            tabeGame[indexNull - 1] = tabeGame[tag - 1];
            tabeGame[tag - 1] = temp;
            return tabeGame.ToList<int>();
        }
        private bool IsGameOver(List<int> tabelGame)
        {
            count = 1;
            for (int i = 0; i < MainForm.NUMBEROFBLOCKS; i++)
                if (tabelGame[i] != count++)
                    return false;
            return true;
        }
        private bool Realy(Tree first, Dictionary<List<int>, int> cost)
        {
            foreach (var item in cost)
            {
                if (item.Key.SequenceEqual(first.tabel))
                    return false;
            }
            return true;
        }

        public List<int> AStar()
        {
            Tree root;
            root.parent = new List<Tree>();
            root.index = -1;
            root.tabel = TabelGame;
            //root.countResh = 0;
            PriorityQueue<Tree, int> priorityQueue = new PriorityQueue<Tree, int>();
            priorityQueue.Enqueue(root, 0);
            Tree current = root;
            List<Tree> neighbords = new List<Tree>();
            Dictionary<List<int>, int> cost = new Dictionary<List<int>, int>();
            cost[root.tabel] = 0;
            int newCost;
            int prior;
            //tabeGame = new List<int>(numberOfBlocks);
            while (!priorityQueue.Empty())
            {
                neighbords.Clear();
                current = priorityQueue.Dequeue();
                if (IsGameOver(current.tabel))
                    priorityQueue.Clear();
                else
                {
                    cost[current.tabel] = FuncForFindShortestWay(current);
                    neighbords = GenerateTree(current);
                    foreach (var next in neighbords)
                    {
                        newCost = FuncForFindShortestWay(next);
                        if (Realy(next, cost))// || (newCost < cost[next.tabel]))
                        {
                            cost[next.tabel] = newCost;
                            prior = newCost; //+ LastMove(next);
                            priorityQueue.Enqueue(next, prior);
                        }
                    }
                }
            }

            List<int> resultIndex = new List<int>();
            Tree f = current;
            while (f.parent.Count != 0)
            {
                resultIndex.Add(f.index);
                f = f.parent[0];
            }
            //resultIndex.Reverse();
            Console.WriteLine("We did it? ");
            return resultIndex;
        }
        private int FuncForFindShortestWay(Tree f)
        {
            return ManhDist(f) + CountBlocksIsOutOfPlace(f.tabel);
        }

        private int CountBlocksIsOutOfPlace(List<int> tabel)
        {
            count = 0;
            for (int i = 0; i < tabel.Count; i++)
                if (tabel[i] != i + 1)
                    count++;
            return count;
        }
        private int ManhDist(Tree f)
        {
            int dist = 0;
            for (int i = 0; i < numberOfBlocks; i++)
            {
                dist += ManhDistMatrix(f.tabel[i] - 1, i);
            }
            return dist;
        }
        private int ManhDistMatrix(int a, int b)
        {
            //if (a == 15)
            //    return Math.Abs(0 % numberOfBlockInLine - b % numberOfBlockInLine) + Math.Abs(0 / numberOfBlockInLine - b / numberOfBlockInLine);
            return Math.Abs(a % numberOfBlockInLine - b % numberOfBlockInLine) + Math.Abs(a / numberOfBlockInLine - b / numberOfBlockInLine);
        }
        private int LastMove(Tree f)
        {
            if (f.tabel[numberOfBlocks - 1] == numberOfBlocks - 1 || f.tabel[numberOfBlocks - 1] == numberOfBlocks - numberOfBlockInLine)
                return 0;
            return 2;
        }
        //public List<int> AStar()
        //{
        //    Tree root;
        //    root.index = -1;
        //    root.tabel = TabelGame;
        //    root.his = new List<int>();
        //    PriorityQueue<Tree, int> priority = new PriorityQueue<Tree, int>();
        //    priority.Enqueue(root, 0);
        //    Tree current = root;
        //    while (!priority.Empty())
        //    {
        //        current = priority.Dequeue();
        //        if (IsGameOver(current.tabel))
        //            priority.Clear();
        //        else
        //        {

        //        }
        //    }
        //    return current.his;

        //}
    }
}
