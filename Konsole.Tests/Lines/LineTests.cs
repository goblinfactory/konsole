using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using Konsole.Drawing;
using Konsole.Internal;
using NUnit.Framework;

namespace Konsole.Tests
{
    [UseReporter(typeof(DiffReporter))]
    public class Drawing_LineTests
    {
        [Test]
        public void draw_box_should_draw_box()
        {
            var console = new BufferedWriter(200, 20);
            // draw box 40 wide, and 6 high
            new Draw(console).Box(2, 2, 42, 8, "my test box", LineThickNess.Single);
            Approvals.Verify(console.Buffer);
        }

        [Test]
        public void should_be_able_to_draw_complex_forms_with_mixed_lines()
        {
            var console = new BufferedWriter(200, 50);
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
            Approvals.Verify(console.Buffer);            
        }

        [Test]
        public void should_support_drawing_any_positive_size_boxes()
        {
            var console = new BufferedWriter(200, 100);
            var line = new Draw(console);

            // negative width box should not render anything
            // ---------------------------------------------
            console.PrintAt(0, 0, "10,1-8,2 height:2 width:-2");
            line.Box(10, 1, 8, 2, "my test box", LineThickNess.Single);

            // 1 pixel wide should render a single character square
            // ---------------------------------------------------
            console.PrintAt(0, 2, "0,1-0,1 height:1 width:1");
            line.Box(0, 3, 0, 3, "my test box", LineThickNess.Single);

            // range of box sizes, alternating double and single line.
            // -------------------------------------------------------
            for (int i = 0; i < 10; i++)
            {
                var tl = new XY(0, 5 * i + 7);
                var br = new XY(0 + i, 5 * i + 10);
                var desc = string.Format("({0} - {1}) height:{2} width:{3}", tl, br, (br.Y - tl.Y) + 1, (br.X - tl.X) + 1);
                console.PrintAt(tl.X, tl.Y - 1, desc);
                line.Box(tl.X, tl.Y, br.X, br.Y, "my test box", i % 2 == 0 ? LineThickNess.Single : LineThickNess.Double);
            }

            // wide 1 line high box
            // -------------------
            console.PrintAt(0, 44, "(0,45 - 9,45) height:1 width:10");
            line.Box(0, 45, 9, 45, "my test box", LineThickNess.Double);

            Approvals.Verify(console.Buffer);
        }


        [Test]
        [TestCase(LineThickNess.Single, LineThickNess.Single, MergeOrOverlap.Merge)] 
        [TestCase(LineThickNess.Single, LineThickNess.Double, MergeOrOverlap.Merge)] 
        [TestCase(LineThickNess.Double, LineThickNess.Double, MergeOrOverlap.Merge)] 
        [TestCase(LineThickNess.Single, LineThickNess.Single, MergeOrOverlap.Overlap)] 
        [TestCase(LineThickNess.Single, LineThickNess.Double, MergeOrOverlap.Overlap)] 
        [TestCase(LineThickNess.Double, LineThickNess.Double, MergeOrOverlap.Overlap)]
        public void overlapping_boxes_and_merge_tests(LineThickNess firstThickness, LineThickNess secondThickness, MergeOrOverlap merge)
        {
            using (ApprovalResults.ForScenario(firstThickness, secondThickness, merge))
            {
                var console = new BufferedWriter(80, 35);
                console.WriteLine("box1 :{0}, box2:{1}, MergeOrOverlap:{2}", firstThickness, secondThickness, merge);
                var line = new Draw(console, firstThickness, merge);

                // draw two overlapping boxes
                line.Box(10, 10, 20, 20, firstThickness);
                line.Box(15, 15, 25, 25, secondThickness);

                Approvals.Verify(console.Buffer);

            }
        }

    }

    /// <summary>
    /// allow us to put the expected console result in the test itself
    /// removes the first line, and trims the rest.
    /// </summary>
    public static class TestStringExtensions
    {
        public static string t(this string src)
        {
            var lines = src.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(l => l.TrimEnd())
                .ToArray();
            return string.Join("\n", lines);
        }
    }
}
