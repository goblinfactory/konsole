using System;
using Konsole.Internal;
using static System.ConsoleColor;
namespace Konsole
{

    public class CharBar : IProgress
    {
        private int _max;
        private readonly IConsole _console;
        private int _y;
        private int _x;
        public int Width { get; }
        private static object _locker = new object();
        private char _barChar;

        public int Y => _y;
        public int X => _x;

        public ConsoleColor Color { get; }
        public CharBar(IConsole console, int x, int y, int width, int max, char barChar, ConsoleColor color)
        {
            lock (_locker)
            {
                Color = color;
                _barChar = barChar;
                _console = console;
                _x = x;
                _y = y;
                _current = 0;
                _max = max;
                Width = width;
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
                    int remaining = _console.WindowWidth - _x;      // e.g. window+width = 10, _x = 5, remaining = 5
                    int barWidth = Width;
                    int numBars = (int)(barWidth * perc);
                    var bar = _current > 0
                        ? new string(_barChar, numBars).PadRight(barWidth)
                        : new string(' ', barWidth);

                    if (Width > remaining) bar = bar.Substring(0, remaining);
                    _console.CursorTop = _y;
                    _console.CursorLeft = _x; 
                    _console.ForegroundColor = Color;
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

