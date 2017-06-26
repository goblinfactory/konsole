using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole.Drawing;
using Konsole.Menus;

namespace Konsole.Layouts
{
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
            return _LeftRight(c,title, true, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitRight(this IConsole c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return _LeftRight(c, title, true, true, thickness, foreground);
        }


        private static IConsole _LeftRight(IConsole c, string title, bool right, bool showBorder, LineThickNess? thickness, ConsoleColor foreground)
        {
            lock (Window._staticLocker)
            {
                if(showBorder && thickness == null) throw new ArgumentOutOfRangeException(nameof(showBorder),"cannot be false while thickness is none.");
                int h = c.WindowHeight;
                int w = c.WindowWidth / 2 + (right ? c.WindowWidth % 2 : 0);
                int offset = right ? c.WindowWidth - w : 0;

                if (showBorder)
                {
                    c.DoCommand(c, () =>
                    {
                        new Draw(c).Box(offset, 0, w - 1 + offset, h - 1, title, thickness);
                    });
                    return Window._CreateWindow(1 + offset, 1, w - 2, h - 2, foreground, c.BackgroundColor, true, c, null);
                }
                return Window._CreateWindow(offset, 0, w, h, foreground, c.BackgroundColor, true, c, null);
            }
        }


        private static IConsole _TopBot(IConsole c, string title, bool bottom, bool showBorder, LineThickNess? thickness, ConsoleColor foreground)
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
                    return Window._CreateWindow(1, 1 + offset, w - 2, h - 2, foreground, c.BackgroundColor, true, c, null);
                }
                return Window._CreateWindow(0, 0 + offset, w, h, foreground, c.BackgroundColor, true, c, null);
            }
        }

    }

}
