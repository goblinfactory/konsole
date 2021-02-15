using System;
using Konsole.Forms;

namespace Konsole
{
    public class Faster
    {
        private readonly IConsole _console;
        
        private static readonly IBoxStyle _THIN_CHARS = new ThinBoxStyle();
        private static readonly IBoxStyle _THICK_CHARS = new ThickBoxStyle();
        private static readonly IBoxStyle _BLANK_CHARS = new BlankBoxStyle();
        public Faster(IConsole console)
        {
            _console = console;
        }
        public IConsole Box(int sx, int sy, int ex, int ey, string title, LineThickNess? thickness)
        {
            _console.DoCommand(_console, () =>
            {
                _Box(sx, sy, ex, ey, title, thickness ?? LineThickNess.Single);
            });

            return _console;
        }

        //TODO: need to add color (Style) to this, foreground and background.

        public IConsole Box(int sx, int sy, int ex, int ey, LineThickNess? thickness)
        {
            _console.DoCommand(_console, () =>
            {
                _Box(sx, sy, ex, ey, null, thickness ?? LineThickNess.Single);
            });

            return _console;
        }

        internal void _Box(int sx, int sy, int ex, int ey, string title, LineThickNess thickness)
        {
            Style _style = _console.Style.WithThickness(thickness);
            IBoxStyle chars = new ThinBoxStyle();
            switch (thickness)
            {
                case LineThickNess.Single:
                    chars = _THIN_CHARS;
                    break;
                case LineThickNess.Double:
                    chars = _THICK_CHARS;
                    break;
                case LineThickNess.BlankChar:
                    chars = _BLANK_CHARS;
                    break;
            }

                _console.Colors = _style.Line;
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
                DrawBoxLines(sx, sy, ex, ey, chars);
            if (string.IsNullOrWhiteSpace(title)) return;
                var titleText = $" {title} ";
                int len = titleText.Length;
                int maxLen = width - 2;
                if (len > maxLen)
                {
                    titleText = maxLen > 0 ? titleText.Substring(0, maxLen) : "";
                }
                if (!string.IsNullOrWhiteSpace(titleText))
                {
                    int titleX = sx + (width / 2) - (titleText.Length / 2);
                    _console.PrintAt(_style.Title, titleX, sy, titleText);
                }
        }

        private void DrawBoxLines(int sx, int sy, int ex, int ey, IBoxStyle chars)
        {
            DrawCorners(sx, sy, ex, ey, chars);
            DrawHorizontal(sx + 1, sy, ex - 1, chars);
            DrawVertical(sx, sy + 1, ey - 1, chars);
            DrawVertical(ex, sy + 1, ey - 1, chars);
            DrawHorizontal(sx + 1, ey, ex - 1, chars);
        }

        private enum HV {  Undefined, Horizontal, Vertical }


        private void DrawHorizontal(int sx, int sy, int ex, IBoxStyle line)
        {
            if (ex - sx < 0) return;
            if (sx > ex) throw new ArgumentOutOfRangeException("start x cannot be bigger than end x.");
            int length = ex - sx + 1;
             _console.PrintAt(sx, sy, new string(line.T, length));
        }

        private void DrawVertical(int sx, int sy, int ey, IBoxStyle line)
        {
            if (ey - sy < 0) return;
            if (sy > ey) throw new ArgumentOutOfRangeException("start y cannot be bigger than end y.");
            _console.PrintAt(sx, sy, line.L);
            for (int i = sy + 1; i < ey; i++) _console.PrintAt(sx, i, line.L);
            _console.PrintAt(sx, ey, line.L);
        }

        private void DrawCorners(int sx, int sy, int ex, int ey, IBoxStyle line)
        {
            _console.PrintAt(sx, sy, line.TL);
            _console.PrintAt(ex, sy, line.TR);
            _console.PrintAt(sx, ey, line.BL);
            _console.PrintAt(ex, ey, line.BR);
        }
    }
}
