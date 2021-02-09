using Konsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static System.ConsoleColor;

namespace SandboxDeleteme
{
    class Program
    {
        // using static System.ConsoleColor;
        static void Main(string[] args)
        {
            var buffer = new MockConsole(30, 5);
            var screenshots = new List<string[]>();

            var r = new Random();
            var bass = new[] { 20, 15, 30 };

            var mixer = Window.OpenBox("mixer", 30, 5);
            Console.CursorVisible = false;
            CharBar CreateBar(int row, ConsoleColor color) => new CharBar(mixer, 6, row, mixer.WindowWidth - 7, 100, '#', color);

            mixer.WriteLine(Yellow, "left: ");
            mixer.WriteLine(Yellow, "right: ");

            var left = CreateBar(0, Green);
            var right = CreateBar(1, Red);

            mixer.PrintAt(0, 2, "Ace of base! (bada-boom)      ");

            int w = left.Width;
            int last = 0;
            foreach (var boom in bass)
            {
                Thread.Sleep(r.Next(100));
                left.Refresh(r.Next(boom));
                Thread.Sleep(r.Next(100));
                right.Refresh(r.Next(last));
                
                last = boom;
            }
            mixer.Clear();
            mixer.WriteLine("finished, press enter to quit!");
            Console.CursorVisible = false;
            Console.ReadLine();
        }

        public static void Foo()
        {
            var w = new Window(20, 5);
            (var left, var right) = w.SplitLeftRight("left", "right");

            left.WriteLine("one\r\ntwo\rthree\nfour");
            right.WriteLine("one\r\ntwo\rthree\nfour");
            var expected1 = new[]
            {
                "┌─ left ─┬─ right ─┐",
                "│three   │three    │",
                "│four    │four     │",
                "│        │         │",
                "└────────┴─────────┘"
            };
            
            Console.ReadLine();
            left.Clear();
            right.Clear();
            left.Write("one\r\ntwo\rthree\nfour");
            right.Write("one\r\ntwo\rthree\nfour");
            var expected2 = new[]
            {
                "┌─ left ─┬─ right ─┐",
                "│two     │two      │",
                "│three   │three    │",
                "│four    │four     │",
                "└────────┴─────────┘"
            };


        }
    }
}
