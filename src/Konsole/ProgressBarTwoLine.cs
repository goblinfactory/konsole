using System;

namespace Konsole
{
    public class ProgressBarTwoLine : IProgressBar
    {
        private int _max;
        private readonly char _character;
        private readonly IConsole _console;
        private int _y;
        private int _current = 0;
        private ConsoleColor _c;
        private string _item = "";

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

        /// <summary>
        /// this is the item that the progressbar represents, e.g, "MyFiles.zip"
        /// </summary>
        public string Item
        {
            get { return _item; }
            set
            {
                bool changed = _item != value;
                _item = value;
                if (changed) Refresh(Current, Item);
            }
        }

        public int Current {  get { return _current; } }

        //todo: test for textWidth
        //todo: test for setting max to 0...avoid divide by zero exception!

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
                Refresh(_current, Item);
            }
        }

        public void Refresh(int current)
        {
            Refresh(current, Item);
        }

        public void Refresh(int current, string format, params object[] args)
        {
            var item = string.Format(format, args);
            Refresh(current, item);
        }

        private static object _locker = new object();

        private string _line1 ="";
        private string _line2 ="";

        public void Refresh(int current, string item)
        {
            lock (_locker)
            {
                _item = item ?? "";
                var itemText = item ?? "";
                var state = _console.State;
                _current = current;
                try
                {
                    decimal perc = Max > 0 ? (decimal) current/(decimal) _max : 0;
                    var bar = new string(_character, (int) (((decimal) (_console.WindowWidth - 30))*perc));
                    var line = string.Format(FORMAT, current, _max, (int) (perc*100));
                    var barWhitespace = _console.WindowWidth - (bar.Length + line.Length  + 1);
                    _console.CursorTop = _y;
                    _console.CursorLeft = 0;
                    _console.ForegroundColor = _c;
                    _console.Write(line);
                    _console.ForegroundColor = ConsoleColor.Green;
                    _console.Write(bar);
                    _console.WriteLine(barWhitespace > 0 ? new String(' ', barWhitespace) : "");
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

