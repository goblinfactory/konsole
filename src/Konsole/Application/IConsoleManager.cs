using System;
using System.Threading.Tasks;

namespace Konsole
{
    public interface IConsoleManager
    {
        void Remove(Guid consoleId);
        void Remove(string title);
        void Add(IConsole console);

        IConsole[] Consoles { get; }
        IConsole[] ConsolesByTabOrder();
        void Refresh(ControlStatus status);

        /// <summary>
        /// while true loop that handles keyboard events and tabbing, with optional quit key, typically escape. Escape is default.
        /// </summary>
        Task RunAsync();

        int CntChildren { get; }

        bool HasChildren { get; }
    }
}   
