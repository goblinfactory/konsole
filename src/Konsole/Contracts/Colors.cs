using System;
using static System.ConsoleColor;

namespace Konsole
{
    public class Colors
    {
        public ConsoleColor Foreground { get; } = White;
        public ConsoleColor Background { get; } = Black;

        public Colors()
        {
            
        }

        public Colors ToSelectedItem()
        {
            return new Colors(Background, Background.ToSelectedItemBackground());
        }

        public Colors(ConsoleColor foreground, ConsoleColor background)
        {
            Foreground = foreground;
            Background = background;
        }

        public static Colors GrayOnDarkBlue
        {
            get
            {
                return new Colors(Gray, DarkBlue);
            }
        }

        public static Colors GrayOnBlack
        {
            get
            {
                return new Colors(Gray, Black);
            }
        }

        public static Colors WhiteOnBlack
        {
            get
            {
                return new Colors(White, Black);
            }
        }

        public static Colors BlackOnWhite
        {
            get
            {
                return new Colors(Black, White);
            }
        }

        public static Colors WhiteOnBlue
        {
            get
            {
                return new Colors(White, Blue);
            }
        }

        public static Colors WhiteOnDarkBlue
        {
            get
            {
                return new Colors(White, DarkBlue);
            }
        }

        public static Colors BlueOnWhite
        {
            get
            {
                return new Colors(Blue, White);
            }
        }

        public static Colors DarkBlueOnGray
        {
            get
            {
                return new Colors(DarkBlue, Gray);
            }
        }

        public static Colors DarkBlueOnWhite
        {
            get
            {
                return new Colors(DarkBlue, White);
            }
        }
    }
}
