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

    public class ConstructorWithTextShould
    {
        [Test]
        public void create_simple_1_liner_progress_bar()
        {
         //Assert.Inconclusive();   
        }
    }

    public class ConstructorShould
    {
        [Test]
        public void move_cursor_down_to_preserve_space_for_the_progressbar()
        {
            var con = new MockConsole();
            Assert.AreEqual(0, con.CursorTop);
            var pb1 = new ProgressBar(con, PbStyle.DoubleLine, 10);
            Assert.AreEqual(2, con.CursorTop);
            var pb2 = new ProgressBar(con, PbStyle.SingleLine, 10);
            Assert.AreEqual(3, con.CursorTop);
        }

        //[Test]
        public void render_the_progressBar_in_its_starting_state()
        {
            
            var con = new MockConsole(40,8);
            //var pb1 = new ProgressBar(10, "CATS", con);
            //var pb2 = new ProgressBar(10, "DOGS", con);
            Console.WriteLine(con.BufferString);
            Assert.AreEqual(4, con.CursorTop);
            
        }


        [Test]
        public void be_threadsafe()
        {
            var sw = new Stopwatch();
            sw.Start();
            int numThreads = 50;
            // what do I mean by 'threadsafe' specifically? i.e. what am I going to test that should be happening?
            // create (N) threads, start all as fast as possible, ensure each of the progressbars get a unique position and none of the progress bars overlap 
            var bag = new List<ProgressBar>();
            var console = new MockConsole(80, numThreads * 2 + 1);
            var tasks = new Task[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                var i1 = i;
                tasks[i] = new Task(() =>
                {
                    int num = i1;
                    var pb = new ProgressBar(console, PbStyle.DoubleLine, 100);
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
            ypositions.Should().BeEquivalentTo(expected);
            

        }
    }
}
