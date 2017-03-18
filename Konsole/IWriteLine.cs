using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole
{
    public interface IWriteLine
    {
        /// <summary>
        /// writes out to the console using the requested color, resetting the color back to the console afterwards.
        /// </summary>
        void WriteLine(ConsoleColor color, string format, params object[] args);
        void WriteLine(string format, params object[] args);
        /// <summary>
        /// writes out to the console using the requested color, resetting the color back to the console afterwards.
        /// </summary>
        void Write(ConsoleColor color, string format, params object[] args);
        void Write(string format, params object[] args);
    }
}
