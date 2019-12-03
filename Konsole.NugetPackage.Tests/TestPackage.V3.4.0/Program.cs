using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;
using Konsole.Layouts;

namespace TestPackage.V3._4._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
        }
        static void Test1()
        {
            var pb = new ProgressBar(10);

            for (int i =0; i<11; i++)
            {
                pb.Refresh(i, "cat "+ i);
                Console.ReadKey(true);
            }
            
            Console.ReadKey(true);
            var w = new Window();
            var left = w.SplitLeft("left");
            w.SplitRight("right");
            var top = left.SplitTop("top");
            var bottom = left.SplitBottom("bottom");

            for (int i = 0; i < 10; i++)
            {
                top.WriteLine(i.ToString());
                bottom.WriteLine((100-i).ToString());
            }
            Console.ReadKey();
        }

        static void Test2()
        {
            var main_window_height = Console.WindowHeight;
            var main_window_width = Console.WindowWidth;

            var mainWindow = new Window(0, 0, main_window_width, main_window_height);

            var progress_window_height = 4;
            var command_window_height = 3;
            var window_padding = 2;

            var progress_console = default(IConsole);
            var logging_console = default(IConsole);
            var command_console = default(IConsole);

            progress_console = Window.Open(0, 0, main_window_width, progress_window_height, "operation progress", Konsole.Drawing.LineThickNess.Single);
            command_console = Window.Open(0, main_window_height - command_window_height, main_window_width, command_window_height, "system command", Konsole.Drawing.LineThickNess.Single);
            logging_console = Window.Open(0, progress_window_height, main_window_width, main_window_height - progress_console.WindowHeight - command_console.WindowHeight - (window_padding * 2), "operation logging", Konsole.Drawing.LineThickNess.Single);
            Console.ReadLine();
        }
    }
}
