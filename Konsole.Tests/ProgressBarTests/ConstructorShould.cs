using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.ProgressBarTests
{
    public class ConstructorShould
    {
        [Test]
        public void move_cursor_down_two_lines_to_preserve_space_for_the_progressbar()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void be_threadsafe()
        {
            var sw = new Stopwatch();
            sw.Start();
            int numThreads = 50;
            // what do I mean by 'threadsafe' specifically? i.e. what am I going to test that should be happening?
            // create 10 threads, start all as fast as possible, ensure each of the progressbars get a unique position and none of the progress bars overlap 
            var bag = new List<ProgressBar>();
            var console = new MockConsole(80, numThreads * 2 + 1);
            var tasks = new Task[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                var i1 = i;
                tasks[i] = new Task(() =>
                {
                    int num = i1;
                    var pb = new ProgressBar(100, console);
                    Thread.Sleep(10);
                    pb.Refresh(50,$"test {num}");
                    bag.Add(pb);
                });
            }
            sw.Stop();
            foreach (var t in tasks) t.Start();
            Task.WaitAll(tasks,2000);
            // confirm all the progressbars have a unique and non overlapping space on the console
            var ypositions = bag.Select(b => b.Y).OrderBy(i => i).ToArray();
            // ensure no duplicates 
            var expected = Enumerable.Range(0, numThreads).Select(i => i*2).ToArray();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(console.BufferWrittenString);
            ypositions.ShouldBeEquivalentTo(expected);
            

        }
    }
}
