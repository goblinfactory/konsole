using System;
using Konsole.Internal;

namespace Konsole
{
    public partial class Window
    {
        /// <summary>
        /// Create a new window inline starting on the next line, at current `CursorTop + 1, using the specified width with foreground and background color. 
        /// </summary>
        public static IConsole OpenInline(IConsole echoConsole, int padLeft, int width, int height, ConsoleColor foreground, ConsoleColor background, params K[] options)
        {
            lock (_staticLocker)
            {
                if (echoConsole.CursorLeft > 0)
                {
                    var win1 = new Window(padLeft, echoConsole.CursorTop + 1, width, height, foreground, background, true, echoConsole, options);
                    echoConsole.CursorTop += height + 1;
                    echoConsole.CursorLeft = 0;
                    return win1.Concurrent();
                }
                var win2 = new Window(padLeft, echoConsole.CursorTop, width, height, foreground, background, true, echoConsole, options);
                echoConsole.CursorTop += height;
                return win2.Concurrent();
            }
        }

        public static IConsole OpenInline(IConsole echoConsole, int height)
        {
            var balance = echoConsole.WindowHeight - echoConsole.CursorTop;
            return OpenInline(echoConsole, 0, echoConsole.WindowWidth, IntegerExtensions.Min(height, balance), echoConsole.ForegroundColor, echoConsole.BackgroundColor);
        }

    }
}
