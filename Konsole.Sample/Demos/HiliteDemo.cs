using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Sample.Demos
{
    public class HiliteDemo
    {
        public static void Run(IConsole con)
        {
            var normal = ConsoleColor.Black;
            var hilite = ConsoleColor.White;

            var console = new Window(40, 10, true);
            console.ForegroundColor = ConsoleColor.Red;

            console.BackgroundColor = normal;
            console.WriteLine("menu item 1");
            console.WriteLine("menu item 2");
            console.Write("menu ");

            console.BackgroundColor = hilite;
            console.Write("item");

            console.BackgroundColor = normal;
            console.WriteLine(" 3");
            console.WriteLine("menu item 4");
            console.WriteLine("menu item 5");
        }
    }
}
