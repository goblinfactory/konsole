using System;
using Konsole.Drawing;

namespace Konsole
{
    public static class SplitBottomExtensions
    {
        // BOTTOMS

        public static IConsole SplitBottom(this Window c)
        {
            return LayoutExtensions._TopBot(c, null, true, false, null, c.ForegroundColor);
        }

        public static IConsole SplitBottom(this Window c, ConsoleColor foreground)
        {
            return LayoutExtensions._TopBot(c, null, true, false, null, foreground);
        }

        public static IConsole SplitBottom(this Window c, string title)
        {
            return LayoutExtensions._TopBot(c, title, true, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitBottom(this Window c, string title, ConsoleColor foreground)
        {
            return LayoutExtensions._TopBot(c, title, true, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitBottom(this Window c, string title, LineThickNess thickness)
        {
            return LayoutExtensions._TopBot(c, title, true, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitBottom(this Window c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return LayoutExtensions._TopBot(c, title, true, true, thickness, foreground);
        }
    }

}
