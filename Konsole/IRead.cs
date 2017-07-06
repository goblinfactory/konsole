using System;

namespace Konsole
{
    public interface IRead
    {
        ConsoleKeyInfo ReadKey(bool intercept = false);

        ///// <summary>
        ///// blocks, like ReadKey, will discard any keys it's not waiting for, returns the first key pressed matching selection
        ///// </summary>
        ///// <returns>first valid ConsoleKey pressed matching selection</returns>
        //ConsoleKey KeyWaitFor(params ConsoleKey[] k);

        ///// <summary>
        ///// blocks, like ReadKey, will discard any keys it's not waiting for, returns the first key pressed matching selection
        ///// </summary>
        ///// <returns>first valid key pressed matching selection</returns>
        //char? KeyWaitFor(params char[] c);

        ///// <summary>
        ///// blocks, like ReadKey, will discard any keys it's not waiting for. Does not return anything
        ///// </summary>
        //void KeyWait(params char[] c);

        /// <summary>
        /// blocks, like ReadKey, will discard any keys it's not waiting for. Does not return anything
        /// </summary>
        void KeyWait(params ConsoleKey[] c);

        //void PressKey(ConsoleKey k);
        //void EnterLine(string text);
        //void Enter(string text);
        //void PressKeys(string text, bool endWithEnterKey);
    }
}