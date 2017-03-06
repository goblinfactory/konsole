using System;
using System.Threading;

namespace Konsole.Sample.Demos
{
    class ProgressBarDemo
    {
        public static void Run(IConsole con)
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
    }
}
