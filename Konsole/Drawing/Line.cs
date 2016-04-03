using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole.Forms;
using Konsole.Testing;

namespace Konsole.Drawing
{
    public class Line
    {
        private readonly IConsole _console;
        private readonly MergeOrOverlap _mergeOrOverlap;

        public Line(IConsole console, MergeOrOverlap mergeOrOverlap = MergeOrOverlap.Merge)
        {
            _console = console;
            _mergeOrOverlap = mergeOrOverlap;
        }

        public static IBoxStyle ThickBox = new ThickBoxStyle();
        public static IBoxStyle ThinBox = new ThinBoxStyle();

        public void Box(int sx, int sy, int ex, int ey, LineThickNess thickness = LineThickNess.Single)
        {
            Box(sx, sy, ex, ey, "", thickness);
        }

        public void Box(int sx, int sy, int ex, int ey, string title, LineThickNess thickness)
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

            var line = (thickness == LineThickNess.Single) ? ThinBox: ThickBox;
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
            int length = (ex - sx) + 1;
            
            for (int i = sx; i < sx + length; i++)
            {
                printAtAndMerge(i, sy, line.T);
            }
        }

        private Dictionary<XY, Cell> _printed = new Dictionary<XY, Cell>();  
        private void printAtAndMerge(int x, int y, char c)
        {
            _console.PrintAt(x, y, c);
        }

        private char Merge(char? old, char @new)
        {
            if (!old.HasValue) return @new;

        }



        public void DrawVertical(int sx, int sy, int ey, IBoxStyle line)
        {
            if (ey-sy<0) return;
            if (sy > ey) throw new ArgumentOutOfRangeException("start y cannot be bigger than end y.");
            for (int i = sy; i < (ey+1); i++) _console.PrintAt(sx, i, line.L);
        }

        public void Merge(int x, int y, char newchar)
        {
            
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
}
