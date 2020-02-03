using Konsole.Platform;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using static System.ConsoleColor;

namespace Konsole.PerformanceTests
{
    //TODO: test if I still need the launch external process?
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
                
                var logs = Solution.Path("logs");
                if (!Directory.Exists(logs)) Directory.CreateDirectory(logs);
                
                var tester = new Tester(log);
                Console.WriteLine($"hello from default size console  {Console.WindowWidth}x{Console.WindowHeight}");
                Screenshot.Take(Path.Combine(logs, "screen1"));
                if(PlatformStuff.IsWindows)
                {
                    Console.SetWindowSize(90, 30);
                    Console.WriteLine($"hello from 90x30");
                    Screenshot.Take(Path.Combine(logs, "screen2"));
                }

                // ----------------------
                //  THE ACTUAL TESTS 
                // ----------------------

                tester.TestIt(DirectoryListViewTestsSetup, iterations, "ListViewTests", DirectoryListViewTests, TakeScreenShot);
                //tester.TestIt(NoSetup, iterations, "NewWindowTest", NewWindowTest, DoNothing);
                //tester.TestIt(NoSetup, iterations, "NewWindowConcurrent", NewWindowConcurrent, DoNothing);
                //tester.TestIt(NewWindowSetup, iterations, "SplitRightLeft", SplitRightLeft, TakeScreenShot);
                //tester.TestIt(NewWindowSetup, iterations, "SplitColumns", SplitColumns, TakeScreenShot);
                //tester.TestIt(NewWindowSetup, iterations, "SplitRows", SplitRows, TakeScreenShot);
                //tester.TestIt(NewWindowSetup, iterations, "SplitRightLeft 2", SplitRows, TakeScreenShot);

                // TODO: write out test results to Logs.IO or similar, track performance over time.

                // ----------------------
                logstream.Flush();
            }
        }

        private static Action<string> DoNothing = (string name) => { };
        private static Action<string> TakeScreenShot = (string name)  =>Screenshot.Take(Solution.Path("logs", name));

        static IConsole NoSetup() => null;
        static IConsole NewWindowSetup() => new Window();
        static void WriteHelpThenExitWithError()
        {
            Console.WriteLine(" args[0] = RUN10 or RUN{nn} or 10 or 20");
            Console.WriteLine(" adding RUN will launch a new process");
            Console.WriteLine(" otherwise it runs the test {n} iterations");
            Environment.Exit(ERRORS);
        }

        public static void NewWindowTest(IConsole console)
        {
            var w = new Window();
            w.Write(".");
        }

        public static void NewWindowConcurrent(IConsole console)
        {
            var w = new Window().Concurrent();
            w.Write(".");
        }
        
        public static void SplitRightLeft(IConsole console)
        {
            var left = console.SplitLeft("left");
            var right = console.SplitRight("right");
        }

        public static void SplitColumns(IConsole console)
        {
            var cols = console.SplitColumns(
                new Split(10, "10"),
                new Split(0, "rest"),
                new Split(20, "20")
                );
        }

        public static void SplitRows(IConsole console)
        {
            var cols = console.SplitRows(
                new Split(10, "10"),
                new Split(0, "rest"),
                new Split(10, "10")
                );
        }

        private static IConsole DirectoryListViewTestsSetup()
        {
            var w = new Window();
            var left = w.SplitLeft("left");
            var right = w.SplitRight("right");
            return left;
        }
        public static void DirectoryListViewTests(IConsole console)
        {
            return;
            //var listView = new DirectoryListView(console, new FileOrDirectory("./");

            //// let's highlight - all files > 4 Mb and make directories green
            //listView.BusinessRuleColors = (o, column) =>
            //{
            //    if (column == 2 && o.Size > 4000000) return new Colors(White, DarkBlue);
            //    if (column == 1 && o.Item is DirectoryInfo) return new Colors(Green, Black);
            //    return null;
            //};
            //listView.Refresh();
        }


        //TODO: HighSpeedWriter

        public static int RunNewProcess(string argument)
        {
            try
            {
                using (Process process = new Process())
                {
                    var path = Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe");
                    process.StartInfo.CreateNoWindow = false;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = path;
                    process.StartInfo.Arguments = argument;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                    process.Start();
                    process.WaitForExit();
                    return process.ExitCode;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("---------");
                Console.WriteLine(e.ToString());
                using var logstream = File.OpenWrite("performance.log");
                using var log = new StreamWriter(logstream);
                log.WriteLine(e.Message);
                log.WriteLine("-------");
                log.WriteLine(e.ToString());
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
            //Console.SetWindowSize(120, 50);
        }
        public void TestIt(Func<IConsole> setup, int iterations, string testName, Action<IConsole> testMethod, Action<string> postTest)
        {
            Console.WriteLine($"Running test :{testName}");
            int cnt = 0;
            var timer = new Stopwatch();
            Console.Write($"{testName} - started, ");
            for (int i = 0; i < iterations; i++)
            {
                Console.Clear();
                var console = setup();
                timer.Start();
                testMethod(console);
                cnt++;
                timer.Stop();
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
            var successMessage = $"TEST:{testName,-20} [{rps:00000.00}] requests per second, [{response:0000}]  ms per requst. ";
            postTest($"{testName}-{response:0}ms");
            Console.WriteLine(successMessage);
            log.WriteLine(successMessage);
   }

    }
}
