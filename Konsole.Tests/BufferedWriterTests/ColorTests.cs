using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    public class ColorTests
    {
        [Test]
        public void When_printing_with_WriteLine_should_preserve_the_foreground_and_background_color()
        {
            var console = new MockConsole(3, 3);
            console.ForegroundColor = ConsoleColor.Red;
            console.BackgroundColor = ConsoleColor.White;
            console.PrintAt(0,0,"X");

            var expectedBefore = new []
            {
                "Xrw wk wk",
                " wk wk wk",
                " wk wk wk"
            };

            Assert.AreEqual(expectedBefore,console.BufferWithColor);

            var w = new Window(1, 1, 2, 2, ConsoleColor.Black, ConsoleColor.White, true, console);
            w.ForegroundColor = ConsoleColor.Yellow;
            w.BackgroundColor = ConsoleColor.Black;
            w.WriteLine("Y");
            w.WriteLine("Z");

            var expectedAfter = new[]
            {
                "Xrw wk wk",
                " wkYyk wk",
                " wkZyk wk"
            };

            Assert.AreEqual(expectedAfter, console.BufferWithColor);


        }
    }
}
