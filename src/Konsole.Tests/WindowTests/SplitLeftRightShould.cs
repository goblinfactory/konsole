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
                "┌ left ─┌─ right ┐",
                "│two    │two     │",
                "│three  │three   │",
                "│four   │four    │",
                "└───────└────────┘"
            };

            con.Buffer.Should().BeEquivalentTo(expected);
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
                "┌─ left ─┌─ right ┐",
                "│two     │two     │",
                "│three   │three   │",
                "│four    │four    │",
                "└────────└────────┘"
            };

            con.Buffer.Should().BeEquivalentTo(expected);
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
                "┌─ left ─┌─ right ─┐",
                "│two     │two      │",
                "│three   │three    │",
                "│four    │four     │",
                "└────────└─────────┘"
            };

            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        [TestCase(1, 19)]
        [TestCase(2, 20)]
        [TestCase(3, 21)]
        public void LeftHalf_and_RightHalf_WithoutBorder_ShouldFillTheParentConsole(int test, int width)
        {
            // test to show how uneven lines are split between left and right windows.
            var c = new MockConsole(width, 5);
            (var left, var right) = c.SplitLeftRight(BorderCollapse.None);
            left.WriteLine("one");
            left.WriteLine("two");
            left.WriteLine("three");

            right.WriteLine("four");
            right.WriteLine("five");
            right.Write("six");
            Console.WriteLine(c.BufferString);

            var _19Cols = new[]
            {
                    "one      four      ",
                    "two      five      ",
                    "three    six       ",
                    "                   ",
                    "                   ",
            };

            var _20Cols = new[]
            {
                    "one       four      ",
                    "two       five      ",
                    "three     six       ",
                    "                    ",
                    "                    "
            };

            var _21Cols = new[]
            {
                    "one       four       ",
                    "two       five       ",
                    "three     six        ",
                    "                     ",
                    "                     ",

            };

            var expecteds = new[]
            {
                _19Cols, _20Cols, _21Cols
            };
            c.Buffer.Should().BeEquivalentTo(expecteds[test - 1]);
        }
    }
}
