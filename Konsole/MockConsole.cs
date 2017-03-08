using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole
{
    /// <summary>
    /// MockConsole - is a default window with width of 120 and height of 60, White on Black background, that will not echo to real console
    /// that has window state, colors, cursor, text written, that will simulate (quite well) a real console.
    /// </summary>
    public class MockConsole : Window
    {
        public MockConsole(int x, int y, params K[] options) : base(x, y, null, options) { }
        public MockConsole() : base(120, 60, false) { }
        public MockConsole(int width, int height) : base(width, height, false) { }
    }
}
