using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ApplicationForNIR
{
    class Helper
    {
        private static SortedList<int, List<Pair>> allEdges;

        static Helper()
        {
            allEdges = new SortedList<int, List<Pair>>();

            for (int k = 3; k <= 11; k++)
            {
                List<Pair> res = new List<Pair>();

                for (int j = 0; j < k; j++)
                {
                    for (int i = 0; i < k; i++)
                    {
                        if (i < j)
                        {
                            res.Add(new Pair(i, j));
                        }
                    }
                }

                allEdges.Add(k, res);
            }
        }

        /// <summary>
        /// Convert integer number to 6-bit string
        /// </summary>
        private static String NumberToBinaryString(int number)
        {
            var res = Convert.ToString(number, 2);
            while (res.Length < 6)
            {
                res = res.Insert(0, "0");
            }

            return res;
        }

        /// <summary>
        /// Get edges of graph
        /// </summary>
        private static List<Pair> GetExistedEdges(String graph6)
        {
            int n = graph6[0] - 63;
            List<Pair> all = allEdges[n];

            List<Pair> res = new List<Pair>();
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i < graph6.Length; i++)
            {
                sb.Append(NumberToBinaryString(graph6[i] - 63));
            }

            for (int j = 0; j < sb.Length; j++)
            {
                if (sb[j] == '1')
                {
                    res.Add(all[j]);
                }
            }

            return res;
        }

        /// <summary>
        /// Decode g6 format to matrix
        /// </summary>
        public static int[,] DecodeGraph6(String graph6)
        {
            int n = graph6[0] - 63;
            var edges = GetExistedEdges(graph6);

            int[,] res = new int[n, n];

            foreach (Pair p in edges)
            {
                res[p.First, p.Second] = 1;
                res[p.Second, p.First] = 1;
            }

            return res;
        }

        /// <summary>
        /// Convert vector to string format
        /// </summary>
        public static string GetStringVector(List<int> vector)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("(");

            for (int i = 0; i < vector.Count; i++)
            {
                sb.Append(vector[i]).Append(", ");
            }

            sb = sb.Remove(sb.Length - 2, 2);
            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// Comparator
        /// </summary>
        public static int CompareListByCount(List<int> a, List<int> b)
        {
            if (a.Count > b.Count)
            {
                return 1;
            }

            if (a.Count < b.Count)
            {
                return -1;
            }

            return a[1].CompareTo(b[1]);
        }

        /// <summary>
        /// Print matrix to console
        /// </summary>
        private static void PrintMatrix(int[,] res)
        {
            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    Console.Write(res[i, j]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Print matrix to stream writer
        /// </summary>
        private static void PrintMatrix(int[,] res, StreamWriter sw)
        {
            sw.WriteLine();

            for (int i = 0; i < res.GetLength(0); i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    sw.Write(res[i, j]);
                }
                sw.WriteLine();
            }

            sw.WriteLine();
        }
    }
}
