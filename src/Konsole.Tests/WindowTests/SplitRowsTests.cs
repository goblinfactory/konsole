using FluentAssertions;
using Konsole.Drawing;
using NUnit.Framework;
using System;

namespace Konsole.Tests.WindowTests
{
    public class SplitRowsTests
    {
        [Test]
        public void Split_Should_return_one_console_per_numbered_input_and_one_console_for_the_zero_containing_the_balance()
        {
            var c = new MockConsole(20, 11);
            var consoles = c.SplitRows(
                    new Split(3, "headline", LineThickNess.Single, ConsoleColor.Yellow),
                    new Split(0, "content", LineThickNess.Single),
                    new Split(3, "status", LineThickNess.Single, ConsoleColor.Yellow)
            );

            var headline = consoles[0];
            var content = consoles[1];
            var status = consoles[2];

            headline.Write("my headline");
            content.WriteLine("content goes here");
            status.Write("I get clipped & scroll off.");

            var expected = new[]
            {
                    "┌──── headline ────┐",
                    "─my headline       ─",
                    "└──────────────────┘",
                    "┌───── content ────┐",
                    "│content goes here │",
                    "│                  │",
                    "│                  │",
                    "└──────────────────┘",
                    "┌───── status ─────┐",
                    "─roll off.         ─",
                    "└──────────────────┘"
            };

            c.Buffer.Should().BeEquivalentTo(expected);
        }
    }
}
