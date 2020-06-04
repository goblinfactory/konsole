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

        #region ON DARKBLUE
        public static Colors GrayOnDarkBlue
        {
            get
            {
                return new Colors(Gray, DarkBlue);
            }
        }
        public static Colors WhiteOnDarkBlue
        {
            get
            {
                return new Colors(White, DarkBlue);
            }
        }

        #endregion

        #region ON BLACK
        public static Colors YellowOnBlack
        {
            get
            {
                return new Colors(Yellow, Black);
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
        public static Colors GreenOnBlack
        {
            get
            {
                return new Colors(Green, Black);
            }
        }
        #endregion
        #region ON DARKRED
        public static Colors GrayOnDarkRed
        {
            get
            {
                return new Colors(Gray, DarkRed);
            }
        }
        #endregion

        #region ON DARK GREY
        public static Colors WhiteOnDarkGrey
        {
            get
            {
                return new Colors(White, DarkGray);
            }
        }

        public static Colors YellowOnDarkGrey
        {
            get
            {
                return new Colors(Yellow, DarkGray);
            }
        }
        #endregion region

        #region ON BLUE
        public static Colors YellowOnBlue
        {
            get
            {
                return new Colors(Yellow, Blue);
            }
        }

        public static Colors GrayOnBlue
        {
            get
            {
                return new Colors(Gray, Blue);
            }
        }


        public static Colors WhiteOnBlue
        {
            get
            {
                return new Colors(White, Blue);
            }
        }
        public static Colors CyanOnBlue
        {
            get
            {
                return new Colors(Cyan, Blue);
            }
        }
        #endregion

        #region ON GRAY
        public static Colors BlackOnGray
        {
            get
            {
                return new Colors(Black, Gray);
            }
        }
        public static Colors MagentaOnGray
        {
            get
            {
                return new Colors(Magenta, Gray);
            }
        }
        public static Colors DarkBlueOnGray
        {
            get
            {
                return new Colors(DarkBlue, Gray);
            }
        }

        #endregion

        #region ON WHITE
        public static Colors BlackOnWhite
        {
            get
            {
                return new Colors(Black, White);
            }
        }

        public static Colors RedOnWhite
        {
            get
            {
                return new Colors(Red, White);
            }
        }

        public static Colors BlueOnWhite
        {
            get
            {
                return new Colors(Blue, White);
            }
        }

        public static Colors DarkBlueOnWhite
        {
            get
            {
                return new Colors(DarkBlue, White);
            }
        }

        #endregion

        #region ON RED
        public static Colors WhiteOnRed
        {
            get
            {
                return new Colors(White, Red);
            }
        }
        #endregion

    }
}
