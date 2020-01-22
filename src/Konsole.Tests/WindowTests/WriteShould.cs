using System;
using ApprovalTests.Reporters;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    [UseReporter(typeof(DiffReporter))]
    public class WriteShould
    {
        [Ignore("fix later after refactoring write and writeline to seperate partial")]
        [Test]
        public void when_writing_to_last_char_on_screen_move_cursor_to_next_line()
        {
            var con = new MockConsole(6, 4);
            con.Write("123456");
            con.Write("XY    ");
            var expected = new[]
            {
                "123456",
                "      ",
                "XY    ",
                "      "
            };
            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Ignore("fix later after refactoring write and writeline to seperate partial")]
        [Test]
        public void when_printing_ends_exactly_on_last_char_of_screen_do_not_automatically_scroll_until_user_starts_printing_again()
        {
            // so that we can allow user to print using all the lines of the window.
            var con = new MockConsole(6, 3);
            con.Write("123456");
            con.Write("ABCDEF");
            con.Write("789012");
            var expected = new[]
            {
                "123456",
                "ABCDEF",
                "789012"
            };

            con.Buffer.Should().BeEquivalentTo(expected);
            con.Write(".");
            expected = new[]
            {
                "ABCDEF",
                "789012",
                ".     "
            };
            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void allow_embedded_interpolations_without_exception()
        {
            var con = new MockConsole(7, 3);
            con.Write("{0}");
            con.Write("{0}", "cat");
            con.Write("{json}");
            var expected = new[]
            {
                "{0}cat{",
                "json}  ",
                "       "
            };
            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void write_relative_to_the_window_being_printed_to_not_the_parent()
        {
            var c = new MockConsole(6, 4);
            c.WriteLine("------");
            c.WriteLine("------");
            c.WriteLine("------");
            c.Write("------");
            var w = new Window(1, 1, 4, 2, c, K.Transparent);
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
            c.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void write_to_end_of_line_and_WriteLine_should_write_to_current_line_and_move_cursor_to_beginning_of_next_line()
        {
            var console = new MockConsole(80, 20);
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
            var parent = new MockConsole(80, 20);
            var state = parent.State;

            var console = new Window(parent);
            state.Should().BeEquivalentTo(parent.State);

            console.Write("This is");
            state.Should().BeEquivalentTo(parent.State);

            console.Write(" a test line");
            state.Should().BeEquivalentTo(parent.State);
        }


    }
}