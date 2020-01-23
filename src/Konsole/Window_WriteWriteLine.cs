using System;

namespace Konsole
{
    public partial class Window
    {
        public void Write(string text)
        {
            _write(text);
        }

        public void Write(string format, object args0)
        {
            var text = string.Format(format, args0);
            Write(text);
        }

        public void Write(string format, params object[] args)
        {
            var text = string.Format(format, args);
            Write(text);
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            var foreground = ForegroundColor;
            try
            {
                ForegroundColor = color;
                Write(format, args);
            }
            finally
            {
                ForegroundColor = foreground;
            }
        }

        public void WriteLine(ConsoleColor color, string text)
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


        public void WriteLine(string text)
        {
            if (_clipping && OverflowBottom)
            {
                return;
            }

            if (OverflowBottom)
            {
                ScrollDown();
                Write(text);
                Cursor = new XY(0, Cursor.Y + 1);
                return;
            }

            Write(text);
            Cursor = new XY(0, Cursor.Y + 1);
            if (OverflowBottom && !_clipping)
            {
                ScrollDown();
            }
        }


        //TODO: convert everything to redirect all calls to PrintAt, so that writing to parent works flawlessly!
        private void _write(string text)
        {
            if (_clipping && OverflowBottom)
                return;
            DoCommand(_echoConsole, () =>
            {
                var overflow = "";
                while (overflow != null)
                {
                    if (!_lines.ContainsKey(Cursor.Y)) return;
                    var result = _lines[Cursor.Y].WriteToRowBufferReturnWrittenAndOverflow(ForegroundColor, BackgroundColor, Cursor.X, text);
                    overflow = result.Overflow;
                    if (_echo && _echoConsole != null)
                    {
                        _echoConsole.ForegroundColor = ForegroundColor;
                        _echoConsole.BackgroundColor = BackgroundColor;
                        _echoConsole.PrintAt(CursorLeft + _x, CursorTop + _y, result.Written);
                    }
                    if (overflow == null)
                    {
                        Cursor = Cursor.IncX(text.Length);
                    }
                    else
                    {
                        Cursor = new XY(0, Cursor.Y + 1);
                        if (_clipping && OverflowBottom) break;
                        if (OverflowBottom)
                            ScrollDown();
                    }
                    text = overflow;
                }
            });
        }

    }
}
