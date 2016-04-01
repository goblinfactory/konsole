using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole.Forms;

namespace Konsole.Drawing
{
    public class Line
    {
        private readonly IConsole _console;

        public Line(IConsole console)
        {
            _console = console;
        }

        public static IBoxStyle ThickBox = new ThickBoxStyle();
        public static IBoxStyle ThinBox = new ThinBoxStyle();

        public void Box(int sx, int sy, int ex, int ey, string title, ThickNess thickness)
        {
            int width = (ex - sx) + 1;
            int height = (ey - sy) + 1;
            // if box is not visible, return.
            if (ex - sx < 0) return;
            if (ey - sy < 0) return;

            // if box is 1 character hight and wide render square and return
            if (height == 1 && width == 1)
            {
                _console.PrintAt(sx, sy, '☐');
                return;
            }

            var line = (thickness == ThickNess.Single) ? ThinBox: ThickBox;
            DrawCorners(sx,sy,ex,ey, line);
             // top edge
            DrawHorizontal(sx + 1, sy, ex - 1, line);
            // left edge
            DrawVertical(sx, sy+1, ey-1, line);
            // right edge
            DrawVertical(ex, sy+1, ey-1, line);
            // bottom edge
            DrawHorizontal(sx+1, ey, ex-1, line);
        }

        public void DrawHorizontal(int sx, int sy, int ex, IBoxStyle line)
        {
            if (ex - sx < 0) return;
            if (sx > ex) throw new ArgumentOutOfRangeException("start x cannot be bigger than end x.");
            var linechars = new string(line.T, (ex - sx) + 1);
            _console.PrintAt(sx, sy, linechars);
        }

        public void DrawVertical(int sx, int sy, int ey, IBoxStyle line)
        {
            if (ey-sy<0) return;
            if (sy > ey) throw new ArgumentOutOfRangeException("start y cannot be bigger than end y.");
            for (int i = sy; i < (ey+1); i++) _console.PrintAt(sx, i, line.L);
        }



        private bool Horizontal(int sy, int ey)
        {
            return (sy== ey);
        }

        private void DrawCorners(int sx, int sy, int ex, int ey, IBoxStyle line)
        {
            // for now, ignore overlaps from corners
            _console.PrintAt(sx,sy, line.TL);
            _console.PrintAt(ex,sy, line.TR);
            _console.PrintAt(sx,ey, line.BL);
            _console.PrintAt(ex,ey, line.BR);
        }
    }

    public enum Join
    {
        Merge, 
        Overlap
    }

    public enum ThickNess
    {
        Single,
        Double
    }
}
