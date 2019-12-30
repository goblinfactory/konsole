using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace Konsole.Tests.WindowTests
{
    public class SplitRowsShould
    {
        [Test]
        public void return_one_console_per_numbered_input_and_one_console_for_the_zero_containing_the_balance()
        {
            var con = new MockConsole(20, 11);
            var consoles = con.SplitRows(
                    new Split(3, "headline", LineThickNess.Single, ConsoleColor.Yellow),
                    new Split(0, "content", LineThickNess.Single),
                    new Split(3, "status", LineThickNess.Single, ConsoleColor.Yellow)
            );

            var headline = consoles[0];
            var content = consoles[1];
            var status = consoles[2];

            headline.Write("my headline that scrolls because of wrapping");
            content.Write("content goes here, and this content get's wrapped, and if long enough will cause a bit of scrolling.");
            status.Write("I get clipped & scroll off.");

            var expected = new[]
            {
                    "┌──── headline ────┐",
                    "─wrapping          ─",
                    "└──────────────────┘",
                    "┌───── content ────┐",
                    "│ if long enough wi│",
                    "│ll cause a bit of │",
                    "│scrolling.        │",
                    "└──────────────────┘",
                    "┌───── status ─────┐",
                    "─roll off.         ─",
                    "└──────────────────┘"
            };

            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WhenNestedInRows_return_one_console_per_numbered_input_and_one_console_for_the_zero_containing_the_balance()
        {
            var con = new MockConsole(20, 22);
            var top = con.SplitTop();
            var bottom = con.SplitBottom();
            var consoles = top.SplitRows(
                    new Split(3, "headline", LineThickNess.Single, ConsoleColor.Yellow),
                    new Split(0, "content", LineThickNess.Single),
                    new Split(3, "status", LineThickNess.Single, ConsoleColor.Yellow)
            );

            var headline = consoles[0];
            var content = consoles[1];
            var status = consoles[2];

            headline.Write("my headline that scrolls because of wrapping");
            content.Write("content goes here, and this content get's wrapped, and if long enough will cause a bit of scrolling.");
            status.Write("I get clipped & scroll off.");

            var expected = new[]
            {
                    "┌──── headline ────┐",
                    "─wrapping          ─",
                    "└──────────────────┘",
                    "┌───── content ────┐",
                    "│ if long enough wi│",
                    "│ll cause a bit of │",
                    "│scrolling.        │",
                    "└──────────────────┘",
                    "┌───── status ─────┐",
                    "─roll off.         ─",
                    "└──────────────────┘"
            };

            con.Buffer.Take(11).Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WhenNestedInColumn_return_one_console_per_numbered_input_and_one_console_for_the_zero_containing_the_balance()
        {
            var con = new MockConsole(40, 11);
            var left = con.SplitLeft();
            var consoles = left.SplitRows(
                    new Split(3, "headline", LineThickNess.Single, ConsoleColor.Yellow),
                    new Split(0, "content", LineThickNess.Single),
                    new Split(3, "status", LineThickNess.Single, ConsoleColor.Yellow)
            );

            var headline = consoles[0];
            var content = consoles[1];
            var status = consoles[2];

            headline.Write("my headline that scrolls because of wrapping");
            content.Write("content goes here, and this content get's wrapped, and if long enough will cause a bit of scrolling.");
            status.Write("I get clipped & scroll off.");

            var expected = new[]
            {
                    "┌──── headline ────┐",
                    "─wrapping          ─",
                    "└──────────────────┘",
                    "┌───── content ────┐",
                    "│ if long enough wi│",
                    "│ll cause a bit of │",
                    "│scrolling.        │",
                    "└──────────────────┘",
                    "┌───── status ─────┐",
                    "─roll off.         ─",
                    "└──────────────────┘"
            };

            _ = con.Buffer.Select(row => row.Substring(0, 20)).ToArray().Should().BeEquivalentTo(expected);
        }


    }
}
