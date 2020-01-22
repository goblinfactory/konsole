using Konsole.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static System.ConsoleColor;

namespace Konsole.Samples
{
    public static class WindowClientServerSamples
    {
        public static void Demo()
        {
            var con = Window.OpenBox("client server demo", 110, 30);

            con.WriteLine("starting client server demo");
            var client = new Window(1, 4, 20, 20, ConsoleColor.Gray, ConsoleColor.DarkBlue, con).Concurrent();
            var server = new Window(25, 4, 20, 20, con).Concurrent();
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
            var names = Window.OpenBox("names", 50, 4, 40, 10);
            TestData.MakeNames(40).OrderByDescending(n => n).ToList()
                 .ForEach(n => names.WriteLine(n));

            con.WriteLine("starting numbers demo");
            var numbers = Window.OpenBox("{numbers", 50, 15, 40, 10, new BoxStyle() { ThickNess = LineThickNess.Double, Body = new Colors(White, Blue) });
            Enumerable.Range(1, 200).ToList()
                 .ForEach(i => numbers.WriteLine(i.ToString())); // shows scrolling

            Console.ReadKey(true);
        }
    }
}
