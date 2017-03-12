using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
        public class CursorLeftShould
        {
            [Test]
            public void return_the_x_position_that_the_next_character_will_be_written_to()
            {
                var console = new MockConsole(30, 20);
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
                var console = new MockConsole(10, 20);
                console.CursorLeft = 5;
                console.CursorTop = 1;
                console.Write("4");
                Assert.AreEqual(new[] { "          ","     4    " }, console.BufferWritten);
            }

            [Test]
            public void Setting_cursor_left_should_set_cursor_x_position()
            {
                var console = new MockConsole(10, 20);
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
                var console = new MockConsole(80, 20);
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
}