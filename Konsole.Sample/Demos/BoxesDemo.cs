using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole.Drawing;

namespace Konsole.Sample.Demos
{
    public class BoxesDemo
    {
        public static void Run()
        {
            var console = new Writer();

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
                //.Box(sx + 35, ey - 4, ex - 5, ey - 2); faulty! need to fix
                .Line(sx + 35, ey - 4, ex - 5, ey - 4, LineThickNess.Double)
                .Line(sx + 35, ey - 2, ex - 5, ey - 2, LineThickNess.Double)
                .Line(sx + 35, ey - 4, sx + 35, ey - 2, LineThickNess.Single) // faulty! need to fix
                .Line(ex - 5, ey - 4, ex - 5, ey - 2, LineThickNess.Single);  // faulty! need to fix

            console.PrintAt(sx + 2, sy + 1, "DEMO INVOICE");
        }

    }
}
