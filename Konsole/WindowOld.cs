//using System;
//using System.Text;
//using Konsole.Internal;

//namespace Konsole
//{

//    internal static class WindowRefresher
//    {
//        private static bool SameColors(this Cell prev, Cell current)
//        {
//            return (prev.Background != current.Background || prev.Foreground != current.Foreground);
//        }

//        public static void Refresh(int _x, int _y, IConsole _parent, BufferedWriter windowBuffer)
//        {
//            // maybe none of this is needed, just need to make the buffered writer support window offsets?
//            return;
//            // locking?
//            var fore = _parent.ForegroundColor;
//            var back = _parent.BackgroundColor;
//            try
//            {
//                int y = 0;
//                int partStart = 0;
//                foreach (var line in windowBuffer.BufferWritten)
//                {
//                    var sb = new StringBuilder(windowBuffer.WindowWidth());
//                    Cell prev, curr;
//                    for (int i = 0; i < line.Length; i++)
//                    {
//                        bool lastCharInLine = i == (line.Length);
//                        prev = (i == 0) ? windowBuffer[i, y] : windowBuffer[i-1, y];
//                        curr = windowBuffer[i, y];
//                        if (SameColors(prev, curr) && !lastCharInLine)
//                        {
//                            sb.Append(curr.Char);
//                        }
//                        else
//                        {
//                            _parent.ForegroundColor = prev.Foreground;
//                            _parent.BackgroundColor = prev.Background;
//                            _parent.PrintAt(_x + partStart, _y + y, sb.ToString());
//                            partStart = i;
//                            sb.Clear();
//                            if (!lastCharInLine) sb.Append(curr.Char);
//                        }
//                    }
//                    y++;
//                }

//            }
//            finally
//            {
//                _parent.ForegroundColor = fore;
//                _parent.BackgroundColor = back;
//            }

//        }
//    }

//    public class Window : IConsole
//    {
//        private readonly BufferedWriter _console;
//        private readonly IConsole _parent;
//        private readonly int _x;
//        private readonly int _y;

//        public Window(int x, int y, int width, int height) : this(new Writer(), x, y, width, height)
//        {
//        }

//        public Window(IConsole parent, int x, int y, int width, int height)
//        {
//            _parent = parent;
//            _x = x;
//            _y = y;
//            _console = new BufferedWriter(width, height);
//        }




//        /// <summary>
//        /// prints the state of the current buffer to parent. This is so that we can cater for the windows own overflow settings.
//        /// I did consider simply passing the prints to the parent via an offset, i.e. all WRiteLine to convert to PrintAt but then overflows and wrapping would not work.
//        /// </summary>
//        private void Refresh()
//        {
//            WindowRefresher.Refresh(_x, _y, _parent, _console);
//        }

//        public void WriteLine(string format, params object[] args)
//        {
//            _console.WriteLine(format,args);
//            Refresh();
//        }

//        public void Write(string format, params object[] args)
//        {
//            throw new NotImplementedException();
//        }

//        public int WindowWidth()
//        {
//            throw new NotImplementedException();
//        }

//        public int CursorTop {
//            get
//            {
//                throw new NotImplementedException();
//            }
//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public int CursorLeft
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public ConsoleColor ForegroundColor {
//            get { return _console.ForegroundColor; }
//            set { _console.ForegroundColor = value; }
//        }

//        public ConsoleColor BackgroundColor
//        {
//            get { return _console.BackgroundColor; }
//            set { _console.BackgroundColor = value; }
//        }

//    public void SetCursorPosition(int x, int y)
//        {
//            throw new NotImplementedException();
//        }

//        public void PrintAt(int x, int y, string format, params object[] args)
//        {
//            throw new NotImplementedException();
//        }

//        public void PrintAt(int x, int y, string text)
//        {
//            throw new NotImplementedException();
//        }

//        public void PrintAt(int x, int y, char c)
//        {
//            throw new NotImplementedException();
//        }

//        public void Clear()
//        {
//            // mmm, if echo is on, then this would clear the whole screen instead of just this window? will need to 
//            throw new NotImplementedException();
//        }
//    }
//}
