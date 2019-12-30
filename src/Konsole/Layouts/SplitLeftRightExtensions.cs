using System;
using static Konsole.BorderCollapse;

namespace Konsole
{
    public static class SplitLeftRightExtensions
    {
        public static(IConsole left, IConsole right) SplitLeftRight(this Window c, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, null, null, LineThickNess.Single, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static(IConsole left, IConsole right) SplitLeftRight(this Window c, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, null, null, LineThickNess.Single, border, foreground, background);
        }

        public static(IConsole left, IConsole right) SplitLeftRight(this Window c, string leftTitle, string rightTitle, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, leftTitle, rightTitle, LineThickNess.Single, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static(IConsole left, IConsole right) SplitLeftRight(this Window c, string leftTitle, string rightTitle, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, leftTitle, rightTitle, LineThickNess.Single, border, foreground, background);
        }

        public static(IConsole left, IConsole right) SplitLeftRight(this Window c, string leftTitle, string rightTitle, LineThickNess thickness, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, leftTitle, rightTitle, thickness, border, c.ForegroundColor, c.BackgroundColor);
        }

        public static(IConsole left, IConsole right) SplitLeftRight(this Window c, string leftTitle, string rightTitle, LineThickNess thickness, ConsoleColor foreground, ConsoleColor background, BorderCollapse border = Collapse)
        {
            return _SplitLeftRight(c, leftTitle, rightTitle, thickness, border, foreground, background);
        }

        internal static (IConsole left, IConsole right) _SplitLeftRight(IConsole c, string leftTitle, string rightTitle, LineThickNess thickness, BorderCollapse border, ConsoleColor foreground, ConsoleColor background)
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

            lock (Window._staticLocker)
            {
                int h = c.WindowHeight;
                int leftWidth = (c.WindowWidth - 3) / 2;
                int rightWidth = c.WindowWidth - (3 + leftWidth);

                int offset = leftWidth + 3;

                c.DoCommand(c, () =>
                {
                    //todo need unit test for merging two boxes :D
                    new Draw(c)
                    .Box(0, 0, leftWidth + 2, h - 1, leftTitle, thickness)
                    .Box(offset, 0, offset + rightWidth + 1, h - 1, rightTitle, thickness);
                });

                var leftWin = Window._CreateFloatingWindow(1, 1, leftWidth, h - 2, foreground, background, true, c, null);
                var rightWin = Window._CreateFloatingWindow(offset + 1, 1, rightWidth, h - 2, foreground, background, true, c, null);
                return (leftWin, rightWin);
            }
        }
    }
}
