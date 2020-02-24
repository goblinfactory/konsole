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
            var client = con.Open(new WindowSettings { SX = 1, SY = 4, Height = 20, Width = 20, Theme = new StyleTheme(Gray, DarkBlue) });
            var server = con.Open(25, 4, 20, 20);
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
            var numbers = Window.OpenBox("numbers", new WindowSettings
            {
                SX = 50,
                SY = 15,
                Width = 40,
                Height = 10,
                Theme = new Style(thickNess: LineThickNess.Double, body: new Colors(White, Blue)).ToTheme()
            });
            
            Enumerable.Range(1, 200).ToList()
                 .ForEach(i => numbers.WriteLine(i.ToString())); // shows scrolling
            Console.ReadKey(true);
        }
    }
}
