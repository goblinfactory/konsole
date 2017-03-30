using System;

namespace Konsole
{
    public class ProgressBar
    {
        private int _max;
        private readonly char _character;
        private readonly IConsole _console;
        private readonly string _format;
        private int _y;
        private int _current = 1;
        private ConsoleColor _c;

        public const string FORMAT = "Item {0,-5} of {1,-5}. ({2,-3}%) ";

        public ProgressBar(int max) : this(max, '#', FORMAT, new Writer(), "") { }
        public ProgressBar(int max, string title) : this(max, '#', FORMAT, new Writer(), title) { }
        public ProgressBar(int max, IConsole console) : this(max, '#', FORMAT, console, "") { }

        /// <summary>
        ///  private hook to make this class easier to test in multithreaded scenarios.
        /// </summary>
        protected Action OnConstructor = () => { };

        public int Y
        {
            get { return _y; }
        }

        public string Title { get; private set; }

        public string Line1
        {
            get { return _line1; }
        }

        public string Line2
        {
            get { return _line2; }
        }

        public ProgressBar(int max, char character, string format, IConsole console) : this(max, character, format, console, "") { }

        public ProgressBar(int max, char character, string format, IConsole console, string title)
        {
            lock (_locker)
            {
                Title = title;
                _console = console;
                _y = _console.CursorTop;
                _c = _console.ForegroundColor;
                _current = 0;
                _max = max;
                _character = character;
                _format = format ?? FORMAT;
                // reserve the space we need to print
                if (!string.IsNullOrWhiteSpace(Title))
                {
                    _console.WriteLine(Title);
                }
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
                    var line = string.Format(_format, current, _max, (int) (perc*100));
                    _console.CursorTop = _y;
                    _console.CursorLeft = 0; // hard code to full screen. Later if we change this to support a width and offset we can revise this. good enough for now.
                    _console.ForegroundColor = _c;
                    if (!string.IsNullOrWhiteSpace(Title))
                    {
                        _console.WriteLine(Title);
                    }
                    _console.Write(line);
                    _console.ForegroundColor = ConsoleColor.Green;
                    _console.WriteLine(bar);
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

