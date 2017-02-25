using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole
{
    public class Window
    {
        private readonly BufferedWriter _parentWindow;
        private readonly int _x;
        private readonly int _y;
        private readonly int _width;
        private readonly int _height;
        private readonly bool _show;
        private readonly BufferedWriter _buffer;

        public Window(IConsole console, int x, int y, int width, int height)
        {
            
        }

        public string[] Buffer { get; set; }

        //public Window(BufferedWriter parentWindow,  int x, int y, int width, int height, bool show)
        //{
        //    _parentWindow = parentWindow;
        //    _buffer = new BufferedWriter(width, height, show);
        //    _x = x;
        //    _y = y;
        //    _width = width;
        //    _height = height;
        //    _show = show;
        //}


        public Window WriteLine(string p0)
        {
            return this;
        }

        public Window newWindow(int i, int i1, int i2, int i3)
        {
            throw new NotImplementedException();
        }
    }
}
