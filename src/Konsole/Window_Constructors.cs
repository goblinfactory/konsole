using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole
{
    public partial class Window
    {
        public Window() : this(
    0,
    0,
    GetHostWidthHeight().width,
    GetHostWidthHeight().height,
    ConsoleColor.White,
    ConsoleColor.Black,
    true, null
    )
        {
        }

        /// <summary>
        /// Create a new inline window starting on the next line, at current `CursorTop + 1, using the specified width or the whole screen width if none is provided. Default color of White on Black.
        /// </summary>
        public Window(int width, int height, params K[] options)
            : this(null, null, width, height, ConsoleColor.White, ConsoleColor.Black, true, null, options)
        {
        }

        /// <summary>
        /// Open a window Inline at the current cursorTop position, width and height wide and tall.  The parent's cursorTop is incremented so that it will continue to print underneath the newly created window. The constructor is threadsafe, so creating multiple windows will ensure they will not overlap. While the constructor is threadsafe, the returned instance is not. Calling any of the Split methods will return a threadsafe window based off this window. You can call .Concurrent() on the newly created window to return a ConcurrentWriter wrapping the window instance.
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">the height of the window</param>
        /// <param name="foreground">Foreground color</param>
        /// <param name="background">Background color</param>
        public Window(int width, int height, ConsoleColor foreground, ConsoleColor background)
            : this(null, null, width, height, foreground, background, true, null)
        {
        }

        public Window(int width, int height, ConsoleColor foreground, ConsoleColor background, params K[] options)
    : this(null, null, width, height, foreground, background, true, null, options)
        {
        }

        public Window(IConsole console, int width, int height, ConsoleColor foreground, ConsoleColor background,
            params K[] options)
            : this(null, null, width, height, foreground, background, true, console, options)
        {
        }

        public Window(IConsole echoConsole, int x, int y, int width, int height, ConsoleColor foreground,
            ConsoleColor background)
            : this(x, y, width, height, foreground, background, true, echoConsole)
        {
        }

        public Window(IConsole console, int width, int height, params K[] options)
    : this(null, null, width, height, ConsoleColor.White, ConsoleColor.Black, true, console, options)
        {
        }

        public Window(IConsole echoConsole, int x, int y, int width, int height)
            : this(x, y, width, height, ConsoleColor.White, ConsoleColor.Black, true, echoConsole)
        {
        }

        public Window(IConsole echoConsole, int width, int height)
            : this(null, null, width, height, echoConsole.BackgroundColor, echoConsole.ForegroundColor, true, echoConsole)
        {
        }

        // TODO: fix the window constructors, second parameter is sometimes height, and sometimes not!
        public Window(IConsole echoConsole, int height, ConsoleColor foreground, ConsoleColor background)
    : this(null, null, echoConsole.WindowWidth, height, foreground, background, true, echoConsole)
        {
        }

        //Window will clear the parent console area in the overlapping window.
        // this constructor is safe to have params after IConsole because it's the only constructor that starts with IConsole, all other constructors have other strongly typed first parameter. (i.e. avoid parameter confusion)
        public Window(IConsole echoConsole, params K[] options)
            : this(0, 0, (int?)(null), (int?)null, ConsoleColor.White, ConsoleColor.Black, true, echoConsole, options)
        {
        }

        public Window(int x, int y, int width, int height, IConsole echoConsole = null, params K[] options)
            : this(x, y, width, height, ConsoleColor.White, ConsoleColor.Black, true, echoConsole, options)
        {
        }

        public Window(int x, int y, int width, int height, ConsoleColor foreground, ConsoleColor background,
    IConsole echoConsole, params K[] options)
    : this(x, y, width, height, foreground, background, true, echoConsole, options)
        {

        }

        public Window(int x, int y, int width, int height, ConsoleColor foreground, ConsoleColor background,
            params K[] options) : this(x, y, width, height, foreground, background, true, null, options)
        {

        }

    }
}
