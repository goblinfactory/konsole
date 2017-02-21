using System;

namespace Konsole.Internal
{
    public struct Cell
    {
        public char Char;
        public ConsoleColor Color;
        public ConsoleColor Background;

        public Cell(char c, ConsoleColor color, ConsoleColor background)
        {
            Char = c;
            Color = color;
            Background = background;
        }
        public Cell WithChar(char c, ConsoleColor color, ConsoleColor background)
        {
            return new Cell(c, color, background);
        }

        public Cell WithChar(char c)
        {
            return new Cell(c, Color, Background);
        }

    }
}