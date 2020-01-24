using System;
using System.Collections.Generic;
using Konsole.Drawing;
using Konsole.Forms;

namespace Konsole
{
    public class Draw
    {
        private readonly IConsole _console;
        private readonly MergeOrOverlap _mergeOrOverlap;
        private LineMerger _lineMerger;
        private Dictionary<XY, char> _printed = new Dictionary<XY, char>();

        private IBoxStyle _thick = new ThickBoxStyle();
        private IBoxStyle _thin = new ThinBoxStyle();

        public LineThickNess Thickness { get; private set; }

        public Draw(IConsole console, LineThickNess thickness = LineThickNess.Single, MergeOrOverlap mergeOrOverlap = MergeOrOverlap.Merge)
        {
            _console = console;
            _mergeOrOverlap = mergeOrOverlap;
            _lineMerger = new LineMerger();
            Thickness = thickness;
        }

        public static IBoxStyle ThickBox = new ThickBoxStyle();
        public static IBoxStyle ThinBox = new ThinBoxStyle();

        public Draw Box(int sx, int sy, int ex, int ey, LineThickNess? thickness = null)
        {
            Box(sx, sy, ex, ey, "", thickness ?? Thickness);
            return this;
        }


        public Draw Box(int sx, int sy, int ex, int ey, string title, LineThickNess? thicknessOverride = null)
        {
            // TODO : remove duplication!

            var thickness = thicknessOverride ?? Thickness;
            int width = ex - sx + 1;
            int height = ey - sy + 1;
            // if box is not visible, return.
            if (ex - sx < 0) return this;
            if (ey - sy < 0) return this;

            // if box is 1 character hight and wide render square and return
            if (height == 1 && width == 1)
            {
                _console.PrintAt(sx, sy, '☐');
                return this;
            }
            var line = thickness == LineThickNess.Single ? ThinBox : ThickBox;
            DrawCorners(sx, sy, ex, ey, line);
            // top edge
            //Line(sx + 1, sy, ex - 1, sy, thickness);
            DrawHorizontal(sx + 1, sy, ex - 1, line);

            // left edge
            //Line(sx, sy + 1, sx, ey - 1, thickness);
            DrawVertical(sx, sy + 1, ey - 1, line);

            // right edge
            //Line(ex, sy + 1, ex, ey - 1, thickness);
            DrawVertical(ex, sy + 1, ey - 1, line);

            // bottom edge
            //Line(sx + 1, ey, ex - 1, ey, thickness);
            DrawHorizontal(sx + 1, ey, ex - 1, line);

            // print centered title
            var titleText = $" {title} ";
            int len = titleText.Length;
            int maxLen = width - 2;
            if (len > maxLen)
            {
                titleText = maxLen > 0 ? titleText.Substring(0, maxLen) : "";
            }
            if (!string.IsNullOrWhiteSpace(titleText))
            {
                _console.PrintAt(sx + width / 2 - titleText.Length / 2, sy, titleText);
            }

            return this;
        }

        public Draw Box(int sx, int sy, int ex, int ey, string title, Colors lineColor, Colors titleColor, LineThickNess? thicknessOverride = null)
        {
            _console.DoCommand(_console, () =>
            {
                _console.Colors = lineColor;
                var thickness = thicknessOverride ?? Thickness;
                int width = ex - sx + 1;
                int height = ey - sy + 1;
                // if box is not visible, return.
                if (ex - sx < 0) return;
                if (ey - sy < 0) return;

                if (height == 1 && width == 1)
                {
                    _console.PrintAt(sx, sy, '☐');
                    return;
                }
                DrawBoxLines(sx, sy, ex, ey, thickness);
                var titleText = $" {title} ";
                int len = titleText.Length;
                int maxLen = width - 2;
                if (len > maxLen)
                {
                    titleText = maxLen > 0 ? titleText.Substring(0, maxLen) : "";
                }
                if (!string.IsNullOrWhiteSpace(titleText))
                {
                    _console.Colors = titleColor;
                    int titleX = sx + (width / 2) - (titleText.Length / 2);
                    _console.PrintAt(titleX, sy, titleText);
                }
            });
            return this;
        }

        private void DrawBoxLines(int sx, int sy, int ex, int ey, LineThickNess thickness)
        {
            var line = thickness == LineThickNess.Single ? ThinBox : ThickBox;
            DrawCorners(sx, sy, ex, ey, line);
            DrawHorizontal(sx + 1, sy, ex - 1, line);
            DrawVertical(sx, sy + 1, ey - 1, line);
            DrawVertical(ex, sy + 1, ey - 1, line);
            DrawHorizontal(sx + 1, ey, ex - 1, line);
        }

        /// <summary>
        /// Draw a line between two points, either horizontally or vertically. 
        /// </summary>
        /// <param name="sx">the start x position. (0 ordinal)</param>
        /// <param name="sy">the start y position. (0 ordinal)</param>
        /// <param name="ex">the end x position. (0 ordinal)</param>
        /// <param name="ey">the end y position. (0 ordinal)</param>
        /// <param name="_thicknessOverride"></param>
        /// <returns>the draw instance being used so that you can continue to chain drawing calls together. The returned instance knows what has been drawn, so that it can merge the lines drawn. Starting or using a different draw instance will stop the ability of the draw to merge lines.</returns>
        /// <remarks>Lines do not need to be connected. If the two coordinates for start and end are the same, then this will draw a horizontal line. If you need to draw a single character tall or wide line then use DrawHorizontal, or DrawVertical.</remarks>
        public Draw Line(int sx, int sy, int ex, int ey, LineThickNess? _thicknessOverride = null)
        {
            return _Line(sx, sy, ex, ey, _thicknessOverride, HV.Undefined);
        }

        private enum HV {  Undefined, Horizontal, Vertical }

        private Draw _Line(int sx, int sy, int ex, int ey, LineThickNess? _thicknessOverride, HV hvHint)
        {
            var thickness = _thicknessOverride ?? Thickness;
            IBoxStyle line = thickness == LineThickNess.Single ? _thin : _thick;

            // hv hint is only required when sx, sy and ex, ey are all the same 
            // added to try to sort out issue I have with box 3 lines high resulting
            // in the line height being only 1 high, and thus sx, sy, ex, ey all being 
            // equal, meaning the default as per code without this change results in a 
            // horzontal line.
            // I was certain I could fix this by changing the calling code to call
            // drawHorizontal, drawVertical but that didnt work? need to test again.

            if (hvHint != HV.Undefined)
            {
                if(hvHint == HV.Horizontal) return DrawVertical(sx, sy, ey, line);
                if (hvHint == HV.Vertical) return DrawVertical(sx, sy, ey, line);
            }

            // horizontal or vertical?
            if (sy == ey) return DrawHorizontal(sx, sy, ex, line);
            if (sx == ex) return DrawVertical(sx, sy, ey, line);
            // throw new ArgumentOutOfRangeException("cannot draw diagonal lines");
            return this;
        }


        private Draw DrawHorizontal(int sx, int sy, int ex, IBoxStyle line)
        {
            if (ex - sx < 0) return this;
            if (sx > ex) throw new ArgumentOutOfRangeException("start x cannot be bigger than end x.");
            int length = ex - sx + 1;
            PrintAtAndMerge(sx, sy, line.T, LineMerger.Position.First);
            for (int i = sx + 1; i < sx + length - 1; i++)
            {
                PrintAtAndMerge(i, sy, line.T, LineMerger.Position.Middle);
            }
            PrintAtAndMerge(sx + length - 1, sy, line.T, LineMerger.Position.Last);
            return this;
        }

        private Draw FastHorizontal(int sx, int sy, int ex, IBoxStyle line)
        {
            if (ex - sx < 0) return this;
            if (sx > ex) throw new ArgumentOutOfRangeException("start x cannot be bigger than end x.");
            int length = ex - sx + 1;
            PrintAtAndMerge(sx, sy, line.T, LineMerger.Position.First);
            for (int i = sx + 1; i < sx + length - 1; i++)
            {
                PrintAtAndMerge(i, sy, line.T, LineMerger.Position.Middle);
            }
            PrintAtAndMerge(sx + length - 1, sy, line.T, LineMerger.Position.Last);
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
            // already printed
            char printedChar = _printed[key];
            var newChar = _lineMerger.Merge(printedChar, position, c);
            _printed[key] = newChar;
            _console.PrintAt(x, y, newChar);
        }

        public Draw DrawVertical(int sx, int sy, int ey, IBoxStyle line)
        {
            if (ey - sy < 0) return this;
            if (sy > ey) throw new ArgumentOutOfRangeException("start y cannot be bigger than end y.");
            PrintAtAndMerge(sx, sy, line.L, LineMerger.Position.First);
            for (int i = sy + 1; i < ey; i++) PrintAtAndMerge(sx, i, line.L, LineMerger.Position.Middle);
            PrintAtAndMerge(sx, ey, line.L, LineMerger.Position.Last);
            return this;
        }



        private void DrawCorners(int sx, int sy, int ex, int ey, IBoxStyle line)
        {
            printAt(sx, sy, line.TL);
            printAt(ex, sy, line.TR);
            printAt(sx, ey, line.BL);
            printAt(ex, ey, line.BR);
        }
    }
}
