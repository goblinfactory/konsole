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
            con.Write("three");
            //con.Write("four");
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
                    "│one    │one     │",
                    "│two    │two     │",
                    "│three  │three   │",
                    "└───────└────────┘"
            };

            // WTF??

            // ┌─ left ─┐┌─ right
            //  one     ││one
            //  two     ││two
            //  three   ││three
            //  ────────┘└───────

            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        [TestCase(1, 20)]
        [TestCase(2, 21)]
        [TestCase(3, 22)]
        public void LeftHalf_and_RightHalf_ShouldFillTheParentConsole(int test, int width)
        {
            // test to show how uneven lines are split between left and right windows.
            var con = new MockConsole(width, 5);
            (var left, var right) = con.SplitLeftRight("left", "right");
            left.WriteLine("one");
            left.WriteLine("two");
            left.WriteLine("three");
            //left.Write("four");

            right.WriteLine("five");
            right.WriteLine("six");
            right.WriteLine("seven");
            //right.Write($"eight {test}");

            Console.WriteLine(con.BufferString);

            var _18Cols = new[]
            {
                    "┌ left ─┌─ right ┐",
                    "│one    │four    │",
                    "│two    │five    │",
                    "│three  │six     │",
                    "└───────└────────┘"
            };

            var _19Cols = new[]
            {
                    "┌─ left ─┌─ right ┐",
                    "│one     │four    │",
                    "│two     │five    │",
                    "│three   │six     │",
                    "└────────└────────┘"
            };

            var _20Cols = new[]
            {
                    "┌─ left ─┌─ right ─┐",
                    "│one     │four     │",
                    "│two     │five     │",
                    "│three   │six      │",
                    "└────────└─────────┘"
            };

            var expecteds = new[]
            {
                _18Cols, _19Cols, _20Cols
            };
            con.Buffer.Should().BeEquivalentTo(expecteds[test - 1]);
        }


        [Test]
        [TestCase(1, 19)]
        [TestCase(2, 20)]
        [TestCase(3, 21)]
        public void LeftHalf_and_RightHalf_WithoutBorder_ShouldFillTheParentConsole(int test, int width)
        {
            // test to show how uneven lines are split between left and right windows.
            var c = new MockConsole(width, 5);
            (var left, var right) = c.SplitLeftRight();
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


        [Test]
        public void WhenScrolling_ShouldScroll()
        {
            // dammit? this is working with them mock console but not the real console????

            var c = new MockConsole(19, 5);
            (var left, var right) = c.SplitLeftRight("left", "right");
            left.WriteLine("one");
            left.WriteLine("two");
            left.WriteLine("three");
            left.WriteLine("four");
            // used write here so that last line does not add aditional scroll
            left.Write("five");

            right.WriteLine("foo");
            right.WriteLine("cats");
            right.WriteLine("dogs");
            // last line is already scrolling ie at the bottom of the screen so this adds an additional scroll
            right.WriteLine("dots");
            Console.WriteLine(c.BufferString);
            var expectedParent = new[]
            {
                    "┌─ left ─┬─ right ┐",
                    "│three   │dogs    │",
                    "│four    │dots    │",
                    "│five    │        │",
                    "└────────┴────────┘"
            };

            c.Buffer.Should().BeEquivalentTo(expectedParent);
        }
    }
}
