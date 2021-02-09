using System;
using Konsole.Internal;
using static System.ConsoleColor;
namespace Konsole
{

    public class ProgressCharBar : IProgress
    {
        private int _max;
        private readonly IConsole _console;
        private int _y;
        private static object _locker = new object();
        private char _barChar;

        public int Y => _y;

        public ProgressCharBar()                                          : this(new Writer(), 100, '#', Green) { }
        public ProgressCharBar(int max)                                   : this(new Writer(), max, '#', Green) { }
        
        public ProgressCharBar(ConsoleColor color)                        : this(new Writer(), 100, '#', color) { }
        public ProgressCharBar(int max, char barChar)                     : this(new Writer(), max, barChar, Green) { }

        public ProgressCharBar(int max, char barChar, ConsoleColor color) : this(new Writer(), max, barChar, color) { }

        public ProgressCharBar(IConsole console)                          : this(console, 100, '#', Green) { }
        public ProgressCharBar(IConsole console, int max)                 : this(console, max, '#', Green) { }
        public ProgressCharBar(IConsole console, ConsoleColor color)      : this(console, 100, '#', color) { }
        public ProgressCharBar(IConsole console, int max, char barChar)   : this(console, max, barChar, Green) { }

        public ConsoleColor BarColor { get; set; }

        public ProgressCharBar(IConsole console, int max, char barChar, ConsoleColor color)
        {
            lock (_locker)
            {
                BarColor = color;
                _barChar = barChar;
                _console = console;
                _y = _console.CursorTop;
                _current = 0;
                _max = max;
                _console.WriteLine("");
            }
        }

        public int Max
        {
            get { return _max; }
            set
            {
                _max = value;
                Refresh(_current);
            }
        }

        private int _current = 0;
        public int Current => _current;
        
        public void Refresh()
        {
            Refresh(_current);
        }
        public void Refresh(int current)
        {
            lock (_locker)
            {                
                var state = _console.State;
                _current = current.Max(Max);
                try
                {
                    decimal perc = _max == 0 ? 0 : (decimal) _current/(decimal) _max;
                    int barWidth = _console.WindowWidth;
                    var bar = _current > 0
                        ? new string(_barChar, (int) ((decimal) (barWidth)*perc)).PadRight(barWidth)
                        : new string(' ', barWidth);
                    _console.CursorTop = _y;
                    _console.CursorLeft = 0; 
                    _console.ForegroundColor = BarColor;
                    _console.Write(bar);
                }
                finally
                {
                    _console.State = state;
                }
            }
        }

        public void Next()
        {
            _current++;
            Refresh(_current);
        }


    }

}

