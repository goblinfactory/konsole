using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole
{
    public class Colors
    {
        public ConsoleColor Foreground { get; } = ConsoleColor.White;
        public ConsoleColor Background { get; } = ConsoleColor.Black;

        public Colors()
        {
            
        }

        public Colors(ConsoleColor foreground, ConsoleColor background)
        {
            Foreground = foreground;
            Background = background;
        }
    }
}
