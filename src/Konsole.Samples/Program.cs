using Konsole.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.ConsoleColor;

namespace Konsole.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("build task 1");
            Console.WriteLine("build task 2");
            Console.WriteLine("build task 3");

            Console.CursorVisible = false;
            var window = new Window(40, 7);
            var console = new ConcurrentWriter();
            var processing = window.SplitTop("processing");
            var status = window.SplitBottom("status");
            var compressProgress = new ProgressBar(processing, 100);
            var encryptProgress = new ProgressBar(processing, 100);

            var tasks = new List<Task>();

            tasks.Add(DoStuff("Compress", compressProgress, status, 20));
            tasks.Add(DoStuff("Encrypt", encryptProgress, status, 40));

            // simulate a build task writing to Console output
            for (int i = 4; i <= 10; i++)
            {
                Thread.Sleep(1000);
                console.WriteLine($"I am build task output number {i}");
            }

            Task.WaitAll(tasks.ToArray());

            // this is safe, because there's only 1 thread here
            // so it doesn't matter if this uses IConsole or not.
            Console.CursorTop = console.CursorTop + 1;
            Console.CursorVisible = true;
            Console.WriteLine("------------");
            Console.WriteLine("All finished");
            Console.WriteLine("------------");
        }

        private static int _files = 0;
        private static int _bytes = 0;
        static void UpdateStatus(IConsole status, int bytes)
        {
            var files = Interlocked.Increment(ref _files);
            var kb = (Interlocked.Add(ref _bytes, bytes) / 1000);
            status.PrintAtColor(Black, 16, 0, $" {bytes} Kb  ", Red);
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
                    UpdateStatus(status, new Random().Next(1000));
                }
            });
            return checkStuff;
        }
    }
}
