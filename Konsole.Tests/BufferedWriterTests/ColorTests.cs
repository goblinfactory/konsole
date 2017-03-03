using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Tests.BufferedWriterTests
{
    public class ColorTests
    {
        public void When_printing_should_preserve_the_foreground_and_background_color()
        {
            var console = new MockConsole(5, 3);
            console.ForegroundColor = ConsoleColor.Red;
            console.BackgroundColor = ConsoleColor.White;
            console.PrintAt(0,2,"apples");

            var w = new Window(5,3,console);
            w.ForegroundColor = ConsoleColor.Yellow;
            console.BackgroundColor = ConsoleColor.Black;
            w.WriteLine("one");
            w.WriteLine("two");
        }
    }
}
