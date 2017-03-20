using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;
using Konsole.Drawing;
using Konsole.Internal;

namespace TestPackage.V2._0._1
{
    class Program
    {
        static void Main(string[] args)
        {
            var con = new Writer();
            con.WriteLine("starting client server demo");
            var client = new Window(1, 4, 20, 20, ConsoleColor.Gray, ConsoleColor.DarkBlue, con);
            var server = new Window(25, 4, 20, 20, con);
            client.WriteLine("CLIENT");
            client.WriteLine("------");
            server.WriteLine("SERVER");
            server.WriteLine("------");
            client.WriteLine("<-- PUT some long text to show wrapping");
            server.WriteLine(ConsoleColor.DarkYellow, "--> PUT some long text to show wrapping");
            server.WriteLine(ConsoleColor.Red, "<-- 404|Not Found|some long text to show wrapping|");
            client.WriteLine(ConsoleColor.Red, "--> 404|Not Found|some long text to show wrapping|");


            con.WriteLine("starting names demo");
            // let's open a window with a box around it by using Window.Open
            var names = Window.Open(50, 4, 40, 10, "names");
            TestData.MakeNames(40).OrderByDescending(n => n).ToList().ForEach(n => names.WriteLine(n));

            con.WriteLine("starting numbers demo");
            var numbers = Window.Open(50, 15, 40, 10, "numbers", LineThickNess.Double, ConsoleColor.White, ConsoleColor.Blue);
            Enumerable.Range(1, 200).ToList().ForEach(i => numbers.WriteLine(i.ToString()));

            Console.ReadLine();

        }
    }
}
