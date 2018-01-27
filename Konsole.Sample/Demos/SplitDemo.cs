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
        public static void DemoSplitLeftRight(IConsole con)
        {
            con.ForegroundColor = ConsoleColor.White;
            var left = con.SplitLeft("left");
            var right = con.SplitRight("right");
            var names = TestData.MakeNames(10);
            foreach (var name in names)
            {
                left.WriteLine(ConsoleColor.Blue, name);
                right.WriteLine(ConsoleColor.Yellow, name);
            }        
        }

        public static void DemoSplitTopBottom(IConsole con)
        {
            con.ForegroundColor = ConsoleColor.White;
            var top = con.SplitTop("server top");
            var bottom = con.SplitBottom("server bottom");
            var names = TestData.MakeNames(10);
            foreach (var name in names)
            {
                top.WriteLine(ConsoleColor.Blue, name);
                bottom.WriteLine(ConsoleColor.Yellow, name);
            }

        }

    }
}
