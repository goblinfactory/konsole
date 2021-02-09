using System;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests.SplitWithoutBorderTests
{
    public class SplitRowsTests
    {
        [Test]
        public void TopHalf_and_BottomHalf_ShouldFillTheParentConsole()
        {
            var c = new MockConsole(10, 6);
            var rows = c.SplitRows(new Split(2),new Split());
            var top = rows[0];
            var bottom = rows[1];

            bottom.WindowHeight.Should().Be(4);

            top.WriteLine("one");
            top.WriteLine("two");
            top.WriteLine("three");
            top.Write("four");

            bottom.WriteLine("cat");
            bottom.WriteLine("in");
            bottom.WriteLine("the");
            bottom.Write("hat");

            c.Buffer.Should().BeEquivalentTo(new[] {
                "three     ",
                "four      ",
                "cat       ",
                "in        ",
                "the       ",
                "hat       "
            });
        }

        [Test]
        public void Uneven_number_of_rows_with_wildcards_should_add_1_row_to_bottom_row()
        {
            // test to show how uneven lines are split between top and bottom windows.
            var c = new MockConsole(10, 7);
            var rows = c.SplitRows(new Split(), new Split());
            var top = rows[0];
            var bottom = rows[1];
            top.WindowHeight.Should().Be(3);
            bottom.WindowHeight.Should().Be(4);
        }

        [Test]
        public void equal_wildcard_splits_should_split_evenly()
        {
            // test to show how uneven lines are split between top and bottom windows.
            var c = new MockConsole(10, 9);
            var rows = c.SplitRows(new Split(), new Split(), new Split());
            var r1 = rows[0];
            var r2 = rows[1];
            var r3 = rows[2];
            r1.WindowHeight.Should().Be(3);
            r2.WindowHeight.Should().Be(3);
            r3.WindowHeight.Should().Be(3);
        }
    }
}
