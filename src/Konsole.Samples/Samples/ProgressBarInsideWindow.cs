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
            var w = Window.OpenBox("tasks", 60, 8);
            var right = w.SplitLeft("files");
            var left = w.SplitRight("users");
            
            var pb1 = new ProgressBar(left, 100);
            pb1.Refresh(50, "hotel-california.mp3");

            var pb2 = new ProgressBar(right, 100);
            pb2.Refresh(10, "Clint Eastwood");
        }
        //end-snippet
    }
}
