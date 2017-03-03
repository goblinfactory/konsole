using System;
using System.Collections.Generic;
using System.Linq;
using Konsole.Internal;

namespace Konsole
{



    public class Window : IConsole
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _width;
        private readonly int _height;
        private readonly bool _echo;

        // Echo console is a default wrapper around the real Console, that we can swap out during testing. single underscore indicating it's not for general usage.
        private IConsole _echoConsole { get; set; }




        private readonly ConsoleColor _startForeground;
        private readonly ConsoleColor _startBackground;

        private readonly Dictionary<int, Row> _lines = new Dictionary<int, Row>();

        private XY _cursor;
        private int _lastLineWrittenTo = -1;
        private object _lock = new object();

        public Cell this[int x, int y]
        {
            get
            {
                int row = y > (_height - 1) ? (_height - 1) : y;
                int col = x > (_width - 1) ? (_width - 1) : x;
                return _lines[row].Cells[col];
            }
        }

        private XY Cursor
        {
            get { return _cursor; }
            set
            {
                _cursor = value;
                if (_cursor.Y > _lastLineWrittenTo && _cursor.X != 0) _lastLineWrittenTo = _cursor.Y;
                if (_cursor.Y > _lastLineWrittenTo && _cursor.X == 0) _lastLineWrittenTo = _cursor.Y - 1;
                //gotoCursor();
            }
        }

        private void gotoCursor()
        {
            if (_echo)
            {
                // since this is a window, that's offset of x,y on parent, do the offset now
                _echoConsole.CursorTop = _cursor.Y + _y;
                _echoConsole.CursorLeft = _cursor.X + _x;
            }
        }

        // to avoid constructor hell, and really hard errors, try to ensure that there is really only 1 constructor and all other constructors defer to that constructor. Do not get constructor A --> calls B --> calls C

        public Window() : this(0, 0, -1, -1, ConsoleColor.White, ConsoleColor.Black, true, null)
        {
        }

        public Window(int width, int height, IConsole echo)
            : this(0, 0, width, height, ConsoleColor.White, ConsoleColor.Black, true, echo)
        {
        }

        public Window(bool echo = true, IConsole echoConsole = null)
            : this(0, 0, -1, -1, ConsoleColor.White, ConsoleColor.Black, echo, echoConsole)
        {
        }

        public Window(int width, int height, bool echo = true, IConsole echoConsole = null)
            : this(0, 0, width, height, ConsoleColor.White, ConsoleColor.Black, echo, echoConsole)
        {
        }

        public Window(int x, int y, int width, int height, bool echo = true, IConsole echoConsole = null)
            : this(x, y, width, height, ConsoleColor.White, ConsoleColor.Black, echo, echoConsole)
        {
        }

        public Window(int x, int y, int width, int height, ConsoleColor foreground, ConsoleColor background,
            bool echo = true, IConsole echoConsole = null)
        {
            _x = x;
            _y = y;
            _echo = echo;
            _echoConsole = echoConsole;
            if (_echo && _echoConsole == null) _echoConsole = new Writer();
            _width = width == -1 ? (_echoConsole?.WindowWidth() ?? 120) : width;
            _height = height == -1 ? (_echoConsole?.WindowHeight() ?? 80) : height;
            _startForeground = foreground;
            _startBackground = background;

            init();
        }

        private void init()
        {
            ForegroundColor = _startForeground;
            BackgroundColor = _startBackground;
            Cursor = new XY(0, 0);
            _lastLineWrittenTo = -1;
            _lines.Clear();
            for (int i = 0; i < _height; i++) _lines.Add(i, new Row(_width, ' ', _startForeground, _startBackground));
        }

        /// <summary>
        /// use this method to return an 'approve-able' text buffer representing the background color of the buffer
        /// </summary>
        /// <param name="highliteColor">the background color to look for that indicates that text has been hilighted</param>
        /// <param name="hiChar">the char to use to indicate a highlight</param>
        /// <param name="normal">the chart to use for all other</param>
        /// <returns></returns>
        public string[] BufferHighlighted(ConsoleColor highliteColor, char hiChar = '#', char normal = ' ')
        {
            var buffer = new HiliteBuffer(highliteColor, hiChar, normal);
            var rows = _lines.Select(l => l.Value).ToArray();
            var texts = buffer.ToApprovableText(rows);
            return texts;
        }

        public string BufferHighlightedString(ConsoleColor highliteColor, char hiChar = '#', char normal = ' ')
        {
            var buffer = new HiliteBuffer(highliteColor, hiChar, normal);
            var rows = _lines.Select(l => l.Value).ToArray();
            var text = buffer.ToApprovableString(rows);
            return text;
        }

        /// <summary>
        /// returns the buffer with additional 2 characters representing the background color and foreground color
        /// colors rendered using the `ColorMapper.cs`
        /// </summary>
        /// <returns></returns>
        public string[] BufferWithColor
        {
            get
            {
                var buffer = _lines.Select(l => l.Value.ToStringWithColorChars());
                return buffer.ToArray();
            }
        }

        private string ColorString(Row row)
        {
            var chars = row.Cells.SelectMany(r => r.Value.ToChars()).ToArray();
            return new string(chars);
        }


        /// <summary>
        /// get the entire buffer (all the lines for the whole mock console) regardless of whether they have been written to or not, untrimmed.
        /// </summary>
        public string[] Buffer => _lines.Values.Take(_height).Select(b => b.ToString()).ToArray();

        /// <summary>
        /// get the entire buffer (all the lines for the whole mock console) regardless of whether they have been written to or not, untrimmed. as a single `crln` concatenated string.
        /// </summary>
        public string BufferString => string.Join("\r\n", Buffer);

        /// <summary>
        /// get all the lines written to for the whole mock console, untrimmed
        /// </summary>
        public string[] BufferWritten // should be buffer written
        {
            get { return _lines.Values.Take(_lastLineWrittenTo + 1).Select(b => b.ToString()).ToArray(); }
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
                return
                    _lines.Values.Take(_lastLineWrittenTo + 1).Select(b => b.ToString().TrimEnd(new[] {' '})).ToArray();
            }
        }



        public void WriteLine(string format, params object[] args)
        {
            SafeWrite(_echoConsole, () =>
            {
                gotoCursor();
                var text = string.Format(format, args);
                string overflow = "";
                var result = _lines[Cursor.Y].WriteToRowBufferReturnWrittenAndOverflow(ForegroundColor, BackgroundColor, Cursor.X, text);
                overflow = result.Overflow;

                if (_echo) _echoConsole.WriteLine(result.Written);

                Cursor = new XY(0, Cursor.Y < _height ? Cursor.Y + 1 : _height);
                if (overflow != null) WriteLine(overflow);
            });
        }

        public void Write(string format, params object[] args)
        {
            var text = string.Format(format, args);
            Write(text);
        }


        public void Clear()
        {
            if (_echo) _echoConsole.Clear();
            init();
        }


        public void Write(string text)
        {
            var overflow = "";
            // don't automatically expand buffer, for now, user should know what to expect, this is normally an error if you go beyond the expected length.
            while (overflow != null)
            {
                if (!_lines.ContainsKey(Cursor.Y)) throw new ArgumentOutOfRangeException("Reached the bottom of your console window. (Y) Value. Please extend the size of your console buffer. Requested line number was:" + Cursor.Y);
                var result = _lines[Cursor.Y].WriteToRowBufferReturnWrittenAndOverflow(ForegroundColor, BackgroundColor, Cursor.X, text);
                overflow = result.Overflow;

                var xinc = overflow?.Length ?? 0;
                if (overflow == null)
                {
                    Cursor = Cursor.IncX(text.Length);
                }
                else
                {
                    Cursor = new XY(0, Cursor.Y + 1);
                    //Write(overflow);
                    //overflow = null;
                }

                text = overflow;
            }
        }


        public int WindowHeight()
        {
            return _height;
        }

        public int CursorTop
        {
            get { return Cursor.Y; }
            set { Cursor = Cursor.WithY(value); }
        }

        public XY XY { get; set; }

        public int CursorLeft
        {
            get { return Cursor.X; }
            set { Cursor = Cursor.WithX(value); }
        }

        public int WindowWidth()
        {
            return _width;
        }

        public ConsoleColor BackgroundColor { get; set; }

        public ConsoleColor ForegroundColor { get; set; }

        

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


        private void SafeWrite(IConsole parent, Action action)
        {
            if (parent == null)
            {
                action();
                return;
            }
            var state = parent.GetState();
            try
            {
                parent.ForegroundColor = ForegroundColor;
                parent.BackgroundColor = BackgroundColor;
                action();
            }
            finally
            {
                parent.RestoreState(state);
            }
        }
    }
}
