using System;

namespace Goblinfactory.Konsole.Mocks
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
        public Cell WithChar(char c)
        {
            return new Cell(c, Color, Background);
        }

        
    }
}