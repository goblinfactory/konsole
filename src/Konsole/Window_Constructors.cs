namespace Konsole
{
    public partial class Window
    {
        public Window() : this(null,
        new WindowSettings
            {
                SX = 0,
                SY = 0,
                Width = GetHostWidthHeight().width,
                Height = GetHostWidthHeight().height
            })
        {
        }

        public Window(WindowSettings settings) : this(null, settings)
        {
        }

        public Window(IConsole console, WindowSettings settings)
        {
            // need to move the box draw inside here ...do that after I've got everything working again?
            
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
                if (_echo && echoConsole == null) _echoConsole = new Writer();

                Status = status;
                Theme = theme ?? _echoConsole.Theme;
                Colors = Style.Body;

                _y = y ?? echoConsole?.CursorTop ?? _echoConsole.CursorTop + height ?? 0;
                _x = x ?? 0;
                _width = GetStartWidth(_echo, width, _x, echoConsole);
                _height = GetStartHeight(height, _y, echoConsole);

                _transparent = settings.Transparent;
                _clipping = settings.Clipping;
                _scrolling = settings.Scrolling;
                if (settings.hasTitle)
                {
                    var style = theme.GetActive(status);
                    (_x, _y, _width, _height) = DrawBoxReturnInsideDimensions(_echoConsole, _x, _y, _width, _height, settings.Title, style);
                }

                _absoluteX = echoConsole?.AbsoluteX ?? 0 + _x;
                _absoluteY = echoConsole?.AbsoluteY ?? 0 + _y;

                init();
                // if we're creating an inline window
                bool inline = (_echoConsole != null && x == null && y == null);
                if (inline)
                {
                    _echoConsole.CursorTop += _height;
                    _echoConsole.CursorLeft = 0;
                }
            }
        }

        private static (int x, int y, int width, int height) DrawBoxReturnInsideDimensions(IConsole console, int x, int y, int width, int height, string title, Style style)
        {
            new Draw(console, style, Drawing.MergeOrOverlap.Fast).Box(x, y, x + width - 1, y + height - 1, title);
            return (x + 1, y + 1, width - 2, height - 2);
        }

        /// <summary>
        /// Create a new inline window starting on the next line, at current `CursorTop + 1, using the specified width or the whole screen width if none is provided. Default color of White on Black.
        /// </summary>
        public Window(int width, int height) : this(new WindowSettings { Width = width, Height = height })
        {

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

        public Window(IConsole console, int width, int height)
    : this(console, new WindowSettings { Width = width, Height = height })
        {
        }

        public Window(IConsole console, int x, int y, int width, int height)
    : this(console, new WindowSettings { SX = x, SY = y, Width = width, Height = height })
        {
        }

        public Window(IConsole console, int x, int y, int width, int height, StyleTheme theme)
            : this(console, new WindowSettings { SX = x, SY = y, Width = width, Height = height, Theme = theme })
        {
        }

        public Window(IConsole console, int x, int y, int width, int height, Style style)
            : this(console, new WindowSettings { SX = x, SY = y, Width = width, Height = height, Theme = style.ToTheme() })
        {
        }
    }
}
