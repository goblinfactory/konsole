using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole.Samples
{
    public static class ProgressBarAtBottomOfScreen
    {
        public static void Run()
        {
            Console.Clear();
            int height = Console.WindowHeight;
            Console.CursorTop = height - 3;
            var pbl = new List<ProgressBar>();
            for(int i = 1; i<6; i++)
            {
                var pb = new ProgressBar(100);
                pbl.Add(pb);
                pb.Refresh(100, $"hello {i}");
                Console.ReadKey(true);
            }
        }

    }
}
