using Konsole.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Konsole.Samples
{
    public static class ProgressBarsWorkedParallelExample
    {
        static bool start = false;
        public static void Run()
        {
            // in this example it's safe to mix and match
            // writing with the System.Console and 
            // the background threads updating the progress
            // bars, because that never happens concurrently.
            // all the tasks finish before we resume writing
            // to the console.

            // if we want to write to the console while background
            // tasks are updating progressbar then we must 
            // use a concurrentWriter.

            var dirCnt = 15;
            var filesPerDir = 30;
            
            var r = new Random();
            var dirs = TestData.MakeObjectNames(dirCnt);

            Console.WriteLine("Press enter to start");

            var tasks = new List<Task>();
            var bars = new ConcurrentBag<ProgressBar>(); 
            foreach (var d in dirs)
            {
                var files = TestData.MakeNames(r.Next(filesPerDir));
                //var bar = new ProgressBar(files.Count());
                //var bar = new ProgressBar(TextWidth, PbStyle.DoubleLine, Max);
                var bar = new ProgressBar(PbStyle.DoubleLine, files.Count());
                bars.Add(bar);
                bar.Refresh(0, d);
                tasks.Add(ProcessFakeFiles(d, files, bar));
            }
            Console.ReadLine();
            start = true;
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("finished.");
        }

        static Task ProcessFakeFiles(string dirname, string[] files, ProgressBar bar)
        {
            return Task.Run(() =>
            {
                var r = new Random();
                while (!start) { Thread.Sleep(50); };
                bar.Max = files.Length;
                for(int i =1 ; i<= files.Length; i++)
                {
                    bar.Refresh(i, files[i-1]);
                    Thread.Sleep(r.Next(500));
                }
            });
        }
    }

    public static class QueueExtensions
    {
        public static T[] Take<T>(this Queue<T> queue, int cnt)
        {
            var items = new List<T>();
            for (int i = 0; i < cnt; i++)
            {
                if (queue.Count == 0) break;
                T item;
                if (queue.TryDequeue(out item)) items.Add(item);

            }
            return items.ToArray();
        }
    }

}
