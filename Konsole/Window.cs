using System;

namespace Konsole
{

    public class Window : IConsole
    {
        private readonly BufferedWriter _console;
        private readonly IConsole _parent;
        private readonly int _x;
        private readonly int _y;

        public Window(int x, int y, int width, int height) : this(new Writer(), x, y, width, height)
        {
        }

        public Window(IConsole parent, int x, int y, int width, int height)
        {
            _parent = parent;
            _x = x;
            _y = y;
            _console = new BufferedWriter(width, height);
        }

        /// <summary>
        /// prints the state of the current buffer to parent. This is so that we can cater for the windows own overflow settings.
        /// I did consider simply passing the prints to the parent via an offset, i.e. all WRiteLine to convert to PrintAt but then overflows and wrapping would not work.
        /// </summary>
        private void Refresh()
        {
            // locking?
            var fore = _parent.ForegroundColor;
            var back = _parent.BackgroundColor;
            try
            {
                int y = 0;
                foreach (var line in _console.BufferWritten)
                {
                    // quick hack to set color for a whole line, need to update for each individual cell later
                    _parent.ForegroundColor = _console[0, y].Foreground;
                    _parent.BackgroundColor = _console[0, y].Background;
                    _parent.PrintAt(_x, _y + y, line);
                    y++;
                }

            }
            finally
            {
                _parent.ForegroundColor = fore;
                _parent.BackgroundColor = back;
            }
        }

        public void WriteLine(string format, params object[] args)
        {
            _console.WriteLine(format,args);
            Refresh();
        }

        public void Write(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public int WindowWidth()
        {
            throw new NotImplementedException();
        }

        public int CursorTop {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int CursorLeft
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ConsoleColor ForegroundColor {
            get { return _console.ForegroundColor; }
            set { _console.ForegroundColor = value; }
        }

        public ConsoleColor BackgroundColor
        {
            get { return _console.BackgroundColor; }
            set { _console.BackgroundColor = value; }
        }

    public void SetCursorPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, string text)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, char c)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            // mmm, if echo is on, then this would clear the whole screen instead of just this window? will need to 
            throw new NotImplementedException();
        }
    }
}
