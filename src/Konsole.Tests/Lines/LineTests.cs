using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.Lines
{
    [UseReporter(typeof(DiffReporter))]
    public class Drawing_LineTests
    {
        [Test]
        public void draw_box_should_draw_box()
        {
            var console = new MockConsole(45, 10);
            // draw box 40 wide, and 6 high
            new Draw(console).Box(2, 2, 42, 8, "my test box", LineThickNess.Single);

            var expected = new[]
            {
                "                                             ",
                "                                             ",
                "  ┌───────────── my test box ─────────────┐  ",
                "  │                                       │  ",
                "  │                                       │  ",
                "  │                                       │  ",
                "  │                                       │  ",
                "  │                                       │  ",
                "  └───────────────────────────────────────┘  ",
                "                                             "
            };
            console.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void should_be_able_to_draw_complex_forms_with_mixed_lines()
        {
            var console = new MockConsole(63, 21);
            int height = 18;
            int sy = 2;
            int sx = 2;
            int width = 60;
            int ex = sx + width;
            int ey = sy + height;
            int col1 = 20;

            var draw = new Draw(console, LineThickNess.Double);
            draw
                .Box(sx, sy, ex, ey, "my test box")
                .Line(sx, sy + 2, ex, sy + 2)
                .Line(sx + col1, sy, sx + col1, sy + 2, LineThickNess.Single)
                .Line(sx + 35, ey - 4, ex - 5, ey - 4, LineThickNess.Double)
                .Line(sx + 35, ey - 2, ex - 5, ey - 2, LineThickNess.Double)
                .Line(sx + 35, ey - 4, sx + 35, ey - 2, LineThickNess.Single) // faulty! need to fix
                .Line(ex - 5, ey - 4, ex - 5, ey - 2, LineThickNess.Single);  // faulty! need to fix

            console.PrintAt(sx + 2, sy + 1, "DEMO INVOICE");

            var expected = new[] {
                "                                                               ",
                "                                                               ",
                "  ╔═══════════════════╤═══ my test box ═══════════════════════╗",
                "  ║ DEMO INVOICE      │                                       ║",
                "  ╠═══════════════════╧═══════════════════════════════════════╣",
                "  ║                                                           ║",
                "  ║                                                           ║",
                "  ║                                                           ║",
                "  ║                                                           ║",
                "  ║                                                           ║",
                "  ║                                                           ║",
                "  ║                                                           ║",
                "  ║                                                           ║",
                "  ║                                                           ║",
                "  ║                                                           ║",
                "  ║                                                           ║",
                "  ║                                  ╤═══════════════════╤    ║",
                "  ║                                  │                   │    ║",
                "  ║                                  ╧═══════════════════╧    ║",
                "  ║                                                           ║",
                "  ╚═══════════════════════════════════════════════════════════╝",
            };

            console.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void should_render_title()
        {
            // test different sizes of title from 0 through to overflow size.
            // test different size boxes, from 0 through to large.
            Assert.Inconclusive("new requirement");
        }
    }
}
