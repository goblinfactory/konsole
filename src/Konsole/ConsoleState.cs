using System;

namespace Konsole
{
    public class ConsoleState
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        // for now, not including any width or height settings. (don't know if these can be changed? none of our code does so leaving off.)
        public ConsoleState(ConsoleColor foreground, ConsoleColor background, int top, int left) // NB always X then Y .. need to swap these around
        {
            ForegroundColor = foreground;
            BackgroundColor = background;
            Top = top;
            Left = left;
        }
    }
}