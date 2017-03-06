using System;
using System.Collections.Generic;
using System.Linq;
using Konsole.Drawing;
using Konsole.Internal;

namespace Konsole
{



    public class Window : IConsole
    {
        private bool OverflowBottom
        {
            get { return CursorTop > (_height - 1); }
        }

        private readonly int _x;
        private readonly int _y;
        private readonly int _width;
        private readonly int _height;
        private readonly bool _echo;
        private readonly K[] _options;

        // Echo console is a default wrapper around the real Console, that we can swap out during testing. single underscore indicating it's not for general usage.
        private IConsole _echoConsole { get; set; }


        private bool _transparent = false;
        private bool _clipping = true;
        private bool _scrolling = false;

        public bool Transparent {  get {  return _transparent; } }

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

        //Window will clear the parent console area in the overlapping window.
        // this constructor is safe to have params after IConsole because it's the only constructor that starts with IConsole, all other constructors have other strongly typed first parameter. (i.e. avoid parameter confusion)
        public Window(IConsole echoConsole, params K[] options)
            : this(0, 0, -1, -1, ConsoleColor.White, ConsoleColor.Black, true, echoConsole, options)
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

        public Window(int x, int y, int width, int height, bool echo = true, IConsole echoConsole = null, params K[] options)
            : this(x, y, width, height, ConsoleColor.White, ConsoleColor.Black, echo, echoConsole, options)
        {
        }

        public static Window Open(WindowSettings settings)
        {
            var newSettings = settings.Clone();
            // todo check size, and throw exception if too small!
            // todo check if window can exist inside parent!
            newSettings.X = settings.X + 1;
            newSettings.Y = settings.Y + 1;
            newSettings.Width = settings.Width - 2;
            newSettings.Height = settings.Height - 2;
            var window = new Window(newSettings);
            var state = window._echoConsole.State;
            try
            {
                var con = window._echoConsole;
                con.ForegroundColor = settings.ForegroundColor;
                con.BackgroundColor = settings.BackgroundColor;
                new Draw(window._echoConsole)
                    .Box(
                        settings.X,
                        settings.Y,
                        settings.X + window._width,
                        settings.Y + window._height
                    );
            }
            finally
            {
                window._echoConsole.State = state;
            }
            return window;
        }

        public static Window Open(int x, int y, int width, int height, LineThickNess thickNess = LineThickNess.Double, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            var window = new Window(x+1,y+1, width-2, height-2, foregroundColor,backgroundColor,true);
            var state = window._echoConsole.State;
            try
            {
                window._echoConsole.ForegroundColor = foregroundColor;
                window._echoConsole.BackgroundColor = backgroundColor;
                new Draw(window._echoConsole).Box(x,y,x + window._width, y + window._height);
            }
            finally
            {
                window._echoConsole.State = state;
            }
            return window;
        }

        public ProgressBar ProgressBar(int max)
        {
            return new ProgressBar(max, this);
        }

        public Window(WindowSettings settings)
        {
            _x = settings.X;
            _y = settings.Y;
            _echo = settings.Echo;
            _echoConsole = settings.EchoConsole;
            _transparent = settings.Transparent;
            if (_echo && _echoConsole == null) _echoConsole = new Writer();
            _width = settings.Width ?? (_echoConsole?.WindowWidth() ?? 120);
            _height = settings.Height ?? (_echoConsole?.WindowHeight ?? 80);
            _startForeground = settings.ForegroundColor;
            _startBackground = settings.BackgroundColor;
            init();
        }

        public Window(int x, int y, int width, int height, ConsoleColor foreground, ConsoleColor background,bool echo = true, IConsole echoConsole = null, params K[] options)
        {
            _x = x;
            _y = y;
            _echo = echo;
            _options = options;
            _echoConsole = echoConsole;
            if (_echo && _echoConsole == null) _echoConsole = new Writer();
            _width = width == -1 ? (_echoConsole?.WindowWidth() ?? 120) : width;
            _height = height == -1 ? (_echoConsole?.WindowHeight ?? 80) : height;
            _startForeground = foreground;
            _startBackground = background;

            // todo : refactor : setting options, move out to seperate class and-or method
            if (options.Contains(K.Transparent)) _transparent = true;
            if (options.Contains(K.Clipping) && options.Contains(K.Scrolling)) throw new ArgumentOutOfRangeException(nameof(options),"Cannot specify Clipping as well as Scrolling; pick 1, or leave both out. Clipping is default.");
                if (options.Contains(K.Clipping))
            {
                _clipping = true;
                _scrolling = false;
            }

            if (options.Contains(K.Scrolling))
            {
                _scrolling = false;
                _clipping = true;
            }

            init();
        }

        private void init()
        {
            ForegroundColor = _startForeground;
            BackgroundColor = _startBackground;
            _lastLineWrittenTo = -1;
            _lines.Clear();
            for (int i = 0; i < _height; i++)
            {
                _lines.Add(i, new Row(_width, ' ', ForegroundColor, BackgroundColor));
                if(!_transparent) PrintAt(0, i, new string(' ', _width));
            }
            Cursor = new XY(0, 0);
            _lastLineWrittenTo = -1;
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
            if (OverflowBottom) return;
            // we want to reset the state of parent, but not this object
            DoCommand(_echoConsole, () =>
            {
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
            init();
        }

        public void Write(string text)
        {
            _write(text);
        }


        //TODO: convert everything to redirect all calls to PrintAt, so that writing to parent works flawlessly!
        private void _write(string text)
        {
            if (_clipping && OverflowBottom) return;
            DoCommand(_echoConsole, () =>
            {
                var overflow = "";
                // don't automatically expand buffer, for now, user should know what to expect, this is normally an error if you go beyond the expected length.
                while (overflow != null)
                {
                    if (!_lines.ContainsKey(Cursor.Y))
                        throw new ArgumentOutOfRangeException("Reached the bottom of your console window. (Y) Value. Please extend the size of your console buffer. Requested line number was:" + Cursor.Y);
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
                    }
                    text = overflow;
                }
            });
        }


        public int WindowHeight
        {
            get
            {
                return _height;
            }
        }

        public int CursorTop
        {
            get { return Cursor.Y; }
            set { Cursor = Cursor.WithY(value); }
        }

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

        
        /// <summary>
        /// prints text at x and y location, without affecting the current window or parent state
        /// </summary>
        public void PrintAt(int x, int y, string format, params object[] args)
        {
            var text = string.Format(format, args);
            PrintAt(x,y,text);
        }

        public void PrintAt(int x, int y, string text)
        {
            DoCommand(this, () =>
            {
                Cursor = new XY(x, y);
                Write(text);
            });

        }

        public void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background = null)
        {
            DoCommand(_echoConsole, () =>
            {
                DoCommand(this, () =>
                {
                    State = new ConsoleState(foreground, background ?? BackgroundColor, y, x);
                    Write(text);
                });
            });
        }


        public ConsoleState State
        {
            get
            {
                return new ConsoleState(ForegroundColor, BackgroundColor, CursorTop, CursorLeft);
            }

            set
            {
                CursorLeft = value.Left;
                CursorTop = value.Top;
                ForegroundColor = value.ForegroundColor;
                BackgroundColor = value.BackgroundColor;
            }

        }




        public void PrintAt(int x, int y, char c)
        {
            Cursor = new XY(x,y);
            Write(c.ToString());
        }



        public void DoCommand(IConsole console, Action action)
        {
            if (console == null)
            {
                action();
                return;
            }
            var state = console.State;
            try
            {
                console.ForegroundColor = ForegroundColor;
                console.BackgroundColor = BackgroundColor;

                GotoEchoCursor(console); 
                action();
            }
            finally
            {
                console.State = state;
            }
        }

        private void GotoEchoCursor(IConsole console)
        {
            // since this is a window, that's offset of x,y on parent, do the offset now
            console.CursorTop = _cursor.Y + _y;
            console.CursorLeft = _cursor.X + _x;
        }

    }
}
