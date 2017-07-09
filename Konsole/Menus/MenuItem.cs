using System;

namespace Konsole.Menus
{


    public class MenuItem
    {
        internal object locker = new object();
        public ConsoleKeyInfo? Key { get; }
        public string Title { get; }
        public string Description { get; }
        public Action Action { get; }
        public bool Enabled { get; set; } = true;
        public bool Running { get; internal set; } = false;

        /// <summary>
        /// don't allow this menu item to be called if it is already currently running. e.g. Menu item is 'start server', you do not want to run this twice, or while a server is currently running.
        /// </summary>
        public bool DisableWhenRunning { get; set; } = true;
        public bool IsDefault { get; set; }


        public MenuItem(char key, string title, Action action)
        {
            Key = key.ToKeypress();
            Title = title;
            Action = action;
        }

        public MenuItem(string title, Action action)
        {
            Key = null;
            Title = title;
            Action = action;
        }

        public static MenuItem Quit(char q, string title)
        {
            return new MenuItem(q, title, (Action)null);
        }

        public static MenuItem Quit(string title)
        {
            return new MenuItem(title, (Action)null);
        }

    }
}