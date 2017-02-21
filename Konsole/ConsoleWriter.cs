using System;

namespace Konsole
{
    // see if I can get rid of all usages of this class
    // require all usages to go through Console
    // will dramatically simplify a new user learning about what 
    // this library does!

    public class ConsoleWriter : IConsole
    {
        public void WriteLine(string format, params object[] args)
        {
            System.Console.WriteLine(format, args);
        }

        public void Write(string format, params object[] args)
        {
            System.Console.Write(format, args);
        }

        public int WindowWidth()
        {
            return System.Console.WindowWidth;
        }
        
        public int CursorLeft
        {
            get { return System.Console.CursorLeft; }
            set { System.Console.CursorLeft = value;  }
        }

        public int CursorTop
        {
            get { return System.Console.CursorTop; }
            set { System.Console.CursorTop = value;  }
        }

        public XY XY
        {
            get { return new XY(System.Console.CursorLeft, System.Console.CursorTop); }

            set
            {
                System.Console.CursorLeft = value.X;
                System.Console.CursorTop = value.Y;
            }
        }
        
        public int Y
        {
            get { return System.Console.CursorTop; } 
            set { System.Console.CursorTop = value; }
        }

        public int X
        {
            get { return System.Console.CursorLeft; } 
            set { System.Console.CursorLeft= value; }
        }

        public ConsoleColor ForegroundColor
        {
            get { return System.Console.ForegroundColor; } 
            set { System.Console.ForegroundColor = value; }
        }
        
        public void SetCursorPosition(int x, int y)
        {
            System.Console.SetCursorPosition(x, y);
        }

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            System.Console.SetCursorPosition(x, y);
            System.Console.WriteLine(format, args);            
        }

        public void PrintAt(int x, int y, string text)
        {
            System.Console.SetCursorPosition(x, y);
            System.Console.WriteLine(text);            
        }
        public void PrintAt(int x, int y, char c)
        {
            System.Console.SetCursorPosition(x, y);
            System.Console.Write(c);
        }

        public void Clear()
        {
            System.Console.Clear();
        }
    }
}