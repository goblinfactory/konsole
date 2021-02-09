using System;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests.SplitWithoutBorderTests
{
    public class SplitcolumnsNoBorderTests
    {
        [Test]
        public void even_window_size_wildcard_columns_without_border_should_split_equally()
        {
            var console = new MockConsole(30, 5);
            var cols = console.SplitColumns(new Split(), new Split(), new Split());
            var left = cols[0];
            var middle = cols[1];
            var right = cols[2];

            left.WriteLine("one");
            middle.WriteLine("two");
            right.WriteLine("three");

            console.Buffer.Should().BeEquivalentTo(new[] {
                "one       two       three     ",
                "                              ",
                "                              ",
                "                              ",
                "                              "
            });
        }
    }
}
