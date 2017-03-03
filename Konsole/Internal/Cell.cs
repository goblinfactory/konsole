using System;

namespace Konsole.Internal
{
    public struct Cell
    {
        public char Char;
        public ConsoleColor Foreground;
        public ConsoleColor Background;

        public Cell(char c, ConsoleColor foreground, ConsoleColor background)
        {
            Char = c;
            Foreground = foreground;
            Background = background;
        }
        public Cell WithChar(char c, ConsoleColor color, ConsoleColor background)
        {
            return new Cell(c, color, background);
        }

        public Cell WithChar(char c)
        {
            return new Cell(c, Foreground, Background);
        }

    }

}