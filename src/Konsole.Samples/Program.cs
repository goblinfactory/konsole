using System;
using static System.ConsoleColor;

namespace Konsole.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var win = Window.Open(10, 10, 40, 10, "test");
            var child = Window.Open(20, 0, 30, 6, "child", LineThickNess.Double, White, Black, win);
            child.WriteLine("test");
            Console.ReadLine();
            //using(var writer = new HighSpeedWriter())
            //{
            //    var window = new Window(writer);
            //    Diagnostics.SelfTest.Test(window, () => writer.Flush());
            //}
        }
    }
}