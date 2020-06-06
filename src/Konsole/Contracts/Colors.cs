using System;
using System.Collections.Generic;
using static System.ConsoleColor;

namespace Konsole
{
    internal static class RandPicker
    {
        private static Random rnd = new Random();
        public static ConsoleColor PickColor(this int[] colors)
        {
            return (ConsoleColor)colors[rnd.Next(colors.Length)];
        }
    }

    public class Colors
    {
        public ConsoleColor Foreground { get; } = White;
        public ConsoleColor Background { get; } = Black;

        public Colors()
        {
            
        }
        private static Random rnd = new Random();

        public static Dictionary<ConsoleColor, int[]> BestForegrounds = new Dictionary<ConsoleColor, int[]>()
        {
           { Black,         new [] { 2,3,6,7,8,10,11,12,14,15 }},
           { DarkBlue,      new [] { 6,7,10,11,12,14,15 }},
           { DarkGreen,     new [] { 0,1,7,10,11,14,15 }},
           { DarkCyan,      new [] { 0,1,5,7,11,14,15 }},
           { DarkRed,       new [] { 0,6,7,14,15 }},
           { DarkMagenta,   new [] { 0,2,3,4,6,7,10,11,12,14,15 }},
           { DarkYellow,    new [] { 0,1,4,5,7,9,13,14,15}},
           { Gray,          new [] { 0,1,2,3,4,5,8,9,10,13,14,15 }},
           { DarkGray,      new [] { 0,1,5,6,7,10,11,14,15 }},
           { Blue,          new [] { 0,1,3,6,7,10,11,14,15 }},
           { Green,         new [] { 0,1,2,4,5,7,13,14,15 }},
           { Cyan,          new [] { 0,1,2,3,4,5,8,9,12,13,14,15 }},
           { Red,           new [] { 0,1,7,11,14,15 }},
           { Magenta,       new [] { 0,3,6,7,11,14,15 }},
           { Yellow,        new [] { 0,1,2,3,4,5,8,9,10,12,13 }},
           { White,         new [] { 0,1,2,3,4,5,6,8,9,10,12,13 }},
        };

        public static Dictionary<ConsoleColor, int[]> Disabled = new Dictionary<ConsoleColor, int[]>()
        {
           { Black,         new [] { 8 }},
           { DarkBlue,      new [] { 3,9,13 }},
           { DarkGreen,     new [] { 6,10 }},
           { DarkCyan,      new [] { 8,9 }},
           { DarkRed,       new [] { 5,6,12 }},
           { DarkMagenta,   new [] { 8,9,13 }},
           { DarkYellow,    new [] { 8,12 }},
           { Gray,          new [] { 6,8 }},
           { DarkGray,      new [] { 3,5,6 }},
           { Blue,          new [] { 2,3,5 }},
           { Green,         new [] { 2,3,8,9,11 }},
           { Cyan,          new [] { 3,8,9 }},
           { Red,           new [] { 4,5,9,11,13 }},
           { Magenta,       new [] { 5,6,9,12 }},
           { Yellow,        new [] { 6,7 }},
           { White,         new [] { 7 }},
        };

        public static Colors RandomColor(int background)
        {
            return RandomColor((ConsoleColor)background);
        }
        public static Colors RandomColor(ConsoleColor background)
        {
            var colors = BestForegrounds[background];
            var foreground = (ConsoleColor) colors[rnd.Next(colors.Length)];
            return new Colors(foreground, background);
        }

        public override string ToString()
        {
            return $"{Foreground}On{Background}";
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
