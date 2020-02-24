using System;
using static Konsole.BorderCollapse;

namespace Konsole
{  
    public enum LeftRight {  Left, Right }
    public enum TopBottom {  Top, Bottom }
    public class SplitSettings
    {
        public IConsole Console { get; set; }
        public string Title { get; set; }
        public LeftRight Direction { get; set; }
        
        bool ShowBorder { get; set; }
        
        public LineThickNess? Thickness { get; set; }
        
        public ConsoleColor Foreground { get; set; }
    }

    // TODO: convert to a partial class of Window so that the namespace does not need to be referenced?
    public static class LayoutExtensions
    {
        private static void ensureAlphaNumeric(string title, ConsoleKeyInfo hotkey)
        {
            if (hotkey.Key.IsAlphaNumeric()) throw new ArgumentOutOfRangeException($"Cannot use '{hotkey.KeyChar}' as a hot key for window '{title}' because it is alphanumeric and will interfere with any text input.");
        }

        internal static IConsole _LeftRight(IConsole c, string title, bool right, bool showBorder, LineThickNess? thickness, ConsoleColor foreground, ConsoleKeyInfo ? hotkey = null)
        {
            if(hotkey.HasValue)
            {
                ensureAlphaNumeric(title, hotkey.Value);
            }
            
            lock (Window._locker)
            {
                var theme = c.Theme.WithForeground(foreground);
                if (showBorder && thickness == null) throw new ArgumentOutOfRangeException(nameof(showBorder), "cannot be false while thickness is none.");
                int h = c.WindowHeight;
                int w = c.WindowWidth / 2 + (right ? c.WindowWidth % 2 : 0);
                int offset = right ? c.WindowWidth - w : 0;

                if (showBorder)
                {
                    new Draw(c).Box(offset, 0, w - 1 + offset, h - 1, title, thickness);
                    return Window._CreateFloatingWindow(c, new WindowSettings { SX = 1 + offset, SY = 1, Width = w - 2, Height = h - 2, Theme = theme });
                }
                return Window._CreateFloatingWindow(c, new WindowSettings { SX = offset, SY = 0, Width = w, Height = h, Theme = theme });
            }
        }

        internal static IConsole Top(IConsole c, string title, bool showBorder, LineThickNess? thickness, ConsoleColor foreground)
        {
            return _TopBot(c, title, false, showBorder, thickness, foreground);
        }

        internal static IConsole Bottom(IConsole c, string title, bool showBorder, LineThickNess? thickness, ConsoleColor foreground)
        {
            return _TopBot(c, title, true, showBorder, thickness, foreground);
        }

        private static IConsole _TopBot(IConsole c, string title, bool bottom, bool showBorder, LineThickNess? thickness, ConsoleColor foreground)
        {
            lock (Window._locker)
            {
                var theme = c.Theme.WithForeground(foreground);
                int h = (c.WindowHeight / 2) + (bottom ? 0 : c.WindowHeight % 2);
                int w = c.WindowWidth;
                int offset = bottom ? c.WindowHeight - h : 0;

                if (showBorder)
                {
                    c.DoCommand(c, () =>
                    {
                        new Draw(c).Box(0, offset, w - 1, h - 1 + offset, title, thickness  ?? theme.Active.ThickNess);
                    });
                    return Window._CreateFloatingWindow(c, new WindowSettings { SX = 1, SY = 1 + offset, Width = w - 2, Height = h - 2, Theme = theme });
                }
                return Window._CreateFloatingWindow(c, new WindowSettings { SX = 0, SY = 0 + offset, Width = w, Height = h, Theme = theme });
            }
        }
        internal static IConsole _RowSlice(IConsole c, string title, int rowStart, int size, bool showBorder, LineThickNess? thickness, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Window._locker)
            {
                var theme = c.Theme.WithColor(new Colors(foreground, background));
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
                    return Window._CreateFloatingWindow(c, new WindowSettings { SX = 1, SY = 1 + offset, Width = w - 2, Height = h - 2, Theme = theme });
                }
                return Window._CreateFloatingWindow(c, new WindowSettings { SX = 0, SY = 0 + offset, Width = w, Height = h, Theme = theme });
            }
        }
        internal static IConsole _ColumnSlice(IConsole c, string title, int colStart, int width, bool showBorder, LineThickNess? thickness, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Window._locker)
            {
                var theme = c.Theme.WithColor(new Colors(foreground, background));
                if (showBorder && thickness == null) throw new ArgumentOutOfRangeException(nameof(showBorder), "cannot be false while thickness is none.");
                int height = c.WindowHeight;
                int offset = colStart;

                if (showBorder)
                {
                    c.DoCommand(c, () =>
                    {
                        new Draw(c).Box(offset, 0, offset + width - 1, height - 1, title, thickness);
                    });
                    return Window._CreateFloatingWindow(c,  new WindowSettings { SX = offset + 1, SY = 1, Width = width - 2, Height = height - 2, Theme = theme });
                }
                return Window._CreateFloatingWindow(c, new WindowSettings { SX = offset + 0, SY = 0, Width = width, Height = height, Theme = theme });
            }
        }
    }

}
