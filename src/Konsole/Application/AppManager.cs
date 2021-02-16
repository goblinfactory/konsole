using Konsole;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Konsole
{
    internal class AppManager : IConsoleManager
    {
        public AppManager()
        {

        }

        public void Refresh(ControlStatus status)
        {
            throw new NotImplementedException();
        }

        private ConcurrentDictionary<Guid, IConsole> _children = new ConcurrentDictionary<Guid, IConsole>();
        public IConsole[] ConsolesByTabOrder()
        {
            return _children.Select(c => c.Value).OrderBy(c => c.TabOrder).ToArray();
        }

        public void Remove(Guid consoleId)
        {
            if (!_children.TryRemove(consoleId, out _)) throw new ArgumentOutOfRangeException($"Console with id '{consoleId}' not found.");
        }
        public void Add(IConsole console)
        {
            if (!_children.TryAdd(console.Id, console)) throw new ArgumentOutOfRangeException($"Console with id '{console.Id}' already exists.");
        }

        /// <summary>
        /// while true loop that handles keyboard events and tabbing, with optional quit key, typically escape. Escape is default.
        /// </summary>
        public async Task RunAsync()
        {
            // todo: after I get it all working, do a perf test and see where bottlenecks are, and then optimise there?

            //return Task.CompletedTask;
            var consoles = ConsolesByTabOrder();
            var handler = new ConsoleEventHandler(consoles);
            await handler.HandleEventsAsync();
        }
    }
}
