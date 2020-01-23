using System;

namespace Konsole
{
    // begin-snippet: IWriteColor
    public interface IWriteColor : IWrite, ISetColors
    {
        /// <summary>
        /// writes out to the console using the requested color, resetting the color back to the console afterwards. Implementor Should be threadsafe.
        /// </summary>
        void Write(ConsoleColor color, string format, params object[] args);

        /// <summary>
        /// writes out to the console using the requested color, resetting the color back to the console afterwards. Implementor Should be threadsafe.
        /// </summary>
        void Write(ConsoleColor color, string text);

        /// <summary>
        /// writes out to the console using the requested color, resetting the color back to the console afterwards. Implementor Should be threadsafe.
        /// </summary>
        void WriteLine(ConsoleColor color, string format, params object[] args);

        /// <summary>
        /// writes out to the console using the requested color, resetting the color back to the console afterwards. Implementor Should be threadsafe.
        /// </summary>
        void WriteLine(ConsoleColor color, string text);

        void Clear(ConsoleColor? backgroundColor);
    }
    //end-snippet
}
