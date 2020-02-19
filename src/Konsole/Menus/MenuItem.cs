using System;

namespace Konsole
{
    public class MenuItem
    {
        internal object locker = new object();
        public ConsoleKeyInfo? Key { get; }
        public string Title { get; set; }

        public string Description { get; }
        public Action<MenuItem> Action { get; }
        public bool Enabled { get; set; } = true;
        public bool Running { get; internal set; } = false;

        /// <summary>
        /// don't allow this menu item to be called if it is already currently running. e.g. Menu item is 'start server', you do not want to run this twice, or while a server is currently running.
        /// </summary>
        public bool DisableWhenRunning { get; set; } = true;
        public bool IsDefault { get; set; }


        public MenuItem(char key, string title, Action<MenuItem> action)
        {
            Key = key.ToKeypress();
            Title = title;
            Action = action;
        }

        public MenuItem(string title, Action<MenuItem> action)
        {
            Key = null;
            Title = title;
            Action = action;
        }

        public static MenuItem Quit(char q, string title)
        {
            return new MenuItem(q, title, (Action<MenuItem>)null);
        }

        public static MenuItem Quit(string title)
        {
            return new MenuItem(title, (Action<MenuItem>)null);
        }

    }
}