using System;

namespace Konsole.Menus
{
    public class MenuItem
    {
        public char? Key { get; }
        public string Title { get; }
        public string Description { get; }
        public Action<IConsole> Action { get; }

        public MenuItem(char key, string title, string description, Action<IConsole> action)
        {
            Key = key;
            Title = title;
            Action = action;
            Description = description;
        }

        public MenuItem(string title, string description, Action<IConsole> action)
        {
            Key = null;
            Title = title;
            Action = action;
            Description = description;
        }

    }
}