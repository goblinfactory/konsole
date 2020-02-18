using System;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class SplitLeftRightShould
    {
        static void Fill(IConsole con)
        {
            con.WriteLine("one");
            con.WriteLine("two");
            con.WriteLine("three");
            con.Write("four");
        }

        [Test]
        public void LeftHalf_and_RightHalf_ShouldFillTheParentConsole_18wide()
        {
            var con = new MockConsole(18, 5);
            (var left, var right) = con.SplitLeftRight("left", "right");
            Fill(left);
            Fill(right);

            var expected = new[]
            {
                "┌ left ─┬─ right ┐",
                "│two    │two     │",
                "│three  │three   │",
                "│four   │four    │",
                "└───────┴────────┘"
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
                "│two     │two     │",
                "│three   │three   │",
                "│four    │four    │",
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
                "│two     │two      │",
                "│three   │three    │",
                "│four    │four     │",
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
                    "                   ",
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
                    "                    ",
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
                    "                     ",
            };
            c.Buffer.ShouldBe(expected);
        }

    }
}
