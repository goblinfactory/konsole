using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole.Drawing;

namespace Konsole
{
    public class WindowSettings
    {
        // need validations to confirm allowed settings (write validation tests and requirements first)
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public bool Echo { get; set; } = false;
        public IConsole EchoConsole { get; set; }
        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;
        public int? Height { get; set; } = null;
        public int? Width { get; set; } = null;
        public bool FillBackground { get; set; } = false;

        public WindowSettings Clone()
        {
            var settings = (WindowSettings)MemberwiseClone();
            return settings;
        }
    }
}
