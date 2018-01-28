using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;
using Konsole.Layouts;

namespace TestPackage.V3._2._1
{
    class Program
    {
        static void Main(string[] args)
        {
            var pb = new ProgressBar(50);
            pb.Refresh(25, "cats");
            Console.ReadKey(true);
            pb.Max = 60;
            Console.ReadKey(true);
            var w = new Window();
            var left = w.SplitLeft("left");
            w.SplitRight("right");
            var top = left.SplitTop("top");
            var bottom = left.SplitBottom("bottom");

            for (int i = 0; i < 10; i++)
            {
                top.WriteLine(i.ToString());
                bottom.WriteLine((100-i).ToString());
            }
            Console.ReadKey();
        }
    }
}
