using System;
using System.Linq;
using System.Threading;
using Konsole.Drawing;
using Konsole.Internal;
using Konsole.Layouts;
using Konsole.Menus;

namespace Konsole.Sample.Demos
{
    public class SplitDemo
    {
        public static void Run(IConsole con)
        {
            con.ForegroundColor = ConsoleColor.White;
            var c = con.SplitLeft();
            var s = con.SplitRight();
            var s1 = s.SplitTop("server top");
            var s2 = s.SplitBottom("server bottom");
            var names = TestData.MakeNames(5);
            foreach (var name in names)
            {
                s1.WriteLine(ConsoleColor.Blue, name);
                s2.WriteLine(ConsoleColor.Yellow, name);
            }
        
        }
    }
}
