using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphSolver
{
    class Program
    {
        private static int[] next;
        private static int curCity, N;
        private static double curLen;
        private static double[] s;
        private static List<int> visited = new List<int>();
        private static List<string> Graph = new List<string>();

        /// <summary>
        /// Читаем файл с графом
        /// </summary>
        private static void Read()
        {
            try
            {
                using (var sr = new StreamReader("Output.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        var str = sr.ReadLine();
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
            Console.WriteLine("Решаем...");
            Console.WriteLine("Идём из вершины 1...");
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
                Console.WriteLine(string.Format("Найден минимальный путь из вершины {0} в вершину {1} - {2} км", curCity, node, min));
                next[curCity] = node;
                curLen += min;
                s[i] = 100 * (i - 1) - curLen - GetFirstLen(node);
                Console.WriteLine(string.Format("Прибыль от посещения {0} городов: {1} р.", i, s[i]));
                curCity = node;
                visited.Add(node);
            }
            Console.WriteLine("Города проверены, поиск решения...");

            var max = Double.MinValue;
            var count = -1;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] > max)
                {
                    max = s[i];
                    count = i;
                }
            }
            Console.WriteLine("Решение найдено!");
            Console.WriteLine(string.Format("Максимальная выгода: {0}", max));
            Console.WriteLine(string.Format("Городов нужно посетить: {0}", count));
            var path = "1";
            var index = 1;
            for (int i = 0; i < count; i++)
            {
                path += "->" + Convert.ToString(next[index]);
                index = next[index];
            }
            path += "->1";
            Console.WriteLine("Оптимальный путь:");
            Console.WriteLine(path);
        }

        static void Main(string[] args)
        {
            Read();
            if (Graph.Count == 0)
            {
                Console.WriteLine("Граф пуст!");
                return;
            }
            else
                Console.WriteLine("Граф считан");
            N = Graph[0].Split(' ').Length;
            Console.WriteLine(string.Format("Размерность графа: {0}",N));
            s = new double[N+1];
            next = new int[N+1];
            Work();
            Console.ReadLine();
        }
    }
}
