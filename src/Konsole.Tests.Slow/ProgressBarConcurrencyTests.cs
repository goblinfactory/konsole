using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.Slow
{
    class ProgressBarConcurrencyTests
    {
        private List<Task> _tasks = new List<Task>();

        [Test]
        public void menu_plus_two_windows_full_of_progress_bars_each_window_on_seperate_thread_no_scrolling_of_pbars()
        {
            var console = new MockConsole();
            Window.HostConsole = console;
            var client = new Window(35, 0, 40, 25, "client", LineThickNess.Single, White, DarkBlue);
            var server = new Window(77, 0, 40, 25, "server", LineThickNess.Single, White, DarkYellow);
            console.WriteLine("one");
            console.WriteLine("two");
            var menu = new Menu(console, "Progress Bars", ConsoleKey.Escape, 30,

                new MenuItem('1', "AutoResetEvent client", m => RunProgressBars(client, "client", "cats")),
                new MenuItem('2', "AutoResetEvent server", m => RunProgressBars(server, "server", "dogs"))

            );
            // need a unit test for the menu before fixing it.

            var kb = new MockKeyboard(0, GetKeyInfos());

            menu.Keyboard = kb;
            menu.Run();
            console.WriteLine("three");
            console.WriteLine("stopping");
            Task.WaitAll(_tasks.ToArray());
            console.WriteLine("finished, press enter to close.");
            var actual = console.BufferWritten;

            var expected = new[]{
                "one                                ┌─────────────── client ───────────────┐  ┌─────────────── server ───────────────┐   ",
                "two                                │Item 24    of 24   . (100%) ########  │  │Item 24    of 24   . (100%) ########  │   ",
                "         Progress Bars             │cats99                                │  │dogs99                                │   ",
                "                                   │Item 11    of 11   . (100%) ########  │  │Item 11    of 11   . (100%) ########  │   ",
                " 1. AutoResetEvent client          │cats99                                │  │dogs99                                │   ",
                " 2. AutoResetEvent server          │Item 46    of 46   . (100%) ########  │  │Item 46    of 46   . (100%) ########  │   ",
                "                                   │cats99                                │  │dogs99                                │   ",
                "three                              │Item 77    of 77   . (100%) ########  │  │Item 77    of 77   . (100%) ########  │   ",
                "stopping                           │cats99                                │  │dogs99                                │   ",
                "finished, press enter to close.    │Item 65    of 65   . (100%) ########  │  │Item 65    of 65   . (100%) ########  │   ",
                "                                   │cats99                                │  │dogs99                                │   ",
                "                                   │Item 43    of 43   . (100%) ########  │  │Item 43    of 43   . (100%) ########  │   ",
                "                                   │cats99                                │  │dogs99                                │   ",
                "                                   │Item 35    of 35   . (100%) ########  │  │Item 35    of 35   . (100%) ########  │   ",
                "                                   │cats99                                │  │dogs99                                │   ",
                "                                   │Item 94    of 94   . (100%) ########  │  │Item 94    of 94   . (100%) ########  │   ",
                "                                   │cats99                                │  │dogs99                                │   ",
                "                                   │Item 10    of 10   . (100%) ########  │  │Item 10    of 10   . (100%) ########  │   ",
                "                                   │cats99                                │  │dogs99                                │   ",
                "                                   │Item 64    of 64   . (100%) ########  │  │Item 64    of 64   . (100%) ########  │   ",
                "                                   │cats99                                │  │dogs99                                │   ",
                "                                   │                                      │  │                                      │   ",
                "                                   │                                      │  │                                      │   ",
                "                                   │                                      │  │                                      │   ",
                "                                   └──────────────────────────────────────┘  └──────────────────────────────────────┘   ",
                };

            actual.ShouldBe(expected);
        }


        void RunProgressBars(IConsole console, string service, string prefix)
        {
            var r = new Random(1);
            var pbs = Enumerable.Range(1, 10).Select(i => new ProgressBar(console, PbStyle.DoubleLine, r.Next(100))).ToArray();
            var t = Task.Run(() =>
            {
                Thread.Sleep(50); // convert to async?
                for (int i = 0; i < 100; i++)
                {
                    foreach (var pb in pbs)
                    {
                        pb.Refresh(i.Max(pb.Max), prefix + i);
                    }
                }
            });
            _tasks.Add(t);
        }

        static IEnumerable<ConsoleKeyInfo> GetKeyInfos()
        {
            return GetKeys().Select(k => k.ToKeypress());
        }
        static IEnumerable<ConsoleKey> GetKeys()
        {
            yield return ConsoleKey.DownArrow;
            yield return ConsoleKey.Enter;
            yield return ConsoleKey.DownArrow;
            yield return ConsoleKey.Enter;
            for (int i = 0; i < 100; i++)
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

    public static class IntExtentions
    {
        public static int Max(this int src, int max)
        {
            return src > max ? max : src;
        }
    }



}