using System;
using Konsole.Drawing;

namespace Konsole
{
    public static class SplitRightExtensions
    {
        // *****************************
        // **                         **
        // **     NO BOX BORDERS      **
        // **                         **
        // *****************************
        public static IConsole SplitRight(this IConsole c)
        {
            return LayoutExtensions._LeftRight(c, null, true, false, null, c.ForegroundColor);
        }

        public static IConsole SplitRight(this IConsole c, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, null, true, false, null, foreground);
        }

        //public static IConsole SplitRight(this Window c)
        //{
        //    return LayoutExtensions._LeftRight(c, null, true, false, null, c.ForegroundColor);
        //}

        //public static IConsole SplitRight(this Window c, ConsoleColor foreground)
        //{
        //    return LayoutExtensions._LeftRight(c, null, true, false, null, foreground);
        //}

        public static IConsole SplitRight(this IConsole c, ConsoleKeyInfo hotkey)
        {
            return LayoutExtensions._LeftRight(c, null, true, false, null, c.ForegroundColor, hotkey);
        }
        public static IConsole SplitRight(this IConsole c, ConsoleKey hotkey)
        {
            return LayoutExtensions._LeftRight(c, null, true, false, null, c.ForegroundColor, hotkey.ToKeypress());
        }

        // *****************************
        // **                         **
        // **    WITH    BORDERS      **
        // **                         **
        // *****************************

        //public static IConsole SplitRight(this Window c, string title, ConsoleColor foreground)
        //{
        //    return LayoutExtensions._LeftRight(c, title, true, true, LineThickNess.Single, foreground);
        //}

        //public static IConsole SplitRight(this Window c, string title, LineThickNess thickness)
        //{
        //    return LayoutExtensions._LeftRight(c,title, true, true, thickness, c.ForegroundColor);
        //}

        //public static IConsole SplitRight(this Window c, string title, LineThickNess thickness, ConsoleColor foreground)
        //{
        //    return LayoutExtensions._LeftRight(c, title, true, true, thickness, foreground);
        //}

        public static IConsole SplitRight(this IConsole c, string title)
        {
            return LayoutExtensions._LeftRight(c, title, true, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitRight(this IConsole c, string title, ConsoleKeyInfo hotkey)
        {
            return LayoutExtensions._LeftRight(c, title, true, true, c.Style.ThickNess, c.ForegroundColor, hotkey);
        }
        public static IConsole SplitRight(this IConsole c, string title, ConsoleKey hotkey)
        {
            return LayoutExtensions._LeftRight(c, title, true, true, c.Style.ThickNess, c.ForegroundColor, hotkey.ToKeypress());
        }

        public static IConsole SplitRight(this IConsole c, string title, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, title, true, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitRight(this IConsole c, string title, LineThickNess thickness)
        {
            return LayoutExtensions._LeftRight(c, title, true, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitRight(this IConsole c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, title, true, true, thickness, foreground);
        }


    }

}
