//using System;
//using Konsole.Internal;

//namespace Konsole
//{
//    // can I replace all of these with new Window(); without a height and width, and it's automatically inline? i.e. there is NEVER a case where you want a new windows without a start X and Y without it being inline?
//    public partial class Window
//    {
//        public static IConsole OpenInline(int height)
//        {
//            var echoConsole = Window.HostConsole;
//            var balance = echoConsole.WindowHeight - echoConsole.CursorTop;
//            return OpenInline(echoConsole, 0, echoConsole.WindowWidth, IntegerExtensions.Min(height, balance), echoConsole.ForegroundColor, echoConsole.BackgroundColor);
//        }

//        public static IConsole OpenInline(IConsole echoConsole, int height)
//        {
//            var balance = echoConsole.WindowHeight - echoConsole.CursorTop;
//            return OpenInline(echoConsole, 0, echoConsole.WindowWidth, IntegerExtensions.Min(height, balance), echoConsole.ForegroundColor, echoConsole.BackgroundColor);
//        }

//        public static IConsole OpenInline(IConsole echoConsole, int height, WindowSettings settings)
//        {
//            var balance = echoConsole.WindowHeight - echoConsole.CursorTop;
//            return OpenInline(echoConsole, 0, echoConsole.WindowWidth, IntegerExtensions.Min(height, balance), echoConsole.ForegroundColor, echoConsole.BackgroundColor);
//        }

//        /// <summary>
//        /// Create a new window inline starting on the next line, at current `CursorTop + 1, using the specified width with foreground and background color. Moves the cursor to underneath the newly created window.
//        /// </summary>
//        public static IConsole OpenInline(IConsole echoConsole, int height, int width, WindowSettings settings)
//        {
//            lock (_staticLocker)
//            {
//                if (echoConsole.CursorLeft > 0)
//                {
//                    var win1 = new Window(settings.PadLeft, echoConsole.CursorTop + 1, settings.Width ?? echoConsole.WindowWidth, settings.Height ?? echoConsole.WindowHeight - echoConsole.CursorTop, foreground, background, true, echoConsole, options);
//                    echoConsole.CursorTop += height + 1;
//                    echoConsole.CursorLeft = 0;
//                    return win1.Concurrent();
//                }
//                var win2 = new Window(padLeft, echoConsole.CursorTop, width, height, foreground, background, true, echoConsole, options);
//                echoConsole.CursorTop += height;
//                return win2.Concurrent();
//            }
//        }

//    }
//}
