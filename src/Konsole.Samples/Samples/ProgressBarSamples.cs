using Konsole.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.ConsoleColor;

namespace Konsole.Samples
{
    public class ProgressBarSamples
    {

        public static void Run()
        {
            void Pause() => Console.ReadKey(true);

            Console.WriteLine("progress bar in a floating inline window");
            // --------------------------------------------------------
            Pause();
            var w = new Window(80, 10);
            var pb1 = new ProgressBar(w, 100);
            var pb2 = new ProgressBar(w, 100);
            pb1.Refresh(25, "hello world! 1");
            pb2.Refresh(25, "hello world! 2");
            
            Pause();

            Console.WriteLine("progress bar in a floating inline window + split window LeftRight");
            // -----------------------------------------------------------------------------------

            w = new Window(80, 10, White, DarkBlue);
            var left = w.SplitLeft("left");
            var right = w.SplitRight("right");
            var pb = new ProgressBar(left, 100);
            pb.Refresh(25, "hello world!");
            Console.ReadKey(true);

            // ------------------------------------------------------------
            w = new Window(80, 10, White, DarkGray);
            w.WriteLine("progress bar in a floating inline window + split window TopBottom");
            var top = w.SplitTop("top");
            pb = new ProgressBar(top, 100);
            pb.Refresh(25, "hello world!");
            Console.ReadKey(true);

            // -----------------------------------------------------------------------------------
            //w = new Window(80, 20, White, DarkGray);
            //w.WriteLine("progress bar in a floating inline window + split window TopBottom");
            //var top = w.SplitTop("top");
            //pb = new ProgressBar(top, 100);
            //pb.Refresh(25, "hello world!");
            //Console.ReadLine();


            //Console.Clear();
            //Console.CursorVisible = false;

            //var window = new Window();
            //var menuWin = window.SplitLeft("menu");
            //var sampleWin = window.SplitRight("sample");

            //var menu = new Menu(window, "samples", ConsoleKey.Escape, 20,
            //    new MenuItem("inline windows", () => { }),                              // Inline windows at the current cursor position
            //    new MenuItem("inline ProgressBars", () => InlineProgressBar(sampleWin)) // InlineProgressBars at the current cursor position
            //);
            //menu.Run();
        }

        static void InlineProgressBar(IConsole console)
        {
            console.Clear();
            console.WriteLine("build task 1");
            console.WriteLine("build task 2");
            console.WriteLine("build task 3");
            var window = new Window(console, 40, 7, White, Black);
            var processing = window.SplitTop("processing");
            var status = window.SplitBottom("status");
            var compressProgress = new ProgressBar(processing, 100);
            var encryptProgress = new ProgressBar(processing, 100);

            var tasks = new List<Task>();

            tasks.Add(DoStuff("Compress", compressProgress, status, 20));
            tasks.Add(DoStuff("Encrypt", encryptProgress, status, 40));

            // simulate a build task writing to Console output
            for (int i = 4; i <= 15; i++)
            {
                Thread.Sleep(1000);
                console.WriteLine($"I am build task output number {i}");
            }

            Task.WaitAll(tasks.ToArray());
            console.WriteLine("------------");
            console.WriteLine("All finished");
            console.WriteLine("------------");
            console.WriteLine("");
            console.WriteLine("press enter to return to menu");
            Console.ReadLine();
            console.Clear();
        }

        private static Random _rnd = new Random();
        private static int _files = 0;
        private static int _bytes = 0;
        static void UpdateStatus(IConsole status)
        {
            var files = Interlocked.Increment(ref _files);
            var kb = (Interlocked.Add(ref _bytes, _rnd.Next(5000)) / 1000);
            status.PrintAtColor(Black, 16, 0, $" {_bytes} Kb  ", Red);
            status.PrintAtColor(Black, 0, 0, $" {files++} files ", White);
        }

        static Task DoStuff(string prefix, ProgressBar progress, IConsole status, int speed)
        {
            var testFiles = TestData.MakeObjectNames(100);
            var checkStuff = Task.Run(() => {
                int files = 1;
                for (int i = 1; i <= 100; i++)
                {
                    Thread.Sleep(speed + new Random().Next(100));
                    progress.Refresh(i, $"{prefix} : {testFiles[i % 100]}");
                    UpdateStatus(status);
                }
            });
            return checkStuff;
        }
    }
}
