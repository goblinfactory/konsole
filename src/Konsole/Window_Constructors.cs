using System;

namespace Konsole
{
    public static class WindowOpenExtensions
    {
        public static Window Open(this IConsole console)
        {
            return new Window(console);
        }

        public static Window Open(this IConsole console, WindowSettings settings)
        {
            return new Window(console, settings);
        }

        public static Window Open(this IConsole console, int width, int height)
        {
            return new Window(console, new WindowSettings { Width = width, Height = height });
        }

        public static Window Open(this IConsole console, int sx, int sy, int width, int height)
        {
            return new Window(console, new WindowSettings { SX = sx, SY = sy, Width = width, Height = height });
        }
        public static Window Open(this IConsole console, ConsoleColor foreground, ConsoleColor background)
        {
            return new Window(console, new WindowSettings { Theme = new StyleTheme(foreground, background) });
        }

        public static Window Open(this IConsole console, int width, int height, ConsoleColor foreground, ConsoleColor background)
        {
            return new Window(console, new WindowSettings { Width = width, Height = height, Theme = new StyleTheme(foreground, background) });
        }
        public static Window Open(this IConsole console, Style style)
        {
            return new Window(console, new WindowSettings { Theme = style.ToTheme() });
        }

        public static Window Open(this IConsole console, int width, int height, Style style)
        {
            return new Window(console, new WindowSettings { Width = width, Height = height, Theme = style.ToTheme() });
        }
        public static Window Open(this IConsole console, StyleTheme theme)
        {
            return new Window(console, new WindowSettings { Theme = theme });
        }

        public static Window Open(this IConsole console, int width, int height, StyleTheme theme)
        {
            return new Window(console, new WindowSettings { Width = width, Height = height, Theme = theme });
        }

    }

    public partial class Window
    {
        private static WindowSettings FullScreenSettings()
        {
            return new WindowSettings
            {
                SX = 0,
                SY = 0,
                Width = GetHostWidthHeight().width,
                Height = GetHostWidthHeight().height
            };
        }
        public Window() : this(null, FullScreenSettings())
        {

        }

        public Window(WindowSettings settings) : this(null, settings)
        {
        }

        internal Window(IConsole console) : this(console, new WindowSettings())
        {
        }

        public Window (int width, int height) : this(new WindowSettings { Width = width, Height = height })
        { 
        }

        public Window(int sx, int sy, int width, int height) : this(new WindowSettings { SX = sx, SY = sy, Width = width, Height = height })
        {
        }

        private string _title = null;
        private bool HasTitle = false;
        internal Window(IConsole console, WindowSettings settings)
        {
            // need to convert the constructor into a propert "settings" setter, followed by a "refresh"
            // so that you can update a window by changing properties and calling refresh again.
            
            lock (_locker)
            {
                int? x = settings.SX;
                int? y = settings.SY;
                int? width = settings.Width;
                int? height = settings.Height;
                ControlStatus status = settings.Status;
                StyleTheme theme = settings.Theme;
                bool _echo = settings._echo;
                var echoConsole = console ?? Window.HostConsole;
                if (_echo && echoConsole == null) _console = new Writer();

                Status = status;
                Theme = theme ?? _console.Theme;
                Colors = Style.Body;

                _y = y ?? echoConsole?.CursorTop ?? _console.CursorTop + height ?? 0;
                _x = x ?? 0;
                _width = GetStartWidth(_echo, width, _x, echoConsole);
                _height = GetStartHeight(height, _y, echoConsole);

                _transparent = settings.Transparent;
                _clipping = settings.Clipping;
                _scrolling = settings.Scrolling;
                _title = settings.Title;
                if (settings.hasTitle)
                {
                    HasTitle = true;
                    // we have a box around the outside, so window dimensions are "inside"
                    _x = _x + 1;
                    _y = _y + 1;
                    _width = _width - 2;
                    _height = _height - 2;
                }

                _absoluteX = echoConsole?.AbsoluteX ?? 0 + _x;
                _absoluteY = echoConsole?.AbsoluteY ?? 0 + _y;

                init();
                // if we're creating an inline window
                bool inline = (_console != null && x == null && y == null);
                if (inline)
                {
                    _console.CursorTop += _height;
                    _console.CursorLeft = 0;
                }
            }
        }

        /// <summary>
        /// Open a window Inline at the current cursorTop position, width and height wide and tall.  The parent's cursorTop is incremented so that it will continue to print underneath the newly created window. The constructor is threadsafe, so creating multiple windows will ensure they will not overlap. While the constructor is threadsafe, the returned instance is not. Calling any of the Split methods will return a threadsafe window based off this window. You can call .Concurrent() on the newly created window to return a ConcurrentWriter wrapping the window instance.
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">the height of the window</param>
        public Window(int width, int height, StyleTheme theme)
            : this(new WindowSettings { Width = width, Height = height, Theme = theme })
        {
        }

        /// <summary>
        /// Open a window Inline at the current cursorTop position, width and height wide and tall.  The parent's cursorTop is incremented so that it will continue to print underneath the newly created window. The constructor is threadsafe, so creating multiple windows will ensure they will not overlap. While the constructor is threadsafe, the returned instance is not. Calling any of the Split methods will return a threadsafe window based off this window. You can call .Concurrent() on the newly created window to return a ConcurrentWriter wrapping the window instance.
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">the height of the window</param>
        public Window(int width, int height, Style style)
            : this(new WindowSettings { Width = width, Height = height, Theme = style.ToTheme() })
        {
        }

        /// <summary>
        /// Open a window Inline at the current cursorTop position, width and height wide and tall.  The parent's cursorTop is incremented so that it will continue to print underneath the newly created window. The constructor is threadsafe, so creating multiple windows will ensure they will not overlap. While the constructor is threadsafe, the returned instance is not. Calling any of the Split methods will return a threadsafe window based off this window. You can call .Concurrent() on the newly created window to return a ConcurrentWriter wrapping the window instance.
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">the height of the window</param>
        public Window(int width, int height, ConsoleColor foreground, ConsoleColor background)
            : this(new WindowSettings { Width = width, Height = height, Theme = new StyleTheme(foreground, background) })
        {
        }
    }
}
