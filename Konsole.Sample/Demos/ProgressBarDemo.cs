using System;
using System.Linq;
using System.Threading;
using Konsole.Internal;

namespace Konsole.Sample.Demos
{
    class ProgressBarDemo
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
    }
}
