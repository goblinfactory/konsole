using System;

namespace TestPackage.V3._2._0
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
