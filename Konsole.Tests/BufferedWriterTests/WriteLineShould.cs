using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    class WriteLineShould
    {
        [Test]
        public void preserve_the_foreground_and_background_color()
        {
            var console = new MockConsole(3, 3);
            console.ForegroundColor = ConsoleColor.Red;
            console.BackgroundColor = ConsoleColor.White;
            console.PrintAt(0, 0, "X");

            var expectedBefore = new[]
            {
                "Xrw wk wk",
                " wk wk wk",
                " wk wk wk"
            };

            Assert.AreEqual(expectedBefore, console.BufferWithColor);

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

        [Test]
        public void increment_cursortop_position_of_calling_window()
        {
            var console = new Window(80, 20, false);
            Assert.AreEqual(0, console.CursorTop);
            console.WriteLine("line1");
            Assert.AreEqual(1, console.CursorTop);
            console.Write("This ");
            Assert.AreEqual(1, console.CursorTop);
            console.Write("is ");
            Assert.AreEqual(1, console.CursorTop);
            console.WriteLine("a test line.");
            Assert.AreEqual(2, console.CursorTop);
            console.WriteLine("line 3");
            Assert.AreEqual(3, console.CursorTop);
        }

        [Test]
        public void not_increment_cursortop_or_left_of_parent_window()
        {
            var parent = new Window(80,20,false);
            var state = parent.State;

            var console = new Window(parent);

            console.WriteLine("This");
            state.ShouldBeEquivalentTo(parent.State);

            console.WriteLine(" is a test line.");
            state.ShouldBeEquivalentTo(parent.State);
            
        }

    }
}
