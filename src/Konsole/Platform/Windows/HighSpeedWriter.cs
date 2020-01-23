using Konsole.Platform;
using Konsole.Platform.Windows;
using Microsoft.Win32.SafeHandles;
using System;
using static Konsole.Platform.Windows.Kernel32Draw;

// this class will later be changed to create a platform specific writer that it will delegate to depending
// on the actual platform. For now, we're hard coding support for windows to keep things as easy to work with 
// as possible.

namespace Konsole
{
    /// <summary>
    /// HighSpeedWriter is native to each platform. Currently only windows supported. The plan is to create more highspeed writers for more platforms.
    /// The highspeed writer completely replaces the native Console and unlike the normal Konsole.Window is not compatible with side by side console writing.
    /// i.e. you have to use Konsole for ALL console writing.
    /// </summary>
    public class HighSpeedWriter : IDisposable, IConsole, IHighspeedWriter
    {
        bool disposedValue = false;
        readonly short _height;
        readonly short _width;
        ConsoleRegion _consoleWriteArea;
        Scroller _scroller;
        CharAndColor[] _buffer;
        SafeFileHandle _consoleFileHandle;

        public bool AutoFlush { get; set; } = false;
        public char ClearScreenChar { get; set; }

        public HighSpeedWriter() : this((short)Console.WindowWidth, (short)Console.WindowHeight)
        {

        }

        public HighSpeedWriter(short width, short height, Colors defaultColors = null, char clearScreenChar = ' ')
        {
            PlatformStuff.EnsureRunningWindows();
            new PlatformStuff().LockResizing(width, height);
            _consoleFileHandle = OpenConsole();
            _height = height;
            _width = width;
            Colors = defaultColors ?? new Colors(ConsoleColor.Gray, ConsoleColor.Black);
            _buffer = new CharAndColor[_width * _height];
            _scroller = new Scroller(_buffer, _width, _height, clearScreenChar, Colors);
            _consoleWriteArea = new ConsoleRegion(0, 0, (short)(_width - 1), (short)(_height - 1));
            ClearScreen();
        }


        public void ClearScreen()
        {
            CharAndColor @char = Colors.Set(ClearScreenChar);
            for (int x = 0; x < _width; x++)
                for (int y = 0; y < _height; y++)
                {
                    int xy = y * _width + x;
                    _buffer[xy] = @char;
                }
        }

        private void doFlush()
        {
            if (AutoFlush) Flush();
        }
        public void Flush()
        {
            WriteConsoleOutputW(
                _consoleFileHandle,
                _buffer,
                new COORD() { X = _width, Y = _height },
                new COORD() { X = 0, Y = 0 }, ref _consoleWriteArea);
        }

        Colors IHighspeedWriter.Colors {
        get
            {
                return Colors;
            }
            set
            {
                Colors = value;
            }
        }

        public Colors Colors
        {
            get
            {
                return new Colors(ForegroundColor, BackgroundColor);
            }
            set
            {
                ForegroundColor = value.Foreground;
                BackgroundColor = value.Background;
            }
        }

        private short _backgroundColor;
        private short _foregroundColor;
        public short ColorAttributes {
            get { return (short)(_foregroundColor + ((short)(_backgroundColor << 4))); }
            }
        public ConsoleColor BackgroundColor { 
            get { return (ConsoleColor)_backgroundColor; }
            set { _backgroundColor = (short)value; }
        }

        public ConsoleColor ForegroundColor {
            get { return (ConsoleColor) _foregroundColor; }
            set { _foregroundColor = (short)value; }
        }

        //TODO: Move this to default interface implementations C#8
        public ConsoleState State
        {
            get { return new ConsoleState(ForegroundColor, BackgroundColor, CursorTop, CursorLeft, CursorVisible); }
            set
            {
                ForegroundColor = value.ForegroundColor;
                BackgroundColor = value.BackgroundColor;
                CursorTop = value.Top;
                CursorLeft = value.Left;
            }
        }


        public int AbsoluteX => 0;

        public int AbsoluteY => 0;

        public int WindowWidth => _width;

        public int WindowHeight => _height;

        public int CursorTop { get => Console.CursorTop; set => Console.CursorTop = value; }
        public int CursorLeft { get => Console.CursorLeft; set => Console.CursorLeft = value; }

        public bool CursorVisible { get => Console.CursorVisible; set => Console.CursorVisible = value;  }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                _consoleFileHandle.Dispose();
                disposedValue = true;
            }
        }

         ~HighSpeedWriter()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void DoCommand(IConsole console, Action action)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void PrintAt(int x, int y, string text)
        {
            // int theory there should be no overflow because Window._write has already taken care of that. 
            // so all I need to do is handle printing one line of text (just the buffer for that text)
            int span = text.Length;
            for(int i = 0; i< span; i++)
            {
                PrintAt(x + i, y, text[i]);
            }
            doFlush();
        }

        private CharAndColor ToCell(char c)
        {
            return new CharAndColor { Char = new CharInfo { UnicodeChar = c }, Attributes = ColorAttributes };
        }
        public void PrintAt(int x, int y, char c)
        {
            _buffer[x + y * _width] = ToCell(c);
        }

        public void PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void ScrollDown()
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void Clear()
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void Clear(ConsoleColor? backgroundColor)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            _scroller.MoveBufferArea(sourceLeft, sourceTop - 1, sourceWidth, sourceHeight, targetLeft, targetTop -1);
        }

        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void WriteLine(string format, params object[] args)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void Write(string format, params object[] args)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void WriteLine(string text)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void Write(string text)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void Write(ConsoleColor color, string text)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }

        public void WriteLine(ConsoleColor color, string text)
        {
            throw new InvalidOperationException("Please use a window based off the writer to do any writing!");
        }
    }
}

