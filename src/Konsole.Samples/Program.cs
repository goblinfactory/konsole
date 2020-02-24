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

            //var window2 = new Window();
            //Console.CursorVisible = false;
            //ControlSample.Demo(window2);
            //Console.ReadKey(true);

            var window = new Window();
            var (menuCon, contentCon) = window.SplitLeftRight();

            var menuItems = GetDir().GetDirectories().Select(d => new MenuItem(d.Name, (m) => RunDemo(d.Name, contentCon))).ToArray();

            var menu = new Menu(menuCon, "DEMO", menuItems);
            menu.Run();
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
