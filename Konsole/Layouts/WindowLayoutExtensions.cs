using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole.Drawing;
using Konsole.Menus;

namespace Konsole.Layouts
{
    public static class WindowLayoutExtensions
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

        public static IConsole SplitLeft(this Window c)
        {
            return LayoutExtensions._LeftRight(c, null, false, false, null, c.ForegroundColor);
        }

        public static IConsole SplitLeft(this Window c, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, null, false, false, null, foreground);
        }

        public static IConsole SplitLeft(this Window c, string title)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitLeft(this Window c, string title, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitLeft(this Window c, string title, LineThickNess thickness)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitLeft(this Window c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, thickness, foreground);
        }

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
