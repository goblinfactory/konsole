using System;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests.SplitWithoutBorderTests
{
        public class SplitTop_and_SplitBottom_Tests
        {
            [Test]
            [TestCase(1, 6)]
            [TestCase(2, 7)]
            public void TopHalf_and_BottomHalf_ShouldFillTheParentConsole(int test, int height)
            {
                // test to show how uneven lines are split between top and bottom windows.
                var c = new MockConsole(10, height);
                var top = c.SplitTop();
                var bottom = c.SplitBottom();
                top.WriteLine("one");
                top.WriteLine("two");
                top.WriteLine("three");
                top.Write("four");

                bottom.WriteLine("cat");
                bottom.WriteLine("in");
                bottom.WriteLine("the");
                bottom.Write("hat");
            Console.WriteLine(c.BufferString);

                var _6Rows = new[]
                {
                "two       ",
                "three     ",
                "four      ",
                "in        ",
                "the       ",
                "hat       "
            };

                var _7Rows = new[]
                {
                "one       ",
                "two       ",
                "three     ",
                "four      ",
                "in        ",
                "the       ",
                "hat       "
            };
                var expecteds = new[]
                {
                _6Rows, _7Rows
                };
                c.Buffer.Should().BeEquivalentTo(expecteds[test - 1]);
            }
        }
}
