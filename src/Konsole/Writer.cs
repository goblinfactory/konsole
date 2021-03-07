using System;
using System.Runtime.InteropServices;
using Konsole.Internal;

namespace Konsole
{
    public class Writer : IConsole
    {
        private bool _isWindows;

        public Writer()
        {
            _isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
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

        public void Write(string text)
        {
            Console.Write(text);
        }

        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            var foreground = ForegroundColor;
            try
            {
                ForegroundColor = color;
                Console.Write(format, args);
            }
            finally
            {
                ForegroundColor = foreground;
            }
        }

        public void WriteLine(Colors colors, string text)
        {
            var _colors = Colors;
            try
            {
                Colors = colors;
                Console.WriteLine(text);
            }
            finally
            {
                Colors = _colors;
            }
        }
        public void Write(Colors colors, string text)
        {
            var _colors = Colors;
            try
            {
                Colors = colors;
                Console.Write(text);
            }
            finally
            {
                Colors = _colors;
            }
        }

        public void Write(ConsoleColor color, string text)
        {
            var foreground = ForegroundColor;
            try
            {
                ForegroundColor = color;
                Console.Write(text);
            }
            finally
            {
                ForegroundColor = foreground;
            }
        }

        public void WriteLine(ConsoleColor color, string text)
        {
            var foreground = ForegroundColor;
            try
            {
                ForegroundColor = color;
                Console.WriteLine(text);
            }
            finally
            {
                ForegroundColor = foreground;
            }

        }



        public ConsoleState State
        {
            get 
            {  
                return new ConsoleState(
                    Console.ForegroundColor,
                    Console.BackgroundColor, 
                    Console.CursorTop, 
                    Console.CursorLeft
                );
            }
            set
            {
                Console.ForegroundColor = value.ForegroundColor;
                Console.BackgroundColor = value.BackgroundColor;
                Console.CursorTop = value.Top;
                Console.CursorLeft = CheckWidth(value.Left);
            }
        }

        public int AbsoluteX => 0;
        public int AbsoluteY => 0;

        public int WindowWidth => Console.WindowWidth;
        public int WindowHeight => Console.WindowHeight;

        public int CursorLeft
        {
            get { return Console.CursorLeft; }
            set { Console.CursorLeft = CheckWidth(value); }
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
        public int CursorTop
        {
            get { return Console.CursorTop; }
            set { Console.CursorTop = value;  }
        }

        //public XY XY
        //{
        //    get { return new XY(Console.CursorLeft, Console.CursorTop); }

        //    set
        //    {
        //        Console.CursorLeft = CheckWidth(value.X);
        //        Console.CursorTop = value.Y;
        //    }
        //}
        
        //public int Y
        //{
        //    get { return Console.CursorTop; } 
        //    set { Console.CursorTop = value; }
        //}

        //public int X
        //{
        //    get { return Console.CursorLeft; } 
        //    set { Console.CursorLeft= CheckWidth(value); }
        //}

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
                action();
            }
            finally
            {
                console.State = state;
            }
        }

        public ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; } 
            set { Console.ForegroundColor = value; }
        }
            
        public ConsoleColor BackgroundColor {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        private bool _nonWinCursorVisible = false;
        public bool CursorVisible
        {
            get { return _isWindows ? Console.CursorVisible : _nonWinCursorVisible; }
            set {
                if ((_isWindows))
                {
                    Console.CursorVisible = value;
                }
                else
                    _nonWinCursorVisible = value;
            }
        }

        // method written in response to issue #28 https://github.com/goblinfactory/konsole/issues/28 (Crash after window resize)

        //// commented changed below to see if I can stop the jitter...does mean issue above is still a problem...
        //// maybe handle using try catch, and only if there's an exception we check the width! ooohhh
        //private static int? _consoleWindowWidth = null;
        //private static int? _consoleBufferWidth = null;

        //private static int CheckWidth(int x)
        //{
        //    //return  x.Min(Console.WindowWidth, Console.BufferWidth);
        //    return x.Min(
        //        _consoleWindowWidth ?? (int)(_consoleWindowWidth = Console.WindowWidth),
        //        _consoleBufferWidth ?? (int)(_consoleBufferWidth = Console.BufferWidth)
        //    );
        //}

        private static int CheckWidth(int x)
        {
            return x.Min(Console.WindowWidth, Console.BufferWidth);
        }

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            SetCursorPosition(CheckWidth(x), y);
            Console.WriteLine(format, args);            
        }

        public void PrintAt(int x, int y, string text)
        {
            SetCursorPosition(CheckWidth(x), y);
            Console.Write(text);            
        }
        public void PrintAt(int x, int y, char c)
        {
            if (x >= Console.WindowWidth || x>=Console.BufferWidth) return;
            SetCursorPosition(CheckWidth(x), y);
            Console.Write(c);
        }

        public void PrintAt(Colors colors, int x, int y, char c)
        {
            var _colors = Colors;
            try
            {
                Colors = colors;
                PrintAt(x, y, c);
            }
            finally
            {
                Colors = _colors;
            }
        }

        public void PrintAt(ConsoleColor color, int x, int y, char c)
        {
            var _colors = Colors;
            try
            {
                ForegroundColor = color;
                PrintAt(x, y, c);
            }
            finally
            {
                Colors = _colors;
            }
        }

        public void PrintAt(Colors colors, int x, int y, string text)
        {
            var _colors = Colors;
            try
            {
                Colors = colors;
                PrintAt(x, y, text);
            }
            finally
            {
                Colors = _colors;
            }
        }

        public void PrintAt(ConsoleColor color, int x, int y, string text)
        {
            var _colors = Colors;
            try
            {
                ForegroundColor = color;
                PrintAt(x, y, text);
            }
            finally
            {
                Colors = _colors;
            }
        }

        public void PrintAt(Colors colors, int x, int y, string format, params object[] args)
        {
            var _colors = Colors;
            try
            {
                Colors = colors;
                PrintAt(x, y, format, args);
            }
            finally
            {
                Colors = _colors;
            }
        }

        public void PrintAt(ConsoleColor color, int x, int y, string format, params object[] args)
        {
            var _colors = Colors;
            try
            {
                ForegroundColor = color;
                PrintAt(x, y, format, args);
            }
            finally
            {
                Colors = _colors;
            }
        }

        public void ScrollDown()
        {
            // do nothing?? mmm.
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void Clear(ConsoleColor? background)
        {
            Console.Clear();
        }

        public void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background = null)
        {
            DoCommand(this, () =>
            {
                State = new ConsoleState(foreground, background ?? BackgroundColor, y, x);
                Write(text);
            });
        }


        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop,
            char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar, sourceForeColor, sourceBackColor);
        }

        private void SetCursorPosition(int x, int y)
        {
            try
            {
                Console.SetCursorPosition(CheckWidth(x), y);
            }
            catch (ArgumentOutOfRangeException) { }
        }

        public Style Style
        {
            get
            {
                return Theme.GetActive(Status);
            }
        }

        public StyleTheme Theme { get; set; } = StyleTheme.Default;

        public ControlStatus Status { get; set; } = ControlStatus.Active;
        public int? TabOrder { get; set; }
        public bool Enabled { get; set; }
        public string Title { get; set; }

        public IConsoleManager Manager => throw new NotImplementedException();

        public Guid Id { get; } = Guid.NewGuid();

        public IConsoleApplication Parent => null;

        public bool Clipping { get; set; } = false;
        public bool Scrolling { get; set; } = false;
    }
}