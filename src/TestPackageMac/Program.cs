using System;
using Konsole;
using System.Diagnostics;

namespace TestPackageMac
{
    class Program
    {

        static void Main(string[] args)
        {
            var p = Process.GetCurrentProcess();
            Console.WriteLine($"Paused so you can attach debugger. Current process is :'{p.ProcessName}'");
            Console.ReadLine();
            Console.Clear();
            var w = new Window();
            var left = w.SplitLeft("left");
            var right = w.SplitRight("right");

            var top = left.SplitTop("top");

            // this program will cause PlatformNotSupportedException to be thrown
            // as soon as writing into bottom window causes the window to need to 
            // be scrolled. 

            // this is the last feature I need to resolve in order to make
            // konsole fully cross platform and totally usable.

            var bot = left.SplitBottom("bot");
            right.WriteLine("hello from Konsole, press enter to quit");
            for (int i = 0; i < 20; i++)
            {
                bot.WriteLine($"number {i}");
                Console.ReadKey(true);
            }
            Console.Clear();
        }
    }
}
