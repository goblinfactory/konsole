using System;
using Konsole.Drawing;
using Konsole.Internal;
using Konsole.Menus;

namespace Konsole
{
    public interface IConsole : IWrite
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
        void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background);
        void ScrollUp();
        void Clear();

        void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft,
            int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor);
    }
}