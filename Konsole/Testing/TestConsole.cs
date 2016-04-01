using System;
using System.Collections.Generic;
using System.Linq;

namespace Konsole.Testing
{

    public class TestConsole : IConsole
    {
        private readonly int _width;
        private readonly int _height;
        private readonly bool _echo;
        private ConsoleColor _color;
        private ConsoleColor _background;
        private Dictionary<int, Line> _lines;
        private XY _cursor;

        private XY Cursor
        {
            get { return _cursor;}
            set
            {
                _cursor = value;
                if (_cursor.Y > _lastLineWrittenTo && _cursor.X!=0) _lastLineWrittenTo = _cursor.Y;
                if (_cursor.Y > _lastLineWrittenTo && _cursor.X==0) _lastLineWrittenTo = _cursor.Y-1;
            }
        }
        private int _lastLineWrittenTo;

        public TestConsole() : this(80, 20, false) { }
        public TestConsole(int width, int height, bool echo = false) : this(width, height, ConsoleColor.White, ConsoleColor.Black, echo) { }

        public TestConsole(int width, int height, ConsoleColor color, ConsoleColor background, bool echo = false)
        {
            _width = width;
            _height = height;
            _echo = echo;
            _color = color;
            _background = background;
            Cursor = new XY(0, 0);
            _lastLineWrittenTo = 0;
            _lines = new Dictionary<int, Line>();
            for (int i = 0; i < height; i++) _lines.Add(i, new Line(width,' ', color, background));
        }

        public string Buffer
        {
            get { return string.Join("\r\n", LinesText); }
        }


        /// <summary>
        /// get all the lines written to for the whole mock console, all trimmed.
        /// </summary>
        public string[] TrimmedLines
        {
            get
            {
                return _lines.Values.Take(_lastLineWrittenTo+1).Select( b => b.ToString().TrimEnd(new []{' '})).ToArray();
            }
        }

        /// <summary>
        /// get all the lines written to for the whole mock console.
        /// </summary>
        public string[] LinesText
        {
            get
            {
                return _lines.Values.Take(_lastLineWrittenTo + 1).Select(b => b.ToString()).ToArray();
            }
        }


        public void WriteLine(string format, params object[] args)
        {
            var text = string.Format(format, args);
            if (_echo) Console.WriteLine(text);
            string overflow = "";
            overflow = _lines[Cursor.Y].WriteFormatted(_color, _background, Cursor.X, text);
            Cursor = new XY(0, Cursor.Y < _height ? Cursor.Y + 1 : _height);
            if (overflow != null) WriteLine(overflow);
        }

        public void Write(string format, params object[] args)
        {
            var text = string.Format(format, args);
            Write(text);
        }

        public void Write(string text)
        {
            if (_echo) Console.Write(text);
            var overflow = "";
            while (overflow != null)
            {
                overflow = _lines[Cursor.Y].WriteFormatted(_color,_background, Cursor.X, text);
                var xinc = overflow == null ? 0 : overflow.Length;
                if (overflow == null)
                {
                    Cursor = Cursor.IncX(text.Length);
                }
                else
                {
                    Cursor = new XY(0, Cursor.Y + 1);
                    Write(overflow);
                }
            }
        }

        public int CursorTop
        {
            get { return Y; }
            set { Y = value; }
        }

        public int Y
        {
            get { return Cursor.Y;  }
            set { Cursor = Cursor.WithY(value); }
        }

        public int CursorLeft
        {
            get { return X; }
            set { X = value; }
        }

        public int X
        {
            get { return Cursor.X;  }
            set { Cursor = Cursor.WithX(value); }
        }

        public int WindowWidth()
        {
            return _width;
        }

        public ConsoleColor BackgroundColor
        {
            get { return _background; }
            set
            {
                if (_echo) Console.BackgroundColor = value;
                _background = value;
            }
        }

        public ConsoleColor ForegroundColor
        {
            get { return _color; }
            set
            {
                if (_echo) Console.ForegroundColor = value;
                _color = value;
            }
        }

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            Cursor = new XY(x, y);
            Write(format, args);
        }

        public void PrintAt(int x, int y, string text)
        {
            Cursor = new XY(x, y);
            Write(text);
        }

        public void SetCursorPosition(int x, int y)
        {
            Cursor = new XY(x, y);
        }

    }
}
