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
        /// <summary>
        /// Open a full screen styled window with a lined box border with a title. Styling allows for setting foreground and background color of the Line, Title, and body, as well as the line thickness, single or double using default styling, white on black, single thickness line. 
        /// </summary>
        /// <param name="title">title text centered on the top line</param>
        /// <returns>threadsafe concurrentWriter wrapping the inside scrollable window inside the box.</returns>
        public static IConsole OpenBox(string title)
        {
            return _OpenBox(Window.HostConsole, title, null, null, null, null, new BoxStyle());
        }

        /// <summary>
        /// Open a fullscreen boxed window with a lined box border with a title. Styling allows for setting foreground and background color of the Line, Title, and body, as well as the line thickness, single or double using default styling, white on black, single thickness line. 
        /// </summary>
        /// <param name="title">title text centered on the top line</param>
        /// <param name="style">Line colors, Line thickness, content colors, Title colors</param>
        /// <returns>threadsafe concurrentWriter wrapping the inside scrollable window inside the box.</returns>
        public static IConsole OpenBox(string title, BoxStyle style)
        {
            return _OpenBox(Window.HostConsole, title, null, null, null, null, style);
        }

        /// <summary>
        /// Open a styled floating or inline window with a lined box border with a title. Styling allows for setting foreground and background color of the Line, Title, and body, as well as the line thickness, single or double using default styling, white on black, single thickness line. 
        /// </summary>
        /// <param name="title">title text centered on the top line</param>
        /// <param name="width">width (columns)</param>
        /// <param name="height">height (rows)</param>
        /// <param name="style">Line colors, Line thickness, content colors, Title colors</param>
        /// <returns>threadsafe concurrentWriter wrapping the inside scrollable window inside the box.</returns>
        public static IConsole OpenBox(string title, int width, int height, BoxStyle style = null)
        {
            return _OpenBox(Window.HostConsole, title, null, null, width, height, style ?? new BoxStyle());
        }

        /// <summary>
        /// Open a styled floating or inline window with a lined box border with a title. Styling allows for setting foreground and background color of the Line, Title, and body, as well as the line thickness, single or double using default styling, white on black, single thickness line. 
        /// </summary>
        /// <param name="title">title text centered on the top line</param>
        /// <param name="width">width (columns)</param>
        /// <param name="height">height (rows)</param>
        /// <returns>threadsafe concurrentWriter wrapping the inside scrollable window inside the box.</returns>
        public static IConsole OpenBox(string title, int sx, int sy, int width, int height)
        {
            return _OpenBox(Window.HostConsole, title, sx, sy, width, height, new BoxStyle());
        }

        /// <summary>
        /// Open a styled floating or inline window with a lined box border with a title. Styling allows for setting foreground and background color of the Line, Title, and body, as well as the line thickness, single or double. Returns a window instance representing the window inside the box. The returned instance is threadsafe.
        /// </summary>
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
