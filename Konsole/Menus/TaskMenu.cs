using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Menus
{
    // q: should I create taskmenuItem's ?? might make it easier to specialise?
    // e.g. seperate messages and waiting times for each menuitem...
    // plus whether to allow multiple instances or not?

    /// <summary>
    /// a menu that will start a new background task for each of the menu items.
    /// menu items will be disabled for the duration of the task until the task completes. Once complete
    /// then menu item will be re-enabled to prevent users from queueing up more than one concurrent menu action.
    /// </summary>
    public class TaskMenu : Menu
    {
        private readonly int _msToWaitToComplete;
        private List<Task> _tasks = new List<Task>();
        private readonly object _locker = new object();

        public TaskMenu(int msToWaitToComplete, string title, ConsoleKey quit, int width, params MenuItem[] menuActions) : base(title, quit, width, menuActions)
        {
            _msToWaitToComplete = msToWaitToComplete;
        }

        internal TaskMenu(IConsole output, int msToWaitToComplete, string title, ConsoleKey quit, int width, params MenuItem[] menuActions) : base(output, title, quit, width, menuActions)
        {
            _msToWaitToComplete = msToWaitToComplete;
        }

        protected override void RunItem(ConsoleState state, MenuItem item)
        {
            lock (_locker)
            {
                var task = Task.Run(() => base.RunItem(state, item));
                _tasks.Add(task);
            }
        }


    }
}
