using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;

namespace TestPackage.V2._0._2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("single line progress bar");
            var pb = new ProgressBar(50);
            pb.Refresh(25, "cats");
            Console.WriteLine("press any key to update refresh");
            Console.ReadKey(true);
            pb.Max = 60;
            Console.WriteLine("progress updated, press any key to test multi-line refresh");
            Console.ReadKey(true);

            // ============================================================
            var pb2 = new ProgressBar(PbStyle.DoubleLine,50);
            pb2.Refresh(25,"cats");
            Console.WriteLine("press any key to update refresh");
            Console.ReadKey(true);
            pb2.Max = 100;
            Console.WriteLine("progress updated, press enter to quit");
            Console.ReadLine();

        }
    }
}
