using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Samples
{
    public static class MenuConcurrencyTestDemo
    {
        private static List<Task> _tasks = new List<Task>();
        private static int _menuCnt = 0;
        public static void SeperateThreadsForMenuAndTwoWindows(IConsole console, int numItems)
        {
            Window.HostConsole = console;
            var client = new Window(35, 0, 40, 25, "client", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkBlue);
            var server = new Window(77, 0, 40, 25, "server", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkYellow);
            // print two lines before the menu
            console.WriteLine("line 1");
            console.WriteLine("line 2");
            // create and run a menu inline, at the current cursor position
            var menu = new Menu(console, "Test samples", ConsoleKey.Escape, 30,
                new MenuItem('1', "cats", m => RunMenuItem(client, "client", "cats", numItems)),
                new MenuItem('2', "dogs", m => RunMenuItem(server, "server", "dogs", numItems)),
                new MenuItem('3', "item 1", m => { }),
                new MenuItem('4', "item 2", m => { }),
                new MenuItem('5', "item 3", m => { }),
                new MenuItem('6', "item 4", m => { }),
                new MenuItem('7', "item 5", m => { })

                );
            // line below should print after (below) the menu.
            console.WriteLine("line 3");
            // console should continue working and cursor should be set to below the menu.
            var kb = new MockKeyboard(0, GetKeyInfos(numItems));

            kb.OnCharPress += (c => { 
                console.PrintAt(Colors.WhiteOnDarkBlue, 12, 0, $"[{++_menuCnt}]"); 
            });

            menu.Keyboard = kb;
            menu.Run();
            Task.WaitAll(_tasks.ToArray());
        }

        static void RunMenuItem(IConsole console, string service, string prefix, int numItems)
        {
            var t = Task.Run(() =>
            {
                console.WriteLine("starting " + service);
                for (int i = 1; i <= numItems; i++)
                {
                    if (i==numItems)
                        console.Write("{0} {1}", prefix, i.ToString());
                    else
                        console.WriteLine("{0} {1}", prefix, i.ToString());
                }
            });
            _tasks.Add(t);
        }


        static IEnumerable<ConsoleKeyInfo> GetKeyInfos(int numItems)
        {
            return GetKeys(numItems / 10).Select(k => k.ToKeypress());
        }

        static IEnumerable<ConsoleKey> GetKeys(int numItems)
        {
            yield return ConsoleKey.Enter;
            yield return ConsoleKey.DownArrow;
            yield return ConsoleKey.Enter;
            yield return ConsoleKey.DownArrow;


            for (int i = 0; i < numItems; i++)
            {
                yield return ConsoleKey.DownArrow;
                yield return ConsoleKey.DownArrow;
                yield return ConsoleKey.DownArrow;
                yield return ConsoleKey.UpArrow;
                yield return ConsoleKey.UpArrow;
            }
            yield return ConsoleKey.Escape;
        }

    }
}
