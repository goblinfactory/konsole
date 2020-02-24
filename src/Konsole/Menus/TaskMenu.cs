using System;
using System.Collections.Generic;
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
        private readonly List<Task> _tasks = new List<Task>();
        private readonly object _locker = new object();
        public Action OnTimeoutWaiting = () => { };
        /// <summary>
        /// called before the menu waits for all background tasks to complete, regardless of whether any were started.
        /// </summary>
        public Action OnStopping = () => { };

        /// <summary>
        /// Called after all background tasks have completed, regardless of whether any were started.
        /// </summary>
        public Action OnStopped = () => { };

        public TaskMenu(int msToWaitToComplete, string title, ConsoleKey quit, int width, params MenuItem[] menuActions) 
            : this(Window.HostConsole, msToWaitToComplete, title, quit, width, menuActions)
                {
            
                }

        public TaskMenu(IConsole output, int msToWaitToComplete, string title, ConsoleKey quit, int width, params MenuItem[] menuActions) 
            : base(output, title, quit, width, menuActions)
        {
            _msToWaitToComplete = msToWaitToComplete;
            OnBeforeExitMenu += WaitForAllMenusToComplete;
        }


        internal void WaitForAllMenusToComplete()
        {
            OnStopping();
            bool allStopped;
            lock (_locker)
            {
                allStopped = _tasks != null && Task.WaitAll(_tasks.ToArray(), _msToWaitToComplete);
            }
            //todo fix the timeout check
            //if (!allStopped) OnTimeoutWaiting();
            if (allStopped) OnStopped();
        }

        //todo: add in task cancellation tokens automatically generated and passed to the menu items! 

        protected override void RunItem(ConsoleState state, MenuItem item)
        {
            var task = Task.Run(() => base.RunItem(state, item));

            lock (_locker)
            {
                _tasks.Add(task);
            }
        }

        public override void Run()
        {
            base.Run();
            WaitForAllMenusToComplete();
        }
    }
}
