using System;
using Konsole.Drawing;

namespace Konsole
{
    public static class SplitBottomExtensions
    {
        public static IConsole SplitBottom(this IConsole c)
        {
            return LayoutExtensions.Bottom(c, null, false, null, c.ForegroundColor);
        }

        public static IConsole SplitBottom(this IConsole c, ConsoleColor foreground)
        {
            return LayoutExtensions.Bottom(c, null, false, null, foreground);
        }

        public static IConsole SplitBottom(this IConsole c, string title)
        {
            return LayoutExtensions.Bottom(c, title, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitBottom(this IConsole c, string title, ConsoleColor foreground)
        {
            return LayoutExtensions.Bottom(c, title, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitBottom(this IConsole c, string title, LineThickNess thickness)
        {
            return LayoutExtensions.Bottom(c, title, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitBottom(this IConsole c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return LayoutExtensions.Bottom(c, title, true, thickness, foreground);
        }
    }

}
