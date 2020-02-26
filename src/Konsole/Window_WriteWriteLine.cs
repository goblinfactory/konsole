using System;

namespace Konsole
{
    public partial class Window
    {
        public void Write(string text)
        {
            lock(_locker)
            {
                _write(text);
            }
        }

        public void Write(string format, object args0)
        {
            lock (_locker)
            {
                _write(format, args0);
            }
        }

        private void _write(string format, object args0)
        {
                var text = string.Format(format, args0);
                _write(text);
        }

        private void _write(string format, params object[] args)
        {
            var text = string.Format(format, args);
            _write(text);
        }


        public void Write(string format, params object[] args)
        {
            lock (_locker)
            {
                _write(format, args);
            }
        }

        private void _write(ConsoleColor color, string format, params object[] args)
        {
            var foreground = ForegroundColor;
            try
            {
                ForegroundColor = color;
                _write(format, args);
            }
            finally
            {
                ForegroundColor = foreground;
            }
        }
        public void Write(ConsoleColor color, string format, params object[] args)
        {
            lock(_locker)
            {
                _write(color, format, args);
            }
        }

        private void _writeLine(ConsoleColor color, string text)
        {
            var foreground = ForegroundColor;
            try
            {
                ForegroundColor = color;
                WriteLine(text);
            }
            finally
            {
                ForegroundColor = foreground;
            }
        }

        public void WriteLine(ConsoleColor color, string text)
        {
            lock (_locker)
            {
                _writeLine(color, text);
            }
        }

        public void WriteLine(Colors colors, string text)
        {
            var _colors = Colors;
            try
            {
                Colors = colors;
                WriteLine(text);
            }
            finally
            {
                Colors = _colors;
            }
        }

        public void Write(Colors colors, string text)
        {
            var _colors = Colors;
            try
            {
                Colors = colors;
                Write(text);
            }
            finally
            {
                Colors = _colors;
            }
        }

        public void Write(ConsoleColor color, string text)
        {
            var foreground = ForegroundColor;
            try
            {
                ForegroundColor = color;
                Write(text);
            }
            finally
            {
                ForegroundColor = foreground;
            }
        }

        public void WriteLine(string format, object arg0)
        {
            WriteLine(string.Format(format, arg0));
        }

        public void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        /// <summary>
        /// Write the text to the window in the {color} color, withouting resetting the window's current foreground colour. Optionally causes text to wrap, and if text moves beyond the end of the window causes the window to scroll.
        /// </summary>
        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            var foreground = ForegroundColor;
            try
            {
                ForegroundColor = color;
                WriteLine(format, args);
            }
            finally
            {
                ForegroundColor = foreground;
            }
        }


        private void _writeLine(string text)
        {
            if (!Scrolling && OverflowBottom)
            {
                return;
            }

            if (OverflowBottom)
            {
                _scrollDown();
                _write(text);
                Cursor = new XY(0, Cursor.Y + 1);
                return;
            }

            _write(text);
            Cursor = new XY(0, Cursor.Y + 1);
            if (OverflowBottom && Scrolling)
            {
                _scrollDown();
            }
        }
        public void WriteLine(string text)
        {
            lock(_locker)
            {
                _writeLine(text);
            }
        }

        private void __write(IConsole console, string text)
        {
            var overflow = "";
            while (overflow != null)
            {
                if (Clipping && Cursor.X >= WindowWidth) break;
                if (!_lines.ContainsKey(Cursor.Y)) return;
                var result = _lines[Cursor.Y].Write(ForegroundColor, BackgroundColor, Cursor.X, text);
                overflow = result.Overflow;
                if (_echo && _console != null)
                {
                    _console.ForegroundColor = ForegroundColor;
                    _console.BackgroundColor = BackgroundColor;
                    _console.PrintAt(CursorLeft + _x, CursorTop + _y, result.Written);
                }
                if (overflow == null)
                {
                    Cursor = Cursor.IncX(text.Length);
                }
                else
                {
                    if (Clipping)
                    {
                        Cursor = new XY(WindowWidth, Cursor.Y);
                        break;
                    }
                    Cursor = new XY(0, Cursor.Y + 1);
                    if (!Scrolling && OverflowBottom)
                        break;
                    if (OverflowBottom)
                        ScrollDown();
                }
                text = overflow;
            }

        }
        private void _write(string text)
        {
            if (!Scrolling && OverflowBottom) return;
            DoCommand(_console, () => __write(_console, text));
        }

    }
}
