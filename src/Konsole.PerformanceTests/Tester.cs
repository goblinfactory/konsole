using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Konsole.PerformanceTests
{
    public class Tester
    {
        private readonly StreamWriter log;
        public Tester(StreamWriter log)
        {
            this.log = log;
            //Console.SetWindowSize(120, 50);
        }
        public void TestIt(Func<(IConsole, HighSpeedWriter)> setup, int iterations, string testName, Action<IConsole, HighSpeedWriter, int> testMethod, Action<string> postTest)
        {
            Console.WriteLine($"Running test :{testName}");
            int cnt = 0;
            var timer = new Stopwatch();
            Console.Write($"{testName} - started, ");
            Console.Clear();
            var (_ , hw1) = setup();

            if (Program.IsAzure && hw1 != null)
            {
                var msg = $"test '{testName}' skipped, not compatible on azure build servers for security reasons";
                Console.WriteLine(msg);
                log.WriteLineAsync(msg);
                log.Flush();
                return;
            }

            for (int i = 0; i < iterations; i++)
            {

                // add a brief pause before clearing screen otherwise we won't see much, just flicker
#if DEBUG
    //Console.ReadKey(true);
#endif
                var (console, hw) = setup();
                timer.Start();
                testMethod(console, hw, i);
                timer.Stop();
                cnt++;
            }

            if (iterations != cnt)
            {
                var errorMessage = $"Error, iterations:${iterations} != cnt:{cnt}";
                Console.WriteLine(errorMessage);
                log.WriteLineAsync(errorMessage);
                log.Flush();
                log.Close();
                Environment.Exit(-1);
            }

            double rps = (double)iterations / timer.Elapsed.TotalSeconds;
            double response = (1 / rps) * 1000;
            var n = DateTime.Now;
            var successMessage = $"{n.ToShortDateString()} {n.ToShortTimeString()} : TEST: {testName,-35} [{rps:00000.00}] requests per second, [{response:0000}]  ms per requst. ";
            postTest($"{testName}-{response:0}ms");
            Console.WriteLine(successMessage);
            log.WriteLine(successMessage);
        }

    }
}
