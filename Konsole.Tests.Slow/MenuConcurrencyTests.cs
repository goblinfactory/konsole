using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Konsole.Drawing;
using Konsole.Menus;
using NUnit.Framework;

namespace Konsole.Tests.Slow
{
    [UseReporter(typeof(DiffReporter))]
    public class MenuConcurrencyTests
    {
        private List<Task> _tasks = new List<Task>();

        [Test]
        public void SeperateThreadsForMenuAndTwoWindows()
        {
            var console = new MockConsole();
            var client = Window.Open(35, 0, 40, 25, "client", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkBlue, console);
            var server = Window.Open(77, 0, 40, 25, "server", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkYellow, console);
            // print two lines before the menu
            console.WriteLine("line 1");
            console.WriteLine("line 2");
            // create and run a menu inline, at the current cursor position
            var menu = new Menu(console,console, "ZeroMQ samples", ConsoleKey.Escape, 30,
                new MenuItem('1', "cats", () => RunMenuItem(client, "client", "cats")),
                new MenuItem('2', "dogs", () => RunMenuItem(server, "server", "dogs")),
                new MenuItem('3', "item 1", () => {}),
                new MenuItem('4', "item 2", () => {}),
                new MenuItem('5', "item 3", () => {}),
                new MenuItem('6', "item 4", () => {}),
                new MenuItem('7', "item 5", () => {})
                
                );
            // line below should print after (below) the menu.
            console.WriteLine("line 3");
            // console should continue working and cursor should be set to below the menu.
            var kb = new MockKeyboard(0, GetKeys()); ;

            menu.Keyboard = kb;
            menu.Run();
            Task.WaitAll(_tasks.ToArray());
            Approvals.Verify(console.BufferWrittenString);

        }

        void RunMenuItem(IConsole console, string service, string prefix)
        {
            var t = Task.Run(() =>
            {
                Console.WriteLine("starting " + service);
                for (int i = 0; i < 8000; i++) console.WriteLine("{0} {1}", prefix, i.ToString());
            });
            _tasks.Add(t);
        }


        IEnumerable<ConsoleKey> GetKeys()
        {
            yield return ConsoleKey.Enter;
            yield return ConsoleKey.DownArrow;
            yield return ConsoleKey.Enter;
            yield return ConsoleKey.DownArrow;
            

            for (int i = 0; i < 1000; i++)
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
