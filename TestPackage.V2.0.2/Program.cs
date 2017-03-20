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
            var pb = new ProgressBar(50);
            pb.Refresh(25, "cats");
            Console.ReadKey(true);
            pb.Max = 60;
            Console.ReadKey(true);
        }
    }
}
