using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    class BufferHighLightedShould
    {
        [Test]
        public void return_a_buffer_line_using_provided_char_to_indicate_any_printed_character_with_a_matching_background_color()
        {
            var normal = ConsoleColor.Black;
            var hilite = ConsoleColor.White;

            var console = new MockConsole(11, 5);
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
            Assert.AreEqual(expected, hlBuffer);
        }

    }
}
