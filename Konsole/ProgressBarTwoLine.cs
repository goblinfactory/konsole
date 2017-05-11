using System;

namespace Konsole
{
    public class ProgressBarTwoLine : IProgressBar
    {
        private int _max;
        private readonly char _character;
        private readonly IConsole _console;
        private int _y;
        private int _current = 1;
        private ConsoleColor _c;

        private const string FORMAT = "Item {0,-5} of {1,-5}. ({2,-3}%) ";

        public int Y
        {
            get { return _y; }
        }

        public string Line1
        {
            get { return _line1; }
        }

        public string Line2
        {
            get { return _line2; }
        }

        internal ProgressBarTwoLine(int max, int? textWidth, char character, IConsole console) 
        {
            lock (_locker)
            {
                _console = console;
                _y = _console.CursorTop;
                _c = _console.ForegroundColor;
                _current = 0;
                _max = max;
                _character = character;
                // reserve the space we need to print
                _console.WriteLine("");
                _console.WriteLine("");
            }
        }

        public int Max
        {
            get { return _max; }
            set
            {
                _max = value;
                Refresh(_current, _item);
            }
        }

        public void Refresh(int current, string format, params object[] args)
        {
            var item = string.Format(format, args);
            Refresh(current, item);
        }

        private static object _locker = new object();

        private string _line1 ="";
        private string _line2 ="";
        private string _item = "";

        public void Refresh(int current, string item)
        {
            lock (_locker)
            {
                _item = item;
                var itemText = item ?? "";
                var state = _console.State;
                _current = current;
                try
                {
                    float perc = (float) current/(float) _max;
                    var bar = new String(_character, (int) ((float) (_console.WindowWidth - 30)*perc));
                    var line = string.Format(FORMAT, current, _max, (int) (perc*100));
                    var barWhitespace = _console.WindowWidth - (bar.Length + line.Length  + 1);
                    _console.CursorTop = _y;
                    _console.CursorLeft = 0; // hard code to full screen. Later if we change this to support a width and offset we can revise this. good enough for now.
                    _console.ForegroundColor = _c;
                    _console.Write(line);
                    _console.ForegroundColor = ConsoleColor.Green;
                    _console.Write(bar);
                    if (barWhitespace>0)
                        _console.WriteLine(new String(' ',barWhitespace)); // clear the space to the right of the bar in case the bar has been made smaller.
                    else 
                        _console.WriteLine("");
                    _console.ForegroundColor = _c;
                    _line2 = itemText.PadRight(_console.WindowWidth - 2);
                    _console.WriteLine(_line2);
                    _line1 = $"{line} {bar}";
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

}

