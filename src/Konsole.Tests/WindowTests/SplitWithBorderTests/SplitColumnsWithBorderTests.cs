using System;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests.SplitTests
{
    public class SplitColumnsWithBorderTests
    {
        [Test]
        public void even_window_size_wildcard_columns_should_split_equally()
        {
            var console = new MockConsole(30, 5);
            var cols = console.SplitColumns("left", "middle", "right");
            var left = cols[0];
            var middle = cols[1];
            var right = cols[2];

            left.WriteLine("one");
            middle.WriteLine("two");
            right.WriteLine("three");

            console.Buffer.Should().BeEquivalentTo(new[] {
                "┌─ left ─┐┌ middle ┐┌─ right ┐",
                "│one     ││two     ││three   │",
                "│        ││        ││        │",
                "│        ││        ││        │",
                "└────────┘└────────┘└────────┘"
            });
        }
    }

}
