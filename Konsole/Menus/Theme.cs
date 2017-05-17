using System;

namespace Konsole.Menus
{
    public class Theme
    {
        public ConsoleColor Background { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor Border { get; set; } = ConsoleColor.DarkBlue;
        public ConsoleColor Foreground { get; set; } = ConsoleColor.Gray;
        public ConsoleColor ShortcutKeyHilite { get; set; } = ConsoleColor.White;
        public ConsoleColor ShortcutKeyHiliteSelected { get; set; } = ConsoleColor.Red;
        public ConsoleColor SelectedItemBackground { get; set; } = ConsoleColor.Gray;
        public ConsoleColor SelectedItemForeground { get; set; } = ConsoleColor.DarkBlue;
    }
}