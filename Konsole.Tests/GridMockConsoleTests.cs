using System;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Maintenance;
using ApprovalTests.Reporters;
using Goblinfactory.Konsole;
using Goblinfactory.Konsole.Mocks;
using NUnit.Framework;


namespace Goblinfactory.ProgressBar.Tests.Internal
{
    [UseReporter(typeof(DiffReporter))] 
    public class GridMockConsoleTests
    {

        [Test]
        public void cursor_X_andY_tests()
        {
            var console = new MockConsole(20,20);
        }

        [Test]
        public void write_and_write_line_simple_usages()
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
            Console.WriteLine(console.Buffer);
            Assert.That(console.LinesTextTrimmed, Is.EqualTo(expected));
        }

        [Test]
        public void cursor_top_should_show_current_line()
        {
            var console = new MockConsole(80, 20);
            Assert.AreEqual(0, console.Y);
            console.WriteLine("line1");
            Assert.AreEqual(1, console.Y);
            console.Write("This ");
            Assert.AreEqual(1, console.Y);
            console.Write("is ");
            Assert.AreEqual(1, console.Y);
            console.WriteLine("a test line.");
            Assert.AreEqual(2, console.Y);
            console.WriteLine("line 3");
            Assert.AreEqual(3, console.Y);
        }

        [Test]
        public void setting_cursor_top_should_allow_us_to_overwrite_lines()
        {
            var console = new MockConsole(80, 20);
            console.WriteLine("line 0");
            console.WriteLine("line 1");
            console.WriteLine("line 2");
            console.Y = 1;
            console.WriteLine("new line 1");
            var expected = new[]
            {
                "line 0",
                "new line 1",
                "line 2"
            };
            Console.WriteLine(console.Buffer);
            Assert.That(console.LinesTextTrimmed, Is.EqualTo(expected));
        }

        [Test]
        public void setting_x_and_y_tests()
        {
            var console = new MockConsole(5, 5);
            console.PrintAt(0, 0, "*");
            console.PrintAt(2, 2, "*");
            console.PrintAt(4, 4, "*");
            // all lines are trimmed
            var trimmed = new[]
            {
                "*",
                "",
                "  *",
                "",
                "    *",
            };

            var buffer = new[]
            {
                "*    ",
                "     ",
                "  *  ",
                "     ",
                "    *"
            };

            Console.WriteLine(console.Buffer);
            Assert.That(console.LinesTextTrimmed, Is.EqualTo(trimmed));
            Assert.That(console.LinesText, Is.EqualTo(buffer));

        }


        [Test]
        public void overflow_text_should_wrap_onto_next_line()
        {
            var console = new MockConsole(8, 20);
            console.WriteLine("1234567890");
            console.WriteLine("---");
            console.WriteLine("12345678901234567890");
            Console.WriteLine(console.Buffer);
            Console.WriteLine();
            Approvals.Verify(console.Buffer);
        }


    }
}
