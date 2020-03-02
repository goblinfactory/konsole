using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole.Samples
{
    public class ProgressBarInsideWindow
    {
        //begin-snippet: ProgressBarInsideWindow
        public static void Main(string[] args)
        {
            var w = Window.OpenBox("tasks", 64, 9);
            var left = w.SplitLeft("files");
            var right = w.SplitRight("users");
            
            var pb1 = new ProgressBar(left, 100);
            pb1.Refresh(50, "hotel-california.mp3");

            var pb2 = new ProgressBar(right, 100);
            pb2.Refresh(10, "Clint Eastwood");

            Console.ReadKey(true);
            var pbl = new List<ProgressBar>();
            for (int i = 1; i < 6; i++)
            {
                var pb = new ProgressBar(left, 100);
                pbl.Add(pb);
                pb.Refresh(100, $"hello {i}");
                Console.ReadKey(true);
            }
        }
        //end-snippet
    }
}
