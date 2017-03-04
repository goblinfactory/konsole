using System;
using FluentAssertions;
using Konsole.Tests.Helpers;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class PrintAtShould
    {
        [Test]
        public void Not_change_cursor_position_when_printing()
        {
            var c = new MockConsole(5, 3);
            c.PrintAt(4, 1, "O");
            c.WriteLine("one");

            var expected = new[]
            {
                "one  ",
                "    O",
                "     "
            };
            Assert.AreEqual(expected, c.Buffer);
        }

        [Test]
        public void print_to_the_parent()
        {
            var console = new MockConsole(5, 3);
            console.ForegroundColor = ConsoleColor.Red;
            console.BackgroundColor = ConsoleColor.White;
            console.PrintAt(0, 0, "X");

            var w = new Window(console);
            w.PrintAt(3, 1, "123");

            var expectedAfter = new[]
            {
                "X    ",
                "   12",
                "3    "
            };

            Assert.AreEqual(expectedAfter, console.Buffer);
        }


        [Test]
        public void echo_printing_to_parent_in_the_right_fore_and_back_colors()
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

            Precondition.Check( ()=> expectedBefore.ShouldBeEquivalentTo(console.BufferWithColor));

            var w = new Window(console);
            w.ForegroundColor = ConsoleColor.DarkGreen;
            w.BackgroundColor = ConsoleColor.DarkCyan;
            w.PrintAt(1, 1, "YY");

            var expectedAfter = new[]
            {
                "Xrw wk wk",
                " wkYGCYGC",
                " wk wk wk"
            };

            Assert.AreEqual(expectedAfter, console.BufferWithColor);
        }


        [Test]
        public void not_change_the_parent_echo_console_fore_or_background_color()
        {
            var console = new MockConsole(3, 3);
            var state = console.State;

            var w = new Window(console);
            w.ForegroundColor = ConsoleColor.DarkGreen;
            w.BackgroundColor = ConsoleColor.DarkCyan;
            w.PrintAt(2, 2, "Y");
            console.State.ShouldBeEquivalentTo(state);
        }


        [Test]
        public void preserve_the_cursor_position()
        {
            var console = new MockConsole(3, 3);
            var state = console.State;

            var w = new Window(console);
            w.PrintAt(2, 2, "Y");

            console.State.ShouldBeEquivalentTo(state);
        }


    }
}
