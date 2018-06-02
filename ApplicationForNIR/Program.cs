using System;
using System.Diagnostics;

namespace ApplicationForNIR
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Get all statistics
            GraphHelper.GetConditionsStatistics();

            // Get all hamiltonian degrees vectors
            GraphHelper.GetAllHamiltonianVectors();

            // Get Posha and Chvatal vectors statistics
            GraphHelper.GetPoshaAndChvatalVectorStatistics();

            // Get Chvatal vectors analysis
            GraphHelper.GetChvatalVectorsAnalysis();

            // Get graphs that Chvatal and not Posha
            GraphHelper.GetGraphsThatChvatalAndNotPosha();

            // Get graphs visualisation
            GraphHelper.GraphVisualisation();

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00} дней {1:00} час {2:00} минут {3:00}.{4:00} секунд",
                ts.Days, ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("Затраченное время: " + elapsedTime);

            Console.ReadKey();
        }
    }
}
