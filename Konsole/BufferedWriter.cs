using System;
using System.Collections.Generic;
using System.Linq;
using Konsole.Internal;

namespace Konsole
{



    public class BufferedWriter : IConsole
    {
        private readonly int _width;
        private readonly int _height;
        private readonly bool _echo;
        private ConsoleColor _color;
        private ConsoleColor _background;
        private Dictionary<int, Row> _lines;
        private XY _cursor;
        private int _lastLineWrittenTo;

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

        // is there a way to detect current console buffer and settings?
        public BufferedWriter(bool echo = false) : this(System.Console.WindowWidth, System.Console.WindowHeight, echo) { }
        public BufferedWriter(int width, int height, bool echo = false) : this(width, height, ConsoleColor.White, ConsoleColor.Black, echo) { }

        public BufferedWriter(int width, int height, ConsoleColor color, ConsoleColor background, bool echo = false)
        {
            _width = width;
            _height = height;
            _echo = echo;
            _color = color;
            _background = background;
            Cursor = new XY(0, 0);
            _lastLineWrittenTo = 0;
            _lines = new Dictionary<int, Row>();
            for (int i = 0; i < height; i++) _lines.Add(i, new Row(width,' ', color, background));
        }

        /// <summary>
        /// use this method to return an 'approve-able' buffer representing the background color of the buffer
        /// </summary>
        /// <param name="highliteColor">the background color to look for that indicates that text has been hilighted</param>
        /// <param name="hiChar">the char to use to indicate a highlight</param>
        /// <param name="normal">the chart to use for all other</param>
        /// <returns></returns>
        public string HilighterBuffer(ConsoleColor highliteColor, char hiChar = '#', char normal = ' ')
        {
            var buffer = new HiliteBuffer(highliteColor, hiChar, normal);
            var rows = _lines.Select(l => l.Value).ToArray();
            var text = buffer.ToApprovableText(rows);
            return text;
        }


        /// <summary>
        /// get the entire buffer (all the lines for the whole mock console) regardless of whether they have been written to or not, untrimmed.
        /// </summary>
        public string[] Buffer => _lines.Values.Take(_height).Select(b => b.ToString()).ToArray();

        /// <summary>
        /// get all the lines written to for the whole mock console, untrimmed
        /// </summary>
        public string[] BufferWritten // should be buffer written
        {
            get
            {
                return _lines.Values.Take(_lastLineWrittenTo + 1).Select(b => b.ToString()).ToArray();
            }
        }

        /// <summary>
        /// get all the lines written to for the whole mock console - bufferWrittenString
        /// </summary>
        public string BufferWrittenString => string.Join("\r\n", BufferWritten);


        /// <summary>
        /// get all the lines written to for the whole mock console, all trimmed.
        /// </summary>
        public string[] BufferWrittenTrimmed
        {
            get
            {
                return _lines.Values.Take(_lastLineWrittenTo+1).Select( b => b.ToString().TrimEnd(new []{' '})).ToArray();
            }
        }



        public void WriteLine(string format, params object[] args)
        {
            var text = string.Format(format, args);
            if (_echo) System.Console.WriteLine(text);
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

        public void Clear()
        {

            if (_echo) System.Console.Clear();
            _lines = new Dictionary<int, Row>();
            _lastLineWrittenTo = 0;
            XY = new XY(0, 0);
            _cursor.Y = 0;
        }


        public void Write(string text)
        {
            if (_echo) System.Console.Write(text);
            var overflow = "";
            // don't automatically expand buffer, tester should know what to expect, this is normally an error if you go beyond the expected length.
            if (!_lines.ContainsKey(Cursor.Y)) throw new ArgumentOutOfRangeException("Reached the bottom of your console window. (Y) Value. Please extend the size of your console buffer and re-run the test. Requested line number was:" + Cursor.Y);
            while (overflow != null)
            {
                overflow = _lines[Cursor.Y].WriteFormatted(_color,_background, Cursor.X, text);
                var xinc = overflow?.Length ?? 0;
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

        public XY XY { get; set; }

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
                if (_echo) System.Console.BackgroundColor = value;
                _background = value;
            }
        }

        public ConsoleColor ForegroundColor
        {
            get { return _color; }
            set
            {
                if (_echo) System.Console.ForegroundColor = value;
                _color = value;
            }
        }

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            Cursor = new XY(x,y);
            Write(format, args);
        }

        public void SetCursorPosition(int x, int y)
        {
            Cursor = new XY(x, y);
        }


        public void PrintAt(int x, int y, string text)
        {
            Cursor = new XY(x,y);
            Write(text);
        }

        public void PrintAt(int x, int y, char c)
        {
            Cursor = new XY(x,y);
            Write(c.ToString());
        }

    }
}
