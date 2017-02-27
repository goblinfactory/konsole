using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole
{

    public class Window : IConsole
    {
        private readonly BufferedWriter _console;
        private readonly IConsole _parent;
        private readonly int _x;
        private readonly int _y;

        public Window(IConsole parent, int x, int y, int width, int height)
        {
            _parent = parent;
            _x = x;
            _y = y;
            _console = new BufferedWriter(width,height);
        }

        /// <summary>
        /// prints the state of the current buffer to parent. This is so that we can cater for the windows own overflow settings.
        /// I did consider simply passing the prints to the parent via an offset, i.e. all WRiteLine to convert to PrintAt but then overflows and wrapping would not work.
        /// </summary>
        private void Refresh()
        {
            int y = 0;
            foreach (var line in _console.Buffer)
            {
                _parent.PrintAt(_x, _y+y, line);
                //foreach (var c in line)
                //{
                //    _parent.PrintAt(_x + x, _y + y, c);
                //    x++;
                //}
                y++;
            }
        }

        public void WriteLine(string format, params object[] args)
        {
            _console.WriteLine(format,args);
            Refresh();
        }

        public void Write(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public int WindowWidth()
        {
            throw new NotImplementedException();
        }

        public int CursorTop { get; set; }
        public int CursorLeft { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
        public void SetCursorPosition(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, string text)
        {
            throw new NotImplementedException();
        }

        public void PrintAt(int x, int y, char c)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            // mmm, if echo is on, then this would clear the whole screen instead of just this window? will need to 
            throw new NotImplementedException();
        }
    }
}
