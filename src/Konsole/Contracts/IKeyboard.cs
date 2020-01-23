using System;

namespace Konsole
{
    // begin-snippet: IKeyboard
    public interface IKeyboard
    {
        ConsoleKeyInfo ReadKey(bool intercept = false);

        /// <summary>
        /// blocks, like ReadKey, will discard any keys it's not waiting for. Does not return anything, raises key press events.
        /// </summary>
        void WaitForKeyPress(Case @case, params char[] chars);

        /// <summary>
        /// Waits for one of the keys in chars[] to be pressed.Raises charPress and keyPress events for all keys pressed.If no keys requested, waits for any key. 
        /// blocks, like ReadKey, will discard any keys it's not waiting for. Does not return anything, raises key press events. Chars matching is case insensitive. e.g. you can press 'a' or 'A' to match 'a' or 'A'.
        /// </summary>
        void WaitForKeyPress(params char[] chars);

        /// <summary>
        /// Registers a handler.
        /// </summary>
        /// <remarks>Do not register a handler that actually does the work, unless you know that your handler is real short and quick and does not block; rather register a handler that pushes a message onto a queue. The reason for this is that the multicast delegate actually calls each registered handler in series and any hanlder that blocks, blocks all other registered handlers.</remarks>
        void OnCharPressed(char[] chars, Action<char> key);

        void OnCharPressed(char c, Action<char> key);
    }
    //end-snippet
}