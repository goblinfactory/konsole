using System;

namespace Konsole.Menus
{


    public class MenuItem
    {
        internal object locker = new object();
        public ConsoleKey? Key { get; }
        public string Title { get; }
        public string Description { get; }
        public Action Action { get; }
        public bool Enabled { get; set; } = true;
        public bool Running { get; internal set; } = false;
        public bool DisableWhenRunning { get; set; } = true;
        public bool IsDefault { get; set; }


        public MenuItem(char key, string title, Action action) 
        {
            Key = (ConsoleKey)key;
            Title = title;
            Action = action;
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