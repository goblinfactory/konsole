using System;
using Konsole.Drawing;

namespace Konsole.Menus
{
    public class WindowTheme
    {
        public ConsoleColor Background { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor Border { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor Foreground { get; set; } = ConsoleColor.Gray;
        public bool ShowBorder { get; set; }
        public LineThickNess BorderThickness { get; set; }

        public static WindowTheme DarkBlue => new WindowTheme
        {
            Background = ConsoleColor.DarkBlue,
            Border = ConsoleColor.Gray,
            Foreground = ConsoleColor.Gray,
            ShowBorder = true,
            BorderThickness = LineThickNess.Single
        };
        public static WindowTheme Gray => new WindowTheme
        {
            Background = ConsoleColor.Gray,
            Border = ConsoleColor.Black,
            Foreground = ConsoleColor.Black,
            ShowBorder = true,
            BorderThickness = LineThickNess.Single
        };
        public static WindowTheme Black => new WindowTheme
        {
            Background = ConsoleColor.Black,
            Border = ConsoleColor.Gray,
            Foreground = ConsoleColor.Gray,
            ShowBorder = true,
            BorderThickness = LineThickNess.Single
        };

    }
}