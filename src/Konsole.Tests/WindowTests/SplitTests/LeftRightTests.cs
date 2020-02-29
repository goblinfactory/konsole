using System;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests.SplitTests
{
        public class LeftRightTests
        {
            [Test]
            [TestCase(1, 19)]
            [TestCase(2, 20)]
            [TestCase(3, 21)]
            public void LeftHalf_and_RightHalf_ShouldFillTheParentConsole(int test, int width)
            {
                // test to show how uneven lines are split between left and right windows.
                var c = new MockConsole(width, 5);
                var left = c.SplitLeft("left");
                var right = c.SplitRight("right");
                left.WriteLine("one");
                left.WriteLine("two");
                left.Write("three");

                right.WriteLine("four");
                right.WriteLine("five");
                right.Write("six");
                Console.WriteLine(c.BufferString);

                var _19Cols = new[]
                {
                    "┌ left ─┐┌─ right ┐",
                    "│one    ││four    │",
                    "│two    ││five    │",
                    "│three  ││six     │",
                    "└───────┘└────────┘"
            };

                var _20Cols = new[]
                {
                    "┌─ left ─┐┌─ right ┐",
                    "│one     ││four    │",
                    "│two     ││five    │",
                    "│three   ││six     │",
                    "└────────┘└────────┘"
            };

                var _21Cols = new[]
                {
                    "┌─ left ─┐┌─ right ─┐",
                    "│one     ││four     │",
                    "│two     ││five     │",
                    "│three   ││six      │",
                    "└────────┘└─────────┘"

            };

                var expecteds = new[]
                {
                _19Cols, _20Cols, _21Cols
            };
                c.Buffer.ShouldBe(expecteds[test - 1]);
            }


            [Test]
            [TestCase(1, 19)]
            [TestCase(2, 20)]
            [TestCase(3, 21)]
            public void LeftHalf_and_RightHalf_WithoutBorder_ShouldFillTheParentConsole(int test, int width)
            {
                // test to show how uneven lines are split between left and right windows.
                var c = new MockConsole(width, 5);
                var left = c.SplitLeft();
                var right = c.SplitRight();
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
                c.Buffer.ShouldBe(expecteds[test - 1]);
            }


            [Test]
            public void WhenScrolling_ShouldScroll()
            {
                // dammit? this is working with them mock console but not the real console????

                var c = new MockConsole(20, 5);
                var left = c.SplitLeft("left");
                var right = c.SplitRight("right");
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
                    "┌─ left ─┐┌─ right ┐",
                    "│three   ││dogs    │",
                    "│four    ││dots    │",
                    "│five    ││        │",
                    "└────────┘└────────┘"
            };

                c.Buffer.ShouldBe(expectedParent);
            }



        }
}
