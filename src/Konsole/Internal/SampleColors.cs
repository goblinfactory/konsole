using Konsole;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole.Internal
{
    public static class SampleColors
    {
        public static void PrintSampleColors(IConsole console)
        {
            //TODO: Change this to print color names
            // and also print to RGB
            console.DoCommand(console, () => {
                for (int back = 0; back < 16; back++)
                {
                    console.Write($"back[{back}]");
                    for (int i = 0; i < 16; i++)
                    {
                        console.BackgroundColor = (ConsoleColor)back;
                        var color = (ConsoleColor)i;
                        console.Write(color, $" {i} ");
                    }
                }
            });
        }
    }
}
