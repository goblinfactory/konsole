using System;
using static System.ConsoleColor;

namespace Konsole.Samples
{
    public static class InlineWindowSandbox
    {
        public static void InlineWindowMovesParentConsoleCursorTopToBelow()
        {
            Console.WriteLine("this is the top line");
            var w1 = new Window(6, White, Red);
            Console.ReadKey(true);

            w1.WriteLine("line1");
            w1.WriteLine("line2");
            w1.WriteLine("line3");
            w1.WriteLine("line4");
            w1.WriteLine("line5");
            Console.ReadKey(true);
            w1.WriteLine("line6");
            w1.WriteLine("line7");
            Console.ReadKey(true);

            Console.WriteLine("line2");
            Console.ReadKey(true);
            var w2 = w1.Open(3, Black, White);
            Console.ReadKey(true);
            w2.WriteLine(Red, "w2: hello1");
            w2.WriteLine(Red, "w2: hello2");
            Console.ReadKey(true);

            w2.WriteLine(Red, "w2: hello3");
            w2.WriteLine(Red, "w2: hello4");
            Console.ReadKey(true);

            w1.WriteLine(Yellow, "w1: hello1");
            w1.WriteLine(Yellow, "w1: hello2");
            w1.Write(Yellow, "w1: hello3");
            Console.ReadKey(true);
        }
    }
}
