using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;
using static System.ConsoleColor;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Konsole.Samples
{
    class Program
    {
        static void Fill(IConsole con)
        {
            con.WriteLine("one");
            con.WriteLine("two");
            con.WriteLine("three");
            con.WriteLine("four");
            con.Write("five");
        }

        

        static void Main(string[] args)
        {

            var styleThemes = StyleTheme.GetStyleThemes();

            foreach(var theme in styleThemes)
            {
                // fix the colors so that default rendering is EPIC with all the themes
                var dirwin = new Window(new WindowSettings { Title = theme.ToString(), Width = 50, Height = 30, Theme = theme });
                var at = dirwin.Style.Title;
                var listView = new DirectoryListView(dirwin, "../../..");

                listView.BusinessRuleColors = (o, column) =>
                {
                    if (column == 2 && o.Size > 4000000) return new Colors(White, Red);
                    if (column == 1  && o.Is == FileOrDirectory.Me.Directory) return new Colors(Green, Black);
                    return null;
                };


                listView.Refresh();
                Console.ReadKey(true);
                Console.Clear();
            }

            return;

            var parent = new Window(10, 6, "parent");
            var child = parent.Open("child");
            Fill(child);
            Console.ReadLine();

            //var parent = new Window(10, 7, "parent");
            ////var child = new Window(10, 7, "parent");
            //var child = parent.Open("child");
            //child.WriteLine("one");
            //child.WriteLine("two");
            //child.WriteLine("three");
            //child.WriteLine("four"); 
            //child.Write("five");
            //Console.ReadLine();

            Console.CursorVisible = false;

            var win = new Window(new WindowSettings { Title = "files", Width = 50, Height = 30, Theme = Style.WhiteOnRed.ToTheme() });
            InlineWindowTests.NestedWindowsWithTitles(win);

            //ProgressBarWithNewWindowConcurrencyTests.Test(win);
            Console.ReadKey(true);
            Console.Clear();

            //return;

            // TODO - drop a screenshot into each sample folder so 
            // that someone browsing the code can see what the code
            // renders w/o having to hunt through documentation.

            // NB! I may need to allow windows to create a progress bar (i.e. create a new window) BELOW the top of the window ... to "scroll" it up?
            ProgressBarInsideWindow.Main(null);
            Console.ReadKey(true);
            Console.Clear();

            ProgressBarAtBottomOfScreen.Run();
            Console.Clear();
            var window2 = new Window();
            Console.CursorVisible = false;
            MenuConcurrencyTestDemo.SeperateThreadsForMenuAndTwoWindows(window2, 100);
            window2.Clear();
            WindowClientServerSamples.Demo();
            window2.Clear();
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
