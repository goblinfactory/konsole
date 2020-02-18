using FluentAssertions;
using NUnit.Framework;
using static Konsole.Drawing.MergeOrOverlap;

namespace Konsole.Tests.Lines
{
    public class OverlappingBoxTests
    {
        [Test]
        public void overlapping_boxes_double_double()
        {
            var console = new MockConsole(12, 10);
            var line = new Draw(console, LineThickNess.Single, Merge);
            line.Box(0, 0, 8, 6, LineThickNess.Double);
            line.Box(3, 3, 11, 9, LineThickNess.Double);

            var expected = new[]
            {
               "╔═══════╗   ",
               "║       ║   ",
               "║       ║   ",
               "║  ╔════╬══╗",
               "║  ║    ║  ║",
               "║  ║    ║  ║",
               "╚══╬════╝  ║",
               "   ║       ║",
               "   ║       ║",
               "   ╚═══════╝"
            };
            console.Buffer.ShouldBe(expected);
        }

        [Test]
        public void overlapping_boxes_single_single()
        {
            var console = new MockConsole(12, 10);
            var line = new Draw(console, LineThickNess.Single, Merge);
            line.Box(0, 0, 8, 6, LineThickNess.Single);
            line.Box(3, 3, 11, 9, LineThickNess.Single);

            var expected = new[]
            {
               "┌───────┐   ",
               "│       │   ",
               "│       │   ",
               "│  ┌────┼──┐",
               "│  │    │  │",
               "│  │    │  │",
               "└──┼────┘  │",
               "   │       │",
               "   │       │",
               "   └───────┘"
            };

            console.Buffer.ShouldBe(expected);
        }

        [Test]
        public void overlapping_boxes_single_double()
        {
            var console = new MockConsole(12, 10);
            var line = new Draw(console, LineThickNess.Single, Merge);
            line.Box(0, 0, 8, 6, LineThickNess.Single);
            line.Box(3, 3, 11, 9, LineThickNess.Double);

            var expected = new[]
            {
               "┌───────┐   ",
               "│       │   ",
               "│       │   ",
               "│  ╔════╪══╗",
               "│  ║    │  ║",
               "│  ║    │  ║",
               "└──╫────┘  ║",
               "   ║       ║",
               "   ║       ║",
               "   ╚═══════╝"
            };
            console.Buffer.ShouldBe(expected);
        }

        // -------------------------------------------------------
        // I have tests for both single_double and double_single
        // while this might look redundant, when you first draw
        // a single line, you end up when merging a double left
        // aproaches a single vertical.
        // when drawing it in the reverse sequence you have
        // a single line approaching a double vertical,
        // a very different requirement.
        // -------------------------------------------------------

        [Test]
        public void overlapping_boxes_double_single()
        {
            var console = new MockConsole(12, 10);
            var line = new Draw(console, LineThickNess.Single, Merge);
            line.Box(0, 0, 8, 6, LineThickNess.Double);
            line.Box(3, 3, 11, 9, LineThickNess.Single);

            var expected = new[]
            {
               "╔═══════╗   ",
               "║       ║   ",
               "║       ║   ",
               "║  ┌────│──┐",
               "║  │    ║  │",
               "║  │    ║  │",
               "╚══╪════╝  │",
               "   │       │",
               "   │       │",
               "   └───────┘"
            };

            console.Buffer.ShouldBe(expected);
        }
    }
}
