using System;

namespace Konsole
{
    // see if I can get rid of all usages of this class
    // require all usages to go through Console
    // will dramatically simplify a new user learning about what 
    // this library does!

    public class Writer : IConsole
    {
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public ConsoleState State
        {
            get {  return new ConsoleState(Console.ForegroundColor,Console.BackgroundColor, Console.CursorTop, Console.CursorLeft);}
            set
            {
                Console.ForegroundColor = value.ForegroundColor;
                Console.BackgroundColor = value.BackgroundColor;
                Console.CursorTop = value.Top;
                Console.CursorLeft = value.Left;
            }
        }

        public int WindowWidth()
        {
            return Console.WindowWidth;
        }

        public int WindowHeight
        {
            get
            {
                return Console.WindowHeight;
            }
        }

        public int CursorLeft
        {
            get { return Console.CursorLeft; }
            set { Console.CursorLeft = value;  }
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
                Console.CursorLeft = value.X;
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
            set { Console.CursorLeft= value; }
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

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(format, args);            
        }

        public void PrintAt(int x, int y, string text)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(text);            
        }
        public void PrintAt(int x, int y, char c)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(c);
        }

        public void ScrollUp()
        {
            // do nothing
        }

        public void Clear()
        {
            Console.Clear();
        }


        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop,
            char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            Console.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar, sourceForeColor, sourceBackColor);
        }

    }
}