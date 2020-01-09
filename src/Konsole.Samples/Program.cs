using Konsole.Forms;
using Konsole.Internal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static System.ConsoleColor;
using static System.ConsoleColor;

namespace Konsole.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            void Wait() => Console.ReadKey(true);
            decimal amazon = 84;
            decimal bp = 146;

            void Tick(IConsole con, string sym, decimal newPrice, ConsoleColor color, char sign, decimal perc) {
                con.Write(White, $"{sym,-10}");
                con.WriteLine(color, $"{newPrice:0.00}");
                con.WriteLine(color, $"  ({sign}{newPrice}, {perc}%)");
                con.WriteLine("");
            }

            Console.WriteLine("line one");
            var nyse = Window.OpenBox("NYSE", 20, 12, new BoxStyle() { ThickNess = LineThickNess.Single, Title = new Colors(White, Red) });
            
            Console.WriteLine("line two");
            var ftse100 = Window.OpenBox("FTSE 100", 20, 12, new BoxStyle() { ThickNess = LineThickNess.Double, Title = new Colors(White, Blue) });
            Console.Write("line three");


            while(true) {
                Tick(nyse, "AMZ", amazon -= 0.04M, Red, '-', 4.1M);
                Tick(ftse100, "BP", bp += 0.05M, Green, '+', 7.2M);
                Wait();
            }

            
        }

        static void Main3(string[] args)
        {
            static void Compress(IConsole status, string file)
            {
                status.WriteLine($"compressing {file}");
                Thread.Sleep(new Random().Next(10000));
                status.WriteLine(Green, $"{file} (OK)");
            }

            static void Index(IConsole status, string file)
            {
                status.WriteLine($"indexing {file}");
                Thread.Sleep(new Random().Next(10000));
                status.WriteLine(Green, " finished.");
            }

            // seems to keep the cursor on top! (not showing up as an inline window)
            //var window = new Window(50, 20);

            var console = new Window();
            console.WriteLine(console.CursorTop.ToString());

            var compressWindow = console.OpenBox("compress", 40, 4);
            console.WriteLine(console.CursorTop.ToString());

            var encryptWindow = console.OpenBox("encrypt", 40, 4);
            console.WriteLine(console.CursorTop.ToString());


            var tasks = new List<Task>();

            while (true)
            {
                // no background threads so can use Console
                console.Write("Enter name of file to process (quit) to exit:");
                var file = Console.ReadLine();
                if (file == "quit") break;
                tasks.Add(Task.Run(()=> Compress(compressWindow, file)));
                tasks.Add(Task.Run(()=> Index(encryptWindow, file)));
                console.WriteLine($"processing {file}");
            }
            
            console.WriteLine("waiting for background tasks");
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("done.");
        }

        static void Main2(string[] args)
        {
            Console.WriteLine("build task 1");
            Console.WriteLine("build task 2");
            Console.WriteLine("build task 3");

            Console.CursorVisible = false;
            var window = new Window(40, 7).Concurrent();
            var console = new ConcurrentWriter();
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

            Console.CursorVisible = true;
            Console.WriteLine("------------");
            Console.WriteLine("All finished");
            Console.WriteLine("------------");
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
