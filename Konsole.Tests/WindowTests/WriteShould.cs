using System;
using ApprovalTests.Reporters;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    [UseReporter(typeof(DiffReporter))]
    public class WriteShould
    {

        [Test]
        public void write_relative_to_the_window_being_printed_to_not_the_parent()
        {
            var c = new MockConsole(6, 4);
            c.WriteLine("------");
            c.WriteLine("------");
            c.WriteLine("------");
            c.WriteLine("------");
            var w = new Window(1, 1, 4, 2, true, c, K.Transparent);
            w.Write("X");
            w.Write(" Y");
            var expected = new[]
            {
                "------",
                "-X Y--",
                "------",
                "------"
            };
            Console.WriteLine(c.BufferWrittenString);
            c.Buffer.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void write_to_end_of_line_and_WriteLine_should_write_to_current_line_and_move_cursor_to_beginning_of_next_line()
        {
            var console = new Window(80, 20, false);
            console.WriteLine("line1");
            console.Write("This ");
            console.Write("is ");
            console.WriteLine("a test line.");
            console.WriteLine("line 3");

            var expected = new[]
            {
                "line1",
                "This is a test line.",
                "line 3"
            };
            System.Console.WriteLine(console.BufferWrittenString);
            Assert.That(console.BufferWrittenTrimmed, Is.EqualTo(expected));
        }


        [Test]
        public void print_to_the_parent_if_echo_set()
        {
            var console = new MockConsole(3, 3);
            console.ForegroundColor = ConsoleColor.Red;
            console.BackgroundColor = ConsoleColor.White;
            console.PrintAt(0, 0, "X");

            var w = new Window(console);
            w.Write("YY");

            var expectedAfter = new[]
            {
                "YY ",
                "   ",
                "   "
            };

            Assert.AreEqual(expectedAfter, console.Buffer);
        }


        [Test]
        public void not_increment_cursortop_or_left_of_parent_window()
        {
            var parent = new Window(80, 20, false);
            var state = parent.State;

            var console = new Window(parent);
            state.ShouldBeEquivalentTo(parent.State);

            console.Write("This is");
            state.ShouldBeEquivalentTo(parent.State);

            console.Write(" a test line");
            state.ShouldBeEquivalentTo(parent.State);
        }


    }
}