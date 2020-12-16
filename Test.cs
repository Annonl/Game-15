using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace Игра_в_15
{
    class Test
    {
        static int id;
        static List<int> tabelGame = new List<int>();
        static List<int> resh = new List<int>();
        static public long memory;
        static public int len;
        static public void OutputResult()
        {
            StreamReader reader = new StreamReader("tabels.txt");
            Algoritm algoritm = new Algoritm();
            int count = 0;
            long sec = 0;
            Stopwatch stopwatch = new Stopwatch();
            using (StreamWriter writer = new StreamWriter("out.txt"))
            {
                for (int i = 0; i < 5; i++)
                {
                    string str = reader.ReadLine();
                    List<int> lst = new List<int>(str.Split(' ').Select(int.Parse));
                    tabelGame = lst;
                    Algoritm.TabelGame = tabelGame;
                    stopwatch.Start();
                    resh = algoritm.AStar();
                    stopwatch.Stop();
                    id = resh.Count;
                    count += id;
                    sec += stopwatch.ElapsedMilliseconds;
                    writer.Write("Time - {0}, Count - {1}, Len(cost) - {2}, Memory(MB) - {3}", stopwatch.ElapsedMilliseconds, id, len, memory);
                    stopwatch.Reset();
                    writer.WriteLine();
                }
                writer.WriteLine(count / 5 + " " + sec / 5);
            }
            reader.Close();
        }
    }
}
