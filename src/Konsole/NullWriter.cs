using System;

namespace Konsole
{
    public class NullWriter : IConsole
    {

        public NullWriter()
        {           
        }

        public void WriteLine(string text)
        {
        }

        public void WriteLine(string format, params object[] args)
        {
        }

        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
        }

        public void Write(string format, params object[] args)
        {
        }

        public void Write(string text)
        {

        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
        }


        public ConsoleState State
        {
            get 
            {  
                return new ConsoleState(
                    ForegroundColor,BackgroundColor, 
                    CursorTop, 
                    CursorLeft,
                    CursorVisible
                );
            }
            set
            {
                ForegroundColor = value.ForegroundColor;
                BackgroundColor = value.BackgroundColor;
                CursorTop = value.Top;
                CursorLeft = value.Left;
                CursorVisible = value.CursorVisible;
            }
        }

        public int AbsoluteX => 0;
        public int AbsoluteY => 0;

        public int WindowWidth { get; set; } = 120;
        public int WindowHeight { get; set; } = 60;

        public int CursorLeft { get; set; } = 1;


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
        public int CursorTop { get; set; } = 1;

        public XY XY
        {
            get 
            { 
                return new XY(CursorLeft, CursorTop); 
            }

            set
            {
                CursorLeft = value.X;
                CursorTop = value.Y;
            }
        }
        
        public int Y
        {
            get { return CursorTop; } 
            set { CursorTop = value; }
        }

        public int X
        {
            get { return CursorLeft; } 
            set { CursorLeft= value; }
        }

        /// <summary>
        /// Run command and preserve the state, i.e. restore the console state after running command.
        /// </summary>
        public void DoCommand(IConsole console, Action action)
        {
            var state = State;
            try
            {
                action();
            }
            finally
            {
                State = state;
            }
        }

        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Gray;

        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
    

    public bool CursorVisible { get; set; } = true;

        public void PrintAt(int x, int y, string format, params object[] args)
        {
        }

        public void PrintAt(int x, int y, string text)
        {
        }
        public void PrintAt(int x, int y, char c)
        {
        }

        public void ScrollDown() {}

        public void Clear(){}

        public void Clear(ConsoleColor? background)
        {
            
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
        }


        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop,
            char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        { 
        }

        private void SetCursorPosition(int x, int y)
        {
        }

        public void Write(ConsoleColor color, string text)
        {
        }

        public void WriteLine(ConsoleColor color, string text)
        {
        }
    }
}