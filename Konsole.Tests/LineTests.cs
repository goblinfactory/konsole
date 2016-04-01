using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Konsole.Drawing;
using Konsole.Testing;
using NUnit.Framework;

namespace Konsole.Tests
{
    [UseReporter(typeof(DiffReporter))]
    public class Drawing_LineTests
    {
        [Test]
        public void draw_box_should_draw_box()
        {
            var console = new TestConsole(200, 20);
            // draw box 40 wide, and 6 high
            new Line(console).Box(2, 2,42, 8, "my test box", ThickNess.Single);
            Approvals.Verify(console.Buffer);
        }

        [Test]
        public void should_support_drawing_any_positive_size_boxes()
        {
            var console = new TestConsole(200, 50);
            var line = new Line(console);
            console.PrintAt(0, 0, "10,1-8,2 height:2 width:-2");
            line.Box(10, 1, 8, 2, "my test box", ThickNess.Single);
            console.PrintAt(0, 2, "0,1-0,1 height:1 width:1");
            line.Box(0, 3, 0, 3, "my test box", ThickNess.Single);
            for (int i = 0; i < 10; i++)
            {
                var tl = new XY(0, 4*i+5);
                var br = new XY(0 + i, 4*i + 7);
                var desc = string.Format("({0} - {1}) height:{2} width:{3}", tl, br, (br.Y - tl.Y) + 1,(br.X - tl.X) + 1);
                console.PrintAt(tl.X,tl.Y-1, desc);
                line.Box(tl.X, tl.Y, br.X, br.Y, "my test box", i % 2 == 0 ? ThickNess.Single : ThickNess.Double);                
            }

            console.PrintAt(0, 44, "(0,45 - 9,45) height:1 width:10");
            line.Box(0,45,9,45, "my test box", ThickNess.Double);                

            Approvals.Verify(console.Buffer);            
        }


        //[Test]
        //public void overlapping_boxes_when_set_to_merge_should_merge_lines()
        //{
        //    var console = new TestConsole(200, 20);
        //    // draw box 40 wide, and 6 high
        //    new Line(console).Box(2, 2, 42, 8, "my test box", ThickNess.Single);
        //    // daw an overlapping box and merge where they touch
        //    new Line(console).Box(5, 5, 15, 15, "overlap", ThickNess.Double, Join.Merge);
        //    Approvals.Verify(console.Buffer);
        //}
    }
}
