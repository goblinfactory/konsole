using System;

namespace Konsole
{
    public static class ConsoleKeyInfoExtensions
    {
        public static bool Shift(this ConsoleKeyInfo ki)
        {
            return (ki.Modifiers & ConsoleModifiers.Shift) != 0;
        }

        public static bool Control(this ConsoleKeyInfo ki)
        {
            return (ki.Modifiers & ConsoleModifiers.Control) != 0;
        }

        public static bool Alt(this ConsoleKeyInfo ki)
        {
            return (ki.Modifiers & ConsoleModifiers.Alt) != 0;
        }


        public static bool SameAs(this ConsoleKey lhs, ConsoleKey? rhs)
        {
            if (rhs == null) return false;
            char lkey = (char) lhs;
            char rkey = (char) rhs;
            return char.ToUpper(lkey) == char.ToUpper(rkey);
        }

        public static ConsoleKeyInfo ToKeypress(this char c, bool shift, bool alt, bool control)
        {
            var ck = (ConsoleKey)char.ToUpper(c);
            return new ConsoleKeyInfo(c, ck, shift, alt, control);
        }


        public static ConsoleKeyInfo ToKeypress(this char c)
        {
            var ck = (ConsoleKey) char.ToUpper(c);
            return new ConsoleKeyInfo(c, ck, false, false, false);
        }

        public static ConsoleKeyInfo ToKeypress(this ConsoleKey key, bool shift, bool alt, bool control)
        {
            return new ConsoleKeyInfo((char) key, key, shift, alt, control);
        }

        public static ConsoleKeyInfo ToKeypress(this ConsoleKey key)
        {
            return new ConsoleKeyInfo((char)key, key, false, false, false);
        }
    }
}