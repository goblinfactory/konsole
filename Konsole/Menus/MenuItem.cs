using System;

namespace Konsole.Menus
{


    public class MenuItem
    {
        public ConsoleKey? Key { get; }
        public string Title { get; }
        public string Description { get; }
        public Action Action { get; }
        public Action<IConsole> ConsoleAction { get; } = null;
        public bool IsDefault { get; set; }



        //public MenuItem(char? key, string title, Action action, bool isDefault = false)
        //{
        //    Key = (ConsoleKey?)key;
        //    Title = title;
        //    Action = action;
        //    IsDefault = isDefault;
        //}

        //public MenuItem(ConsoleKey key, string title, Action action, bool isDefault = false)
        //{
        //    Key = key;
        //    Title = title;
        //    Action = action;
        //    IsDefault = isDefault;
        //}

        public MenuItem(char key, string title, Action action) 
        {
            Key = (ConsoleKey)key;
            Title = title;
            Action = action;
            ConsoleAction = null;
        }

        public MenuItem(char key, string title, Action<IConsole> action)
        {
            Key = (ConsoleKey) key;
            Title = title;
            Action = null;
            ConsoleAction = action;
        }

        public MenuItem(string title, Action<IConsole> action)
        {
            Key = null;
            Title = title;
            ConsoleAction = action;
            Action = null;
        }

        public MenuItem(string title, Action action)
        {
            Key = null;
            Title = title;
            Action = action;
        }

        public static MenuItem Quit(string title)
        {
            return new MenuItem('q', title, (Action)null);
        }

    }
}