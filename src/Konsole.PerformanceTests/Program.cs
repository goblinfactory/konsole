using System;
using System.Diagnostics;
using System.IO;

namespace Konsole.PerformanceTests
{
    class Program
    {
        static int completed = 0;
        static void Main(string[] args)
        {
            int iterations = args == null || args.Length!=1 ? 10 : int.Parse(args[0]);

            using (var logstream = File.OpenWrite("performance.log"))
            {
                using var log = new StreamWriter(logstream);
                log.WriteLine($"starting test [ {iterations} ] iterations.");

                var tester = new Tester(log);

                // ----------------------
                //  THE ACTUAL TESTS 
                // ----------------------

                tester.TestIt(iterations, "SplitRightLeft", SplitRightLeft);
                // ----------------------
                logstream.Flush();
            }
        }

        public static void SplitRightLeft()
        {
            var w = new Window();
            var left = w.SplitLeft("left");
            var right = w.SplitRight("right");
        }


    }

    public class Tester
    {
        private readonly StreamWriter log;

        public Tester(StreamWriter log)
        {
            this.log = log;
            Console.SetWindowSize(120, 50);
        }
        public void TestIt(int iterations, string test, Action action)
        {
            int cnt = 0;
            var timer = new Stopwatch();
            log.Write($"{test} - started, ");
            for (int i = 0; i < iterations; i++)
            {
                Console.Clear();
                timer.Start();
                action();
                cnt++;
                timer.Stop();
            }
            log.WriteLine(" finished.");
            if (iterations != cnt)
            {
                log.WriteLineAsync($"Error, iterations:${iterations} != cnt:{cnt}");
                log.Flush();
                log.Close();
                Environment.Exit(-1);
            }

            double rps = (double)iterations / timer.Elapsed.TotalSeconds;
            log.WriteLine($"TEST:{test,-20} [{rps:000000.00}] requests per second.");
   }

    }
}
