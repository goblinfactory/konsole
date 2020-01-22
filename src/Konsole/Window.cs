using System;
using System.Collections.Generic;
using System.Linq;
using Konsole.Internal;

namespace Konsole
{
    public partial class Window : IConsole
    {
        internal static IConsole _hostConsole;
        public static IConsole HostConsole
        {
            get
            {
                return _hostConsole ?? (_hostConsole = new ConcurrentWriter());
            }
            set
            {
                _hostConsole = value;
            }
        }

        public string GetVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }
        public bool OverflowBottom => CursorTop >= _height;

        // these two fields made mutable to avoid overcomplicating the constructor overloads.
        // perhaps there's a simpler way to do this?
        private int _absoluteX;
        private int _absoluteY;

        private readonly int _x;
        private readonly int _y;
        private readonly int _width;
        private readonly int _height;
        private readonly bool _echo;

        // Echo console is a default wrapper around the real Console, that we can swap out during testing. single underscore indicating it's not for general usage.
        private IConsole _echoConsole { get; set; }


        private bool _transparent = false;

        public bool Clipping
        {
            get { return _clipping; }
        }

        private bool _clipping = false;

        public bool Scrolling
        {
            get { return _scrolling; }
        }

        private bool _scrolling = true;

        public bool Transparent
        {
            get { return _transparent; }
        }

        private readonly ConsoleColor _startForeground;
        private readonly ConsoleColor _startBackground;

        protected readonly Dictionary<int, Row> _lines = new Dictionary<int, Row>();

        private XY _cursor;
        private int _lastLineWrittenTo = -1;



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
                int x = value.X >= _width ? (_width - 1) : value.X;
                int y = value.Y > _height ? _height : value.Y;
                _cursor = new XY(x, y);

                if (_cursor.Y > _lastLineWrittenTo && _cursor.X != 0) _lastLineWrittenTo = _cursor.Y;
                if (_cursor.Y > _lastLineWrittenTo && _cursor.X == 0) _lastLineWrittenTo = _cursor.Y - 1;
            }
        }

        ///<summary>
        ///Override (replace) this func if you want to use `new Window()` in a unit test and you're not using a mockConsole (as the host) 
        /// in your unit test that will provide the height and width. I had to add this because when running tests on non build 
        /// server where the build agent does not give you an open console handle, and accessing console.width and height throws invalid handle exception
        /// so needs to be overridden.
        ///</summary>
        ///<returns>
        /// the height and width of the operating system. For OSX windows we actually return height - 1 to avoid writing to the bottom line
        /// in a console window that will cause the window to scroll, regardless of printing (for now, this is a hack but works well and allows us to 
        /// safely draw boxes around the rest of the "whole" window.
        ///</returns>
        public static Func<(int width, int height)> GetHostWidthHeight = () =>
        {
            if (OS.IsOSX())
            {
                return (Console.WindowWidth, Console.WindowHeight - 1);
            }
            return (Console.WindowWidth, Console.WindowHeight);
        };

        internal static object _staticLocker = new object();

        /// <summary>
        /// Opens a new Threadsafe window consisting the whole screen
        /// </summary>
        public static IConsole Open()
        {
            return new Window().Concurrent();
        }

        /// <summary>
        /// Opens a new Threadsafe window which as a child of parent.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IConsole Open(IConsole parent)
        {
            return new Window(parent).Concurrent();
        }

        [Obsolete("Please use OpenBox. This will be removed in the next major release.")]
        /// <summary>
        /// Returns a Threadsafe ConcurrentWriter around the newly created window.
        /// </summary>
        public static IConsole Open(int x, int y, int width, int height, string title,
        LineThickNess thickNess = LineThickNess.Double, ConsoleColor foregroundColor = ConsoleColor.Gray,
        ConsoleColor backgroundColor = ConsoleColor.Black, IConsole console = null)
        {
            lock (_staticLocker)
            {
                var echoConsole = console ?? new Writer();
                var window = new Window(x + 1, y + 1, width - 2, height - 2, foregroundColor, backgroundColor, true,
                    echoConsole);
                var state = echoConsole.State;
                try
                {
                    echoConsole.ForegroundColor = foregroundColor;
                    echoConsole.BackgroundColor = backgroundColor;
                    new Draw(echoConsole).Box(x, y, x + (width - 1), y + (height - 1), title, thickNess);

                }
                finally
                {
                    echoConsole.State = state;
                }
                return window.Concurrent();
            }
        }

        protected Window(int x, int y, int width, int height, bool echo = true, IConsole echoConsole = null,
    params K[] options)
    : this(x, y, width, height, ConsoleColor.White, ConsoleColor.Black, echo, echoConsole, options)
        {
        }

        internal static IConsole _CreateFloatingWindow(int? x, int? y, int? width, int? height, ConsoleColor foreground,
            ConsoleColor background,
            bool echo = true, IConsole echoConsole = null, params K[] options)
        {
            lock (_staticLocker)
            {
                var w = new Window(x, y, width, height, foreground, background, echo, echoConsole, options);
                w.SetWindowOffset(x ?? 0, y ?? 0);
                return w.Concurrent();
            }
        }

        // This is the main constructor, all the others overload to this.
        protected internal Window(int? x, int? y, int? width, int? height, ConsoleColor foreground, ConsoleColor background,
            bool echo = true, IConsole echoConsole = null, params K[] options)
        {
            lock (_staticLocker)
            {
                _echo = echo;
                _echoConsole = echoConsole ?? Window.HostConsole;
                if (_echo && _echoConsole == null) _echoConsole = new Writer();

                _y = y ?? _echoConsole?.CursorTop ?? _echoConsole.CursorTop + height ?? 0;
                _x = x ?? 0;
                _absoluteX = echoConsole?.AbsoluteX ?? 0 + _x;
                _absoluteY = echoConsole?.AbsoluteY ?? 0 + _y;
                _width = GetStartWidth(_echo, width, _x, echoConsole);
                _height = GetStartHeight(height, _y, echoConsole);
                _startForeground = foreground;
                _startBackground = background;

                SetOptions(options);
                init();
                // if we're creating an inline window
                var inline = (_echoConsole != null && x == null && y == null);
                if (inline)
                {
                    _echoConsole.CursorTop += _height;
                    _echoConsole.CursorLeft = 0;
                }
            }
        }

        private static int GetStartHeight(int? height, int y, IConsole echoConsole)
        {
            return height ?? (echoConsole?.WindowHeight ?? y);
        }

        private static int GetStartWidth(bool echo, int? width, int x, IConsole echoConsole)
        {
            if (width != null) return width.Value;
            // if echo is false, then this is a mock console and the width is never capped

            // should_clip_child_window_to_not_exceed_parent_boundaries

            int echoWidth = echoConsole?.WindowWidth ?? x;
            int maxWidth = (echoWidth - x);
            int w = width ?? (echoConsole?.WindowWidth ?? 120);
            if (echo && w > maxWidth) w = maxWidth;
            return w;
        }

        private void SetOptions(K[] options)
        {
            if (options == null || options.Length == 0) return;
            if (options.Contains(K.Transparent)) _transparent = true;
            if (options.Contains(K.Clipping) && options.Contains(K.Scrolling))
                throw new ArgumentOutOfRangeException(nameof(options),
                    "Cannot specify Clipping as well as Scrolling; pick 1, or leave both out. Clipping is default.");
            if (options.Contains(K.Clipping))
            {
                _clipping = true;
                _scrolling = false;
            }

            if (options.Contains(K.Scrolling))
            {
                _scrolling = true;
                _clipping = false;
            }
        }

        private void init(ConsoleColor? background = null)
        {
            ForegroundColor = _startForeground;
            BackgroundColor = background ?? _startBackground;
            _lastLineWrittenTo = -1;
            _lines.Clear();
            for (int i = 0; i < _height; i++)
            {
                // TODO optimise this #performance Wrapping every call in a setState restore state is very very inneficient.
                _lines.Add(i, new Row(_width, ' ', ForegroundColor, BackgroundColor));
                if (!_transparent) PrintAt(0, i, new string(' ', _width));
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
        /// get the entire buffer (all the lines for the whole console) regardless of whether they have been written to or not, untrimmed.
        /// </summary>
        public string[] Buffer => _lines.Values.Take(_height).Select(b => b.ToString()).ToArray();

        /// <summary>
        /// get the entire buffer (all the lines for the whole console) regardless of whether they have been written to or not, untrimmed. as a single `crln` concatenated string.
        /// </summary>
        public string BufferString => string.Join("\r\n", Buffer);

        /// <summary>
        /// get all the lines written to for the whole console, untrimmed
        /// </summary>
        public string[] BufferWritten // should be buffer written
        {
            get { return _lines.Values.Take(_lastLineWrittenTo + 1).Select(b => b.ToString()).ToArray(); }
        }

        /// <summary>
        /// get all the lines written to for the whole console - bufferWrittenString
        /// </summary>
        public string BufferWrittenString => string.Join("\r\n", BufferWritten);


        /// <summary>
        /// get all the lines written to for the whole console, all trimmed.
        /// </summary>
        public string[] BufferWrittenTrimmed
        {
            get
            {
                return
                    _lines.Values.Take(_lastLineWrittenTo + 1).Select(b => b.ToString().TrimEnd(new[] { ' ' })).ToArray();
            }
        }

        public void Clear()
        {
            Clear(null);
        }

        public void Clear(ConsoleColor? background)
        {
            init(background);
        }

        public virtual void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop,
            char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            if (!_echo) return;
            if (_echoConsole != null)
                _echoConsole.MoveBufferArea(sourceLeft + AbsoluteX, sourceTop + AbsoluteY, sourceWidth, sourceHeight, targetLeft + AbsoluteX, targetTop + AbsoluteY, sourceChar, sourceForeColor, sourceBackColor);

            else
            {
                throw new Exception("Should never get here, something gone wrong in the logic, possibly in the constructor checks?");
            }

        }

        // scroll the screen up 1 line, and pop the top line off the buffer
        //NB!Need to test if this is cross platform ?
        public void ScrollDown()
        {
            for (int i = 0; i < (_height - 1); i++)
            {
                _lines[i] = _lines[i + 1];
            }
            _lines[_height - 1] = new Row(_width, ' ', ForegroundColor, BackgroundColor);
            Cursor = new XY(0, _height - 1);
            if (_echoConsole != null)
            {
                _echoConsole.MoveBufferArea(_x, _y + 1, _width, _height - 1, _x, _y, ' ', ForegroundColor, BackgroundColor);
            }
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

        public Colors Colors
        {
            get
            {
                return new Colors(ForegroundColor, BackgroundColor);
            }
            set
            {
                ForegroundColor = value.Foreground;
                BackgroundColor = value.Background;
            }
        }

        internal void SetWindowOffset(int x, int y)
        {
            _absoluteX = x;
            _absoluteY = y;
        }

        public int AbsoluteY => _absoluteY;
        public int AbsoluteX => _absoluteX;
        public int WindowWidth => _width;

        public ConsoleColor BackgroundColor { get; set; }

        private bool _noEchoCursorVisible = true;

        public bool CursorVisible
        {
            get { return _echoConsole?.CursorVisible ?? _noEchoCursorVisible; }
            set
            {
                if (_echoConsole == null)
                    _noEchoCursorVisible = value;
                else
                    _echoConsole.CursorVisible = value;
            }
        }



        public ConsoleColor ForegroundColor { get; set; }


        /// <summary>
        /// prints text at x and y location, without affecting the current window or parent state
        /// </summary>
        public void PrintAt(int x, int y, string format, params object[] args)
        {
            var text = string.Format(format, args);
            PrintAt(x, y, text);
        }

        public void PrintAt(int x, int y, string text)
        {
            DoCommand(this, () =>
            {
                Cursor = new XY(x, y);
                Write(text);
            });

        }

        /// <summary>
        /// Print the text, optionally wrapping and causing any scrolling in the current window, at cursor position X,Y in foreground and background color without impacting the current window's cursor position or colours. This method is only threadsafe if you have created a window by using .ToConcurrent() after creating a new Window(), or the window was created using Window.Open(...) which returns a threadsafe window.
        /// </summary>
        public void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background = null)
        {
            DoCommand(_echoConsole, () =>
            {
                DoCommand(this, () =>
                {
                    State = new ConsoleState(foreground, background ?? BackgroundColor, y, x, CursorVisible);
                    Write(text);
                });
            });
        }


        public ConsoleState State
        {
            get
            {
                return new ConsoleState(ForegroundColor, BackgroundColor, CursorTop, CursorLeft, CursorVisible);
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
            Cursor = new XY(x, y);
            Write(c.ToString());
        }


        /// <summary>
        /// Run command and preserve the state, i.e. restore the console state after running command.
        /// </summary>
        public void DoCommand(IConsole console, Action action)
        {
            //TODO write test that proves we need to lock right here!
            //lock(_staticLocker)
            if (console == null)
            {
                action();
                return;
            }
            var state = console.State;
            try
            {
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
            console.CursorTop = _cursor.Y + _y;
            console.CursorLeft = (_cursor.X + _x);
        }

        public void Fill(ConsoleColor color, int sx, int sy, int width, int height)
        {
            DoCommand(this, () =>
            {
                ForegroundColor = color;
                var line = new String(' ', width);
                for (int y = sy; y < height; y++)
                {
                    PrintAt(sx, y, line);
                }
            });
        }

    }
}
