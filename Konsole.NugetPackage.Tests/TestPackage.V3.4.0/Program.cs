using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;
using Konsole.Layouts;

namespace TestPackage.V3._4._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
        }
        static void Test1()
        {
            var pb = new ProgressBar(10);

            for (int i =0; i<11; i++)
            {
                pb.Refresh(i, "cat "+ i);
                Console.ReadKey(true);
            }
            
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
