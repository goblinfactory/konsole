using System;
using System.Linq;
using System.Threading;

namespace Konsole
{

    public class Keyboard : IKeyboard
    {
        private IKeyboard _keyboard = null;

        public Keyboard()
        {
                
        }

        public Keyboard(IKeyboard k)
        {
            _keyboard = k;
        }

        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            if (_keyboard != null) return _keyboard.ReadKey(intercept);
            return Console.ReadKey(intercept);
        }


        /// <summary>
        /// Waits for one of the keys in chars[] to be pressed. Raises charPress and keyPress events for all keys pressed. If no keys requested, waits for any key. 
        /// blocks, like ReadKey, will discard any keys it's not waiting for. Does not return anything, raises key press events. Chars matching is case insensitive. e.g. you can press 'a' or 'A' to match 'a' or 'A'.
        /// </summary>
        public void WaitForKeyPress(params char[] chars)
        {
            WaitForKeyPress(Case.Insensitive, chars);
        }

        /// <summary>
        /// Waits for one of the keys in chars[] to be pressed.Raises charPress and keyPress events for all keys pressed.If no keys requested, waits for any key. 
        /// blocks, like ReadKey, will discard any keys it's not waiting for. Does not return anything, raises key press events. 
        /// </summary>
        public void WaitForKeyPress(Case @case, params char[] chars)
        {
            if (_keyboard != null)
            {
                _keyboard.WaitForKeyPress(@case, chars);
                return;
            }
            if (chars.Length == 0)
            {
                Console.ReadKey(true);
                return;
            }
            ConsoleKeyInfo? key = null;

            while (!chars.Any(c => isMatch(@case, key, c)))
            {
                key = Console.ReadKey(true);
                _charPressed(key.Value.KeyChar);
            }
        }


        //public void KeyWait(params char[] chars)
        //{
        //    ConsoleKeyInfo[] keys = chars.Select(c => c.ToKeypress()).ToArray();
        //    KeyWait(keys);
        //}

        //public void KeyWait(ConsoleKeyInfo[] c, bool ignoreCase = true)
        //{
        //    if (_keyboard != null)
        //    {
        //        _keyboard.KeyWait(c);
        //        return;
        //    }
        //    if (c.Length == 0)
        //    {
        //        Console.ReadKey(true);
        //        return;
        //    }
        //    ConsoleKeyInfo? key = null;

        //    while (!c.Any(k => isMatch(ignoreCase,key, k)))
        //    {
        //        key = Console.ReadKey(true);
        //        _onCharPress(key.Value.KeyChar);
        //    }
        //}

        internal static ConsoleKeyInfo CloneModifiers(ConsoleKeyInfo c1, char c)
        {
            bool shift = (c1.Modifiers & ConsoleModifiers.Shift) != 0;
            bool alt = (c1.Modifiers & ConsoleModifiers.Alt) != 0;
            bool control = (c1.Modifiers & ConsoleModifiers.Control) != 0;

            var ki = new ConsoleKeyInfo(c, c1.Key, shift, alt, control);
            return ki;
        }

        private bool isMatch(Case @case, ConsoleKeyInfo? key, char c)
        {
            if (key == null) return false;
            if (@case == Case.Sensitive)
            {
                return key.Value.KeyChar == c;
            }
            return char.ToUpper(c)== char.ToUpper(key.Value.KeyChar);
        }


        private bool isMatch(bool ignoreCase, ConsoleKeyInfo? k1, ConsoleKeyInfo k2)
        {
            if (!k1.HasValue) return false;
            if (k1 == k2) return true;
            if (ignoreCase)
            {
                var c1 = char.ToUpper(k1.Value.KeyChar);
                var c1k = CloneModifiers(k1.Value, c1);

                var c2 = char.ToUpper(k2.KeyChar);
                var c2k = CloneModifiers(k2, c2);
                return c1k == c2k;
            }
            return false;
        }

        public void OnCharPressed(char c, Action<char> action)
        {
            OnCharPressed(new[] {c}, action);
        }

        // NB Needs unit test.
        public void OnCharPressed(char[] chars, Action<char> action)
        {
            if (_keyboard != null)
            {
                _keyboard.OnCharPressed(chars, action);
            }
            else
            {
                _charPressed += (c) =>
                {
                    foreach (var c1 in chars.Where(cc=> c == cc))
                    {
                        action(c);
                    }
                };
            }
        }

        Action<char> _charPressed = (c) => { };
    }
}