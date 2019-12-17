using System;

namespace Konsole
{
    public class Colors
    {
        public ConsoleColor Foreground { get; } = ConsoleColor.White;
        public ConsoleColor Background { get; } = ConsoleColor.Black;

        public Colors()
        {
            
        }

        public Colors(ConsoleColor foreground, ConsoleColor background)
        {
            Foreground = foreground;
            Background = background;
        }



        public static Colors WhiteOnBlack
        {
            get
            {
                return new Colors(ConsoleColor.Gray, ConsoleColor.Black);
            }
        }

        public static Colors BlackOnWhite
        {
            get
            {
                return new Colors(ConsoleColor.Black, ConsoleColor.Gray);
            }
        }
    }
}
