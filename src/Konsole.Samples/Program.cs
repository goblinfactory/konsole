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

        public static StyleTheme RTheme()
        {
            var bodyback = rnd.Next(16);
            var lineBack = rnd.Next(100) > 50 ? 0 : rnd.Next(16);
            bool useBodyForLine = rnd.Next(100) > 70;
            var body = Colors.RandomColor(bodyback);
            var line =useBodyForLine ? body :  Colors.RandomColor(lineBack);
            var headers = Colors.RandomColor(rnd.Next(16));
            var selected = Colors.RandomColor(rnd.Next(16));
            var bold = Colors.RandomColor(bodyback);
            return new Style(LineThickNess.Single, body, line, headers, line, selected, bold).ToTheme();
        }

        private static Random rnd = new Random();
        


        static void Main(string[] args)
        {
            var cwin = new Window();
            ListViewDemos.ListViewRenderChessGamesSwapFocus();
            return;
            for (int bg = 0; bg < 16; bg++)
            {
                var colors = Colors.RandomColor(bg);
                cwin.Write(colors, $"[{colors.Background}][{colors.Foreground}] hello world! ");
            }
            Console.ReadKey();
            Console.Clear();

            int cnt = 0;
            while(cnt++<4)
            {
                var theme = RTheme();
                Console.WriteLine(theme.Active);
                var dirs = new Window(new WindowSettings { Title = theme.ToString(), Width = 50, Height = 12, Theme = theme });
                var at = dirs.Style.Title;
                var listView = new DirectoryListView(dirs, "../../..");

                listView.BusinessRuleColors = (o, column) =>
                {
                    if (column == 2 && o.Size > 4000000) return theme.Active.SelectedItem;
                    if (column == 1 && o.Is == FileOrDirectory.Me.Directory) return theme.Active.Bold;
                    return null;
                };


                listView.Refresh();
                Console.ReadKey(true);
                Console.Clear();
            }

           // return;

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
