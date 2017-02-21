using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using Konsole.Testing;
using NUnit.Framework;
using Console = Konsole.Testing.Console;

namespace Konsole.Tests
{
    [UseReporter(typeof(DiffReporter))]
    public class TestConsoleTests
    {
        [UseReporter(typeof(DiffReporter))]
        public class Write_and_WriteLine_tests
        {
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

        public class CursorLeft_tests
        {
            [Test]
            public void CursorLeft_should_return_the_x_position_that_the_next_character_will_be_written_to()
            {
                var console = new Console(30, 20);
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
            public void setting_CursorLeft_position_should_change_x_position_without_affecting_y_position_and_allow_writing_at_different_x_positions()
            {
                var console = new Console(80, 20);
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
                var console = new Console(80, 20);
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
                var console = new Console(80, 20);
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
                System.Console.WriteLine(console.Buffer);
                Assert.That(console.TrimmedLines, Is.EqualTo(expected));
            }

        }


        [Test]
        public void PrintAt_tests()
        {
            var console = new Console(5, 5);
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

            System.Console.WriteLine(console.Buffer);
            Assert.That(console.TrimmedLines, Is.EqualTo(trimmed));
            Assert.That(console.LinesText, Is.EqualTo(buffer));

        }





    }
}
