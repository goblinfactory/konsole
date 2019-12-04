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
    class ProgressBarConcurrencyTests
    {
        private List<Task> _tasks = new List<Task>();

        [Test]
        public void menu_plus_two_windows_full_of_progress_bars_each_window_on_seperate_thread_no_scrolling_of_pbars()
        {
            var console = new MockConsole();
            var client = Window.Open(35, 0, 40, 25, "client", LineThickNess.Single, ConsoleColor.White,
                ConsoleColor.DarkBlue, console);
            var server = Window.Open(77, 0, 40, 25, "server", LineThickNess.Single, ConsoleColor.White,
                ConsoleColor.DarkYellow, console);
            console.WriteLine("one");
            console.WriteLine("two");
            var menu = new Menu(console, "Progress Bars", ConsoleKey.Escape, 30,

                new MenuItem('1', "AutoResetEvent client", () => RunProgressBars(client, "client", "cats")),
                new MenuItem('2', "AutoResetEvent server", () => RunProgressBars(server, "server", "dogs"))

            );
            // need a unit test for the menu before fixing it.

            var kb = new MockKeyboard(0, GetKeyInfos());

            menu.Keyboard = kb;
            menu.Run();
            console.WriteLine("three");
            console.WriteLine("stopping");
            Task.WaitAll(_tasks.ToArray());
            console.WriteLine("finished, press enter to close.");
            Approvals.Verify(console.BufferWrittenString);
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