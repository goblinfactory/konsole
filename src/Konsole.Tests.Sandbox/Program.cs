using System;
using System.Threading.Tasks;
using static System.ConsoleColor;

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
            Console.SetWindowSize(90, 30);
            var window = new Window();
            var (top, bot) = window.SplitTopBottom("top", "bottom");
            var (left, right) = bot.SplitLeftRight("left", "right");
            
            // disable the bottom right for demo purposes.
            // all controls and text inside the right window
            // will be rendered in Disabled styling.
            right.Enabled = false;                                      

            //await window.RunAsync();

            // at this point we should be able to print the window layout...
            // ask each window recursively to dump .. their configuration?
            // window should report it's child windows as well by call
            PrintWindowTitles(top, window, 2);
            top.WriteLine(Yellow, "Press enter to quit");
            Console.ReadLine();
        }

        private static void PrintWindowTitles(IConsole output, IConsole window, int indent) {
            var spacer = new string(' ', indent * 2);
            foreach (var win in window.Manager.ConsolesByTabOrder())
            {
                output.WriteLine($"{spacer}{win.Title}");

                foreach (var win2 in win?.Manager?.ConsolesByTabOrder() ?? new IConsole[0])
                {
                    PrintWindowTitles(output, win2, indent + 1);
                }
            }

        }
    }
}
