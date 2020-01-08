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

        public static IConsole OpenBox(this IConsole c, string title, int sx, int sy, int width, int height)
        {
            return Window._OpenBox(c, title, sx, sy, width, height, new BoxStyle());
        }

        public static IConsole OpenBox(this IConsole c, string title, int sx, int sy, int width, int height, BoxStyle style)
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

        public static IConsole OpenBox(string title, int sx, int sy, int width, int height)
        {
            return _OpenBox(Window.HostConsole, title, sx, sy, width, height, new BoxStyle());
        }

        public static IConsole OpenBox(string title, int sx, int sy, int width, int height, BoxStyle style)
        {
            return _OpenBox(Window.HostConsole, title, sx, sy, width, height, style ?? new BoxStyle());
        }

        internal static IConsole _OpenBox(IConsole _parent, string title, int? _sx, int? _sy, int? _width, int? _height, BoxStyle style)
        {
            IConsole parent = _parent ?? new ConcurrentWriter();
            lock (Window._staticLocker)
            {
                int width = _width ?? parent.WindowWidth;
                int height = _height ?? parent.WindowHeight;

                int sx = _sx ?? 0;
                int sy = _sy ?? 0;
                int ex = sx + width - 1;
                int ey = sy + height - 1;

                parent.DoCommand( parent, () => {
                    // draw commands are all relative to the Draw() console host.
                    var draw = new Draw(parent, style.ThickNess, Drawing.MergeOrOverlap.Overlap);
                    draw.Box(sx, sy, ex, ey, title, style.Line, style.Title, style.ThickNess);
                });
                {
                    // returns a concurrentWindow
                    var window = _CreateFloatingWindow(sx + 1, sy + 1, width - 2, height - 2, style.Body.Foreground, style.Body.Background, true, parent, null);
                    var inline = _sx == null && _sy == null;
                    if (inline)
                    {
                        parent.CursorTop = parent.CursorTop + height;
                        parent.CursorLeft = 0;
                    }
                    return window;
                }

            }
        }
    }
}
