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
                // save current position
                _current = current;
                try
                {
                    float perc = (float) current/(float) _max;
                    var bar = new String(_character, (int) ((float) (_console.WindowWidth() - 30)*perc));
                    var line = string.Format(_format, current, _max, (int) (perc*100));
                    _console.CursorTop = _y;
                    _console.ForegroundColor = _c;
                    _console.Write(line);
                    _console.ForegroundColor = ConsoleColor.Green;
                    _console.WriteLine(bar);
                    _console.ForegroundColor = _c;
                    _console.WriteLine(item.PadRight(_console.WindowWidth()-2));

                }
                finally
                {
                    _console.ForegroundColor = _c;
                }
            }
        }

        public void Next(string item)
        {
            _current++;
            Refresh(_current, item);
        }
    }

}

