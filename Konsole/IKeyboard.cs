using System;

namespace Konsole
{
    public interface IKeyboard
    {
        ConsoleKeyInfo ReadKey(bool intercept = false);

        ///// <summary>
        ///// blocks, like ReadKey, will discard any keys it's not waiting for. Does not return anything, raises key press events.
        ///// </summary>
        //void WaitKey(Case sensitive = Case.Insensitive, params ConsoleKeyInfo[] c);

        

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
        /// Waits for one of the keys in chars[] to be pressed.Raises charPress and keyPress events for all keys pressed.If no keys requested, waits for any key. 
        /// blocks, like ReadKey, will discard any keys it's not waiting for. Does not return anything, raises key press events. 
        /// </summary>
        void CharPressed(Action<char> key, params char[] chars);


        //void CharPressed(Action<char> key, Case sensitive, params char[] chars);

        //void OnKeyPress(Action<ConsoleKeyInfo> key, params ConsoleKeyInfo[] keys);
        //void OnKeyPress(Action<ConsoleKeyInfo> key, Case sensitive = Case.Insensitive, params ConsoleKeyInfo[] chars);

    }
}