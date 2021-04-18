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

        private static bool? _isRunningAzure;
        public static bool IsAzure
        {
            get { return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RoleRoot")); }
        }

        static void Main(string[] args)
        {
            args = args ?? new string[] { "RUN20" };
            if (args.Length != 1)
            {
                WriteHelpThenExitWithError();
            }

            var argument = args[0];

            if (argument.Contains("RUN"))
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

            using (var log = new StreamWriter("performance.log", true))
            {
                try
                {
                    var n = DateTime.Now;
                    log.WriteLine($"starting tests [{n.ToShortDateString()} {n.ToShortTimeString()}] ");
                    var logs = Solution.Path("logs");
                    if (!Directory.Exists(logs)) Directory.CreateDirectory(logs);

                    var tester = new Tester(log);
                    Console.WriteLine($"hello from default size console  {Console.WindowWidth}x{Console.WindowHeight}");
                    Screenshot.Take(Path.Combine(logs, "screen1"));
                    if (PlatformStuff.IsWindows)
                    {
#pragma warning disable CA1416 // Validate platform compatibility
                        Console.SetWindowSize(1, 1);
                        Console.SetBufferSize(90, 30);
                        Console.SetWindowSize(90, 30);
#pragma warning restore CA1416 // Validate platform compatibility
                        Console.CursorVisible = false;
                        Console.WriteLine($"hello from 90x30");
                        Screenshot.Take(Path.Combine(logs, "screen2"));
                    }

                    IConsole left = null;
                    IConsole right = null;

                    // ------------------------------------------------------------
                    //       Window border print tests (splitleft, splitright)
                    // ------------------------------------------------------------
                    tester.TestIt(() =>
                    {
                        Console.Clear();
                        Console.CursorVisible = false;
                        var console = new Window();
                        return (console, null);
                    },
                    40, "split left, split right tests", (IConsole console, HighSpeedWriter hw, int iteration) =>
                    {
                        left = console.SplitLeft("left");
                        right = console.SplitRight("right");
                    }, TakeScreenShot);

                    // ------------------------------------------------------------
                    //         High speed writer tests (scrolling tests)
                    // ------------------------------------------------------------
                    
                    //tester.TestIt(()=>
                    //{
                    //    Console.Clear();
                    //    var hw = new HighSpeedWriter();
                    //    var console = new Window(hw);
                        
                    //    //var console = new Window();
                    //    left = console.SplitLeft("left");
                    //    right = console.SplitRight("right");
                    //    return (console, hw);
                    //},
                    //100, "high speed writer tests", (IConsole console, HighSpeedWriter hw, int iteration) =>
                    //{
                    //    left.WriteLine(Red, $"left iteration {iteration}");
                    //    right.WriteLine(Green, $"left iteration {iteration}");
                    //    hw?.Flush();
                    //} , TakeScreenShot);

                    //tester.TestIt(iterations, "HighSpeedWriterBoxes", HWSplitLeftRightPerformanceTest, false, TakeScreenShot);

                    //tester.TestIt(NoSetup, iterations, "NewWindowTest", NewWindowTest, DoNothing);
                    //tester.TestIt(NoSetup, iterations, "NewWindowConcurrent", NewWindowConcurrent, DoNothing);
                    //tester.TestIt(NewWindowSetup, iterations, "SplitRightLeft", SplitRightLeft, TakeScreenShot);
                    //tester.TestIt(NewWindowSetup, iterations, "SplitColumns", SplitColumns, TakeScreenShot);
                    //tester.TestIt(NewWindowSetup, iterations, "SplitRows", SplitRows, TakeScreenShot);
                    //tester.TestIt(NewWindowSetup, iterations, "SplitRightLeft 2", SplitRows, TakeScreenShot);

                    // TODO: write out test results to Logs.IO or similar, track performance over time.

                    // ----------------------

                }
                catch (Exception ex)
                {
                    Console.Clear();
                    var msg = $"Error running performance test. {ex.Message}";
                    Console.WriteLine(msg);
                    log.WriteLine(msg);
                    log.WriteLine("--------");
                    log.WriteLine(ex.ToString());
                    log.WriteLine("--------");
                }
                finally
                {
                    if (PlatformStuff.IsWindows) Console.CursorVisible = true;
                    log.Flush();
                    log.Dispose();
                }
            }
        }

        private static Action<string> DoNothing = (string name) => { };
        private static Action<string> TakeScreenShot = (string name) => Screenshot.Take(Solution.Path("logs", name));

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
}
