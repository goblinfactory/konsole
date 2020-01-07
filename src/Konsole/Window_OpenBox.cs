using Konsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole
{
    public static class OpenBoxExtensions
    {
        public static IConsole OpenBox(this IConsole c, string title)
        {
            return Window._OpenBox(c, title, null, null, null, null, new BoxStyle());
        }

        public static IConsole OpenBox(this IConsole c, string title, BoxStyle style)
        {
            return Window._OpenBox(c, title, null, null, null, null, style);
        }

        public static IConsole OpenBox(this IConsole c, string title, int width, int height, BoxStyle style)
        {
            return Window._OpenBox(c, title, null, null, width, height, style);
        }

        public static IConsole OpenBox(this IConsole c, string title, int sx, int sy, int height, int width, BoxStyle style)
        {
            return Window._OpenBox(c, title, sx, sy, width, height, style);
        }
    }

    public partial class Window
    {
        public static IConsole OpenBox(string title)
        {
            return _OpenBox(Window.HostConsole, title, null, null, null, null, new BoxStyle());
        }

        public static IConsole OpenBox(string title, BoxStyle style)
        {
            return _OpenBox(Window.HostConsole, title, null, null, null, null, style);
        }

        public static IConsole OpenBox(string title, int width, int height, BoxStyle style = null)
        {
            return _OpenBox(Window.HostConsole, title, null, null, width, height, style ?? new BoxStyle());
        }

        public static IConsole OpenBox(string title, int sx, int sy, int width, int height, BoxStyle style)
        {
            return _OpenBox(Window.HostConsole, title, sx, sy, width, height, style ?? new BoxStyle());
        }

        internal static IConsole _OpenBox(IConsole _parent, string title, int? _sx, int? _sy, int? _width, int? _height, BoxStyle style)
        {
            IConsole parent = _parent ?? new ConcurrentWriter();
            bool isInline = _sx == null && _sy == null;
            lock (Window._staticLocker)
            {
                int sx = _sx ?? 0;
                int sy = _sy ?? 0;
                int width = _width ?? parent.WindowWidth;
                int height = _height ?? parent.WindowHeight;

                parent.DoCommand( parent, () => {
                    // subtract 1 because box uses ex, and ey which is 0 ordinal
                    new Draw(parent).Box(sx, sy, width - 1, height - 1, title, style.Line, style.Title, style.ThickNess);
                });
                
                var window = _CreateFloatingWindow(1, 1, width - 2, height - 2, style.Body.Foreground, style.Body.Background, true, parent, null);
                
                if(isInline)
                {
                    parent.CursorTop = parent.CursorTop + 1;
                    parent.CursorLeft = 0;
                }

                return window;
            }
        }
    }
}
