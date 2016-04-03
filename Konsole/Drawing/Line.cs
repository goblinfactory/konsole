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
        private LineMerger _lineMerger;
        private Dictionary<XY, char> _printed = new Dictionary<XY, char>();
        
        private IBoxStyle _thick = new ThickBoxStyle();
        private IBoxStyle _thin = new ThinBoxStyle();

        public LineThickNess Thickness { get; private set; }

        public Line(IConsole console,  LineThickNess thickness = LineThickNess.Single,  MergeOrOverlap mergeOrOverlap = MergeOrOverlap.Merge)
        {
            _console = console;
            _mergeOrOverlap = mergeOrOverlap;
            _lineMerger = new LineMerger();
            Thickness = thickness;
        }

        public static IBoxStyle ThickBox = new ThickBoxStyle();
        public static IBoxStyle ThinBox = new ThinBoxStyle();

        public void Box(int sx, int sy, int ex, int ey, LineThickNess thickness = LineThickNess.Single)
        {
            Box(sx, sy, ex, ey, "", thickness);
        }


        public Line Box(int sx, int sy, int ex, int ey, string title, LineThickNess? thicknessOverride = null)
        {
            var thickness = thicknessOverride ?? Thickness; 
            int width = (ex - sx) + 1;
            int height = (ey - sy) + 1;
            // if box is not visible, return.
            if (ex - sx < 0) return this;
            if (ey - sy < 0) return this;

            // if box is 1 character hight and wide render square and return
            if (height == 1 && width == 1)
            {
                _console.PrintAt(sx, sy, '☐');
                return this;
            }

            var line = (thickness == LineThickNess.Single) ? ThinBox : ThickBox;
            DrawCorners(sx,sy,ex,ey, line);
             // top edge
            DrawHorizontal(sx + 1, sy, ex - 1, thickness);
            // left edge
            DrawVertical(sx, sy+1, ey-1, line);
            // right edge
            DrawVertical(ex, sy+1, ey-1, line);
            // bottom edge
            DrawHorizontal(sx + 1, ey, ex - 1, thickness);
            return this;
        }


        public Line DrawHorizontal(int sx, int sy, int ex, LineThickNess? _thicknessOverride = null)
        {
            var thickness = _thicknessOverride ?? Thickness; 
            IBoxStyle line = thickness == LineThickNess.Single ? _thin : _thick;
            if (ex - sx < 0) return this;
            if (sx > ex) throw new ArgumentOutOfRangeException("start x cannot be bigger than end x.");
            int length = (ex - sx) + 1;
            PrintAtAndMerge(sx, sy, line.T, LineMerger.Position.First);
            for (int i = sx+1; i < (sx + length)-1; i++)
            {
                PrintAtAndMerge(i, sy, line.T,LineMerger.Position.Middle);
            }
            PrintAtAndMerge(sx+length-1, sy, line.T,LineMerger.Position.Last);
            return this;
        }

        private void printAt(int x, int y, char c)
        {
            var key = new XY(x, y);
            _printed[key] = c;
            _console.PrintAt(x, y, c);
        }

        private void PrintAtAndMerge(int x, int y, char c, LineMerger.Position position)
        {
            // if already printed then merge, otherwise just print and update printed.
            var key = new XY(x, y);
            bool printed = _printed.ContainsKey(key);
            if (!printed)
            {
                _console.PrintAt(x, y, c);
                _printed[key] = c;
                return;
            }
            char printedChar = _printed[key];
            var newChar = _lineMerger.Merge(printedChar, position, c);
            _printed[key] = newChar;
            _console.PrintAt(x, y, newChar);
        }

        public void DrawVertical(int sx, int sy, int ey, IBoxStyle line)
        {
            if (ey-sy<0) return;
            if (sy > ey) throw new ArgumentOutOfRangeException("start y cannot be bigger than end y.");
            PrintAtAndMerge(sx, sy, line.L, LineMerger.Position.Middle);
            for (int i = sy+1; i < (ey); i++) PrintAtAndMerge(sx, i, line.L, LineMerger.Position.Middle);
            PrintAtAndMerge(sx, ey, line.L, LineMerger.Position.Last);
        }

        private void DrawCorners(int sx, int sy, int ex, int ey, IBoxStyle line)
        {
            printAt(sx,sy, line.TL);
            printAt(ex,sy, line.TR);
            printAt(sx,ey, line.BL);
            printAt(ex,ey, line.BR);
        }
    }
}
