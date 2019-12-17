using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.Slow
{
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
            var menu = new Menu(console, "ZeroMQ samples", ConsoleKey.Escape, 30,
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
            var kb = new MockKeyboard(0, GetKeyInfos()); ;

            menu.Keyboard = kb;
            menu.Run();
            Task.WaitAll(_tasks.ToArray());


            var expected = new string[]
            {
                "line 1                             ┌─────────────── client ───────────────┐  ┌─────────────── server ───────────────┐   ",  
                "line 2                             │cats 7978                             │  │dogs 7978                             │   ",  
                "                                   │cats 7979                             │  │dogs 7979                             │   ",  
                "    ZeroMQ samples                 │cats 7980                             │  │dogs 7980                             │   ",  
                "    --------------------------     │cats 7981                             │  │dogs 7981                             │   ",  
                "    cats                           │cats 7982                             │  │dogs 7982                             │   ",  
                "    dogs                           │cats 7983                             │  │dogs 7983                             │   ",  
                "    item 1                         │cats 7984                             │  │dogs 7984                             │   ",  
                "    item 2                         │cats 7985                             │  │dogs 7985                             │   ",  
                "    item 3                         │cats 7986                             │  │dogs 7986                             │   ",  
                "    item 4                         │cats 7987                             │  │dogs 7987                             │   ",  
                "    item 5                         │cats 7988                             │  │dogs 7988                             │   ",  
                "                                   │cats 7989                             │  │dogs 7989                             │   ",  
                "line 3                             │cats 7990                             │  │dogs 7990                             │   ",  
                "                                   │cats 7991                             │  │dogs 7991                             │   ",  
                "                                   │cats 7992                             │  │dogs 7992                             │   ",  
                "                                   │cats 7993                             │  │dogs 7993                             │   ",  
                "                                   │cats 7994                             │  │dogs 7994                             │   ",  
                "                                   │cats 7995                             │  │dogs 7995                             │   ",  
                "                                   │cats 7996                             │  │dogs 7996                             │   ",  
                "                                   │cats 7997                             │  │dogs 7997                             │   ",  
                "                                   │cats 7998                             │  │dogs 7998                             │   ",  
                "                                   │cats 7999                             │  │dogs 7999                             │   ",  
                "                                   │                                      │  │                                      │   ",  
                "                                   └──────────────────────────────────────┘  └──────────────────────────────────────┘   "
            };

            var actual = console.BufferWritten;
            actual.Should().BeEquivalentTo(expected);
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


        IEnumerable<ConsoleKeyInfo> GetKeyInfos()
        {
            return GetKeys().Select(k => k.ToKeypress());
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
