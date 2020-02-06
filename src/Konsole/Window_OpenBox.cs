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

        public static IConsole OpenBox(this IConsole c, WindowSettings settings)
        {
            return Window._OpenBox(c, settings);
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
    }

    public partial class Window
    {
        public static IConsole OpenBox(string title)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { Title = title });
        }

        public static IConsole OpenBox(string title, int width, int height)
        {
            return _OpenBox(Window.HostConsole, new WindowSettings { Title = title, Width = width, Height = height });
        }

        public static IConsole OpenBox(WindowSettings settings)
        {
            return _OpenBox(Window.HostConsole, settings);
        }

        public static IConsole OpenBox(IConsole console, string title)
        {
            return _OpenBox(console, new WindowSettings { Title = title });
        }

        public static IConsole OpenBox(IConsole console, string title, int width, int height)
        {
            return _OpenBox(console, new WindowSettings { Title = title, Width = width, Height = height });
        }

        public static IConsole OpenBox(IConsole console, WindowSettings settings)
        {
            return _OpenBox(console, settings);
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
