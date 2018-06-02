using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace ApplicationForNIR
{
    class GraphHelper
    {
        /// <summary>
        /// Get degree of vertex by matrix
        /// </summary>
        public static int GetDegree(int[,] matr, int v)
        {
            int ans = 0;

            for (int j = 0; j < matr.GetLength(1); j++)
            {
                if (matr[v, j] == 1)
                {
                    ans++;
                }
            }

            return ans;
        }

        /// <summary>
        /// Get degrees vector by matrix
        /// </summary>
        public static List<int> GetVector(int[,] matr)
        {
            int n = matr.GetLength(0);

            List<int> vector = new List<int>();
            for (int i = 0; i < n; i++)
            {
                vector.Add(GetDegree(matr, i));
            }

            vector = vector.OrderByDescending(i => i).ToList();
            return vector;
        }

        /// <summary>
        /// Method that find count of hamiltonian graphs
        /// and count of Ore graphs, Posha graphs and Chvatal graphs
        /// </summary>
        public static void GetConditionsStatistics()
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                
                using (StreamWriter sw = new StreamWriter(@"D:\Shared Folder\conditions_statistics.txt"))
                {
                    for (int i = 3; i <= 11; i++)
                    {
                        int ore_true = 0;
                        int ore_false = 0;
                        int posha_true = 0;
                        int posha_false = 0;
                        int chvatal_true = 0;
                        int chvatal_false = 0;

                        int hamiltonian_graph = 0;

                        using (StreamReader sr = new StreamReader(@"D:\Shared Folder\Generated G6\graphs" + i + ".g6"))
                        {
                            string s;
                            while ((s = sr.ReadLine()) != null)
                            {
                                int[,] matrix = Helper.DecodeGraph6(s);
                                List<int> vector = GetVector(matrix);

                                if (HamiltonianGraphsHelper.IsHamiltonianGraph(matrix))
                                {
                                    hamiltonian_graph++;
                                }

                                if (HamiltonianGraphsHelper.OreCondition(matrix))
                                {
                                    ore_true++;
                                }
                                else
                                {
                                    ore_false++;
                                }

                                if (HamiltonianGraphsHelper.PoshaCondition(vector))
                                {
                                    posha_true++;
                                }
                                else
                                {
                                    posha_false++;
                                }

                                if (HamiltonianGraphsHelper.ChvatalCondition(vector))
                                {
                                    chvatal_true++;
                                }
                                else
                                {
                                    chvatal_false++;
                                }
                            }
                        }

                        sw.WriteLine("Количество {0}-вершинных гамильтоновых графов равно = {1}.", i, hamiltonian_graph);
                        sw.WriteLine("Количество {0}-вершинных графов, удовлетворяющих условию Оре, равно = {1}.", i, ore_true);
                        sw.WriteLine("Количество {0}-вершинных графов, не удовлетворяющих условию Оре, равно = {1}.\n", i, ore_false);

                        sw.WriteLine("Количество {0}-вершинных графов, удовлетворяющих условию Поша, равно = {1}.", i, posha_true);
                        sw.WriteLine("Количество {0}-вершинных графов, не удовлетворяющих условию Поша, равно = {1}.\n", i, posha_false);

                        sw.WriteLine("Количество {0}-вершинных графов, удовлетворяющих условию Хватала, равно = {1}.", i, chvatal_true);
                        sw.WriteLine("Количество {0}-вершинных графов, не удовлетворяющих условию Хватала, равно = {1}.\n", i, chvatal_false);
                    }
                    
                    stopWatch.Stop();

                    TimeSpan ts = stopWatch.Elapsed;
                    string elapsedTime = String.Format("{0:00} дней {1:00} час {2:00} минут {3:00}.{4:00} секунд",
                        ts.Days, ts.Hours, ts.Minutes, ts.Seconds,
                        ts.Milliseconds / 10);
                    sw.WriteLine("\nЗатраченное время: " + elapsedTime);
                }
            }
            catch (Exception e)
            {
                Console.Write("Файл не может быть открыт: ");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Method that find all degrees vectors, that
        /// all connected implementations are hamiltonian
        /// </summary>
        public static void GetAllHamiltonianVectors()
        {
            try
            {
                List<string> all_gamiltonian_vectors = new List<string>();
                List<string> not_gamiltonian = new List<string>();

                for (int i = 3; i <= 11; i++)
                {
                    using (StreamReader sr = new StreamReader(@"D:\Shared Folder\Generated G6\graphs" + i + ".g6"))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            int[,] matrix = Helper.DecodeGraph6(s);
                            string vector = Helper.GetStringVector(GetVector(matrix));

                            if (HamiltonianGraphsHelper.IsHamiltonianGraph(matrix) && !all_gamiltonian_vectors.Contains(vector)
                                && !not_gamiltonian.Contains(vector))
                            {
                                all_gamiltonian_vectors.Add(vector);
                            }

                            if (!HamiltonianGraphsHelper.IsHamiltonianGraph(matrix) && !not_gamiltonian.Contains(vector))
                            {
                                not_gamiltonian.Add(vector);
                            }
                        }
                    }
                }

                foreach (string vector in not_gamiltonian)
                {
                    if (all_gamiltonian_vectors.Contains(vector))
                    {
                        all_gamiltonian_vectors.Remove(vector);
                    }
                }

                Console.WriteLine("Количество векторов степеней, все реализации которых гамильтоновы: " + all_gamiltonian_vectors.Count);

                List<List<int>> gamiltonesVectors = new List<List<int>>();
                foreach (string v in all_gamiltonian_vectors)
                {
                    List<int> vector = v.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();
                    gamiltonesVectors.Add(vector);
                }

                gamiltonesVectors.Sort(Helper.CompareListByCount);
                using (StreamWriter sw = new StreamWriter(@"D:\Shared Folder\gamiltones_vectors.txt"))
                {
                    foreach (List<int> s in gamiltonesVectors)
                    {
                        string vec = Helper.GetStringVector(s);
                        sw.WriteLine(vec);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Файл не может быть открыт: ");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Method that find degrees vectors for Posha and Chvatal conditions
        /// </summary>
        public static void GetPoshaAndChvatalVectorStatistics()
        {
            StreamWriter posha_gamiltonian = new StreamWriter(@"D:\Shared Folder\posha_gamiltonian.txt");
            StreamWriter posha_not_gamiltonian = new StreamWriter(@"D:\Shared Folder\posha_not_gamiltonian.txt");
            StreamWriter chvatal_gamiltonian = new StreamWriter(@"D:\Shared Folder\chvatal_gamiltonian.txt");
            StreamWriter chvatal_not_gamiltonian = new StreamWriter(@"D:\Shared Folder\chvatal_not_gamiltonian.txt");

            int posha_true = 0;
            int posha_false = 0;
            int chvatal_true = 0;
            int chvatal_false = 0;

            List<List<int>> posha_and_hamiltonian = new List<List<int>>();
            List<List<int>> chvatal_and_hamiltonian = new List<List<int>>();
            List<List<int>> not_posha_but_hamiltonian = new List<List<int>>();
            List<List<int>> not_chvatal_but_hamiltonian = new List<List<int>>();

            try
            {
                using (StreamReader sr = new StreamReader(@"D:\Shared Folder\Main Results\all_vectors_are_hamiltonian.txt"))
                {
                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        List<int> vector = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();

                        if (HamiltonianGraphsHelper.PoshaCondition(vector))
                        {
                            posha_and_hamiltonian.Add(vector);
                            posha_true++;
                        }
                        else
                        {
                            not_posha_but_hamiltonian.Add(vector);
                            posha_false++;
                        }

                        if (HamiltonianGraphsHelper.ChvatalCondition(vector))
                        {
                            chvatal_and_hamiltonian.Add(vector);
                            chvatal_true++;
                        }
                        else
                        {
                            not_chvatal_but_hamiltonian.Add(vector);
                            chvatal_false++;
                        }
                    }
                }

                Console.WriteLine("Количество векторов степеней, все связные реализации которых гамильтоновы и удовлетворяющих условию Поша: " + posha_true);
                Console.WriteLine("Количество векторов степеней, все связные реализации которых гамильтоновы и не удовлетворяющих условию Поша: " + posha_false + "\n");
                Console.WriteLine("Количество векторов степеней, все связные реализации которых гамильтоновы и удовлетворяющих условию Хватала: " + chvatal_true);
                Console.WriteLine("Количество векторов степеней, все связные реализации которых гамильтоновы и не удовлетворяющих условию Хватала: " + chvatal_false + "\n");

                posha_and_hamiltonian.Sort(Helper.CompareListByCount);
                not_posha_but_hamiltonian.Sort(Helper.CompareListByCount);
                chvatal_and_hamiltonian.Sort(Helper.CompareListByCount);
                not_chvatal_but_hamiltonian.Sort(Helper.CompareListByCount);

                foreach (List<int> vector in posha_and_hamiltonian)
                {
                    string vec = Helper.GetStringVector(vector);
                    posha_gamiltonian.WriteLine(vec);
                }
                foreach (List<int> vector in not_posha_but_hamiltonian)
                {
                    string vec = Helper.GetStringVector(vector);
                    posha_not_gamiltonian.WriteLine(vec);
                }
                foreach (List<int> vector in chvatal_and_hamiltonian)
                {
                    string vec = Helper.GetStringVector(vector);
                    chvatal_gamiltonian.WriteLine(vec);
                }
                foreach (List<int> vector in not_chvatal_but_hamiltonian)
                {
                    string vec = Helper.GetStringVector(vector);
                    chvatal_not_gamiltonian.WriteLine(vec);
                }

                posha_gamiltonian.Close();
                posha_not_gamiltonian.Close();
                chvatal_gamiltonian.Close();
                chvatal_not_gamiltonian.Close();
            }
            catch (Exception e)
            {
                Console.Write("Файл не может быть открыт: ");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Method that get analysis of degrees vectors for Chvatal condition
        /// </summary>
        public static void GetChvatalVectorsAnalysis()
        {
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                using (StreamWriter sw = new StreamWriter(@"D:\Shared Folder\vectors.txt"))
                {
                    using (StreamReader sr = new StreamReader(@"D:\Shared Folder\Main results\khvatal_not_gamiltones.txt"))
                    {
                        int countOfNonplanar = 0;
                        int countOfPlanar = 0;
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            List<int> vector = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();

                            int n = vector.Count;
                            int m = 0;
                            foreach (int x in vector)
                            {
                                m += x;
                            }
                            m /= 2;

                            int nn = 3 * n - 6;
                            bool planarnost = (m <= nn);
                            if (planarnost)
                            {
                                countOfPlanar++;
                            } else
                            {
                                countOfNonplanar++;
                            }

                            int nnn = n * n / 4;

                            sw.WriteLine("{0}:\n\tn = {1}\n\tm = {2}\n\t3n - 6 = {3}\n\tm <= 3n - 6 - {4}\n\tn^2/4 = {5}\n", 
                                Helper.GetStringVector(vector), n, m, nn, planarnost, nnn);
                        }

                        sw.WriteLine("Из них планарных: {0}", countOfPlanar);
                        sw.WriteLine("Из них непланарных: {0}", countOfNonplanar);

                        stopWatch.Stop();

                        TimeSpan ts = stopWatch.Elapsed;
                        string elapsedTime = String.Format("{0:00} дней {1:00} час {2:00} минут {3:00}.{4:00} секунд",
                            ts.Days, ts.Hours, ts.Minutes, ts.Seconds,
                            ts.Milliseconds / 10);
                        sw.WriteLine("\nЗатрачено " + elapsedTime);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Файл не может быть открыт: ");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Method that find all graphs that correspond to Chvatal
        /// and don't correspond to Posha condition
        /// </summary>
        public static void GetGraphsThatChvatalAndNotPosha()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"D:\Shared Folder\ChvatalAndPoshaGraphs.g6"))
                {
                    for (int i = 6; i <= 8; i++)
                    {
                        using (StreamReader sr = new StreamReader(@"D:\Shared Folder\Generated G6\graphs" + i + ".g6"))
                        {
                            string s;
                            while ((s = sr.ReadLine()) != null)
                            {
                                int[,] matrix = Helper.DecodeGraph6(s);
                                List<int> vector = GetVector(matrix);
                                
                                if (!HamiltonianGraphsHelper.PoshaCondition(vector) && HamiltonianGraphsHelper.ChvatalCondition(vector))
                                {
                                    sw.WriteLine(s);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Файл не может быть открыт: ");
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Method that get graph visualisation
        /// </summary>
        public static void GraphVisualisation()
        {
            try
            {
                using (StreamReader sr = new StreamReader(@"D:\Shared Folder\Main Results\KhvatalAndPosha.g6"))
                {
                    int k = 1;

                    string s;
                    while ((s = sr.ReadLine()) != null)
                    {
                        int[,] matrix = Helper.DecodeGraph6(s);
                        
                        using (StreamWriter sw = new StreamWriter(@"D:\Shared Folder\CatalogDOT\graph" + k++ + ".dot"))
                        {
                            sw.WriteLine("graph {");

                            for (int i = 0; i < matrix.GetLength(0); i++)
                            {
                                sw.WriteLine("\t" + (i + 1) + " [shape=point];");
                            }

                            for (int i = 0; i < matrix.GetLength(0); i++)
                            {
                                for (int j = 0; j < matrix.GetLength(0); j++)
                                {
                                    if (i < j && matrix[i,j] == 1)
                                    {
                                        sw.WriteLine("\t" + (i + 1) + " -- " + (j + 1) + ";");
                                    }
                                }
                            }

                            sw.WriteLine("}");
                        }
                    }
                }

                var dotExePath = @"C:\Program Files (x86)\Graphviz2.38\bin\dot.exe";
                if (File.Exists(dotExePath))
                {
                    for (int i = 1; i < 257; i++)
                    {
                        Console.WriteLine("Start graph #" + i);
                        var dotPath = @"D:\Shared Folder\CatalogDOT\graph" + i + ".dot";
                        var pngPath = @"D:\Shared Folder\CatalogJPEG\graph" + i + ".png";

                        ProcessStartInfo startInfo = new ProcessStartInfo(dotExePath);
                        startInfo.Arguments = "-Tpng \"" + dotPath + "\" -o \"" + pngPath + "\"";
                        startInfo.CreateNoWindow = true;
                        Process.Start(startInfo);

                        Console.WriteLine("End graph #" + i + "\n");
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Файл не может быть открыт: ");
                Console.WriteLine(e.Message);
            }
        }
    }
}
