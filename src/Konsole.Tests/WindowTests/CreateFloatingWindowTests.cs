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
            var float1 = Window._CreateFloatingWindow(win, new WindowSettings
            {
                SX = 5,
                SY = 1,
                Width = 7,
                Height = 5,
                Theme = new StyleTheme(White, Black)
            });
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

            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void WhenNestedTwoWindowsDeep_test()
        {
            Precondition.Check(() => WhenNestedInWindow_test());

            var con = new MockConsole(20, 8);
            var win = new Window(con);
            var float1 = Window._CreateFloatingWindow(win, new WindowSettings
            {
                SX = 5,
                SY = 1,
                Width = 7,
                Height = 5,
                Theme = new StyleTheme(White, Black)
            });
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

            con.Buffer.ShouldBe(expected);

            // now test creating a nested window
            var float2 = Window._CreateFloatingWindow(float1, new WindowSettings
            {
                SX = 3,
                SY = 2,
                Width = 3,
                Height = 2,
                Theme = new StyleTheme(White, Black)
            });
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

            con.Buffer.ShouldBe(expected);
        }
    }
}
