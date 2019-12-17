using System;
using Konsole.Drawing;

namespace Konsole
{
    public static class SplitTopExtensions
    {
        // TOPS
        public static IConsole SplitTop(this Window c)
        {
            return LayoutExtensions._TopBot(c, null, false, false, null, c.ForegroundColor);
        }

        public static IConsole SplitTop(this Window c, ConsoleColor foreground)
        {
            return LayoutExtensions._TopBot(c, null, false, false, null, foreground);
        }

        public static IConsole SplitTop(this Window c, string title)
        {
            return LayoutExtensions._TopBot(c, title, false, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitTop(this Window c, string title, ConsoleColor foreground)
        {
            return LayoutExtensions._TopBot(c, title, false, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitTop(this Window c, string title, LineThickNess thickness) 
        {
            return LayoutExtensions._TopBot(c, title, false, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitTop(this Window c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return LayoutExtensions._TopBot(c, title, false, true, thickness, foreground);
        }
    }
}
