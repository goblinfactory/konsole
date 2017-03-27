using System;

namespace Konsole.Menus
{
    public class MenuItem
    {
        public char? Key { get; }
        public string Title { get; }
        public Action<IConsole> Action { get; }

        public MenuItem(char key, string title, Action<IConsole> action)
        {
            Key = key;
            Title = title;
            Action = action;
        }

        public MenuItem(string title, Action<IConsole> action)
        {
            Key = null;
            Title = title;
            Action = action;
        }

    }
}