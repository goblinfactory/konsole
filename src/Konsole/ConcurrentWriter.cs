using System;


namespace Konsole
{
    public static class ConcurrentWriterExtensions
    {
        public static ConcurrentWriter Concurrent(this Window window)
        {
            return new ConcurrentWriter(window);
        }
    }

    public class ConcurrentWriter  : IConsole
    {
        public static object _locker = new object();

        private readonly IConsole _window;

        public ConcurrentWriter(IConsole window)
        {
            _window = window;
        }

        /// <summary>
        /// returns a thread safe concurrent writer that writes to the current console as if it were the actual Console. Keeps the existing operating system CursorTop position. Use this class in conjunction with inline windows for maximum simplicity.
        /// </summary>
        /// <remarks>
        /// Previously the concurrentwriter required a window instance and would only write concurrently to a new Window. That still exists but you can now create a `ConcurrentWriter` without needing to first create a window. This allows for thread safe writing to the console without needing a window. See the new section in the readme under `Threading` for more information and for examples.
        /// </remarks>
        public ConcurrentWriter()
        {
            _window = new Writer();
        }

        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            lock (_locker)
            {
                _window.WriteLine(color,format, args);
            }
        }

        public void WriteLine(string text)
        {
            lock (_locker)
            {
                _window.WriteLine(text);
            }
        }

        public void WriteLine(string format, params object[] args)
        {
            lock (_locker)
            {
                _window.WriteLine(format,args);
            }
        }

        public void WriteLine(ConsoleColor color, string text)
        {
            lock (_locker)
            {
                _window.WriteLine(color, text);
            }
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            lock (_locker)
            {
                _window.Write(color,format,args);
            }
        }

        public void Write(string format, params object[] args)
        {
            lock (_locker)
            {
                _window.Write(format,args);
            }
        }

        public void Write(string text)
        {
            lock (_locker)
            {
                _window.Write(text);
            }
        }

        public void Write(ConsoleColor color, string text)
        {
            lock (_locker)
            {
                _window.Write(color, text);
            }
        }

        public ConsoleState State
        {
            get
            {
                lock (_locker)
                {
                    return _window.State;
                }
            }
            set
            {
                lock (_locker)
                {
                    _window.State = value;
                }
            }
        }

        public int AbsoluteX => _window.AbsoluteX;
        public int AbsoluteY => _window.AbsoluteY;

        public int WindowWidth
        {
            get
            {
                lock (_locker) return _window.WindowWidth;
            }
        }

        public int WindowHeight
        {
            get
            {
                lock (_locker) return _window.WindowHeight;
            }
        }

        public int CursorTop
        {
            get
            {
                lock (_locker) return _window.CursorTop;
            }
            set
            {
                lock (_locker) _window.CursorTop = value;
            }
        }

        public int CursorLeft
        {
            get
            {
                lock (_locker) return _window.CursorLeft;
            }
            set
            {
                lock (_locker) _window.CursorLeft = value;
            }
        }

        public Colors Colors
        {
            get
            {
                lock (_locker) return _window.Colors;
            }
            set
            {
                lock (_locker) _window.Colors = value;
            }
        }

        public void DoCommand(IConsole console, Action action)
        {
            lock (_locker)
            {
                _window.DoCommand(console, action);
            }
        }

        public ConsoleColor ForegroundColor {
            get
            {
                lock (_locker) return _window.ForegroundColor;
            }
            set
            {
                lock (_locker) _window.ForegroundColor = value;
            }
        }

        public ConsoleColor BackgroundColor {
            get
            {
                lock (_locker) return _window.BackgroundColor;
            }
            set
            {
                lock (_locker) _window.BackgroundColor = value;
            }
        }

        public bool CursorVisible
        {
            get
            {
                lock (_locker) return _window.CursorVisible;
            }
            set
            {
                lock (_locker) _window.CursorVisible = value;
            }
        }

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            lock (_locker)
            {
                _window.PrintAt(x,y,format,args);
            }
        }

        public void PrintAt(int x, int y, string text)
        {
            lock (_locker)
            {
                _window.PrintAt(x, y, text);
            }
        }

        public void PrintAt(int x, int y, char c)
        {
            lock (_locker)
            {
                _window.PrintAt(x, y, c);
            }

        }

        public void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background)
        {
            lock (_locker)
            {
                _window.PrintAtColor(foreground,x,y,text,background);
            }
        }

        public void ScrollDown()
        {
            lock (_locker)
            {
                _window.ScrollDown();
            }
        }

        public void Clear()
        {
            Clear(null);
        }

        public void Clear(ConsoleColor? backgroundColor)
        {
            lock (_locker)
            {
                _window.Clear(backgroundColor);
            }

        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop,
            char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            lock (_locker)
            {
                _window.MoveBufferArea(sourceLeft,sourceTop,sourceWidth,sourceHeight,targetLeft,targetTop,sourceChar,sourceForeColor,sourceBackColor);
            }
        }
    }
}
