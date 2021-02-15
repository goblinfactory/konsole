using System;
using System.Collections.Generic;
using System.Text;
using Konsole.Internal;

namespace Konsole.Samples
{
    public static class ControlSample
    {
        public class MyInput : Control<MyInput, string>
        {
            private string _text = "";
            private int cursorPos = 0;
            public override XY? Cursor => new XY(SX + cursorPos, Y);

            public override string Value => _text;

            public MyInput(IConsole console, string caption, int? captionWidth) : base(console, null, null, caption, captionWidth, null, null)
            {

            }

            public MyInput(IConsole console, string caption) : base(console, null, null, caption, null, null, null)
            {

            }

            public override (bool isDirty, bool handled) HandleKeyPress(ConsoleKeyInfo info, char key)
            {
                if (info.Key.IsAlphaNumeric())
                {
                    _text = $"{_text}{info.KeyChar}";
                    return (true, true);
                }
                return (false, false);
            }

            protected override void Render(ControlStatus status, Style style)
            {
                switch (status)
                {
                    case ControlStatus.Active:
                        _console.PrintAt(style.Body, SX, SY, $" [{_text.FixLeft(10)}] ");
                        break;
                    case ControlStatus.InactiveSelected:
                        _console.PrintAt(style.SelectedItem, SX, SY, $"[[{_text.FixLeft(10)}]]");
                        break;
                    case ControlStatus.Disabled:
                        _console.PrintAt(style.Body, SX, SY, $"  {_text.FixLeft(10)}  ");
                        break;
                    case ControlStatus.Inactive:
                        _console.PrintAt(style.Body, SX, SY, $"  {_text.FixLeft(10)}  ");
                        break;
                }
            }
        }


        public static void Demo(IConsole console)
        {
            var t1 = new MyInput(console, "Name", 10);
            var t2 = new MyInput(console, "Initials", 10);
            var t3 = new MyInput(console, "Last Name", 10);
            
            t1.HandleKeyPresses("cats");
            t2.HandleKeyPresses("foo");
        }
    }
}
