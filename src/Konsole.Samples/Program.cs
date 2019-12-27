using System;
using static System.ConsoleColor;

namespace Konsole.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var win = new Window();
            var left = win.SplitLeft("left", LineThickNess.Single);
            var right = win.SplitRight("right", LineThickNess.Single);

            var botright = right.SplitBottom("bottom").SplitRight("right").SplitBottom("bot").SplitRight("r");
            Console.ReadLine();
            botright.Write("cats dogs");
            Console.ReadLine();
            botright.Write("... apples pies other stuff that should cause wrapping and scrolling!");
            Console.ReadLine();
            
            using (var writer = new HighSpeedWriter())
            {
                var window = new Window(writer);
                Diagnostics.SelfTest.Test(window, () => writer.Flush());
            }
        }
    }
}