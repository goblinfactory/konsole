using System;
using System.Linq;
using System.Threading;

namespace Konsole
{
    public class Reader : IRead
    {
        public ConsoleKeyInfo ReadKey(bool intercept = false)
        {
            return Console.ReadKey(intercept);
        }

        public void KeyWait(params ConsoleKey[] c)
        {
            if (c.Length == 0)
            {
                Console.ReadKey(true);
                return;
            }
            ConsoleKey? key = null;

            while (!c.Any(k => k == key))
            {
                key = Console.ReadKey(true).Key;
            }
        }
    }
}