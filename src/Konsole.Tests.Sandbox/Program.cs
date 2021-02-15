using System;
using System.Threading.Tasks;

namespace Konsole.Tests.RunOne
{

    class Program
    {
        // consider using ControlSample! there are the start of some automation tests.
        // TODO: experiment with control composition, e.g. CharBoxes for password, with navigation being "captured" by control until finished with exit key, e.g. enter or tab.
        // All "windows" will become keyHandlers and will delgate jeyHandling "down", the reverse of how print commands delegated UP the compositional hierarchy.
        static async Task Main(string[] args)
        {
            Console.CursorVisible = false;

            var win = new Window();
            var (top, bot) = win.SplitTopBottom("top", "bottom");
            var (left, right) = bot.SplitLeftRight("left", "right");

            //await win.RunAsync();
        }
    }
}
