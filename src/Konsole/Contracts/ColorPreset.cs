using System;
using static System.ConsoleColor;

namespace Konsole
{
    public enum ColorPreset
    {
        Undefined = 0,
        GrayOnBlack,
        WhiteOnBlack,
        BlackOnWhite,
        WhiteOnBlue,
        WhiteOnDarkBlue,
        BlueOnWhite,
        DarkBlueOnWhite
    }

    public static class ColorPresetExtensions
    {
        public static Style ToColor(this ColorPreset preset)
        {
            switch (preset)
            {
                case ColorPreset.Undefined: return Style.GrayOnBlack;
                case ColorPreset.GrayOnBlack: return Style.GrayOnBlack;
                case ColorPreset.WhiteOnBlack: return Style.WhiteOnBlack;
                case ColorPreset.BlackOnWhite: return Style.BlackOnWhite;
                case ColorPreset.WhiteOnBlue: return Style.WhiteOnBlue;
                case ColorPreset.WhiteOnDarkBlue: return Style.WhiteOnDarkBlue;
                case ColorPreset.BlueOnWhite: return Style.BlueOnWhite;
                case ColorPreset.DarkBlueOnWhite: return Style.DarkBlueOnWhite;
                default:return Style.GrayOnBlack;
            }
        }

        public static ConsoleColor ToSelectedItemForeground(this ConsoleColor foreground)
        {
            switch (foreground)
            {
                case Black: return Black;
                case DarkBlue: return Blue;
                case DarkCyan: return Cyan;
                case DarkGray: return Gray;
                case DarkGreen: return Green;
                case DarkMagenta: return Magenta;
                case DarkRed: return Red;
                case DarkYellow: return Yellow;


                case Blue: return White;
                case Cyan: return White;
                case Gray: return White;
                case Green: return White;
                case Magenta: return White;
                case Red: return White;
                case White: return White;
                case Yellow: return Red;
                default: return Red;
            }
        }

        public static ConsoleColor ToSelectedItemBackground(this ConsoleColor background)
        {
            switch (background)
            {
                case Black: return Gray;
                case DarkBlue: return Gray;
                case DarkCyan: return Gray;
                case DarkGray: return Gray;
                case DarkGreen: return Gray;
                case DarkMagenta: return Gray;
                case DarkRed: return Gray;
                case DarkYellow: return Gray;


                case Blue: return DarkGray;
                case Cyan: return DarkGray;
                case Gray: return DarkGray;
                case Green: return DarkGray;
                case Magenta: return DarkGray;
                case Red: return DarkGray;
                case White: return DarkGray;
                case Yellow: return DarkGray;
                default: return DarkGray;
            }
        }

        public static ConsoleColor ToHeading(this ConsoleColor background)
        {
            switch (background)
            {
                case Black: return Green;
                case Blue: return Yellow;
                case Cyan: return DarkRed;
                case DarkBlue: return Yellow;
                case DarkCyan: return Yellow;
                case DarkGray: return Red;
                case DarkGreen: return Magenta;
                case DarkMagenta: return Yellow;
                case DarkRed: return Yellow;
                case DarkYellow: return Red;
                case Gray: return Red;
                case Green: return Red;
                case Magenta: return Green;
                case Red: return Green;
                case White: return Yellow;
                case Yellow: return Red;
                default: return Red;
            }
        }
    }

}
