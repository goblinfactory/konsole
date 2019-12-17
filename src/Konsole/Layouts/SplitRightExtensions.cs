using System;
using Konsole.Drawing;

namespace Konsole
{
    public static class SplitRightExtensions
    {
        public static IConsole SplitRight(this Window c)
        {
            return LayoutExtensions._LeftRight(c, null, true, false, null, c.ForegroundColor);
        }

        public static IConsole SplitRight(this Window c, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, null, true, false, null, foreground);
        }

        public static IConsole SplitRight(this Window c, string title)
        {
            return LayoutExtensions._LeftRight(c, title, true, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitRight(this Window c, string title, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, title, true, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitRight(this Window c, string title, LineThickNess thickness)
        {
            return LayoutExtensions._LeftRight(c,title, true, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitRight(this Window c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, title, true, true, thickness, foreground);
        }
    }

}
