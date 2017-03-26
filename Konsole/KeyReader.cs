using System;

namespace Konsole
{
    public class KeyReader : IReadKey
    {
        public ConsoleKeyInfo ReadKey()
        {
            var key = Console.ReadKey(true);
            return key;
        }

        public bool CursorVisible
        {
            get { return Console.CursorVisible; }
            set { Console.CursorVisible = value; }
        }
    }
}