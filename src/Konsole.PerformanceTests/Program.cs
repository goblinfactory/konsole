using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Konsole.PerformanceTests
{
    class Program
    {
        const int ERRORS = -1;
        const string RUN = "RUN";
        static void Main(string[] args)
        {

            if (args.Length != 1)
            {
                WriteHelpThenExitWithError();
            }

            var argument = args[0];
            
            if(argument.Contains("RUN"))
            {
                Console.WriteLine("launching console process, wait for finish, propogate return code");
                var iterationArg = argument.Replace(RUN, "");
                var returnCode = RunNewProcess(iterationArg);
                Environment.Exit(returnCode);
            }

            int iterations;
            if (!int.TryParse(argument, out iterations))
            {
                WriteHelpThenExitWithError();
            }
            
            using (var logstream = File.OpenWrite("performance.log"))
            {
                using var log = new StreamWriter(logstream);
                log.WriteLine($"starting test [ {iterations} ] iterations.");

                var tester = new Tester(log);
                Console.SetWindowSize(90, 30);

                // ----------------------
                //  THE ACTUAL TESTS 
                // ----------------------

                tester.TestIt(iterations, "SplitRightLeft", SplitRightLeft);
                tester.TestIt(iterations, "SplitColumns", SplitColumns);
                tester.TestIt(iterations, "SplitRows", SplitRows);
                // ----------------------
                logstream.Flush();
            }
        }

        static void WriteHelpThenExitWithError()
        {
            Console.WriteLine(" args[0] = RUN10 or RUN{nn} or 10 or 20");
            Console.WriteLine(" adding RUN will launch a new process");
            Console.WriteLine(" otherwise it runs the test {n} iterations");
            Environment.Exit(ERRORS);
        }
        public static void SplitRightLeft()
        {
            var w = new Window();
            var left = w.SplitLeft("left");
            var right = w.SplitRight("right");
        }

        public static void SplitColumns()
        {
            var w = new Window();
            var cols = w.SplitColumns(
                new Split(10, "10"),
                new Split(0, "rest"),
                new Split(20, "20")
                );
        }

        public static void SplitRows()
        {
            var w = new Window();
            var cols = w.SplitRows(
                new Split(10, "10"),
                new Split(0, "rest"),
                new Split(10, "10")
                );
        }

        //TODO: HighSpeedWriter

        public static int RunNewProcess(string argument)
        {
            try
            {
                using (Process process = new Process())
                {
                    var path = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = path;
                    process.StartInfo.Arguments = argument;
                    process.StartInfo.CreateNoWindow = false;
                    process.Start();
                    process.WaitForExit();
                    return process.ExitCode;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return ERRORS;
            }
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
            Console.WriteLine($"Running test :{test}");
            int cnt = 0;
            var timer = new Stopwatch();
            Console.Write($"{test} - started, ");
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
                var errorMessage = $"Error, iterations:${iterations} != cnt:{cnt}";
                Console.WriteLine(errorMessage);
                log.WriteLineAsync(errorMessage);
                log.Flush();
                log.Close();
                Environment.Exit(-1);
            }

            double rps = (double)iterations / timer.Elapsed.TotalSeconds;
            double response = (1 / rps) * 1000;
            var successMessage = $"TEST:{test,-20} [{rps:000000.00}] requests per second, [{response:0000}]  ms per requst. ";
            Console.WriteLine(successMessage);
            log.WriteLine(successMessage);
   }

    }
}
