using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole
{
    public static class ConsoleKeyExtensions
    {
        public static bool IsAlphaNumeric(this ConsoleKey key)
        {
            int k = (int) key;
            return k >= 36 && k <= 71;
        }
    }
}
