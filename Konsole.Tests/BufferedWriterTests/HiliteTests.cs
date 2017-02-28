using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    public class HiliteTests
    {
        [Test]
        public void readable_buffer_should_show_which_lines_are_highlighted()
        {
            var normal = ConsoleColor.Black;
            var hilite = ConsoleColor.White;

            var console = new Window(11, 5, false);
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

            var expected = new[]
            {
                " m e n u   i t e m   1",
                " m e n u   i t e m   2",
                " m e n u  #i#t#e#m   3",
                " m e n u   i t e m   4",
                " m e n u   i t e m   5"
            };

            var hlBuffer = console.BufferHighlighted(hilite, '#', ' ');
            Console.WriteLine(console.BufferHighlightedString(hilite, '#', ' '));
            Console.WriteLine();
            Assert.AreEqual(expected,hlBuffer);
        }

    }
}