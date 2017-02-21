using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Konsole.Tests.ConsoleTests
{
    [UseReporter(typeof(DiffReporter))]
    public class WriteTests
    {
        [Test]
        public void Overflowing_buffer_should_scroll_buffer_up()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void overflow_x_should_wrap_to_next_line()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void overflow_x_and_y_should_wrap_and_scroll()
        {
            Assert.Inconclusive();
        }


        [Test]
        public void overflow_text_should_wrap_onto_next_line()
        {
            var console = new Console(8, 20);
            console.WriteLine("1234567890");
            console.WriteLine("---");
            console.WriteLine("12345678901234567890");
            System.Console.WriteLine(console.Buffer);
            System.Console.WriteLine();
            Approvals.Verify(console.Buffer);
        }

        [Test]
        public void readable_buffer_should_show_which_lines_are_highlighted()
        {
            var normal = ConsoleColor.Black;
            var hilite = ConsoleColor.White;

            var console = new Console(40, 10);
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

            var hlBuffer = console.HilighterBuffer(hilite, '#', ' ');
            System.Console.WriteLine(hlBuffer);
            System.Console.WriteLine();
            Approvals.Verify(hlBuffer);
        }

        [Test]
        public void Write_should_write_to_end_of_line_and_WriteLine_should_write_to_current_line_and_move_cursor_to_beginning_of_next_line()
        {
            var console = new Console(80, 20);
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
            System.Console.WriteLine(console.Buffer);
            Assert.That(console.TrimmedLines, Is.EqualTo(expected));
        }
    }
}