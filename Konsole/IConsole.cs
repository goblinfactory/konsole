using System;
using Konsole.Testing;

namespace Konsole
{
    public interface IConsole : IWriteLine
    {
        int WindowWidth();
        int CursorTop { get; set; }
        int CursorLeft { get; set; }
        ConsoleColor ForegroundColor { get; set; }
        void SetCursorPosition(int x, int y);
        void PrintAt(int x, int y, string format, params object[] args);
        void PrintAt(int x, int y, string text);
        void PrintAt(int x, int y, char c);
        void Clear();
    }
}