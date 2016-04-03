using System;

namespace Konsole
{
    public class ConsoleWriter : IConsole
    {
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public int WindowWidth()
        {
            return Console.WindowWidth;
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
        
        public void SetCursorPosition(int x, int y)
        {
            Console.SetCursorPosition(x, y);
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

        public void Clear()
        {
            Console.Clear();
        }
    }
}