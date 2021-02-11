using System;
using static Konsole.BorderCollapse;

namespace Konsole
{
    public static class SplitTopBottomExtensions
    {
        public static(IConsole top, IConsole bottom) SplitTopBottom(this IConsole c, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, null, null, LineThickNess.Single, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static(IConsole top, IConsole bottom) SplitTopBottom(this IConsole c, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, null, null, LineThickNess.Single, border, foreground, background);
        }

        public static(IConsole top, IConsole bottom) SplitTopBottom(this IConsole c, string topTitle, string bottomTitle, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, topTitle, bottomTitle, LineThickNess.Single, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static(IConsole top, IConsole bottom) SplitTopBottom(this IConsole c, string topTitle, string bottomTitle, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, topTitle, bottomTitle, LineThickNess.Single, border, foreground, background);
        }

        public static(IConsole top, IConsole bottom) SplitTopBottom(this IConsole c, string topTitle, string bottomTitle, LineThickNess thickness, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, topTitle, bottomTitle, thickness, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static(IConsole top, IConsole bottom) SplitTopBottom(this IConsole c, string topTitle, string bottomTitle, LineThickNess thickness, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitTopBottom(c, topTitle, bottomTitle, thickness, border, foreground, background);
        }

        internal static (IConsole top, IConsole bottom) _SplitTopBottom(IConsole c, string topTitle, string bottomTitle, LineThickNess thickness, BorderCollapse border, ConsoleColor foreground, ConsoleColor background)
        {
            if (border == None)
            {
                var top = LayoutExtensions.Top(c, topTitle, false, thickness, foreground);
                var bottom = LayoutExtensions.Bottom(c, bottomTitle, false, thickness, foreground);
                return (top, bottom);
            }
            if (border == Separate)
            {
                var top = LayoutExtensions.Top(c, topTitle, true, thickness, foreground);
                var bottom = LayoutExtensions.Bottom(c, bottomTitle, true, thickness, foreground);
                return (top, bottom);
            }

            lock (Window._locker)
            {
                int h = c.WindowHeight;
                int width = c.WindowWidth;
                int topHeight = (h - 3) / 2;
                int bottomHeight = h - topHeight - 3;
                char leftChar = thickness == LineThickNess.Double ? '╠' : '├';
                char rightChar = thickness == LineThickNess.Double ? '╣' : '┤';

                c.DoCommand(c, () =>
                {
                    new Faster(c)
                    .Box(0, 0, width-1, topHeight + 1, topTitle, thickness);
                    new Faster(c)
                    .Box(0, topHeight + 1, width-1, bottomHeight + topHeight  + 2, bottomTitle, thickness);
                    // print the edges
                    c.PrintAt(0, topHeight + 1, leftChar);
                    c.PrintAt(width, topHeight + 1,rightChar);
                });

                var theme = c.Theme.WithColor(new Colors(foreground, background));
                var topWin = Window._CreateFloatingWindow(c, new WindowSettings { SX = 1, SY = 1, Width = width - 2, Height = topHeight, Theme = theme });
                var bottomWin = Window._CreateFloatingWindow(c, new WindowSettings { SX = 1, SY = topHeight + 2, Width = width - 2, Height = bottomHeight, Theme = theme });
                return (topWin, bottomWin);
            }
        }
    }
}
