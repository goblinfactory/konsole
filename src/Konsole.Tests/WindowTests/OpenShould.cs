using System;
using FluentAssertions;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.WindowTests
{
    public class OpenShould
    {
        [Test]
        public void open_a_window_with_border_using_default_values()
        {
            var c = new MockConsole(10,5);
            var w = Window.Open(0, 0, 10, 5,"title", LineThickNess.Double, ConsoleColor.White, ConsoleColor.Black, c);
            w.WriteLine("one");
            w.WriteLine("two");
            w.Write("three");
            Console.WriteLine(c.BufferString);
            var expected = new[]
            {
                "╔═ title ╗",
                "║one     ║",
                "║two     ║",
                "║three   ║",
                "╚════════╝"
            };
            c.BufferWritten.Should().BeEquivalentTo(expected);
        }


        [Test]
        public void open_a_window_that_can_be_scrolled()
        {
            var c = new MockConsole(10, 8);
            var w = Window.Open(0, 0, 10, 5, "title", LineThickNess.Double, ConsoleColor.White, ConsoleColor.Black, c);
            w.WriteLine("one");
            w.WriteLine("two");
            w.WriteLine("three");
            w.WriteLine("four");
            var expected = new[]
            {
                "╔═ title ╗",
                "║three   ║",
                "║four    ║",
                "║        ║",
                "╚════════╝"
            };

            c.BufferWritten.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void clip_child_window_to_not_exceed_parent_boundaries_test1()
        {
            var con = new MockConsole(40, 10);
            var win = Window.Open(0, 0, 40, 10, "test", LineThickNess.Double, White, Black, con);
            var child = Window.Open(20, 0, 30, 6, "child", LineThickNess.Double, White, Black, win);
            child.WriteLine("test");

            // this is the current behaviour, not ideal, but test needs to be this until
            // the change is made.
            var expected = new[]
            {
            "╔════════════════ test ════════════════╗",
            "║                    ╔═══════════ child║",
            "║                    ║test            ║║",
            "║                    ║                ║║",
            "║                    ║                ║║",
            "║                    ║                ║║",
            "║                    ╚═════════════════║",
            "║                                      ║",
            "║                                      ║",
            "╚══════════════════════════════════════╝"
            };

            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void clip_child_window_to_not_exceed_parent_boundaries_test2()
        {
            var con = new MockConsole(40, 10);
            var win = Window.Open(20, 5, 30, 20, "test", LineThickNess.Double, White, Black, con);
            win.WriteLine("cats and dogs");

            // this is the current behaviour, not ideal, but test needs to be this until
            // the change is made.
            var expected = new[]
            {
            "                                        ",
            "                                        ",
            "                                        ",
            "                                        ",
            "                                        ",
            "                    ╔═══════════ test ══",
            "                    ║cats and dogs     ║",
            "                    ║                  ║",
            "                    ║                  ║",
            "                    ║                  ║"
            };

            con.Buffer.Should().BeEquivalentTo(expected);
        }
    }
}
