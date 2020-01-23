using System;

namespace Konsole
{
    // begin-snippet: IConsoleState
    public interface IConsoleState : ISetColors
    {
        ConsoleState State { get; set; }
        int CursorTop { get; set; }
        int CursorLeft { get; set; }

        /// <summary>
        /// runs an action that may or may not modify the console state that can cause corruptions when thread context swaps. Must lock on a static locker, do try catch, and ensure state is back to what it was before the command ran.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="action"></param>
        /// <remarks>If you're not writing a threadsafe control or threading is not an issue, then you can simply call action() in your implementation.</remarks>
        /// <example>
        /// <code>
        ///  lock(_locker)
        ///  {
        ///    var state = console.State;
        ///    try
        ///    {
        ///      action();
        ///    }
        ///    finally
        ///    {
        ///      console.State = state;</code>
        ///    }
        ///  }</example>
        void DoCommand(IConsole console, Action action);

        bool CursorVisible { get; set; }
    }
    //end-snippet
}   