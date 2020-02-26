using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Konsole.Samples
{
    public static class ConstructorsShouldBeThreadSafe
    {
        public static void Demo(IConsole console)
        {
            //_demo(console);
        }
        public static int[] _demo()
        {
            var sw = new Stopwatch();
            sw.Start();
            int numThreads = 50;
            // what do I mean by 'threadsafe' specifically? i.e. what am I going to test that should be happening?
            // create (N) threads, start all as fast as possible, ensure each of the progressbars get a unique position and none of the progress bars overlap 
            var bag = new ConcurrentBag<ProgressBar>();
            var tasks = new Task[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                var i1 = i + 1;
                tasks[i] = new Task(() =>
                {
                    int num = i1;
                    var pb = new ProgressBar(PbStyle.DoubleLine, 100);
                    Thread.Sleep(10);
                    pb.Refresh(100, $"test {num}");
                    bag.Add(pb);
                });
            }
            sw.Stop();
            foreach (var t in tasks) t.Start();
            Task.WaitAll(tasks, 4000);

            // confirm all the progressbars have a unique and non overlapping space on the console
            var ypositions = bag.Select(b => b.Y).OrderBy(i => i).ToList();
            Console.WriteLine("---");
            ypositions.ForEach(i => Console.Write($" {i}"));
            Console.WriteLine("---");
            return ypositions.ToArray();
        }
    }
}
