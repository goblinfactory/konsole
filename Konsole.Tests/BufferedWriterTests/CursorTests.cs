using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    [UseReporter(typeof(DiffReporter))]
    public class CursorTests
    {

        public class CursorLeft_tests
        {
            [Test]
            public void CursorLeft_should_return_the_x_position_that_the_next_character_will_be_written_to()
            {
                var console = new Window(30, 20, false);
                Assert.AreEqual(0, console.CursorLeft);
                console.Write("Today ");
                Assert.AreEqual(6, console.CursorLeft);
                
                console.Write("is ");
                Assert.AreEqual(9, console.CursorLeft);
                
                console.Write("a good day to test.");
                Assert.AreEqual(28, console.CursorLeft);
                
                // digit 3 should overflow to next line.
                console.Write("123");
                Assert.AreEqual(1, console.CursorTop);
            }

            [Test]
            public void Setting_cursor_position_should_set_cursor_x_and_y_position()
            {
                // #ADH : while this tests passes, it doesnt prove that echo causes the correct out to be displayed. Something to think about! mmm...interesting.
                var console = new Window(10, 20, false);
                console.CursorLeft = 5;
                console.CursorTop = 1;
                console.Write("4");
                Assert.AreEqual(new[] { "          ","     4    " }, console.BufferWritten);
            }

            [Test]
            public void Setting_cursor_left_should_set_cursor_x_position()
            {
                var console = new Window(10, 20, false);
                console.Write("123");
                Assert.AreEqual(3,console.CursorLeft);
                Assert.AreEqual(0, console.CursorTop);
                console.CursorLeft = 5;
                console.Write("4");
                Assert.AreEqual(new[] {"123  4    "}, console.BufferWritten);
            }

            [Test]
            public void setting_CursorLeft_position_should_change_x_position_without_affecting_y_position_and_allow_writing_at_different_x_positions()
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
        }

        public class CursorTop_tests
        {
            [Test]
            public void calling_writeline_should_increment_cursortop_position()
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
            public void setting_cursor_top_to_a_previously_written_line_should_allow_us_to_overwrite_previously_written_lines()
            {
                var console = new Window(80, 20, false);
                console.WriteLine("line 0");
                console.WriteLine("line 1");
                console.WriteLine("line 2");
                console.CursorTop = 1;
                console.WriteLine("new line 1");
                var expected = new[] {
                    "line 0",
                    "new line 1",
                    "line 2"
                };
                System.Console.WriteLine(console.BufferWrittenString);
                Assert.That(console.BufferWrittenTrimmed, Is.EqualTo(expected));
            }

        }


        [Test]
        public void PrintAt_tests()
        {
            var console = new Window(5, 5, false);
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

            System.Console.WriteLine(console.BufferWrittenString);
            Assert.That(console.BufferWrittenTrimmed, Is.EqualTo(trimmed));
            Assert.That(console.BufferWritten, Is.EqualTo(buffer));
        }


        [Test]
        public void PrintAt_with_text_causing_overflow_should_overflow_to_next_line()
        {
            var console = new Window(5, 5, false);
            console.PrintAt(2, 2, "12345");

            var expected = new[]
            {
                "     ",
                "     ",
                "  123",
                "45   ",
                "     "
            };

            System.Console.WriteLine(console.BufferWrittenString);
            Assert.AreEqual(expected, console.Buffer);
        }






    }
}
