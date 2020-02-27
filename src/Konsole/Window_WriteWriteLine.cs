using System;

namespace Konsole
{
    public partial class Window
    {
        // **************************
        // **                      **
        // **  Write(string text)  **
        // **                      **
        // **************************
        public void Write(string text)
        {
            lock(_locker)
            {
                _write(text);
            }
        }
        private void _write(string text)
        {
            if (!Scrolling && OverflowBottom) return;
            DoCommand(_console, () => __write(_console, text));
        }

        // *******************************************
        // **                                       **
        // **  Write(string format, object args0)   **
        // **                                       **
        // *******************************************
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

        // ****************************************************
        // **                                                **
        // **  _write(string format, params object[] args)   **
        // **                                                **
        // ****************************************************
        public void Write(string format, params object[] args)
        {
            lock (_locker)
            {
                _write(format, args);
            }
        }

        private void _write(string format, params object[] args)
        {
            var text = string.Format(format, args);
            _write(text);
        }

        // ***********************************************************************
        // **                                                                   **
        // **  Write(ConsoleColor color, string format, params object[] args)   **
        // **                                                                   **
        // ***********************************************************************
        public void Write(ConsoleColor color, string format, params object[] args)
        {
            lock (_locker)
            {
                _write(color, format, args);
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

        // ***************************************************
        // **                                               **
        // **  WriteLine(ConsoleColor color, string text)   **
        // **                                               **
        // ***************************************************
        public void WriteLine(ConsoleColor color, string text)
        {
            lock (_locker)
            {
                _writeLine(color, text);
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

        // **********************************************
        // **                                          **
        // **  WriteLine(Colors colors, string text)   **
        // **                                          **
        // **********************************************
        public void WriteLine(Colors colors, string text)
        {
            lock(_locker)
            {
                _writeLine(colors, text);
            }
        }
        private void _writeLine(Colors colors, string text)
        {
            var _colors = Colors;
            try
            {
                Colors = colors;
                _writeLine(text);
            }
            finally
            {
                Colors = _colors;
            }
        }


        // ******************************************
        // **                                      **
        // **  Write(Colors colors, string text)   **
        // **                                      **
        // ******************************************
        public void Write(Colors colors, string text)
        {
            lock(_locker)
            {
                _write(colors, text);
            }
        }

        private void _write(Colors colors, string text)
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

        // ***********************************************
        // **                                           **
        // **  Write(ConsoleColor color, string text)   **
        // **                                           **
        // ***********************************************
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

        // ***********************************************
        // **                                           **
        // **  WriteLine(string format, object arg0)    **
        // **                                           **
        // ***********************************************
        public void WriteLine(string format, object arg0)
        {
            lock(_locker)
            {
                _writeLine(format, arg0);
            }
        }

        private void _writeLine(string format, object arg0)
        {
            _writeLine(string.Format(format, arg0));
        }

        // ******************************************************
        // **                                                  **
        // **  WriteLine(string format, params object[] args)  **
        // **                                                  **
        // ******************************************************
        public void WriteLine(string format, params object[] args)
        {
            lock(_locker)
            {
                _writeLine(format, args);
            }
        }

        private void _writeLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        // **************************************************************************
        // **                                                                      **
        // **  WriteLine(ConsoleColor color, string format, params object[] args)  **
        // **                                                                      **
        // **************************************************************************
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

        // ********************************
        // **                            **
        // **  WriteLine(string text)    **
        // **                            **
        // ********************************
        public void WriteLine(string text)
        {
            lock (_locker)
            {
                _writeLine(text);
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

        // ****************
        // **  __write   **
        // ****************
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
    }
}
