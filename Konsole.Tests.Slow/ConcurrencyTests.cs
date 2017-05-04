using System;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Konsole.Drawing;
using NUnit.Framework;

namespace Konsole.Tests.Slow
{

    [UseReporter(typeof(DiffReporter))]
    public class ConcurrencyTests
    {
        [Test]
        public void WindowsAndSingleBackgroundThread()
        {
            var console = new MockConsole(80,20);
            var w1 = Window.Open(0, 0, 20, 20, "w1", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkBlue, console);
            int max = 80000;
            Task.Run(() =>
            {
                for (int i = 0; i < max; i++) w1.Write(" {0} ", i.ToString());
            }).Wait();
            Approvals.Verify(console.BufferWrittenString);
        }

        [Test]
        public void WindowsWithFourBackgroundThreads()
        {
            // run this up to 8000 without the concurrent writer and you'll see what I had to fix.
            int max = 8000;
            var console = new MockConsole(80, 20);
            var w1 = Window.Open(0, 0, 20, 20, "w1", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkBlue, console).Concurrent();
            var w2 = Window.Open(20, 0, 20, 20, "w2", LineThickNess.Single, ConsoleColor.Red, ConsoleColor.DarkYellow, console).Concurrent();
            var w3 = Window.Open(40, 0, 20, 20, "w3", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkYellow, console).Concurrent();
            var w4 = Window.Open(60, 0, 20, 20, "w4", LineThickNess.Single, ConsoleColor.Black, ConsoleColor.White, console).Concurrent();
            Task t1 = Task.Run(() =>
            {
                for (int i = 0; i < max; i++) w1.Write(" {0} ", i.ToString());
            });

            Task t2 = Task.Run(() =>
            {
                for (int i = 0; i < max; i++) w2.Write(" {0} ", i.ToString());
            });

            Task t3 = Task.Run(() =>
            {
                for (int i = 0; i < max; i++) w3.Write(" {0} ", i.ToString());
            });

            Task t4 = Task.Run(() =>
            {
                for (int i = 0; i < max; i++) w4.Write(" {0} ", i.ToString());
            });


            Task.WaitAll(new[] {t1, t2, t3, t4});

            Approvals.Verify(console.BufferWrittenString);


        }

    }
}
