using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole
{
    public static class ConcurrentWriterExtensions
    {
        public static ConcurrentWriter Concurrent(this Window window)
        {
            return new ConcurrentWriter(window);
        }
    }

    public class ConcurrentWriter 
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

        public void Clear()
        {
            lock (_locker)
            {
                _window.Clear();
            }

        }

    }
}
