using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace GraphSolver
{
    class Program
    {
        private static int name;
        private static int[] next;
        private static int curCity, N;
        private static double curLen;
        private static double[] s;
        private static List<int> visited = new List<int>();
        private static List<string> Graph = new List<string>();

        /// <summary>
        /// Читаем файл с графом
        /// </summary>
        private static void Read(string name)
        {
            try
            {
                using (var sr = new StreamReader(name))
                {
                    N = Convert.ToInt32(sr.ReadLine());
                    for (int i = 0; i < N; i++)
                    {
                        var str = sr.ReadLine().Replace('.',',');
                        if (str[str.Length-1] == ' ')
                            str = str.Remove(str.Length-1, 1);
                        Graph.Add(str);
                    }
                }
                if (Graph[Graph.Count - 1] == "")
                    Graph.RemoveAt(Graph.Count - 1);
            }
            catch
            {
                Console.WriteLine("Ошибка чтения!");
            }
        }

        /// <summary>
        /// Проверяет, посещён ли город
        /// </summary>
        /// <param name="num">номер города</param>
        /// <returns>Ответ</returns>
        private static bool Visited(int num)
        {
            var i = visited.IndexOf(num);
            if (i == -1)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Получает расстояние от текущей вершины до первой
        /// </summary>
        /// <param name="num">Текущая вершина</param>
        /// <returns>Расстояние</returns>
        private static double GetFirstLen(int num)
        {
            var len = Graph[num-1].Split(' ')[0];
            return Convert.ToDouble(len);
        }

        /// <summary>
        /// Вычисляем
        /// </summary>
        private static void Work()
        {
            s[1] = 0.0;
            curCity = 1;
            visited.Add(1);
            curLen = 0;

            for (int i = 2; i <= N; i++)
            {
                int node = 0;
                var str = Graph[curCity-1].Split(' ');
                var min = Double.MaxValue;
                for (int j = 0; j < str.Length; j++)
                {
                    if (!Visited(j+1))
                    {
                        var value = Convert.ToDouble(str[j]);
                        if (min > value)
                        {
                            min = value;
                            node = j + 1;
                        }
                    }
                }
                next[curCity] = node;
                curLen += min;
                s[i] = 100 * (i - 1) - curLen - GetFirstLen(node);
                curCity = node;
                visited.Add(node);
            }

            var max = Double.MinValue;
            var count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] > max)
                {
                    max = s[i];
                    count = i - 1;
                }
            }
            Write(max, count);
            
        }

        private static void Write(double max, int count)
        {
            var str = "output\\Output" + Convert.ToString(name) + ".txt";
            using (var sw = new StreamWriter(str))
            {
                var path = "0";
                var index = 1;
                sw.WriteLine(max);
                for (int i = 0; i < count; i++)
                {
                    path += " " + Convert.ToString(next[index] - 1);
                    index = next[index];
                }
                path += " 0";
                sw.WriteLine(path);
            }
        }

        static void Main(string[] args)
        {
            var count = Convert.ToInt32(Console.ReadLine());
            for (int i = 1; i <= count; i++)
            {
                visited.Clear();
                Graph.Clear();
                name = i;
                var str = "input\\Input" + Convert.ToString(i) + ".txt";
                Read(str);
                if (Graph.Count == 0)
                {
                    Console.WriteLine("Граф пуст!");
                    continue;
                }
                else
                    Console.WriteLine("Граф считан");
                s = new double[N + 1];
                next = new int[N + 1];
                Work();
            }
            Console.WriteLine("Готово!");
            Console.ReadLine();
        }
    }
}
