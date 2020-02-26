
namespace Konsole
{
    public enum PbStyle {  SingleLine, DoubleLine }
    public class ProgressBar : IProgressBar
    {

        private IProgressBar _bar;

        public int Y => _bar.Y;
        public string Line1 => _bar.Line1; 
        public string Line2 => _bar.Line2;

        public ProgressBar(int max)                                                 : this(max, null,'#', PbStyle.SingleLine, Window.HostConsole) { }
        public ProgressBar(int max, int textWidth)                                  : this(max, textWidth, '#', PbStyle.SingleLine, Window.HostConsole) { }
        public ProgressBar(int max, int textWidth, char character)                  : this(max, textWidth, character, PbStyle.SingleLine, Window.HostConsole) { }
        public ProgressBar(PbStyle style, int max)                                  : this(max, null, '#', style, Window.HostConsole) { }
        public ProgressBar(PbStyle style, int max, int textWidth)                   : this(max, textWidth, '#', style, Window.HostConsole) { }
        public ProgressBar(PbStyle style, int max, int textWidth, char character)   : this(max, textWidth, character, style, Window.HostConsole) { }

        public ProgressBar(IConsole console, int max)                                                 : this(max, null,'#', PbStyle.SingleLine, console) { }
        public ProgressBar(IConsole console, int max, int textWidth)                                  : this(max, textWidth, '#', PbStyle.SingleLine, console) { }
        public ProgressBar(IConsole console, int max, int textWidth, char character)                  : this(max, textWidth, character, PbStyle.SingleLine, console) { }
        public ProgressBar(IConsole console, PbStyle style, int max)                                  : this(max, null, '#', style, console) { }
        public ProgressBar(IConsole console, PbStyle style, int max, int textWidth)                   : this(max, textWidth, '#', style, console) { }
        public ProgressBar(IConsole console, PbStyle style, int max, int textWidth, char character)   : this(max, textWidth, character, style, console) { }

        private static object _locker = new object();

        // in the private constructor IConsole is right at the end so that it does not clash with the other signatures
        private ProgressBar(int max, int? textWidth, char character, PbStyle style, IConsole console)
        {
            lock(Window._locker)
            {
                switch (style)
                {
                    case PbStyle.DoubleLine:
                        _bar = new ProgressBarTwoLine(max, textWidth, character, console);
                        break;
                    case PbStyle.SingleLine:
                        _bar = new ProgressBarSlim(max, textWidth, character, console);
                        break;
                }
            }
        }

        public int Max
        {
            get { return _bar.Max; }
            set { _bar.Max = value; }
        }

        public void Refresh(int current, string format, params object[] args)
        {
            _bar.Refresh(current,format, args);
        }

        public void Refresh(int current, string item)
        {
            _bar.Refresh(current,item);
        }

        public void Next(string item)
        {
            _bar.Next(item);
        }

    }
}