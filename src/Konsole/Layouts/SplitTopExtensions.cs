using System;
using Konsole.Drawing;

namespace Konsole
{
    public static class SplitTopExtensions
    {
        public static IConsole SplitTop(this IConsole c)
        {
            return LayoutExtensions.Top(c, null, false, null, c.ForegroundColor);
        }

        public static IConsole SplitTop(this IConsole c, ConsoleColor foreground)
        {
            return LayoutExtensions.Top(c, null, false, null, foreground);
        }

        //public static IConsole SplitTop(this IConsole c, string title, StyleTheme theme)
        //{
        //    return LayoutExtensions.Top(c, title, true, theme);
        //}

        public static IConsole SplitTop(this IConsole c, string title)
        {
            return LayoutExtensions.Top(c, title, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitTop(this IConsole c, string title, ConsoleColor foreground)
        {
            return LayoutExtensions.Top(c, title, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitTop(this IConsole c, string title, LineThickNess thickness)
        {
            return LayoutExtensions.Top(c, title, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitTop(this IConsole c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return LayoutExtensions.Top(c, title, true, thickness, foreground);
        }
    }
}
