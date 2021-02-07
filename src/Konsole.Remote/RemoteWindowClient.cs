using System;

namespace Konsole.Remote
{
    public class RemoteWindowClient : IConsole
    {
        public ConsoleState State { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int CursorTop { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int CursorLeft { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CursorVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ConsoleColor ForegroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ConsoleColor BackgroundColor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Colors Colors { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int AbsoluteX => throw new NotImplementedException();

        public int AbsoluteY => throw new NotImplementedException();

        public int WindowWidth => throw new NotImplementedException();

        public int WindowHeight => throw new NotImplementedException();

        public StyleTheme Theme { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Style Style => throw new NotImplementedException();

        public ControlStatus Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Clear(ConsoleColor? backgroundColor)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void DoCommand(IConsole console, Action action)
        {
            throw new NotImplementedException();
        }

        public void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(Colors colors, int x, int y, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(Colors colors, int x, int y, string text)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(Colors colors, int x, int y, char c)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(ConsoleColor color, int x, int y, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(ConsoleColor color, int x, int y, string text)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(ConsoleColor color, int x, int y, char c)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, string text)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, char c)
        {
            throw new NotImplementedException();
        }

        public void ScrollDown()
        {
            throw new NotImplementedException();
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Write(ConsoleColor color, string text)
        {
            throw new NotImplementedException();
        }

        public void Write(Colors colors, string text)
        {
            throw new NotImplementedException();
        }

        public void Write(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Write(string text)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(ConsoleColor color, string text)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(Colors colors, string text)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string text)
        {
            throw new NotImplementedException();
        }
    }
}
