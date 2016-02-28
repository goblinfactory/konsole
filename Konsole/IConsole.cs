using System;

namespace Goblinfactory.Konsole
{
    public interface IConsole
    {
        void WriteLine(string format, params object[] args);
        void Write(string format, params object[] args);
        int WindowWidth();
        int Y { get; set; }
        int X { get; set; }
        ConsoleColor ForegroundColor { get; set; }
        void PrintAt(int x, int y, string format, params object[] args);
        void PrintAt(int x, int y, string text);
    }
}