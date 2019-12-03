using Konsole.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goblinfactory.Konsole.Internal
{
    // class written in response to issue #28 https://github.com/goblinfactory/konsole/issues/28 (Crash after window resize)
    public static class WindowResizeChecker
    {
        public static int CheckWidth(this int x)
        {
            return x.Min(Console.WindowWidth-1, Console.BufferWidth-1);
        }
    }
}
