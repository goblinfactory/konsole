using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole
{
    public partial class Window
    {
        public void Write(string text)
        {
            _write(text);
        }

        //TODO: convert everything to redirect all calls to PrintAt, so that writing to parent works flawlessly!
        private void _write(string text)
        {
            if (_clipping && OverflowBottom)
                return;
            DoCommand(_echoConsole, () =>
            {
                var overflow = "";
                while (overflow != null)
                {
                    if (!_lines.ContainsKey(Cursor.Y)) return;
                    var result = _lines[Cursor.Y].WriteToRowBufferReturnWrittenAndOverflow(ForegroundColor, BackgroundColor, Cursor.X, text);
                    overflow = result.Overflow;
                    if (_echo && _echoConsole != null)
                    {
                        _echoConsole.ForegroundColor = ForegroundColor;
                        _echoConsole.BackgroundColor = BackgroundColor;
                        _echoConsole.PrintAt(CursorLeft + _x, CursorTop + _y, result.Written);
                    }
                    if (overflow == null)
                    {
                        Cursor = Cursor.IncX(text.Length);
                    }
                    else
                    {
                        Cursor = new XY(0, Cursor.Y + 1);
                        if (_clipping && OverflowBottom) break;
                        if (OverflowBottom)
                            ScrollDown();
                    }
                    text = overflow;
                }
            });
        }
    }
}
