using System;
using System.Collections.Generic;

using System.Linq;
using System.Runtime.InteropServices;
using Konsole.Drawing;
using Konsole.Internal;

namespace Konsole
{



    public class Window : IConsole
    {
        public bool OverflowBottom => CursorTop >= _height;

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

        // to avoid constructor hell, and really hard errors, try to ensure that there is really only 1 constructor and all other constructors defer to that constructor. Do not get constructor A --> calls B --> calls C

        public Window() : this(0, 0, (int?) null, (int?) null, ConsoleColor.White, ConsoleColor.Black, true, null)
        {
        }

        public Window(IConsole console, int width, int height, params K[] options)
            : this(0, 0, width, height, ConsoleColor.White, ConsoleColor.Black, true, console, options)
        {
        }

        public Window(int width, int height, params K[] options)
            : this(0, 0, width, height, ConsoleColor.White, ConsoleColor.Black, true, null, options)
        {
        }

        public Window(IConsole console, int width, int height, ConsoleColor foreground, ConsoleColor background,
            params K[] options)
            : this(0, 0, width, height, foreground, background, true, console, options)
        {
        }

        public Window(int width, int height, ConsoleColor foreground, ConsoleColor background)
            : this(0, 0, width, height, foreground, background, true, null)
        {
        }

        public Window(int width, int height, ConsoleColor foreground, ConsoleColor background, params K[] options)
            : this(0, 0, width, height, foreground, background, true, null, options)
        {
        }

        public Window(IConsole echoConsole, int x, int y, int width, int height, ConsoleColor foreground,
            ConsoleColor background)
            : this(x, y, width, height, foreground, background, true, echoConsole)
        {
        }

        public Window(IConsole echoConsole, int x, int y, int width, int height)
            : this(x, y, width, height, ConsoleColor.White, ConsoleColor.Black, true, echoConsole)
        {
        }

        public Window(IConsole echoConsole, int width, int height)
            : this(0, 0, width, height, ConsoleColor.White, ConsoleColor.Black, true, echoConsole)
        {
        }

        //Window will clear the parent console area in the overlapping window.
        // this constructor is safe to have params after IConsole because it's the only constructor that starts with IConsole, all other constructors have other strongly typed first parameter. (i.e. avoid parameter confusion)
        public Window(IConsole echoConsole, params K[] options)
            : this(0, 0, (int?) (null), (int?) null, ConsoleColor.White, ConsoleColor.Black, true, echoConsole, options)
        {
        }

        public Window(int x, int y, int width, int height, IConsole echoConsole = null, params K[] options)
            : this(x, y, width, height, ConsoleColor.White, ConsoleColor.Black, true, echoConsole, options)
        {
        }

        protected Window(int x, int y, int width, int height, bool echo = true, IConsole echoConsole = null,
            params K[] options)
            : this(x, y, width, height, ConsoleColor.White, ConsoleColor.Black, echo, echoConsole, options)
        {
        }

        public static Window Open(int x, int y, int width, int height, string title,
            LineThickNess thickNess = LineThickNess.Double, ConsoleColor foregroundColor = ConsoleColor.Gray,
            ConsoleColor backgroundColor = ConsoleColor.Black, IConsole console = null)
        {
            var echoConsole = console ?? new Writer();
            var window = new Window(x + 1, y + 1, width - 2, height - 2, foregroundColor, backgroundColor, true,
                echoConsole);
            var state = echoConsole.State;
            try
            {
                echoConsole.ForegroundColor = foregroundColor;
                echoConsole.BackgroundColor = backgroundColor;
                new Draw(echoConsole).Box(x, y, x + (width - 1), y + (height - 1), title, LineThickNess.Double);
            }
            finally
            {
                echoConsole.State = state;
            }
            return window;
        }

        public ProgressBar ProgressBar(int max)
        {
            return new ProgressBar(max, this);
        }

        public Window(int x, int y, int width, int height, ConsoleColor foreground, ConsoleColor background,
            IConsole echoConsole, params K[] options)
            : this(x, y, width, height, foreground, background, true, echoConsole, options)
        {

        }

        public Window(int x, int y, int width, int height, ConsoleColor foreground, ConsoleColor background,
            params K[] options) : this(x, y, width, height, foreground, background, true, null, options)
        {

        }

        protected Window(int x, int y, int? width, int? height, ConsoleColor foreground, ConsoleColor background,
            bool echo = true, IConsole echoConsole = null, params K[] options)
        {
            _x = x;
            _y = y;
            _echo = echo;
            _echoConsole = echoConsole;
            // move set width to external static method
            if (_echo && _echoConsole == null) _echoConsole = new Writer();
            _width = GetStartWidth(echo, width, x, echoConsole);
            _height = GetStartHeight(echo, height, y, echoConsole);
            _startForeground = foreground;
            _startBackground = background;

            SetOptions(options);
            init();
        }

        private static int GetStartHeight(bool echo, int? height, int y, IConsole echoConsole)
        {
            //int echoHeight = echoConsole?.WindowHeight ?? 80;
            //int maxHeight = (echoHeight - y);
            return height ?? (echoConsole?.WindowHeight ?? 80);
            //return (height ?? 0) > maxHeight ? maxHeight : height;
        }

        private static int GetStartWidth(bool echo, int? width, int x, IConsole echoConsole)
        {
            // if echo is false, then this is a mock console and the width is never capped

            int echoWidth = echoConsole?.WindowWidth ?? 120;
            int maxWidth = (echoWidth - x);
            int w = width ?? (echoConsole?.WindowWidth ?? 120);
            if (echo && w > maxWidth) w = maxWidth;
            return w;

            //if (width == null) return maxWidth;
            //return width.Value <= maxWidth ? width.Value : maxWidth;
        }

        private void SetOptions(K[] options)
        {
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

        private void init()
        {
            ForegroundColor = _startForeground;
            BackgroundColor = _startBackground;
            _lastLineWrittenTo = -1;
            _lines.Clear();
            for (int i = 0; i < _height; i++)
            {
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


        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            var foreground = ForegroundColor;
            try
            {
                ForegroundColor = color;
                WriteLine(format, args);
            }
            finally
            {
                ForegroundColor = foreground;
            }
        }

        public void WriteLine(string format, params object[] args)
        {
            if (!_clipping && OverflowBottom)
                ScrollUp();
            Write(format,args);
            Cursor = new XY(0, Cursor.Y+1);
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            var foreground = ForegroundColor;
            try
            {
                ForegroundColor = color;
                Write(format, args);
            }
            finally
            {
                ForegroundColor = foreground;
            }
;
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

        public virtual void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop,
            char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            if (!_echo) return;
            if (_echoConsole!=null)
                _echoConsole.MoveBufferArea(sourceLeft,sourceTop,sourceWidth,sourceHeight,targetLeft,targetTop,sourceChar,sourceForeColor,sourceBackColor);
                
            else
            {
                throw new ApplicationException("Should never get here, something gone wrong in the logic, possibly in the constructor checks?");
            }

        }

        public void Write(string text)
        {
            _write(text);
        }

        // scroll the screen up 1 line, and pop the top line off the buffer
        //NB!Need to test if this is cross platform ?
        public void ScrollUp()
        {
            for (int i = 0; i < (_height-1); i++)
            {
                _lines[i] = _lines[i+1];
            }
            _lines[_height-1] = new Row(_width, ' ', ForegroundColor, BackgroundColor);
            Cursor = new XY(0, _height-1);
            if (_echoConsole != null)
            {
                _echoConsole.MoveBufferArea(_x, _y + 1, _width, _height - 1, _x, _y, ' ', ForegroundColor, BackgroundColor);
            }
        }



        //TODO: convert everything to redirect all calls to PrintAt, so that writing to parent works flawlessly!
        private void _write(string text)
        {
            if (_clipping && OverflowBottom)
                return;
            DoCommand(_echoConsole, () =>
            {
                var overflow = "";
                while (overflow != null)
                {
                    if (!_lines.ContainsKey(Cursor.Y)) return;
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
                        if(OverflowBottom)
                            ScrollUp();
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


        public int WindowWidth
        {
            get { return _width; }
        }

        public ConsoleColor BackgroundColor { get; set; }

        private bool _noEchoCursorVisible = true;

        public bool CursorVisible
        {
            get { return _echoConsole?.CursorVisible ?? _noEchoCursorVisible; }
            set
            {
                if(_echoConsole==null)
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
            Cursor = new XY(x,y);
            Write(c.ToString());
        }


        /// <summary>
        /// Run command and preserve the state, i.e. restore the console state after running command.
        /// </summary>
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
