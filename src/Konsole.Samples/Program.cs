using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;

namespace Konsole.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO - drop a screenshot into each sample folder so 
            // that someone browsing the code can see what the code
            // renders w/o having to hunt through documentation.

            // NB! I may need to allow windows to create a progress bar (i.e. create a new window) BELOW the top of the window ... to "scroll" it up?

            var window2 = new Window();
            Console.CursorVisible = false;
            ListViewDemos.ListViewThemeTest();
            Console.Clear();
            WindowClientServerSamples.Demo();
            Console.Clear();
            RealtimeStockPriceMonitorWithHighSpeedWriter.Main(new string[0]);
            Console.ReadKey(true); 

            return;

            var window = new Window();

            //Console.ReadKey(true);
            //return;

            var (menuCon, contentCon) = window.SplitLeftRight();

            var menu1 = new Menu(menuCon, "DEMO", GetDir().GetDirectories().Select(d => new MenuItem(d.Name, (m) => RunDemo(d.Name, contentCon))).ToArray());
            var menu2 = new Menu(contentCon, "DEMO", ConsoleKey.Escape, 0, GetDir().GetDirectories().Select(d => new MenuItem(d.Name, (m) => RunDemo(d.Name, contentCon))).ToArray());
            menu2.Refresh();
            menu1.Run();
        }

        private static void RunDemo(string name, IConsole console)
        {

            console.WriteLine(name);
        }

        private static DirectoryInfo GetDir([CallerFilePath] string path = null)
        {
            var dir = new FileInfo(path);
            var demoPath = Path.Combine(dir.DirectoryName, "Demos");
            return new DirectoryInfo(demoPath);
        }

    }
}
