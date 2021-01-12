using System;
using System.Collections.Generic;
using System.Linq;

namespace Игра_в_15
{
    public class Algoritm
    {
        struct GamePosition
        {
            public List<int> tabel;
            public List<GamePosition> parent;
            public int index;
            public GamePosition(List<int> t, List<GamePosition> p, int i)
            {
                tabel = t;
                parent = p;
                index = i;
            }
        }

        static int numberOfBlocks = MainForm.NUMBEROFBLOCKS;
        static int numberOfBlockInLine = MainForm.NUMBEROFBLOCKSINLINE;
        List<GamePosition> allResul = new List<GamePosition>();
        GamePosition element;
        int indexNull;
        int indexBlock; // номер блока, который мы будем передвигать
        int[] tabeGame = new int[numberOfBlocks];
        int temp;
        int count = 0;
        public List<int> TabelGame { get; set; }

        public int ChooseAlgoritm { get; set; } // выбранный вид алгоритма 1 - алгоритм A*, 0 - Жадный алгоритм

        private List<GamePosition> GenerateNeighbords(GamePosition tree)
        {
            allResul.Clear();
            indexNull = tree.tabel.IndexOf(numberOfBlocks) + 1;
            indexBlock = indexNull + numberOfBlockInLine;
            element.parent = new List<GamePosition>() { tree };
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

        public List<int> AStar()
        {
            long after = 0;
            long before = GC.GetTotalMemory(true); //для тестирования
            GamePosition root = new GamePosition(TabelGame, new List<GamePosition>(), -1);
            PriorityQueue<GamePosition, int> priorityQueue = new PriorityQueue<GamePosition, int>();
            priorityQueue.Enqueue(root, 0);
            GamePosition current = root;
            List<GamePosition> neighbors;
            Dictionary<List<int>, int> cost = new Dictionary<List<int>, int>(new ListComparer())
            {
                [root.tabel] = 0
            };
            int g;
            int prior;
            while (!priorityQueue.Empty())
            {
                current = priorityQueue.Dequeue();
                if (IsGameOver(current.tabel))
                {
                    after = GC.GetTotalMemory(true); // для тестирования
                    priorityQueue.Clear();
                }
                else
                {
                    neighbors = GenerateNeighbords(current);
                    foreach (var neighbor in neighbors)
                    {
                        g = cost[current.tabel] + ChooseAlgoritm;
                        if (!cost.ContainsKey(neighbor.tabel) || (g < cost[neighbor.tabel]))
                        {
                            cost[neighbor.tabel] = g;
                            prior = g + H(neighbor);
                            priorityQueue.Enqueue(neighbor, prior);
                        }
                    }
                }
            }

            List<int> resultIndex = new List<int>();
            GamePosition f = current;
            while (f.parent.Count != 0)
            {
                resultIndex.Add(f.index);
                f = f.parent[0];
            }
            Test.len = cost.Count();
            Test.memory = (after - before) / (1024 * 1024);
            cost.Clear();
            resultIndex.Reverse();
            return resultIndex;
        }
        private int H(GamePosition f)
        {
            return ManhDist(f) + CountBlocksIsOutOfPlace(f) + LastMove(f); //+ CornerTitels(f);
        }

        private int CountBlocksIsOutOfPlace(GamePosition f)
        {
            count = 0;
            for (int i = 0; i < f.tabel.Count; i++)
                if (f.tabel[i] != i + 1)
                    count++;
            return count;
        }
        private int ManhDist(GamePosition f)
        {
            int dist = 0;
            for (int i = 0; i < numberOfBlocks; i++)
            {
                dist += ManhDistMatrix(f.tabel[i] - 1, i);
            }
            return dist;
        }
        private int ManhDistMatrix(int first, int second)
        {
            return Math.Abs(first % numberOfBlockInLine - second % numberOfBlockInLine) + Math.Abs(first / numberOfBlockInLine - second / numberOfBlockInLine);
        }
        private int LastMove(GamePosition f)
        {
            if (f.tabel[numberOfBlocks - 1] == numberOfBlocks - 1 || f.tabel[numberOfBlocks - 1] == numberOfBlocks - numberOfBlockInLine)
                return 0;
            return 2;
        }
        private int CornerTitels(GamePosition f)
        {
            int count = 0;
            if ((f.tabel[2] == 3 || f.tabel[7] == 8) && f.tabel[3] != 4)
                count += 2;
            if (f.tabel[7] == 8 && f.tabel[3] != 4)
                count += 2;
            if ((f.tabel[1] == 2 || f.tabel[5] == 6) && f.tabel[0] != 1)
                count += 2;
            if (f.tabel[5] == 6 && f.tabel[0] != 1)
                count += 2;
            if ((f.tabel[8] == 9 || f.tabel[13] == 14) && f.tabel[12] != 13)
                count += 2;
            if (f.tabel[13] == 14 && f.tabel[12] != 13)
                count += 2;
            return count;
        }
    }
}
