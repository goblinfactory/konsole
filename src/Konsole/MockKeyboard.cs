using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Konsole
{
    // test

    public class MockKeyboard : IKeyboard
    {
        private readonly Queue<ConsoleKeyInfo> _keypresses = new Queue<ConsoleKeyInfo>();

        public MockKeyboard(params ConsoleKeyInfo[] keys)
        {
            // not threadsafe, but this is for testing so good enough.
            foreach (var key in keys) _keypresses.Enqueue(key);
        }

        public MockKeyboard(params ConsoleKey[] keys)
        {
            foreach (var key in keys) _keypresses.Enqueue(key.ToKeypress());
        }

        public MockKeyboard(params char[] keys)
        {
            // not threadsafe, but this is for testing so good enough.
            foreach (var key in keys) _keypresses.Enqueue(key.ToKeypress());
        }

        public MockKeyboard()
        {

        }

        public MockKeyboard(int pauseBetweenPresses, IEnumerable<ConsoleKeyInfo> readKey)
        {
            PauseBetweenKeypresses = pauseBetweenPresses;
            _keyEnumerator = readKey.GetEnumerator();
        }


        public MockKeyboard PressKey(char c, bool shift = false, bool alt = false, bool control = false)
        {
            var keyPress = new ConsoleKeyInfo(c, (ConsoleKey) c, shift, alt, control);
            OnCharPress(c);
            _keypresses.Enqueue(keyPress);
            return this;
        }

        /// <summary>
        /// pause between keypresses. Expected use for this might be automated demos if you want to simulate user typing on a screen, or integration testing where timing is critical.
        /// </summary>
        public int PauseBetweenKeypresses { get; set; } = 0;

        // can add in a randomiser for making the pauses seem a bit more human.
        // or possilby add the the timing to each PressKey so that we can do a record and replay and simulate real user behavior.
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">if called, and you have not queued up enough keystrokes to process the requests.</exception>
        public ConsoleKeyInfo ReadKey()
        {
            if (PauseBetweenKeypresses != 0) Thread.Sleep(PauseBetweenKeypresses);
            if (_keyEnumerator != null)
            {
                _keyEnumerator.MoveNext();
                var key = _keyEnumerator.Current;
                OnCharPress(key.KeyChar);
                return key;
            }
            
            if (_keypresses.Count == 0)
            {
                if (AutoReplyKey == null) throw new InvalidOperationException("MockKeyboard has run out of queued keys to return. Enable auto-reply or queue up more keystrokes.");
                OnCharPress(AutoReplyKey.Value.KeyChar);
                return AutoReplyKey.Value;
            }
            var k = _keypresses.Dequeue();
            OnCharPress(k.KeyChar);
            return k;
        }

        private IEnumerator<ConsoleKeyInfo> _keyEnumerator;

        public ConsoleKeyInfo? AutoReplyKey { get; set; } = null;

        /// <summary>
        /// This call is not strictly compatible with normal Console behavior. If intercept is set to false, will not echo the key to the console.
        /// </summary>
        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return ReadKey();
        }

        private void WaitForKeys(ConsoleKeyInfo[] c)
        {
            if (c.Length == 0)
            {
                ReadKey();
                return;
            }
            ConsoleKeyInfo? key = null;

            while (!c.Any(k => k == key))
            {
                key = ReadKey(true);
            }
        }

        public void WaitForKeyPress(Case @case, params char[] chars)
        {
            ConsoleKeyInfo[] keys;

            keys = (@case == Case.Sensitive)
                ? chars.Select(c => c.ToKeypress()).ToArray()
                : chars.Select(c => char.ToUpper(c).ToKeypress()).Union(chars.Select(c => char.ToLower(c).ToKeypress())
            ).ToArray();

            WaitForKeys(keys);
        }

        public void WaitForKeyPress(params char[] chars)
        {
            var keys = chars.Select(c => c.ToKeypress()).ToArray();
            WaitForKeys(keys);
        }


        public void OnCharPressed(char c, Action<char> action)
        {
            OnCharPressed(new [] {c}, action);
        }

        public void OnCharPressed(char[] chars, Action<char> action)
        {
            OnCharPress += cp =>
            {
                if (chars.Any(c => c == cp))
                {
                    action(cp);
                }
            };
        }

        public Action<char> OnCharPress = (c) => { };

    }
}
