using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPackage.V3._1._0
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Konsole.Window();
            var names = Konsole.Internal.TestData.MakeFileNames(200);
            foreach(var n in names) c.WriteLine(n);
            Console.ReadLine();
        }
    }
}
