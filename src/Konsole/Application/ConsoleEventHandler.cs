using System;
using System.Threading.Tasks;

namespace Konsole
{
    /// <summary>
    /// Not threadsafe because KeyHandler delegates directly to Console for keyboard IO and that cannot easily be made threadsafe in any logical manner. e.g. which thread handles a keypress if two threads want to simulate  Console.ReadKey(true) and capture the press?
    /// </summary>
    public class ConsoleEventHandler : IDisposable
    {
        IConsoleApplication[] _consoles;
        public ConsoleEventHandler(params IConsoleApplication[] consoles)
        {
            _consoles = TabOrderer.SetTabOrdering(consoles);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// while true loop that handles keyboard events and tabbing, with optional quit key, typically escape.Escape is default.
        /// </summary>
        /// <returns></returns>
        public async Task HandleEventsAsync()
        {
            await Task.CompletedTask;
            //Console.CursorVisible = false;
            //bool first = true;

            //// render each console in tab order, starting with first control setting to active, and the rest to inactive
            //foreach (var console in _consoles)
            //{
            //    var m = console.Manager;
            //    if (first)
            //    {
            //     //  m.Refresh(ControlStatus.Active)
            //    }
            //}
        }
    }
}
