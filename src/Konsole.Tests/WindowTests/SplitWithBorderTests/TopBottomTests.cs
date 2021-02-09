using System;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests.SplitTests
{
        public class TopBottomTests
        {
            [Test]
            [TestCase(1, 10)]
            [TestCase(2, 11)]
            [TestCase(3, 12)]
            public void TopHalf_and_BottomHalf_ShouldFillTheParentConsole(int test, int height)
            {
                // test to show how uneven lines are split between top and bottom windows.
                var c = new MockConsole(10, height);
                var top = c.SplitTop("top");
                var bottom = c.SplitBottom("bot");
                top.WriteLine("one");
                top.WriteLine("two");
                top.Write("three");


                bottom.WriteLine("four");
                bottom.WriteLine("five");
                bottom.Write("six");
                Console.WriteLine(c.BufferString);

                var _10Rows = new[]
                {
                "┌── top ─┐",
                "│one     │",
                "│two     │",
                "│three   │",
                "└────────┘",
                "┌── bot ─┐",
                "│four    │",
                "│five    │",
                "│six     │",
                "└────────┘"
            };

                var _11Rows = new[]
                {
                "┌── top ─┐",
                "│one     │",
                "│two     │",
                "│three   │",
                "│        │",
                "└────────┘",
                "┌── bot ─┐",
                "│four    │",
                "│five    │",
                "│six     │",
                "└────────┘",
            };


                var _12Rows = new[]
    {
                "┌── top ─┐",
                "│one     │",
                "│two     │",
                "│three   │",
                "│        │",
                "└────────┘",
                "┌── bot ─┐",
                "│four    │",
                "│five    │",
                "│six     │",
                "│        │",
                "└────────┘"
            };

                var expecteds = new[]
                {
                _10Rows, _11Rows, _12Rows
            };
                c.Buffer.ShouldBe(expecteds[test - 1]);
            }

            [Test]
            public void WhenScrolling_ShouldScroll()
            {
                var c = new MockConsole(10, 10);
                var top = c.SplitTop("top");
                var bottom = c.SplitBottom("bot");
                top.WriteLine("one");
                top.WriteLine("two");
                top.WriteLine("three");
                top.WriteLine("four");
                top.Write("five");

                bottom.WriteLine("cats");
                bottom.WriteLine("dogs");
                bottom.Write("dots");
                Console.WriteLine(c.BufferString);
                var expectedParent = new[]
                {
                    "┌── top ─┐",
                    "│three   │",
                    "│four    │",
                    "│five    │",
                    "└────────┘",
                    "┌── bot ─┐",
                    "│cats    │",
                    "│dogs    │",
                    "│dots    │",
                    "└────────┘",
                };
                c.Buffer.ShouldBe(expectedParent);
            }
        }
}
