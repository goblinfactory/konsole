using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Konsole.Drawing;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    class OpenShould
    {
        [Test]
        public void open_a_window_with_border_using_default_values()
        {
            var c = new MockConsole(10,5);
            var w = Window.Open(0, 0, 10, 5,"title", LineThickNess.Double, ConsoleColor.White, ConsoleColor.Black, c);
            w.WriteLine("one");
            w.WriteLine("two");
            w.WriteLine("three");
            Console.WriteLine(c.BufferString);
            var expected = new[]
            {
                "╔═ title ╗",
                "║one     ║",
                "║two     ║",
                "║three   ║",
                "╚════════╝"
            };
            c.BufferWritten.ShouldBeEquivalentTo(expected);
        }


        [Test]
        public void open_a_window_that_can_be_scrolled()
        {
            // this test fails because scrolling does not scroll the MockConsole buffer
            // the other test is faulty as it's asserting on the window and not on the mock window.
            var c = new MockConsole(10, 8);
            var w = Window.Open(0, 0, 10, 5, "title", LineThickNess.Double, ConsoleColor.White, ConsoleColor.Black, c);
            w.WriteLine("one");
            w.WriteLine("two");
            w.WriteLine("three");
            w.WriteLine("four");
            Console.WriteLine(c.BufferString);
            var expected = new[]
            {
                "╔═ title ╗",
                "║two     ║",
                "║three   ║",
                "║four    ║",
                "╚════════╝"
            };

            c.BufferWritten.ShouldBeEquivalentTo(expected);
        }

    }
}
