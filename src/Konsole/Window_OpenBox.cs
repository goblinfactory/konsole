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
            var _settings = new WindowSettings { Title = title };
            return Window._OpenBox(c, _settings);
        }

        public static IConsole OpenBox(this IConsole c, string title, WindowSettings settings)
        {
            return Window._OpenBox(c, new WindowSettings(settings) { Title = title });
        }

        public static IConsole OpenBox(this IConsole c, string title, int sx, int sy, int width, int height, LineThickNess thickness)
        {
            return Window._OpenBox(c, new WindowSettings() { 
                SX = sx,
                SY = sy,
                Height = height,
                Width = width,
                Theme = c.Style.WithThickness(thickness).ToTheme(),
                Title = title 
            });
        }

        public static IConsole OpenBox(this IConsole c, string title, int width, int height)
        {
            var _settings = new WindowSettings()
            {
                Title = title,
                Width = width,
                Height = height
            };
            return Window._OpenBox(c, _settings);
        }

        public static IConsole OpenBox(this IConsole c, string title, int sx, int sy, int width, int height)
        {
            var _settings = new WindowSettings()
            {
                SX = sx,
                SY = sy,
                Title = title,
                Width = width,
                Height = height,
            };
            return Window._OpenBox(c, _settings);
        }

        public static IConsole OpenBox(this IConsole c, string title, int width, int height, ConsoleColor foreground, ConsoleColor background)
        {
            var _settings = new WindowSettings()
            {
                Title = title,
                Width = width,
                Height = height,
                Theme = new StyleTheme(foreground, background)
            };
            return Window._OpenBox(c, _settings);
        }
        public static IConsole OpenBox(this IConsole c, string title, int width, int height, Style style)
        {
            var _settings = new WindowSettings()
            {
                Title = title,
                Width = width,
                Height = height,
                Theme = style.ToTheme()
            };
            return Window._OpenBox(c, _settings);
        }

        public static IConsole OpenBox(this IConsole c, string title, int width, int height, StyleTheme theme)
        {
            var _settings = new WindowSettings()
            {
                Title = title,
                Width = width,
                Height = height,
                Theme = theme
            };
            return Window._OpenBox(c, _settings);
        }
    }

    public partial class Window
    {
        public static IConsole OpenBox(string title)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { Title = title });
        }

        public static IConsole OpenBox(string title, WindowSettings settings)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings(settings) { Title = title } );
        }

        public static IConsole OpenBox(string title, int sx, int sy, int width, int height, LineThickNess thickness)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings {
                Title = title,
                SX = sx,
                SY = sy,
                Width = width,
                Height = height,
                Theme = Style.Default.WithThickness(thickness).ToTheme()
            }) ;
        }

        public static IConsole OpenBox(string title, int width, int height)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { Title = title, Width = width, Height = height });
        }

        public static IConsole OpenBox(string title, int sx, int sy, int width, int height)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { SX = sx, SY = sy, Title = title, Width = width, Height = height });
        }

        public static IConsole OpenBox(string title, int width, int height, ConsoleColor foreground, ConsoleColor background)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { Title = title, Width = width, Height = height, Theme = new StyleTheme(foreground, background) });
        }

        public static IConsole OpenBox(string title, ConsoleColor foreground, ConsoleColor background)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { Title = title, Theme = new StyleTheme(foreground, background) });
        }
        public static IConsole OpenBox(string title, Style style)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { Title = title, Theme = style.ToTheme() });
        }

        public static IConsole OpenBox(string title, int width, int height, Style style)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { Title = title, Width = width, Height = height, Theme = style.ToTheme() });
        }
        public static IConsole OpenBox(string title, StyleTheme theme)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { Title = title, Theme = theme });
        }

        public static IConsole OpenBox(string title, int width, int height, StyleTheme theme)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { Title = title, Width = width, Height = height, Theme = theme });
        }

        internal static IConsole _OpenBox(IConsole console, WindowSettings settings)
        {
            var status = settings.Status;
            var title = settings.Title;
            var _sx = settings.SX;
            var _sy = settings.SY;
            var _width = settings.Width;
            var _height = settings.Height;
            var theme = settings.Theme;

            IConsole _console = console ??  HostConsole;
            theme = theme ?? console.Theme;


            lock (Window._locker)
            {
                int width = _width ?? _console.WindowWidth;
                int height = _height ?? _console.WindowHeight;
                var inline = _sx == 0 && _sy == null;
                int sx = _sx;
                int sy = _sy ?? _console.CursorTop;
                int ex = sx + width - 1;
                int ey = sy + height - 1;

                var style = theme.Active;
                var draw = new Draw(_console, style, Drawing.MergeOrOverlap.Fast);
                draw.Box(sx, sy, ex, ey, title);

                var window = _CreateFloatingWindow(_console, new WindowSettings { SX = sx + 1, SY = sy + 1, Width = width - 2, Height = height - 2, Status = status, Theme = theme });
                if (inline)
                {
                    _console.CursorTop = _console.CursorTop + height;
                    _console.CursorLeft = 0;
                }
                return window;
            }
        }
    }
}
