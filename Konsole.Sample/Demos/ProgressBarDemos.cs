using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Konsole.Internal;

namespace Konsole.Sample.Demos
{
    class ProgressBarDemos
    {
        public static void SimpleDemo(IConsole con)
        {
            con.WriteLine("'p' Test Progress bars");
            con.WriteLine("----------------------");

            var pb = new ProgressBar(10);
            var pb2 = new ProgressBar(30);
            con.Write("loading...");
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                con.Write($" {i}");
                pb.Refresh(i, $"loading cat {i}");
                pb2.Refresh(i, $"loading dog {i}");

            }
            pb.Refresh(10, "All cats loaded.");
            con.WriteLine(" Done!");
        }



        public static void ProgressivelyFasterDemo(int startingPauseMilliseconds = 50, Window window = null)
        {
            var pb = window?.ProgressBar(300) ?? new ProgressBar(300);
            var names = TestData.MakeNames(300);
            int cnt = names.Count();
            int i = 1;
            foreach (var name in names)
            {
                pb.Refresh(i++, name);
                int pause = startingPauseMilliseconds - (1 * (i * (startingPauseMilliseconds - 1) / cnt));
                if (pause > 0) Thread.Sleep(pause);
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    break;
                }
            }

        }


        public static void ParallelDemo()
        {
            // demo; take the first 10 directories that have files from solution root and then pretends to process (list) them.
            // processing of each directory happens on a different thread, to simulate multiple background tasks, 
            // e.g. file downloading.
            // ==============================================================================================================
            var dirs = Directory.GetDirectories(@"..\..\..\..").Where(d => Directory.GetFiles(d).Any()).Take(7);

            var tasks = new List<Task>();
            var bars = new List<ProgressBar>();
            foreach (var d in dirs)
            {
                var dir = new DirectoryInfo(d);
                var files = dir.GetFiles().Take(50).Select(f => f.FullName).ToArray();
                if (files.Length == 0) continue;
                var bar = new ProgressBar(files.Count());
                bars.Add(bar);
                bar.Refresh(0, d);
                tasks.Add(new Task(() => ProcessFiles(d, files, bar)));
            }
            Console.WriteLine("ready press enter.");
            Console.ReadLine();

            foreach (var t in tasks) t.Start();
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("done.");
        }

        public static void ProcessFiles(string directory, string[] files, ProgressBar bar)
        {
            var cnt = files.Count();
            foreach (var file in files)
            {
                bar.Next(new FileInfo(file).Name);
                Thread.Sleep(150);
            }
        }

    }
}
