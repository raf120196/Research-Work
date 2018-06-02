using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationForNIR
{
    class HamiltonianGraphsHelper
    {
        /// <summary>
        /// Method that check is graph hamiltonian
        /// </summary>
        public static bool IsHamiltonianGraph(int[,] res)
        {
            List<bool> used = new List<bool>();
            for (int i = 0; i < res.GetLength(0); i++)
            {
                used.Add(false);
            }

            List<int> path = new List<int>();
            for (int i = 0; i < res.GetLength(0); i++)
            {
                path.Add(0);
            }

            path[0] = 0;
            bool ans = false;
            DFS(0, res, ref used, ref path, ref ans);

            return ans;
        }

        /// <summary>
        /// Custom DFS
        /// </summary>
        private static void DFS(int deepth, int[,] matr, ref List<bool> used, ref List<int> path, ref bool ans)
        {
            int v = path[deepth];
            used[v] = true;

            if (deepth == matr.GetLength(0) - 1)
            {
                if (matr[v, 0] == 1)
                {
                    ans = true;
                }
            }
            else
            {
                for (int i = 0; i < matr.GetLength(0); i++)
                {
                    if (matr[v, i] == 1 && !used[i])
                    {
                        path[deepth + 1] = i;
                        DFS(deepth + 1, matr, ref used, ref path, ref ans);
                        if (ans)
                        {
                            break;
                        }
                    }
                }
            }

            used[v] = false;
        }

        /// <summary>
        /// Ore condition
        /// </summary>
        public static bool OreCondition(int[,] matr)
        {
            int n = matr.GetLength(0);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j && matr[i, j] == 0 && (GraphHelper.GetDegree(matr, i) + GraphHelper.GetDegree(matr, j)) < n)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Custom additional function for Posha condition
        /// </summary>
        private static int F(List<int> vector, int x)
        {
            int ans = 0;

            foreach (int i in vector)
            {
                if (i <= x)
                {
                    ans++;
                }
            }

            return ans;
        }

        /// <summary>
        /// Posha condition
        /// </summary>
        public static bool PoshaCondition(List<int> vector)
        {
            int n = vector.Count;

            for (int x = 1; x < Math.Ceiling((n - 1) / 2.0); x++)
            {
                if (F(vector, x) >= x)
                {
                    return false;
                }
            }

            if (n % 2 == 1 && F(vector, (n - 1) / 2) > (n - 1) / 2)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Chvatal condition
        /// </summary>
        public static bool ChvatalCondition(List<int> vector)
        {
            int n = vector.Count;

            vector = vector.ToList();
            vector.Sort();
            vector.Insert(0, -1);

            for (int k = 1; k < n; k++)
            {
                if (vector[k] <= k && k < Math.Ceiling(n / 2.0) && vector[n - k] < n - k)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
