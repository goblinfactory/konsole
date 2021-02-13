using System;
using System.Collections.Generic;
using System.Linq;
using Konsole.Internal;
using Konsole.Platform;

namespace Konsole
{
    public partial class Window : IConsole, IPeek
    {
        // *****************
        // ** HostConsole **
        // *****************
        internal static IConsole _hostConsole;
        public static IConsole HostConsole
        {
            get
            {
                lock (_locker) return _HostConsole;
            }
            set
            {
                lock (_locker)_HostConsole = value;
            }
        }

        internal static IConsole _HostConsole
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

        // *****************
        // ** GetVersion  **
        // *****************
        public string GetVersion()
        {
            return GetType().Assembly.GetName().Version.ToString();
        }

        // *********************
        // ** OverflowBottom  **
        // *********************
        public bool OverflowBottom
        {
            get
            {
                lock (_locker) return _OverflowBottom;
            }
        }

        private bool _OverflowBottom
        {
            get {
                return CursorTop >= WindowHeight;
            }
        }

            

        // these two fields made mutable to avoid overcomplicating the constructor overloads.
        // perhaps there's a simpler way to do this?
        private int _absoluteX;
        private int _absoluteY;

        private readonly int _x;
        private readonly int _y;
        private readonly bool _echo;

        /// <summary>
        /// Whether the window has any content (size, height and width) that is inside and visible
        /// of a parent window. e.g. entire window is outside the parent
        /// or either of the height or width are 0.
        /// </summary>
        private readonly bool _hasVisibleContent = true;

        // Echo console is a default wrapper around the real Console, that we can swap out during testing. single underscore indicating it's not for general usage.
        private IConsole _console { get; }

        internal static object _locker = new object();

        public bool Clipping { get; } = false;

        public bool Scrolling { get; } = true;
        public bool Transparent { get; } = false;
        

        protected readonly Dictionary<int, Row> _lines = new Dictionary<int, Row>();

        private XY _cursor;
        private int _lastLineWrittenTo = -1;



        public Cell this[int x, int y]
        {
            get
            {
                lock (_locker)
                {
                    int row = y > (WindowHeight - 1) ? (WindowHeight - 1) : y;
                    int col = x > (WindowWidth - 1) ? (WindowWidth - 1) : x;
                    return _lines[row].Cells[col];
                }
            }
        }

        private XY _Cursor
        {
            get { return _cursor; }
            set
            {
                {
                    int x = value.X >= WindowWidth ? (WindowWidth - 1) : value.X;
                    int y = value.Y > WindowHeight ? WindowHeight : value.Y;
                    _cursor = new XY(x, y);

                    if (_cursor.Y > _lastLineWrittenTo && _cursor.X != 0) _lastLineWrittenTo = _cursor.Y;
                    if (_cursor.Y > _lastLineWrittenTo && _cursor.X == 0) _lastLineWrittenTo = _cursor.Y - 1;
                }
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
            lock (_locker)
            {
                return _GetHostWidthHeight();
            }
        };

        private static Func<(int width, int height)> _GetHostWidthHeight = () =>
        {
            var con = _HostConsole;
            if (OS.IsOSX())
            {
                return (con.WindowWidth, con.WindowHeight - 1);
            }
            return (con.WindowWidth, con.WindowHeight);
        };






        //internal static IConsole _CreateFloatingWindow(int? x, int? y, int? width, int? height, ConsoleColor foreground, ConsoleColor background, bool echo = true, IConsole echoConsole = null, params K[] options)
        //{
        //    var theme = new Style(foreground, background).ToTheme();
        //}

        internal static IConsole _CreateFloatingWindow(IConsole console, WindowSettings settings)
        {
            var w = new Window(console, settings);
            w.SetWindowOffset(settings.SX, settings.SY ?? 0);
            return w;
        }

        internal static IConsole _CreateFloatingWindow(WindowSettings settings)
        {
            return _CreateFloatingWindow(null, settings);
        }

        private StyleTheme _theme = null;

        public StyleTheme Theme
        {
            get
            {
                lock (_locker) return _theme ?? (_theme = _console.Theme);
            }
            set
            {
                lock (_locker) _theme = value;
            }
        }

        public Style Style
        {
            get
            {
                lock (_locker) return Theme.GetActive(Status);
            }
        }

        public ControlStatus Status { get; set; } = ControlStatus.Active;

        private void init()
        {
            if (!_hasVisibleContent) return;
            if (HasTitle)
            {
                new Draw(_console, Style, Drawing.MergeOrOverlap.Fast).Box(_x - 1, _y - 1, _x + WindowWidth, _y + WindowHeight, _title);
            }

            _lastLineWrittenTo = -1;
            _lines.Clear();
            for (int i = 0; i < WindowHeight; i++)
            {
                _lines.Add(i, new Row(WindowWidth, ' ', ForegroundColor, BackgroundColor));
                if (!Transparent) _PrintAt(0, i, new string(' ', WindowWidth));
            }
            _Cursor = new XY(0, 0);
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
            lock (_locker)
            {
                var buffer = new HiliteBuffer(highliteColor, hiChar, normal);
                var rows = _lines.Select(l => l.Value).ToArray();
                var texts = buffer.ToApprovableText(rows);
                return texts;
            }
        }

        public string BufferHighlightedString(ConsoleColor highliteColor, char hiChar = '#', char normal = ' ')
        {
            lock (_locker)
            {
                var buffer = new HiliteBuffer(highliteColor, hiChar, normal);
                var rows = _lines.Select(l => l.Value).ToArray();
                var text = buffer.ToApprovableString(rows);
                return text;
            }
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
                lock (_locker)
                {
                    var buffer = _lines.Select(l => l.Value.ToStringWithColorChars());
                    return buffer.ToArray();
                }
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
        public string[] Buffer
        {
            get
            {
                lock(_locker)
                    return _lines.Values.Take(WindowHeight).Select(b => b.ToString()).ToArray();
            }
    }
        /// <summary>
        /// get the entire buffer (all the lines for the whole console) regardless of whether they have been written to or not, untrimmed. as a single `crln` concatenated string.
        /// </summary>
        public string BufferString
        {
            get
            {
                lock(_locker)
                    return string.Join("\r\n", Buffer);
            }
        }

        /// <summary>
        /// get all the lines written to for the whole console, untrimmed
        /// </summary>
        public string[] BufferWritten // should be buffer written
        {
            get { lock (_locker) return _lines.Values.Take(_lastLineWrittenTo + 1).Select(b => b.ToString()).ToArray(); }
        }

        /// <summary>
        /// get all the lines written to for the whole console - bufferWrittenString
        /// </summary>
        public string BufferWrittenString
        {
            get
            {
                lock (_locker)
                    return string.Join("\r\n", BufferWritten);
            }
        }


        /// <summary>
        /// get all the lines written to for the whole console, all trimmed.
        /// </summary>
        public string[] BufferWrittenTrimmed
        {
            get
            {
                lock (_locker)
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
            lock (_locker)
            {
                if (background.HasValue) BackgroundColor = background.Value;
                init();
            }
        }

        public virtual void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop,
            char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            lock (_locker)
            {
                if (!_echo) return;
                if (_console != null)
                    _console.MoveBufferArea(sourceLeft + AbsoluteX, sourceTop + AbsoluteY, sourceWidth, sourceHeight, targetLeft + AbsoluteX, targetTop + AbsoluteY, sourceChar, sourceForeColor, sourceBackColor);

                else
                {
                    throw new Exception("Should never get here, something gone wrong in the logic, possibly in the constructor checks?");
                }
            }

        }

        // scroll the screen up 1 line, and pop the top line off the buffer
        //NB!Need to test if this is cross platform ?
        public void ScrollDown()
        {
            lock(_locker)
            {
                _scrollDown();
            }
        }

        private void _scrollDown()
        {
            if (!_hasVisibleContent)
            {
                return;
            }
            for (int i = 0; i < (WindowHeight - 1); i++)
            {
                _lines[i] = _lines[i + 1];
            }
            _lines[WindowHeight - 1] = new Row(WindowWidth, ' ', ForegroundColor, BackgroundColor);
            _Cursor = new XY(0, WindowHeight - 1);
            if (_console != null)
            {
                _console.MoveBufferArea(_x, _y + 1, WindowWidth, WindowHeight - 1, _x, _y, ' ', ForegroundColor, BackgroundColor);
                
            }

        }

        public int WindowHeight { get; }

        public int CursorTop
        {
            get { lock (_locker) return _CursorTop; }
            set { lock (_locker) _CursorTop = value; }
        }

        private int _CursorTop
        {
            get { return _Cursor.Y; }
            set { _Cursor = _Cursor.WithY(value); }
        }

        // ******************
        // **  CursorLeft  **
        // ******************

        public int CursorLeft
        {
            get { lock (_locker) return _CursorLeft;  }
            set { lock (_locker) _CursorLeft = value; }
        }

        private int _CursorLeft
        {
            get { return _Cursor.X; }
            set { _Cursor = _Cursor.WithX(value); }
        }

        // ***************
        // **  Colors   **
        // ***************
        public Colors Colors
        {
            get
            {
                lock (_locker) return _Colors;
            }
            set
            {
                lock (_locker)
                {
                    ForegroundColor = value.Foreground;
                    BackgroundColor = value.Background;
                }
            }
        }
        private Colors _Colors
        {
            get
            {
                return new Colors(ForegroundColor, BackgroundColor);
            }
            set
            {
                _Colors = value;
            }
        }


        internal void SetWindowOffset(int x, int y)
        {
            _absoluteX = x;
            _absoluteY = y;
        }

        public int AbsoluteY => _absoluteY;
        public int AbsoluteX => _absoluteX;
        public int WindowWidth { get; }

        // *********************
        // ** BackgroundColor **
        // *********************
        private ConsoleColor _backgroundColor;
        public ConsoleColor BackgroundColor
        {
            get { lock (_locker) return _backgroundColor; }
            set { lock (_locker) _backgroundColor = value; }
        }

        private bool _noEchoCursorVisible = true;

        // *********************
        // ** ForegroundColor **
        // *********************
        private ConsoleColor _foregroundColor;
        public ConsoleColor ForegroundColor { 
            get { lock (_locker) return _foregroundColor;  }
            set { lock (_locker) _foregroundColor = value; }
        }

        // ***********
        // ** State **
        // ***********
        public ConsoleState State
        {
            get
            {
                lock (_locker) return _State;
            }

            set
            {
                lock (_locker)
                {
                    _State = value;
                }
            }
        }

        private ConsoleState _State
        {
            get
            {
                return new ConsoleState(_foregroundColor, _backgroundColor, _CursorTop, _CursorLeft);
            }

            set
            {
                _CursorLeft = value.Left;
                _CursorTop = value.Top;
                _foregroundColor = value.ForegroundColor;
                _backgroundColor = value.BackgroundColor;
            }

        }

        // ****************************************************************
        // **                                                            **
        // ** PrintAt(int x, int y, string format, params object[] args) **
        // **                                                            **
        // ****************************************************************
        public void PrintAt(int x, int y, string format, params object[] args)
        {
            lock (_locker) _PrintAt(x, y, format, args);
        }

        private void _PrintAt(int x, int y, string format, params object[] args)
        {
            _WithState(() => {
                var text = string.Format(format, args);
                _Cursor = new XY(x, y);
                _Write(text);
            });
        }

        // ****************************************
        // **                                    **
        // ** PrintAt(int x, int y, string text) **
        // **                                    **
        // ****************************************
        public void PrintAt(int x, int y, string text)
        {
            lock (_locker) _PrintAt(x, y, text);
        }

        private void _PrintAt(int x, int y, string text)
        {
            _WithState(() =>
            {
                _Cursor = new XY(x, y);
                _Write(text);
            });
        }

        // ***********************************
        // **                               **
        // ** PrintAt(int x, int y, char c) **
        // **                               **
        // ***********************************
        public void PrintAt(int x, int y, char c)
        {
            lock (_locker) _PrintAt(x, y, c);
        }

        private void _PrintAt(int x, int y, char c)
        {
            _WithState(() =>
            {
                _Cursor = new XY(x, y);
                _Write(c.ToString());
            });
        }

        // *******************************************************************************
        // **                                                                           **
        // ** PrintAt(Colors colors, int x, int y, string format, params object[] args) **
        // **                                                                           **
        // *******************************************************************************
        public void PrintAt(Colors colors, int x, int y, string format, params object[] args)
        {
            DoCommand(this, () =>
            {
                _Cursor = new XY(x, y);
                Colors = colors;
                var text = string.Format(format, args);
                Write(text);
            });
        }

        public void PrintAt(ConsoleColor color, int x, int y, string format, params object[] args)
        {
            DoCommand(this, () =>
            {
                _Cursor = new XY(x, y);
                ForegroundColor = color;
                var text = string.Format(format, args);
                Write(text);
            });
        }

        public void PrintAt(Colors colors, int x, int y, string text)
        {
            DoCommand(this, () =>
            {
                _Cursor = new XY(x, y);
                Colors = colors;
                Write(text);
            });
        }

        public void PrintAt(ConsoleColor color, int x, int y, string text)
        {
            DoCommand(this, () =>
            {
                _Cursor = new XY(x, y);
                ForegroundColor = color;
                Write(text);
            });
        }



        public void PrintAt(Colors colors, int x, int y, char c)
        {
            DoCommand(this, () =>
            {
                Colors = colors;
                PrintAt(x, y, c);
            });
        }

        public void PrintAt(ConsoleColor color, int x, int y, char c)
        {
            DoCommand(this, () =>
            {
                ForegroundColor = color;
                PrintAt(x, y, c);
            });
        }

        // ************************************************
        // **                                            **
        // ** DoCommand(IConsole console, Action action) **
        // **                                            **
        // ************************************************
        /// <summary>
        /// Run command and preserve the state, i.e. restore the console state after running command.
        /// </summary>
        public void DoCommand(IConsole console, Action action)
        {
            lock(_locker)
            {
                _DoCommand(console, action);
            }
        }

        private void _DoCommand(IConsole console, Action action)
        {
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

        // ********************************
        // GotoEchoCursor(IConsole console) 
        // ********************************
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

        public Cell Peek(int sx, int sy)
        {
            lock(_locker)
            {
                if (sx > WindowWidth || sy > WindowHeight) return Cell.Default;
                return _lines[sy].Cells[sx].Clone();
            }
        }

        public Row[] Peek(ConsoleRegion region)
        {
            lock (_locker)
            {
                int height = region.EndY - region.StartY;
                if (height < 1 || region.StartY > WindowHeight) return new[] { new Row() };
                int width = region.EndX - region.StartX;
                var rows = Enumerable.Range(region.StartY, region.EndY)
                    .Select(y => Peek(region.StartX, region.StartY, width))
                    .ToArray();
                return rows;
            }
        }

        public Row Peek(int sx, int sy, int width)
        {
            lock (_locker)
            {
                int len = sx + width > WindowWidth ? width - sx : width;
                if (width < 1 || len < 1 || sx > WindowWidth) return new Row();
                // perfect canidate for span, but only if cells were immutable, which they are not!
                var cells = _lines[sy].Cells.Skip(sx).Take(len).Select(c => c.Value.Clone()).ToArray();
                var row = new Row(cells);
                return row;
            }
        }
    }
}
