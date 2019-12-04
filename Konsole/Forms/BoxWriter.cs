using System.Text;
using Konsole.Internal;

namespace Konsole.Forms
{
    public class BoxWriter : IBoxWriter
    {
        private readonly IBoxStyle _lines;
        private readonly int _width;            // total width of the dialog, including lines
        private readonly int _captionWidth;     // width of the widest caption
        private readonly int _textWidth;        // space left over to display value text, taking into account space needed for seperator  ' : '
        private string _padding;                // number of characters to pad with, normally 1.
        private readonly int _insideWidth;      // total width of the inside of the dialog, normally totalwidth -2

        public BoxWriter(IBoxStyle lines, int width, int captionWidth, int padding)
        {
            _lines = lines;
            _captionWidth = captionWidth;
            _padding = new string(' ', padding);
            _insideWidth = width - 2;
            // textwidth = insideWidth - 3 (' : ') - captionWidth
            _textWidth = _insideWidth - (captionWidth + 3);
            _width = width;
        }

        public string Header(string title)
        {
            var sb = new StringBuilder();
            // left line, 1 + title+ 1, right right
            int linelen = (_insideWidth - title.Length) / 2 - 1;
            var leftline = string.Format("{0}{1}", _lines.TL, new string(_lines.T, linelen));
            var rightline = string.Format("{0}{1}", new string(_lines.T, linelen), _lines.TR);
            string spacer = new string(' ', _insideWidth - (leftline.Length + title.Length + rightline.Length)) + " ";
            var topline = string.Format("{0}{1} {2}{3}{4}", _padding, leftline, title, spacer, rightline);
            return topline;
        }

        public string Footer
        {
            get
            {
                return string.Format("{0}{1}{2}{3}", _padding, _lines.BL, new string(_lines.T, _insideWidth), _lines.BR);
            }
        }

        public string Line
        {
            get
            {
                return string.Format("{0}{1}{2}{3}", _padding, _lines.LJ, new string(_lines.T, _insideWidth), _lines.RJ);
            }
        }

        public string Write(string text)
        {
            // if text overflows, add in ellispses
            string writeText = text.Length > _insideWidth
                ? text.Substring(0, _insideWidth - 3) + "..."
                : text.FixLeft(_insideWidth);
            return string.Format("{0}{1}{2}{3}", _padding, _lines.L, writeText, _lines.R);
        }
    }
}