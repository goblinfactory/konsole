using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole.Drawing;
using Konsole.Forms;
using Konsole.Menus;

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

        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            lock (_locker)
            {
                _window.WriteLine(color,format, args);
            }
        }

        public void WriteLine(string format, params object[] args)
        {
            lock (_locker)
            {
                _window.WriteLine(format,args);
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
            throw new NotSupportedException("Not supported in a multithreaded scenario.");
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

        public void ScrollUp()
        {
            lock (_locker)
            {
                _window.ScrollUp();
            }
        }

        public void Clear()
        {
            lock (_locker)
            {
                _window.Clear();
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
