using Konsole;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Konsole
{
    internal class ConsoleManager : IConsoleManager
    {
        public int CntChildren {  get { return _children.Keys.Count; } }
        public bool HasChildren { get { return _children.Keys.Any(); } }
        
        private int _cntChildren = 0;
        public ConsoleManager()
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

        public IConsole[] Consoles
        {
            get
            {
                return _children.Select(c => c.Value).ToArray();
            }
        }

        public void Remove(Guid id)
        {
            if (!_children.TryRemove(id, out _)) throw new ArgumentOutOfRangeException($"Console with id '{id}' not found.");
        }
        public void Remove(string title)
        {
            var con = Consoles.FirstOrDefault(c => c.Title == title);
            if(con == null) throw new ArgumentOutOfRangeException($"Console with title '{title}' not found.");
            if (!_children.TryRemove(con.Id, out _)) throw new ArgumentOutOfRangeException($"Console with title '{title}' not found.");
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
