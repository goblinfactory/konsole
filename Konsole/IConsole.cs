using System;

namespace Konsole
{
    public interface IConsole
    {
        void WriteLine(string format, params object[] args);
        void Write(string format, params object[] args);
        int WindowWidth();
        int CursorTop { get; set; }
        int CursorLeft { get; set; }
        int X { get; set; }
        int Y { get; set; }
        ConsoleColor ForegroundColor { get; set; }
        void SetCursorPosition(int x, int y);
        void PrintAt(int x, int y, string format, params object[] args);
        void PrintAt(int x, int y, string text);
    }
}