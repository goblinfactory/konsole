using System;
using Konsole.Internal;

namespace Konsole
{
    public enum PbStyle
    {
        SingleLine,
        DoubeLine
    }

    public class ProgressBarSlim
    {
        private int _max;
        private readonly char _character;
        private readonly IConsole _console;
        private readonly string _format;
        private int _y;
        private int _current = 0;
        private ConsoleColor _c;
        private static object _locker = new object();
        private string _line = "";
        private string _item = "";
        private PbStyle Style { get; } = PbStyle.SingleLine;

        public int TextWidth { get; } = 30;

        public int Y
        {
            get { return _y; }
        }

        public string Line
        {
            get { return _line; }
        }

        public string Item
        {
            get { return _item; }
            set
            {
                _item = value;
                Refresh(Current, Item);
            }
        }

        public ProgressBarSlim(int max) : this(max, PbStyle.SingleLine, '#', new Writer()) { }
        public ProgressBarSlim(int max, PbStyle style) : this(max, style, null, '#', new Writer()) { }
        public ProgressBarSlim(int max, PbStyle style, int textWidth) : this(max, style, textWidth,'#', new Writer()) { }
        public ProgressBarSlim(int max, PbStyle style, int textWidth, char character) : this(max, style, textWidth, character, new Writer()) { }

        public ProgressBarSlim(int max, IConsole console) : this(max, PbStyle.SingleLine,null, '#', console) { }
        public ProgressBarSlim(int max, PbStyle style, IConsole console) : this(max,style, null, '#', console) { }
        public ProgressBarSlim(int max, PbStyle style, int textWidth, IConsole console) : this(max, style, textWidth, '#', console) { }
        public ProgressBarSlim(int max, PbStyle style, int? textWidth, char character, IConsole console)
        {
            TextWidth = GetTextWidth(console, textWidth);
            lock (_locker)
            {
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
                    float perc = (float) _current/(float) _max;
                    int barWidth = _console.WindowWidth - (TextWidth+8);
                    var bar = _current > 0
                        ? new string(_character, (int) ((float) (barWidth)*perc)).PadRight(barWidth)
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

