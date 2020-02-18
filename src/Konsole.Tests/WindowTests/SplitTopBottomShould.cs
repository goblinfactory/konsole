using System;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class SplitTopBottomShould
    {
        static void Fill(IConsole con)
        {
            con.WriteLine("one");
            con.WriteLine("two");
            con.WriteLine("three");
            con.WriteLine("four");
            con.Write("five");
        }

        [Test]
        public void top_half_and_bottom_half__should_fill_the_parent_console_double_5()
        {
            var con = new MockConsole(10, 5);
            (var top, var bottom) = con.SplitTopBottom("top", "bot");
            Fill(top);
            Fill(bottom);

            var expected = new[]
            {
                "┌── top ─┐",
                "│five    │",
                "├── bot ─┤",
                "│five    │",
                "└────────┘",
            };

            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void top_half_and_bottom_half__should_fill_the_parent_console_single_5()
        {
            var con = new MockConsole(10, 5);
            (var top, var bottom) = con.SplitTopBottom("top", "bot");
            Fill(top);
            Fill(bottom);

            var expected = new[]
            {
                "┌── top ─┐",
                "│five    │",
                "├── bot ─┤",
                "│five    │",
                "└────────┘",
            };

            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void top_half_and_bottom_half__should_fill_the_parent_console_single_10()
        {
            var con = new MockConsole(10, 10);
            (var top, var bottom) = con.SplitTopBottom("top", "bot");
            Fill(top);
            Fill(bottom);

            var expected = new[]
            {
                "┌── top ─┐",
                "│three   │",
                "│four    │",
                "│five    │",
                "├── bot ─┤",
                "│two     │",
                "│three   │",
                "│four    │",
                "│five    │",
                "└────────┘",
            };

            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void top_half_and_bottom_half__should_fill_the_parent_console_single_11()
        {
            var con = new MockConsole(10, 11);
            (var top, var bottom) = con.SplitTopBottom("top", "bot");
            Fill(top);
            Fill(bottom);

            var expected = new[]
            {
                "┌── top ─┐",
                "│two     │",
                "│three   │",
                "│four    │",
                "│five    │",
                "├── bot ─┤",
                "│two     │",
                "│three   │",
                "│four    │",
                "│five    │",
                "└────────┘",
            };

            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void top_half_and_bottom_half__should_fill_the_parent_console_10_high_double()
        {
            var con = new MockConsole(10, 11);
            (var top, var bottom) = con.SplitTopBottom("top", "bot");
            Fill(top);
            Fill(bottom);

            var expected = new[]
            {
                "┌── top ─┐",
                "│two     │",
                "│three   │",
                "│four    │",
                "│five    │",
                "├── bot ─┤",
                "│two     │",
                "│three   │",
                "│four    │",
                "│five    │",
                "└────────┘",
            };

            con.Buffer.ShouldBe(expected);
        }


        [Test]
        public void LeftHalf_and_RightHalf_ShouldFillTheParentConsole_19wide()
        {
            var con = new MockConsole(19, 5);
            (var left, var right) = con.SplitLeftRight("left", "right");
            Fill(left);
            Fill(right);

            var expected = new[]
            {
                "┌─ left ─┬─ right ┐",
                "│three   │three   │",
                "│four    │four    │",
                "│five    │five    │",
                "└────────┴────────┘"
            };

            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void LeftHalf_and_RightHalf_ShouldFillTheParentConsole_20wide()
        {
            var con = new MockConsole(20, 5);
            (var left, var right) = con.SplitLeftRight("left", "right");
            Fill(left);
            Fill(right);

            var expected = new[]
            {
                "┌─ left ─┬─ right ─┐",
                "│three   │three    │",
                "│four    │four     │",
                "│five    │five     │",
                "└────────┴─────────┘"
            };

            con.Buffer.ShouldBe(expected);
        }

        // tests to show how uneven lines are split between left and right windows.
        // ------------------------------------------------------------------------

        [Test]
        public void LeftHalf_and_RightHalf_WithoutBorder_ShouldFillTheParentConsole_19wide()
        {
            var c = new MockConsole(19, 5);
            (var left, var right) = c.SplitLeftRight(BorderCollapse.None);
            Fill(left);
            Fill(right);

            var expected = new[]
            {
                    "one      one       ",
                    "two      two       ",
                    "three    three     ",
                    "four     four      ",
                    "five     five      ",
            };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void LeftHalf_and_RightHalf_WithoutBorder_ShouldFillTheParentConsole_20wide()
        {
            var c = new MockConsole(20, 5);
            (var left, var right) = c.SplitLeftRight(BorderCollapse.None);
            Fill(left);
            Fill(right);

            var expected = new[]
            {
                    "one       one       ",
                    "two       two       ",
                    "three     three     ",
                    "four      four      ",
                    "five      five      ",
            };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void LeftHalf_and_RightHalf_WithoutBorder_ShouldFillTheParentConsole_21wide()
        {
            var c = new MockConsole(21, 5);
            (var left, var right) = c.SplitLeftRight(BorderCollapse.None);
            Fill(left);
            Fill(right);
            var expected = new[]
            {
                    "one       one        ",
                    "two       two        ",
                    "three     three      ",
                    "four      four       ",
                    "five      five       ",
            };
            c.Buffer.ShouldBe(expected);
        }

    }
}
