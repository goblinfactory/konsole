using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.Lines
{
    public class AnySizeBoxTests
    {
        [Test]
        public void negative_width_box_should_not_render_anything()
        {
            var console = new MockConsole(2, 2);
            new Draw(console).Box(10, 1, 8, 2, "my test box");
            console.Buffer.ShouldBe(new[] { 
                "  ", 
                "  " });
        }

        [Test]
        public void _1_pixel_wide_should_render_a_single_character_square()
        {
            var console = new MockConsole(2, 2);
            new Draw(console)
                .Box(0, 1, 0, 1, "my test box", LineThickNess.Single);
            console.Buffer.ShouldBe(new[] { 
                "  ",
                "☐ "
            });
        }

        [Test]
        public void wide_1_line_high_box()
        {
            var console = new MockConsole(11, 2);
            new Draw(console).Box(0, 1, 9, 1, "my test box", LineThickNess.Double);
            
            console.Buffer.ShouldBe(new[] {
                "           ",
                "╚ my test╝ "
            });
        }

        [Test]
        public void three_lines_high_box_single()
        {
            var console = new MockConsole(10, 3);
            new Draw(console).Box(0, 0, 9, 2, "test", LineThickNess.Single);

            var expected = new[]
            {
                "┌─ test ─┐",
                "│        │",
                "└────────┘",
            };

            console.Buffer.ShouldBe(expected);
        }

        [Test]
        public void three_lines_high_box_double()
        {
            var console = new MockConsole(10, 3);
            new Draw(console).Box(0, 0, 9, 2, "test", LineThickNess.Double);

            var expected = new[]
            {
                "╔═ test ═╗",
                "║        ║",
                "╚════════╝",
            };

            console.Buffer.ShouldBe(expected);
        }


        [Test]
        public void should_support_drawing_any_positive_size_width_boxes()
        {
            var console = new MockConsole(50, 40);
            var line = new Draw(console);

            // range of box sizes, alternating double and single line.
            // -------------------------------------------------------
            for (int i = 0; i < 10; i++)
            {
                
                var tl = new XY(0, 4 * i);
                var br = new XY(0 + i, 4 * i + 3);
                line.Box(tl.X, tl.Y, br.X, br.Y, "my test box", i % 2 == 0 ? LineThickNess.Single : LineThickNess.Double);
            }
            var actual = console.Buffer;
            var expected = new[]
            {               
                "┐                                                 ",
                "│                                                 ",
                "│                                                 ",
                "┘                                                 ",
                "╔╗                                                ",
                "║║                                                ",
                "║║                                                ",
                "╚╝                                                ",
                "┌─┐                                               ",
                "│ │                                               ",
                "│ │                                               ",
                "└─┘                                               ",
                "╔ m╗                                              ",
                "║  ║                                              ",
                "║  ║                                              ",
                "╚══╝                                              ",
                "┌ my┐                                             ",
                "│   │                                             ",
                "│   │                                             ",
                "└───┘                                             ",
                "╔ my ╗                                            ",
                "║    ║                                            ",
                "║    ║                                            ",
                "╚════╝                                            ",
                "┌ my t┐                                           ",
                "│     │                                           ",
                "│     │                                           ",
                "└─────┘                                           ",
                "╔ my te╗                                          ",
                "║      ║                                          ",
                "║      ║                                          ",
                "╚══════╝                                          ",
                "┌ my tes┐                                         ",
                "│       │                                         ",
                "│       │                                         ",
                "└───────┘                                         ",
                "╔ my test╗                                        ",
                "║        ║                                        ",
                "║        ║                                        ",
                "╚════════╝                                        "
            };

            //
            actual.ShouldBe(expected);
        }

    }
}
