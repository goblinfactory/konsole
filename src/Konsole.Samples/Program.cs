using Konsole.Internal;
using System;
using System.Linq;
using static System.ConsoleColor;

namespace Konsole.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            //var win = new Window();
            //var left = win.SplitLeft("left", LineThickNess.Single);
            //var right = win.SplitRight("right", LineThickNess.Single);
            //var botright = right.SplitBottom("bottom").SplitRight("right").SplitBottom("bot").SplitRight("r");
            //Console.ReadLine();
            //botright.Write("cats dogs");
            //Console.ReadLine();
            //botright.Write("... apples pies other stuff that should cause wrapping and scrolling!");
            //Console.ReadLine();

            var con = new Window();
            con.WriteLine("starting client server demo");
            var client = new Window(1, 4, 20, 20, Gray, DarkBlue, con);
            var server = new Window(25, 4, 20, 20, con);
            client.WriteLine("CLIENT");
            client.WriteLine("------");
            server.WriteLine("SERVER");
            server.WriteLine("------");
            client.WriteLine("<-- PUT some long text to show wrapping");
            server.WriteLine(DarkYellow, "--> PUT some long text to show wrapping");
            server.WriteLine(Red, "<-- 404|Not Found|some long text to show wrapping|");
            client.WriteLine(Red, "--> 404|Not Found|some long text to show wrapping|");

            con.WriteLine("starting names demo");

            // let's open a window with a box around it
            var names = Window.Open(50, 4, 40, 10, "names");

            TestData.MakeNames(40).OrderByDescending(n => n).ToList()
                 .ForEach(n => names.WriteLine(n));

            con.WriteLine("starting numbers demo");

            var numbers = Window.Open(50, 15, 40, 10, "numbers",LineThickNess.Double, White, Blue);
            
            Enumerable.Range(1, 200).ToList()
                 .ForEach(i => numbers.WriteLine(i.ToString())); // shows scrolling

            Console.ReadLine();

            using (var writer = new HighSpeedWriter())
            {
                var window = new Window(writer);
                Diagnostics.SelfTest.Test(window, () => writer.Flush());
            }
        }
    }
}