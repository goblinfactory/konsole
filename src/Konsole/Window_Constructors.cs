using System;

namespace Konsole
{
    public static class WindowOpenExtensions
    {
        public static Window Open(this IConsole console)
        {
            return new Window(console);
        }

        public static Window Open(this IConsole console, string title)
        {
            return new Window(console, new WindowSettings { Title = title });
        }

        public static Window Open(this IConsole console, WindowSettings settings)
        {
            return new Window(console, settings);
        }

        public static Window Open(this IConsole console, int width, int height)
        {
            return new Window(console, new WindowSettings { Width = width, Height = height });
        }

        public static Window Open(this IConsole console, string title, int sx, int sy, int width, int height)
        {
            return new Window(console, new WindowSettings { Title = title, SX = sx, SY = sy, Width = width, Height = height });
        }

        public static Window Open(this IConsole console, int width, int height, string title)
        {
            return new Window(console, new WindowSettings { Width = width, Title = title, Height = height });
        }

        public static Window Open(this IConsole console, int height)
        {
            return new Window(console, new WindowSettings { Height = height });
        }

        public static Window Open(this IConsole console, int sx, int sy, int width, int height, ConsoleColor foreground, ConsoleColor background)
        {
            return new Window(console, new WindowSettings
            {
                SX = sx,
                SY = sy,
                Width = width,
                Height = height,
                Theme = new StyleTheme(foreground, background)

            });
        }

        public static Window Open(this IConsole console, int sx, int sy, int width, int height, string title, ConsoleColor foreground, ConsoleColor background)
        {
            return new Window(console, new WindowSettings
            {
                SX = sx,
                SY = sy,
                Width = width,
                Height = height,
                Title = title,
                Theme = new StyleTheme(foreground, background)

            });
        }

        public static Window Open(this IConsole console, int sx, int sy, int width, int height, string title, LineThickNess thickness, ConsoleColor foreground, ConsoleColor background)
        {
            return new Window(console, new WindowSettings
            {
                SX = sx,
                SY = sy,
                Width = width,
                Height = height,
                Title = title,
                Theme = new StyleTheme(foreground, background, thickness)

            });
        }

        public static Window Open(this IConsole console, int sx, int sy, int width, int height)
        {
            return new Window(console, new WindowSettings { SX = sx, SY = sy, Width = width, Height = height });
        }
        public static Window Open(this IConsole console, ConsoleColor foreground, ConsoleColor background)
        {
            return new Window(console, new WindowSettings { Theme = new StyleTheme(foreground, background) });
        }

        public static Window Open(this IConsole console, int height, ConsoleColor foreground, ConsoleColor background)
        {
            return new Window(console, new WindowSettings { Height = height, Theme = new StyleTheme(foreground, background) });
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

    public partial class Window : IConsole
    {
        private static WindowSettings FullScreenSettings()
        {
            return new WindowSettings
            {
                SX = 0,
                SY = 0,
                Width = _GetHostWidthHeight().width,
                Height = _GetHostWidthHeight().height
            };
        }
        public Window(string title, LineThickNess thickness) : this(null,
            FullScreenSettings()
            .WithTitle(title)
            .WithStyle(Style.Default.WithThickness(thickness))
        )
        {

        }

        public Window(string title, Style style) : this(null, 
            FullScreenSettings()
            .WithTitle(title)
            .WithStyle(style)
        ) 
        {
        
        }

        public Window(string title) : this(null, FullScreenSettings().WithTitle(title))
        {

        }
        public Window(string title, int width, int height) : this(null,
            new WindowSettings
            {
                Title = title,
                Width = width,
                Height = height
            }
            )
        { }

        public Window(string title, int sx, int sy, int width, int height, Style style) : this(null,
    new WindowSettings
    {
        Title = title,
        SX = sx,
        SY = sy,
        Width = width,
        Height = height,
        Theme = style.ToTheme()
    }
    )
        { }

        public Window(string title, int sx, int sy, int width, int height) : this(null,
            new WindowSettings
            {
                Title = title,
                SX = sx,
                SY = sy,
                Width = width,
                Height = height
            }
            )
        { }

        public Window(IConsole console, string title) : this(console, FullScreenSettings().WithTitle(title))
        {

        }

        public Window() : this(null, FullScreenSettings())
        {

        }

        public Window(WindowSettings settings) : this(null, settings)
        {
        }

        public Window(IConsole console) : this(console, new WindowSettings())
        {
        }

        public Window(int height, ConsoleColor foreground, ConsoleColor background) : this(new WindowSettings { SX = 0, Height = height, Theme = new StyleTheme(foreground, background) })
        {
        }

        public Window(ConsoleColor foreground, ConsoleColor background) : this(new WindowSettings { Theme = new StyleTheme(foreground, background) })
        {
        }

        public Window(int width, int height, string title) : this(new WindowSettings { Width = width, Height = height, Title = title })
        {
        }

        public Window (int width, int height) : this(new WindowSettings { Width = width, Height = height })
        { 
        }
        public Window(int sx, int sy, int width, int height, string title, LineThickNess thickness, ConsoleColor foreground, ConsoleColor background) : this(
            new WindowSettings
            {
                Title = title,
                Theme = new Style(thickness, new Colors(foreground, background)).ToTheme(),
                SX = sx,
                SY = sy,
                Width = width,
                Height = height
            })
        {
        }

        public Window(int sx, int sy, int width, int height) : this(new WindowSettings { SX = sx, SY = sy, Width = width, Height = height })
        {
        }

        private string _title = null;
        private bool HasTitle = false;

        private bool hasVisibleContent(int height, int width)
        {
            return !(
                (height <= 0) || (width <= 0)
                );
        }

        internal Window(IConsole console, WindowSettings settings)
        {            
            lock (_locker)
            {
                int x = settings.SX + settings.PadLeft;
                int? y = settings.SY;
                int? width = settings.Width;
                int? height = settings.Height;
                bool fullScreen = x == 0 && y == null && width == null && height == null;
                ControlStatus status = settings.Status;
                StyleTheme theme = settings.Theme ?? console.Theme;
                Theme = theme;
                // todo - assign _console only once, prepare for readonly in C#8
                _echo = settings._echo;
                _console = console ?? HostConsole;
                if (_echo && _console == null) _console = new Writer();
                bool inline = (_console != null && y == null);
                Status = status;
                
                Colors = Style.Body;

                _y = y ?? _console?.CursorTop ?? this._console.CursorTop + height ?? 0;
                _x = x;
                WindowWidth = GetStartWidth(_echo, width, _x, _console);
                WindowHeight = GetStartHeight(height, _y, _console);

                _hasVisibleContent = hasVisibleContent(WindowHeight, WindowWidth);
                Transparent = settings.Transparent;
                Clipping = settings.Clipping;
                Scrolling = settings.Scrolling;
                _title = settings.Title;
                if (settings.hasTitle)
                {
                    HasTitle = true;
                    // we have a box around the outside, so window dimensions are "inside"
                    _x = _x + 1;
                    _y = _y + 1;
                    WindowWidth = WindowWidth - 2;
                    WindowHeight = WindowHeight - 2;
                }

                (_absoluteX, _absoluteY) = GetAbsolutePosition(HasTitle, _console, _x, _y);

                init();
                    
                // if we're creating an inline window
                // then move cursor underneath the inline
                if (inline && !fullScreen)
                {
                    if(_hasVisibleContent)
                    {
                        _console.CursorTop += WindowHeight + (HasTitle ? 2 : 0);
                        _console.CursorLeft = 0;
                    }
                }

                if(!_hasVisibleContent)
                {
                    _console = new NullWriter();
                }
            }
        }

        private static (int absoluteX, int absoluteY) GetAbsolutePosition(bool hasTitle, IConsole parent, int x, int y)
        {
            // argh, the great offset debarkable of March 2020!
            int offset = 0; // hasTitle ? 1 : 0;
            var absoluteX = (parent?.AbsoluteX ?? 0) + x + offset;
            var absoluteY = (parent?.AbsoluteY ?? 0) + y + offset;
            return (absoluteX, absoluteY);
        }

        private static int GetStartWidth(bool echo, int? width, int x, IConsole echoConsole)
        {
            // what are the rules for getting the start width?
            // and where are the tests that prove that?

            int echoWidth = echoConsole?.WindowWidth ?? x;
            int maxWidth = (echoWidth - x);
            int w = width ?? (echoConsole?.WindowWidth ?? 120);
            if (echo && w > maxWidth) w = maxWidth;
            return w;
        }

        private static int GetStartHeight(int? height, int y, IConsole console)
        {
            // if no height has been provided then use the whole or balance of the window height
            int h = height ?? (console?.WindowHeight ?? y);

            // start height should be clipped to not exceed parent window 
            if (h + y > console.WindowHeight)
            {
                return (console.WindowHeight - y);
            }
            return h;
        }

        /// <summary>
        /// Open a window Inline at the specific sx, sy position with, width and height wide and tall.  The parent's cursorTop is incremented so that it will continue to print underneath the newly created window. The constructor is threadsafe, so creating multiple windows will ensure they will not overlap. While the constructor is threadsafe, the returned instance is not. Calling any of the Split methods will return a threadsafe window based off this window. You can call .Concurrent() on the newly created window to return a ConcurrentWriter wrapping the window instance.
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">the height of the window</param>
        public Window(int padLeft, int width, int height, StyleTheme theme)
            : this(new WindowSettings { PadLeft = padLeft, Width = width, Height = height, Theme = theme })
        {
        }

        /// <summary>
        /// Open a window Inline at the specific sx, sy position with, width and height wide and tall.  The parent's cursorTop is incremented so that it will continue to print underneath the newly created window. The constructor is threadsafe, so creating multiple windows will ensure they will not overlap. While the constructor is threadsafe, the returned instance is not. Calling any of the Split methods will return a threadsafe window based off this window. You can call .Concurrent() on the newly created window to return a ConcurrentWriter wrapping the window instance.
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">the height of the window</param>
        public Window(int sx, int sy, int width, int height, StyleTheme theme)
            : this(new WindowSettings { SX = sx, SY = sy, Width = width, Height = height, Theme = theme })
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
        /// Open a window Inline at the current cursorTop position, width and height wide and tall.  The parent's cursorTop is incremented so that it will continue to print underneath the newly created window.
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">the height of the window</param>
        /// <remarks>The constructor and new window is threadsafe, so creating multiple windows will ensure they will not overlap.</remarks>
        public Window(int width, int height, Style style)
            : this(new WindowSettings { Width = width, Height = height, Theme = style.ToTheme() })
        {
        }

        /// <summary>
        /// Open a window Inline at the current cursorTop position, width and height wide and tall.  The parent's cursorTop is incremented so that it will continue to print underneath the newly created window.
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">the height of the window</param>
        /// <remarks>The constructor and new window is threadsafe, so creating multiple windows will ensure they will not overlap.</remarks>
        public Window(int width, int height, ConsoleColor foreground, ConsoleColor background)
            : this(new WindowSettings { Width = width, Height = height, Theme = new StyleTheme(foreground, background) })
        {
        }
    }
}
