using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Konsole.Internal;
using Konsole.Menus;

namespace Konsole.Sample.Demos
{
    public static class QueueExtensions
    {
        public static IEnumerable<T> Dequeue<T>(this ConcurrentQueue<T> src, int x)
        {
            int cnt = 0;
            bool more = true;
            while (more && cnt<x)
            {
                T item;
                more = src.TryDequeue(out item);
                cnt++;
                yield return item;
            }

            
        }
    }

    class ProgressBarDemos
    {
        public static void SimpleDemo(IConsole con)
        {
            con.WriteLine("'p' Test Progress bars");
            con.WriteLine("----------------------");

            var pb = new ProgressBar(con, 10);
            var pb2 = new ProgressBar(con, 30);
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




        public static void ParallelConstructorDemo()
        {
            Console.WriteLine("ready press enter.");
            Console.ReadLine();

            var dirCnt = 15;
            var filesPerDir = 100;
            var r = new Random();
            var q = new ConcurrentQueue<string>();
            foreach (var name in TestData.MakeNames(2000)) q.Enqueue(name);
            var dirs = TestData.MakeObjectNames(dirCnt).Select(dir => new
            {
                name = dir,
                cnt = r.Next(filesPerDir)
            });

            var tasks = new List<Task>();
            var bars = new ConcurrentBag<ProgressBar>();
            foreach (var d in dirs)
            {
                var files = q.Dequeue(d.cnt).ToArray();
                if (files.Length == 0) continue;
                tasks.Add(new Task(() =>
                {
                    var bar = new ProgressBar(files.Count());
                    bars.Add(bar);
                    bar.Refresh(0, d.name);
                    ProcessFakeFiles(d.name, files, bar);
                }));
            }

            foreach (var t in tasks) t.Start();
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("done.");
        }


        public static void ProgressBarTwoLineDemo(IConsole console)
        {
            console.ForegroundColor = ConsoleColor.Black;
            console.WriteLine("ready press enter.");
            Console.ReadLine();
            console.WriteLine("processing");
            console.ForegroundColor = ConsoleColor.White;
            var dirCnt = 10;
            var filesPerDir = 100;
            var fileCnt = dirCnt * filesPerDir;
            var r = new Random();
            var q = new ConcurrentQueue<string>();
            foreach (var name in TestData.MakeNames(2000)) q.Enqueue(name);
            var dirs = TestData.MakeObjectNames(dirCnt).Select(dir => new
            {
                name = dir,
                cnt = r.Next(filesPerDir)
            });

            var tasks = new List<Task>();
            var bars = new List<ProgressBar>();
            foreach (var d in dirs)
            {
                var files = q.Dequeue(d.cnt).ToArray();
                if (files.Length == 0) continue;
                var bar = new ProgressBar(console, PbStyle.DoubleLine, files.Count());
                bars.Add(bar);
                bar.Refresh(0, d.name);
                tasks.Add(new Task(() => ProcessFakeFiles(d.name, files, bar)));
            }

            foreach (var t in tasks) t.Start();
            Task.WaitAll(tasks.ToArray());
            console.ForegroundColor = ConsoleColor.Black;
            console.WriteLine("done.");
        }

        public static void ProgressBarDemo(IConsole con)
        {
            var r = new Random();
            var dirs = TestData.MakeObjectNames(r.Next(20) + 3);

            var tasks = new List<Task>();
            var bars = new List<ProgressBar>();
            foreach (var d in dirs)
            {
                var dir = new DirectoryInfo(d);
                var files = TestData.MakeFileNames(r.Next(100) + 10);
                if (files.Length == 0) continue;
                var bar = new ProgressBar(con, files.Count());
                bars.Add(bar);
                bar.Refresh(0, d);
                tasks.Add(new Task(() => ProcessRealFiles(d, files, bar)));
            }
            con.ForegroundColor = ConsoleColor.Black;
            con.WriteLine("ready press enter.");
            Console.ReadLine();
            con.WriteLine("processing");

            foreach (var t in tasks) t.Start();
            Task.WaitAll(tasks.ToArray());
            con.WriteLine("done.");
        }

        public static void ProcessFakeFiles(string directory, string[] files, ProgressBar bar)
        {
            foreach (var file in files)
            {
                bar.Next(file);
                Thread.Sleep(50);
            }
        }


        public static void ProcessRealFiles(string directory, string[] files, ProgressBar bar)
        {
            var cnt = files.Count();
            foreach (var file in files)
            {
                bar.Next(new FileInfo(file).Name);
                Thread.Sleep(50);
            }
        }

    }
}
