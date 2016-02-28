using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using Goblinfactory.Konsole.Mocks;
using NUnit.Framework;

namespace Konsole.Tests
{
    [UseReporter(typeof(DiffReporter))] 
    public class MockConsoleTests
    {

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

        public class cursor_top_tests
        {
            [Test]
            public void calling_writeline_should_increment_cursortop_position()
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
            public void setting_cursor_top_to_a_previously_written_line_should_allow_us_to_overwrite_previously_written_lines()
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

        }


        [Test]
        public void printat_tests()
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
