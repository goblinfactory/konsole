using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    [UseReporter(typeof(DiffReporter))]
    public class HiliteTests
    {
        [Test]
        public void readable_buffer_should_show_which_lines_are_highlighted()
        {
            var normal = ConsoleColor.Black;
            var hilite = ConsoleColor.White;

            var console = new Window(40, 10, false);
            console.ForegroundColor = ConsoleColor.Red;

            console.BackgroundColor = normal;
            console.WriteLine("menu item 1");
            console.WriteLine("menu item 2");
            console.Write("menu ");

            console.BackgroundColor = hilite;
            console.Write("item");

            console.BackgroundColor = normal;
            console.WriteLine(" 3");
            console.WriteLine("menu item 4");
            console.WriteLine("menu item 5");

            var hlBuffer = console.BufferHighlighted(hilite, '#', ' ');
            System.Console.WriteLine(hlBuffer);
            System.Console.WriteLine();
            Approvals.Verify(hlBuffer);
        }

    }
}