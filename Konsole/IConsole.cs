using System;
using Konsole.Internal;

namespace Konsole
{

    public interface IConsole : IWriteLine
    {
        ConsoleState State { get; set; }
        int WindowWidth { get; }
        int WindowHeight { get; }
        int CursorTop { get; set; }
        int CursorLeft { get; set; }
        Colors Colors { get; set; }
        void DoCommand(IConsole console, Action action);
        ConsoleColor ForegroundColor { get; set; }
        ConsoleColor BackgroundColor { get; set; }
        bool CursorVisible { get; set; }
        void PrintAt(int x, int y, string format, params object[] args);
        void PrintAt(int x, int y, string text);
        void PrintAt(int x, int y, char c);
        void ScrollUp();
        void Clear();

        // saves the current cursor position, (left and top) as well as colors
        // runs the command, then restores the state.

        void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft,
            int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor);
    }
}