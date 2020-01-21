using System;
using Konsole.Internal;

namespace Konsole
{

    public class ProgressBarSlim : IProgressBar
    {
        private int _max;
        private readonly char _character;
        private readonly IConsole _console;
        private int _y;
        private int _current = 0;
        private ConsoleColor _c;
        private static object _locker = new object();

        private string _line = "";
        private string _item = "";

        public int TextWidth { get; } = 30;

        public int Y => _y;

        [Obsolete("Not used in this 'slim' 1 liner implementation of a progress bar.")]
        public string Line2 => "";

        /// <summary>
        /// the resulting rendered text line, e.g. "MyFiles.zip : ( 50%) ########"
        /// </summary>
        public string Line1 => _line;

        /// <summary>
        /// this is the item that the progressbar represents, e.g, "MyFiles.zip"
        /// </summary>
        public string Item
        {
            get { return _item; }
            set
            {
                _item = value;
                Refresh(Current, Item);
            }
        }

        public ProgressBarSlim(int max)                                  : this(max, '#', new Writer()) { }
        public ProgressBarSlim(int max, int textWidth)                   : this(max, textWidth,'#', new Writer()) { }
        public ProgressBarSlim(int max, int textWidth, char character)   : this(max, textWidth, character, new Writer()) { }
        public ProgressBarSlim(int max, IConsole console)                : this(max, null, '#', console) { }
        public ProgressBarSlim(int max, int textWidth, IConsole console) : this(max, textWidth, '#', console) { }

        public ProgressBarSlim(int max, int? textWidth, char character, IConsole console)
        {
            lock (_locker)
            {
                TextWidth = GetTextWidth(console, textWidth);
                _console = console;
                _y = _console.CursorTop;
                _c = _console.ForegroundColor;
                _current = 0;
                _max = max;
                _character = character;
                _console.WriteLine("");
            }
        }

        internal static int GetTextWidth(IConsole console, int? width)
        {
            return width.HasValue
                ? width.Value
                : console.WindowWidth/4;
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

        public int Current => _current;

        public void Refresh(int current, string format, params object[] args)
        {
            var text = string.Format(format, args);
            Refresh(current, text);
        }


        public void Refresh(int current, string itemText)
        {
            var item = itemText ?? "";
            var clippedText = item.FixLeft(TextWidth);
            lock (_locker)
            {                
                _item = item;
                var state = _console.State;
                _current = current.Max(Max);
                try
                {
                    decimal perc = _max == 0 ? 0 : (decimal) _current/(decimal) _max;
                    int barWidth = _console.WindowWidth - (TextWidth+8);
                    var bar = _current > 0
                        ? new string(_character, (int) ((decimal) (barWidth)*perc)).PadRight(barWidth)
                        : new string(' ', barWidth);
                    var text = string.Format("{0} ({1,-3}%) ", clippedText, (int) (perc*100));
                    _console.CursorTop = _y;
                    _console.CursorLeft = 0; 
                    _console.ForegroundColor = _c;
                    _console.Write(text);
                    _console.ForegroundColor = ConsoleColor.Green;
                    _console.Write(bar);
                    _line = $"{text} {bar}";
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

