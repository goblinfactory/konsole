using System;
using static Konsole.BorderCollapse;

namespace Konsole
{
    public static class SplitTopBottomExtensions
    {
        public static(IConsole top, IConsole bottom) SplitTopBottom(this Window c, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, null, null, LineThickNess.Single, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static(IConsole top, IConsole bottom) SplitTopBottom(this Window c, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, null, null, LineThickNess.Single, border, foreground, background);
        }

        public static(IConsole top, IConsole bottom) SplitTopBottom(this Window c, string topTitle, string bottomTitle, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, topTitle, bottomTitle, LineThickNess.Single, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static(IConsole top, IConsole bottom) SplitTopBottom(this Window c, string topTitle, string bottomTitle, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, topTitle, bottomTitle, LineThickNess.Single, border, foreground, background);
        }

        public static(IConsole top, IConsole bottom) SplitTopBottom(this Window c, string topTitle, string bottomTitle, LineThickNess thickness, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, topTitle, bottomTitle, thickness, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static(IConsole top, IConsole bottom) SplitTopBottom(this Window c, string topTitle, string bottomTitle, LineThickNess thickness, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, topTitle, bottomTitle, thickness, border, foreground, background);
        }

        internal static (IConsole top, IConsole bottom) _SplitTopBottom(IConsole c, string topTitle, string bottomTitle, LineThickNess thickness, BorderCollapse border, ConsoleColor foreground, ConsoleColor background)
        {
            if (border == None)
            {
                var top = LayoutExtensions._TopBot(c, topTitle, false, false, thickness, foreground);
                var bottom = LayoutExtensions._TopBot(c, bottomTitle, true, false, thickness, foreground);
                return (top, bottom);
            }
            if (border == Separate)
            {
                var top = LayoutExtensions._TopBot(c, topTitle, false, true, thickness, foreground);
                var bottom = LayoutExtensions._TopBot(c, bottomTitle, true, true, thickness, foreground);
                return (top, bottom);
            }

            lock (Window._staticLocker)
            {
                int h = c.WindowHeight;
                int width = c.WindowWidth;
                int topHeight = (h - 3) / 2;
                int bottomHeight = h - topHeight - 3;
                char leftChar = thickness == LineThickNess.Double ? '╠' : '├';
                char rightChar = thickness == LineThickNess.Double ? '╣' : '┤';

                c.DoCommand(c, () =>
                {
                    new Draw(c)
                    .Box(0, 0, width-1, topHeight + 1, topTitle, thickness);
                    new Draw(c)
                    .Box(0, topHeight + 1, width-1, bottomHeight + topHeight  + 2, bottomTitle, thickness);
                    // print the edges
                    c.PrintAt(0, topHeight + 1, leftChar);
                    c.PrintAt(width, topHeight + 1,rightChar);
                });

                var topWin = Window._CreateFloatingWindow(1, 1, width-2, topHeight, foreground, background, true, c, null);
                var bottomWin = Window._CreateFloatingWindow(1, topHeight + 2, width - 2, bottomHeight, foreground, background, true, c, null);
                return (topWin, bottomWin);
            }
        }
    }
}
