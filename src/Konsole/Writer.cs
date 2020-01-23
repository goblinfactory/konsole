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
                    Console.CursorLeft,
                    _isWindows ? Console.CursorVisible : false
                );
            }
            set
            {
                Console.ForegroundColor = value.ForegroundColor;
                Console.BackgroundColor = value.BackgroundColor;
                Console.CursorTop = value.Top;
                Console.CursorLeft = CheckWidth(value.Left);
                if(_isWindows)
                {
                    Console.CursorVisible = value.CursorVisible;
                }
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

        public XY XY
        {
            get { return new XY(Console.CursorLeft, Console.CursorTop); }

            set
            {
                Console.CursorLeft = CheckWidth(value.X);
                Console.CursorTop = value.Y;
            }
        }
        
        public int Y
        {
            get { return Console.CursorTop; } 
            set { Console.CursorTop = value; }
        }

        public int X
        {
            get { return Console.CursorLeft; } 
            set { Console.CursorLeft= CheckWidth(value); }
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

        public IConsole BottomHalf(string title = "bottom", WindowTheme border = null, WindowTheme window = null)
        {
            throw new NotImplementedException();
        }

        public IConsole TopHalf(WindowTheme theme = null)
        {
            throw new NotImplementedException();
        }

        public IConsole BottomHalf(WindowTheme theme = null)
        {
            throw new NotImplementedException();
        }

        public IConsole LeftHalf(WindowTheme theme = null)
        {
            throw new NotImplementedException();
        }

        public IConsole RightHalf(WindowTheme theme = null)
        {
            throw new NotImplementedException();
        }

        public IConsole TopHalf(string title = "top", WindowTheme border = null, WindowTheme window = null)
        {
            throw new NotImplementedException();
        }

        public void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background = null)
        {
            DoCommand(this, () =>
            {
                State = new ConsoleState(foreground, background ?? BackgroundColor, y, x, CursorVisible);
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
    }
}