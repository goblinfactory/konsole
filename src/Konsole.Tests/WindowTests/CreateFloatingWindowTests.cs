using FluentAssertions;
using Konsole.Tests.Helpers;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.WindowTests
{
    public class CreateFloatingWindowTests
    {
        [Test]
        public void WhenNestedInWindow_test()
        {
            var con = new MockConsole(20, 8);
            var win = new Window(con);
            var float1 = Window._CreateFloatingWindow(5, 1, 7, 5, White, Black, true, win);
            float1.Write("00000001111111222222233333334444444");

            var expected = new[] {
                "                    ",
                "     0000000        ",
                "     1111111        ",
                "     2222222        ",
                "     3333333        ",
                "     4444444        ",
                "                    ",
                "                    "
            };

            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WhenNestedTwoWindowsDeep_test()
        {
            Precondition.Check(() => WhenNestedInWindow_test());

            var con = new MockConsole(20, 8);
            var win = new Window(con);
            var float1 = Window._CreateFloatingWindow(5, 1, 7, 5, White, Black, true, win);
            float1.Write("00000001111111222222233333334444444");
            var expected = new[] {
                "                    ",
                "     0000000        ",
                "     1111111        ",
                "     2222222        ",
                "     3333333        ",
                "     4444444        ",
                "                    ",
                "                    "
            };

            con.Buffer.Should().BeEquivalentTo(expected);

            // now test creating a nested window
            var float2 = Window._CreateFloatingWindow(3, 2, 3, 2, White, Black, true, float1);
            float2.CursorLeft = 0;
            float2.CursorTop = 0;
            float2.Write(@"===...");

            expected = new[] {
                "                    ",
                "     0000000        ",
                "     1111111        ",
                "     222===2        ",
                "     333...3        ",
                "     4444444        ",
                "                    ",
                "                    "
            };

            con.Buffer.Should().BeEquivalentTo(expected);
        }
    }
}
