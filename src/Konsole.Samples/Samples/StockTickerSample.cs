using System;
using System.Collections.Generic;
using System.Text;
using static System.ConsoleColor;


namespace Konsole.Samples
{
    public static class StockTickerSample
    {

        public static void Run_side_by_side()
        {
            void Wait() => Console.ReadKey(true);

            decimal amazon = 84;
            decimal bp = 146;

            void Tick(IConsole con, string sym, decimal newPrice, ConsoleColor color, char sign, decimal perc)
            {
                con.Write(White, $"{sym,-10}");
                con.WriteLine(color, $"{newPrice:0.00}");
                con.WriteLine(color, $"  ({sign}{newPrice}, {perc}%)");
                con.WriteLine("");
            }

            Console.WriteLine("line one");
            var window = new Window(40, 12);
            var nyse = window.SplitLeft("NYSE");
            var ftse100 = window.SplitRight("FTSE 100");
            Console.WriteLine("line two");

            while (true)
            {
                Tick(nyse, "AMZ", amazon -= 0.04M, Red, '-', 4.1M);
                Tick(ftse100, "BP", bp += 0.05M, Green, '+', 7.2M);
                Wait();
            }
        }

        public static void Run_side_by_side_SplitLeftRight()
        {
            void Wait() => Console.ReadKey(true);

            decimal amazon = 84;
            decimal bp = 146;

            void Tick(IConsole con, string sym, decimal newPrice, ConsoleColor color, char sign, decimal perc)
            {
                con.Write(White, $"{sym,-10}");
                con.WriteLine(color, $"{newPrice:0.00}");
                con.WriteLine(color, $"  ({sign}{newPrice}, {perc}%)");
                con.WriteLine("");
            }

            Console.WriteLine("line one");
            var window = new Window(40, 12);
            (var nyse, var ftse100) = window.SplitLeftRight("NYSE", "FTSE 100");
            Console.WriteLine("line two");

            while (true)
            {
                Tick(nyse, "AMZ", amazon -= 0.04M, Red, '-', 4.1M);
                Tick(ftse100, "BP", bp += 0.05M, Green, '+', 7.2M);
                Wait();
            }
        }

        public static void Run_above_and_below()
        {
            void Wait() => Console.ReadKey(true);

            decimal amazon = 84;
            decimal bp = 146;

            void Tick(IConsole con, string sym, decimal newPrice, ConsoleColor color, char sign, decimal perc)
            {
                con.Write(White, $"{sym,-10}");
                con.WriteLine(color, $"{newPrice:0.00}");
                con.WriteLine(color, $"  ({sign}{newPrice}, {perc}%)");
                con.WriteLine("");
            }

            
            Console.WriteLine("line one");
            var nyse = Window.OpenBox("NYSE", 20, 12, new BoxStyle() { ThickNess = LineThickNess.Single, Title = new Colors(White, Red) });

            Console.WriteLine("line two");
            var ftse100 = Window.OpenBox("FTSE 100", 20, 12, new BoxStyle() { ThickNess = LineThickNess.Double, Title = new Colors(White, Blue) });
            Console.Write("line three");


            while (true)
            {
                Tick(nyse, "AMZ", amazon -= 0.04M, Red, '-', 4.1M);
                Tick(ftse100, "BP", bp += 0.05M, Green, '+', 7.2M);
                Wait();
            }
        }
    }
}
