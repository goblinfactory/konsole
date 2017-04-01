using System;

namespace Konsole.Menus
{
    public class MenuItem
    {
        public char? Key { get; }
        public string Title { get; }
        public string Description { get; }
        public Action<IConsole> Action { get; }
        public bool IsDefault { get; set; }


        public MenuItem(char key, string title, Action<IConsole> action, bool isDefault = false)
        {
            Key = key;
            Title = title;
            Action = action;
            IsDefault = isDefault;
        }

        public MenuItem(string title, Action<IConsole> action)
        {
            Key = null;
            Title = title;
            Action = action;
        }

    }
}