using Konsole;
using System;

namespace SandboxDeleteme
{
    class Program
    {
        static void Main(string[] args)
        {
            Foo();
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
