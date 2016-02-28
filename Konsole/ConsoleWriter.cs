using System;
using Goblinfactory.Konsole;

namespace Goblinfactory.Konsole
{
    public class ConsoleWriter : IConsole
    {
        public void WriteLine(string format, params object[] args) { System.Console.WriteLine(format, args); }
        public void Write(string format, params object[] args) { System.Console.Write(format, args); }
        public int Y { get { return Console.CursorTop; } set { System.Console.CursorTop = value; } }
        public int X { get { return Console.CursorLeft; } set { System.Console.CursorLeft= value; } }
        public int WindowWidth() { return Console.WindowWidth; }
        public ConsoleColor ForegroundColor { get { return Console.ForegroundColor; } set { System.Console.ForegroundColor = value; } }
        public void PrintAt(int x, int y, string format, params object[] args)
        {
            Console.CursorTop = y;
            Console.CursorLeft = x;
            Console.WriteLine(format, args);
        }
        public void PrintAt(int x, int y, string text)
        {
            Console.CursorTop = y;
            Console.CursorLeft = x;
            Console.WriteLine(text);
        }

    }
}