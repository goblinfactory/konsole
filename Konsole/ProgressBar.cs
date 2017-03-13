using System;

namespace Konsole
{
    public class ProgressBar
    {
        private readonly int _max;
        private readonly char _character;
        private readonly IConsole _console;
        private readonly string _format;
        private int _y;
        private int _current = 1;
        private ConsoleColor _c;

        public const string FORMAT = "Item {0,-5} of {1,-5}. ({2,-3}%) ";

        public ProgressBar(int max) : this(max, '#', FORMAT, new Writer()) { }
        public ProgressBar(int max, IConsole console) : this(max, '#', FORMAT, console) { }
        public ProgressBar(int max, char character, string format, IConsole console)
        {
            _console = console;
            _y = _console.CursorTop;
            _c = _console.ForegroundColor;
            _current = 0;
            _max = max;
            _character = character;
            _format = format ?? FORMAT; 
            _console.WriteLine("");
            _console.WriteLine("");
        }

        public int Max { get { return _max; }}

        public void Refresh(int current, string format, params object[] args)
        {
            var item = string.Format(format, args);
            Refresh(current, item);
        }

        private static object _locker = new object();



        public void Refresh(int current, string item)
        {
            lock (_locker)
            {
                var state = _console.State;
                _current = current;
                try
                {
                    float perc = (float) current/(float) _max;
                    var bar = new String(_character, (int) ((float) (_console.WindowWidth - 30)*perc));
                    var line = string.Format(_format, current, _max, (int) (perc*100));
                    _console.CursorTop = _y;
                    _console.CursorLeft = 0; // hard code to full screen. Later if we change this to support a width and offset we can revise this. good enough for now.
                    _console.ForegroundColor = _c;
                    _console.Write(line);
                    _console.ForegroundColor = ConsoleColor.Green;
                    _console.WriteLine(bar);
                    _console.ForegroundColor = _c;
                    _console.WriteLine(item.PadRight(_console.WindowWidth-2));

                }
                finally
                {
                    _console.State = state;
                }
            }
        }

        public void Next(string item)
        {
            _current++;
            Refresh(_current, item);
        }
    }

    public class ConsoleState
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        // for now, not including any width or height settings. (don't know if these can be changed? none of our code does so leaving off.)
        public ConsoleState(ConsoleColor foreground, ConsoleColor background, int top, int left) // NB always X then Y .. need to swap these around
        {
            ForegroundColor = foreground;
            BackgroundColor = background;
            Top = top;
            Left = left;
        }
    }


}

