using System;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class BufferWithColorShould
    {
        [Test]
        public void include_foreground_and_background_rendered_as_seperate_characters()
        {
            var c = new Window(0, 0, 6, 3, foreground: ConsoleColor.Black, background: ConsoleColor.White, echo: false);

            // [ xx ] with brackets in black and xx in red : see CellColorMapper.cs

            c.ForegroundColor = ConsoleColor.Black;
            c.PrintAt(1, 1, "[");
            c.PrintAt(4, 1, "]");
            c.ForegroundColor = ConsoleColor.Red;
            c.BackgroundColor = ConsoleColor.Yellow;
            c.PrintAt(2, 1, "xx");

            //"      "
            //" [xx] "
            //"      "
            var expected = new[]
            {
                " kw kw kw kw kw kw",
                " kw[kwxryxry]kw kw",
                " kw kw kw kw kw kw"
            };

            Assert.AreEqual(expected, c.BufferWithColor);
        }   
    }
}
