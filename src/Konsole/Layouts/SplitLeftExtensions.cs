using System;
using Konsole.Drawing;

namespace Konsole
{
    public static class SplitLeftExtensions
    {
        //internal static IConsole SplitLeft(this Window c)
        //{
        //    return LayoutExtensions._LeftRight(c, null, false, false, null, c.ForegroundColor);
        //}

        //internal static IConsole SplitLeft(this Window c, ConsoleColor foreground)
        //{
        //    return LayoutExtensions._LeftRight(c, null, false, false, null, foreground);
        //}

        //internal static IConsole SplitLeft(this Window c, string title)
        //{
        //    return LayoutExtensions._LeftRight(c, title, false, true, LineThickNess.Single, c.ForegroundColor);
        //}

        //internal static IConsole SplitLeft(this Window c, string title, ConsoleColor foreground)
        //{
        //    return LayoutExtensions._LeftRight(c, title, false, true, LineThickNess.Single, foreground);
        //}

        //internal static IConsole SplitLeft(this Window c, string title, LineThickNess thickness)
        //{
        //    return LayoutExtensions._LeftRight(c, title, false, true, thickness, c.ForegroundColor);
        //}

        //internal static IConsole SplitLeft(this Window c, string title, LineThickNess thickness, ConsoleColor foreground)
        //{
        //    return LayoutExtensions._LeftRight(c, title, false, true, thickness, foreground);
        //}


        public static IConsole SplitLeft(this IConsole c)
        {
            return LayoutExtensions._LeftRight(c, null, false, false, null, c.ForegroundColor);
        }

        public static IConsole SplitLeft(this IConsole c, ConsoleKeyInfo hotkey)
        {
            return LayoutExtensions._LeftRight(c, null, false, false, null, c.ForegroundColor, hotkey);
        }
        public static IConsole SplitLeft(this IConsole c, ConsoleKey hotkey)
        {
            return LayoutExtensions._LeftRight(c, null, false, false, null, c.ForegroundColor, hotkey.ToKeypress());
        }

        public static IConsole SplitLeft(this IConsole c, string title, ConsoleKeyInfo hotkey)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, c.Style.ThickNess, c.ForegroundColor, hotkey);
        }
        public static IConsole SplitLeft(this IConsole c, string title, ConsoleKey hotkey)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, c.Style.ThickNess, c.ForegroundColor, hotkey.ToKeypress());
        }


        public static IConsole SplitLeft(this IConsole c, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, null, false, false, null, foreground);
        }

        public static IConsole SplitLeft(this IConsole c, string title, StyleTheme theme)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, theme, LineThickNess.Single);
        }

        public static IConsole SplitLeft(this IConsole c, string title)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, LineThickNess.Single, c.ForegroundColor);
        }

        public static IConsole SplitLeft(this IConsole c, string title, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, LineThickNess.Single, foreground);
        }

        public static IConsole SplitLeft(this IConsole c, string title, LineThickNess thickness)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, thickness, c.ForegroundColor);
        }

        public static IConsole SplitLeft(this IConsole c, string title, LineThickNess thickness, ConsoleColor foreground)
        {
            return LayoutExtensions._LeftRight(c, title, false, true, thickness, foreground);
        }

    }

}
