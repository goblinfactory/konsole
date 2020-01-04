using System;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class SplitColumnsShould
    {
        [Test]
        public void WhenSplitIsZero_should_use_balance_of_width_less_2_for_border()
        {
            var con = new MockConsole(25, 5);
            var cols = con.SplitColumns(
                new Split(10, "left"),
                new Split(0, "mid"),
                new Split(5, "right")
                );
            cols[1].WindowWidth.Should().Be(8);
        }

        [Test]
        public void WhenNoWildCardSplit_splits_must_match_total_width()
        {
            var con = new MockConsole(30, 5);
            var action = new Action(()=> con.SplitColumns(
                new Split(10, "left"),
                new Split(10, "mid"),
                new Split(11, "right")
            ));

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void when_wildcard_is_used_balance_must_still_be_big_enough()
        {
            var con = new MockConsole(20, 5);
            var action = new Action(() => con.SplitColumns(
                new Split(20, "left"),
                new Split(0, "mid"),
                new Split(20, "right")
            ));
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void split_the_window_into_windows_using_provided_splits()
        {
            var con = new MockConsole(19, 5);
            var cols = con.SplitColumns(
                new Split(9, "left"),
                new Split(0, "right")
                );
            var left = cols[0];
            var right = cols[1];

            left.WriteLine("one");
            left.WriteLine("two");
            left.Write("three");

            right.WriteLine("four");
            right.WriteLine("five");
            right.Write("six");

            var expected = new[]
            {    
                "┌ left ─┐┌─ right ┐",
                "│one    ││four    │",
                "│two    ││five    │",
                "│three  ││six     │",
                "└───────┘└────────┘"
            };
            con.Buffer.Should().BeEquivalentTo(expected);
        }
    }
}
