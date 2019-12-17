using System;
using Konsole.Drawing;

namespace Konsole
{
    // TODO: convert to a partial class of Window so that the namespace does not need to be referenced?
    public static class LayoutExtensions
    {
        // TOPS
        public static IConsole SplitTop(this IConsole c)
        {
            return _TopBot(c, null, false, false, null, c.ForegroundColor);
        }

        public static IConsole SplitTop(this IConsole c, ConsoleColor foreground)
        {
            return _TopBot(c, null, false, false, null, foreground);
        }

        public static IConsole SplitTop(this IConsole c, string title)
        {
            return _TopBot(c, title, false, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitTop(this IConsole c, string title, ConsoleColor foreground)
        {
            return _TopBot(c, title, false, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitTop(this IConsole c, string title, LineThickNess thickness)
        {
            return _TopBot(c, title, false, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitTop(this IConsole c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return _TopBot(c, title, false, true, thickness, foreground);
        }

        // BOTTOMS

        public static IConsole SplitBottom(this IConsole c)
        {
            return _TopBot(c, null, true, false, null, c.ForegroundColor);
        }

        public static IConsole SplitBottom(this IConsole c, ConsoleColor foreground)
        {
            return _TopBot(c, null, true, false, null, foreground);
        }

        public static IConsole SplitBottom(this IConsole c, string title)
        {
            return _TopBot(c, title, true, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitBottom(this IConsole c, string title, ConsoleColor foreground)
        {
            return _TopBot(c, title, true, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitBottom(this IConsole c, string title, LineThickNess thickness)
        {
            return _TopBot(c, title, true, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitBottom(this IConsole c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return _TopBot(c, title, true, true, thickness, foreground);
        }

        public static IConsole SplitLeft(this IConsole c)
        {
            return _LeftRight(c, null, false, false, null, c.ForegroundColor);
        }

        public static IConsole SplitLeft(this IConsole c, ConsoleColor foreground)
        {
            return _LeftRight(c, null, false, false, null, foreground);
        }

        public static IConsole SplitLeft(this IConsole c, string title)
        {
            return _LeftRight(c, title, false, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitLeft(this IConsole c, string title, ConsoleColor foreground)
        {
            return _LeftRight(c, title, false, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitLeft(this IConsole c, string title, LineThickNess thickness)
        {
            return _LeftRight(c, title, false, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitLeft(this IConsole c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return _LeftRight(c, title, false, true, thickness, foreground);
        }

        public static IConsole SplitRight(this IConsole c)
        {
            return _LeftRight(c, null, true, false, null, c.ForegroundColor);
        }

        public static IConsole SplitRight(this IConsole c, ConsoleColor foreground)
        {
            return _LeftRight(c, null, true, false, null, foreground);
        }

        public static IConsole SplitRight(this IConsole c, string title)
        {
            return _LeftRight(c, title, true, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitRight(this IConsole c, string title, ConsoleColor foreground)
        {
            return _LeftRight(c, title, true, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitRight(this IConsole c, string title, LineThickNess thickness)
        {
            return _LeftRight(c, title, true, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitRight(this IConsole c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return _LeftRight(c, title, true, true, thickness, foreground);
        }


        internal static IConsole _LeftRight(IConsole c, string title, bool right, bool showBorder, LineThickNess? thickness, ConsoleColor foreground)
        {
            lock (Window._staticLocker)
            {
                if (showBorder && thickness == null) throw new ArgumentOutOfRangeException(nameof(showBorder), "cannot be false while thickness is none.");
                int h = c.WindowHeight;
                int w = c.WindowWidth / 2 + (right ? c.WindowWidth % 2 : 0);
                int offset = right ? c.WindowWidth - w : 0;

                if (showBorder)
                {
                    c.DoCommand(c, () =>
                    {
                        new Draw(c).Box(offset, 0, w - 1 + offset, h - 1, title, thickness);
                    });
                    return Window._CreateFloatingWindow(1 + offset, 1, w - 2, h - 2, foreground, c.BackgroundColor, true, c, null);
                }
                return Window._CreateFloatingWindow(offset, 0, w, h, foreground, c.BackgroundColor, true, c, null);
            }
        }


        internal static IConsole _TopBot(IConsole c, string title, bool bottom, bool showBorder, LineThickNess? thickness, ConsoleColor foreground)
        {
            lock (Window._staticLocker)
            {
                if (showBorder && thickness == null) throw new ArgumentOutOfRangeException(nameof(showBorder), "cannot be false while thickness is none.");
                int h = (c.WindowHeight / 2) + (bottom ? 0 : c.WindowHeight % 2);
                int w = c.WindowWidth;
                int offset = bottom ? c.WindowHeight - h : 0;

                if (showBorder)
                {
                    c.DoCommand(c, () =>
                    {
                        new Draw(c).Box(0, offset, w - 1, h - 1 + offset, title, thickness);
                    });
                    return Window._CreateFloatingWindow(1, 1 + offset, w - 2, h - 2, foreground, c.BackgroundColor, true, c, null);
                }
                return Window._CreateFloatingWindow(0, 0 + offset, w, h, foreground, c.BackgroundColor, true, c, null);
            }
        }
        internal static IConsole _RowSlice(IConsole c, string title, int rowStart, int size, bool showBorder, LineThickNess? thickness, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Window._staticLocker)
            {
                if (showBorder && thickness == null) throw new ArgumentOutOfRangeException(nameof(showBorder), "cannot be false while thickness is none.");
                int h = size;
                int w = c.WindowWidth;
                int offset = rowStart;

                if (showBorder)
                {
                    c.DoCommand(c, () =>
                    {
                        new Draw(c).Box(0, offset, w - 1, h - 1 + offset, title, thickness);
                    });
                    return Window._CreateFloatingWindow(1, 1 + offset, w - 2, h - 2, foreground, background, true, c, null);
                }
                return Window._CreateFloatingWindow(0, 0 + offset, w, h, foreground, background, true, c, null);
            }
        }
        internal static IConsole _ColumnSlice(IConsole c, string title, int colStart, int width, bool showBorder, LineThickNess? thickness, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Window._staticLocker)
            {
                if (showBorder && thickness == null) throw new ArgumentOutOfRangeException(nameof(showBorder), "cannot be false while thickness is none.");
                int height = c.WindowHeight;
                int offset = colStart;

                if (showBorder)
                {
                    c.DoCommand(c, () =>
                    {
                        new Draw(c).Box(offset, 0, offset + width - 1, height - 1, title, thickness);
                    });
                    return Window._CreateFloatingWindow(offset + 1, 1, width - 2, height - 2, foreground, background, true, c, null);
                }
                return Window._CreateFloatingWindow(offset + 0 , 0, width, height, foreground, background, true, c, null);
            }
        }
    }

}
