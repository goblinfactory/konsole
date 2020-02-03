using System;

namespace Konsole
{
    public struct Cell
    {
        public char Char;
        public ConsoleColor Foreground;
        public ConsoleColor Background;

        public Colors Colors
        {
            get
            {
                return new Colors(Foreground, Background);
            }
            set
            {
                Foreground = value.Foreground;
                Background = value.Background;
            }
        }

        public static Cell Default
        {
            get
            { return new Cell((char)0, 0, 0); }
        }

        public Cell(char c, ConsoleColor foreground, ConsoleColor background)
        {
            Char = c;
            Foreground = foreground;
            Background = background;
        }

        public Cell WithChar(char c)
        {
            return new Cell(c, Foreground, Background);
        }
        public Cell Clone()
        {
            return new Cell(Char, Foreground, Background);
        }
    }

}