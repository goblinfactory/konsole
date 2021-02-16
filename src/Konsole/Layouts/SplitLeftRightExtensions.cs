using System;
using static Konsole.BorderCollapse;

namespace Konsole
{
    public static class SplitLeftRightExtensions
    {
        public static (IConsole left, IConsole right) SplitLeftRight(this IConsole c, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, null, null, LineThickNess.Single, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static (IConsole left, IConsole right) SplitLeftRight(this IConsole c, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, null, null, LineThickNess.Single, border, foreground, background);
        }

        public static (IConsole left, IConsole right) SplitLeftRight(this IConsole c, string leftTitle, string rightTitle, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, leftTitle, rightTitle, LineThickNess.Single, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static (IConsole left, IConsole right) SplitLeftRight(this IConsole c, string leftTitle, string rightTitle, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, leftTitle, rightTitle, LineThickNess.Single, border, foreground, background);
        }

        public static (IConsole left, IConsole right) SplitLeftRight(this IConsole c, string leftTitle, string rightTitle, LineThickNess thickness, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, leftTitle, rightTitle, thickness, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static (IConsole left, IConsole right) SplitLeftRight(this IConsole c, string leftTitle, string rightTitle, LineThickNess thickness, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, leftTitle, rightTitle, thickness, border, foreground, background);
        }

        internal static (IConsole left, IConsole right) _SplitLeftRight(IConsole c, string leftTitle, string rightTitle, LineThickNess? thickness, BorderCollapse border, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Window._locker)
            {
                if (border == None)
                {
                    var left = LayoutExtensions._LeftRight(c, leftTitle, false, false, thickness, foreground);
                    var right = LayoutExtensions._LeftRight(c, rightTitle, true, false, thickness, foreground);
                    return (left, right);
                }
                if (border == Separate)
                {
                    var left = LayoutExtensions._LeftRight(c, leftTitle, false, true, thickness, foreground);
                    var right = LayoutExtensions._LeftRight(c, rightTitle, true, true, thickness, foreground);
                    return (left, right);
                }

                int h = c.WindowHeight;
                int w = c.WindowWidth - 3;
                int leftWidth = w / 2;
                int rightWidth = (w - leftWidth);

                c.DoCommand(c, () =>
                {
                    //todo need unit test for merging two boxes :D for now, lets print them twice so we get true overlap to start with
                    new Faster(c)
                    .Box(0, 0, leftWidth + 1, h - 1, leftTitle, thickness);
                    new Faster(c)
                    .Box(leftWidth + 1, 0, rightWidth + leftWidth + 2, h - 1, rightTitle, thickness);
                    // print the corners
                    c.PrintAt(leftWidth + 1, 0, '┬');
                    c.PrintAt(leftWidth + 1, h - 1, '┴');
                });

                var theme = c.Theme.WithColor(new Colors(foreground, background));
                var leftWin = new Window(c, new WindowSettings { SX = 1, SY = 1, Width = leftWidth, Height = h - 2, Theme = theme });
                var rightWin = new Window(c, new WindowSettings { SX = leftWidth + 2, SY = 1, Width = rightWidth, Height = h - 2, Theme = theme });
                return (leftWin, rightWin);
            }
        }
    }
}
