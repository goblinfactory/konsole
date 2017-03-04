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
        public void Write_should_write_to_end_of_line_and_WriteLine_should_write_to_current_line_and_move_cursor_to_beginning_of_next_line()
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
        public void When_writing_text_that_flows_over_multiple_lines_text_should_flow_over_to_next_lines()
        {
            Assert.Inconclusive("need to test writing something that will span 3 or more lines.");
        }

        [Test]
        public void print_to_the_parent()
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