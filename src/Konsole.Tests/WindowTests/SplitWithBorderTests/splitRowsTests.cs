using System;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests.SplitTests
{
    public class SplitRowsTests
    {
        [Test]
        public void TopHalf_and_BottomHalf_ShouldFillTheParentConsole()
        {
            var c = new MockConsole(10, 10);
            var rows = c.SplitRows(new Split(4, "top"),new Split("bottom"));

            var top = rows[0];
            var bottom = rows[1];

            top.WindowHeight.Should().Be(2);        // total size is 4 with a border, resulting in usable inside height of 2
            bottom.WindowHeight.Should().Be(4);     // total size is 6 with a border, resulting in usable inside height of 4

            top.WriteLine("one");
            top.WriteLine("two");
            top.WriteLine("three");
            top.Write("four");

            bottom.WriteLine("cat");
            bottom.WriteLine("in");
            bottom.WriteLine("the");
            bottom.Write("hat");

            c.Buffer.Should().BeEquivalentTo(new[] {
                "┌── top ─┐",
                "│three   │",
                "│four    │",
                "└────────┘",
                "┌ bottom ┐",
                "│cat     │",
                "│in      │",
                "│the     │",
                "│hat     │",
                "└────────┘"
            });
        }

        
        [Test]
        public void shortcut_notatation_creates_equally_sized_windows_with_border()
        {
            var console = new MockConsole(10, 9);

            //begin-snippet: splitrows

            var rows = console.SplitRows("top","middle", "bottom");
            rows[0].Write("one");
            rows[1].Write("two");
            rows[2].Write("three");
            
            console.Buffer.Should().BeEquivalentTo(new[] {
                "┌── top ─┐",
                "│one     │",
                "└────────┘",
                "┌ middle ┐",
                "│two     │",
                "└────────┘",
                "┌ bottom ┐",
                "│three   │",
                "└────────┘"
            });

            //end-snippet: splitrows
        }



        [Test]
        public void Uneven_number_of_rows_should_add_1_row_to_bottom_row()
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
        public void even_number_of_rows_without_title_ie_border_should_split_rows_evenly()
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

        [Test]
        public void even_number_of_rows_with_title_ie_border_should_split_rows_evenly()
        {
            // test to show how uneven lines are split between top and bottom windows.
            var c = new MockConsole(10, 9);
            var rows = c.SplitRows(new Split("1"), new Split("2"), new Split("3"));
            var r1 = rows[0];
            var r2 = rows[1];
            var r3 = rows[2];
            r1.WindowHeight.Should().Be(1);
            r2.WindowHeight.Should().Be(1);
            r3.WindowHeight.Should().Be(1);
        }
    }
}
