using System;
using ApprovalTests.Reporters;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    class ClippingShould
    {
        [Test]
        public void Clip_all_Write_text_that_overflows_X_width()
        {
            // need to test PrintAt, Write, WriteLine
            var c = new MockConsole(6, 3);
            var w = c.Open(new WindowSettings { Width = 6, Height = 2, Clipping = true });
            w.WriteLine("123456789");
            // should not advance the line (Y) if it's a write...
            w.Write("ABC");
            var expected = new[]
            {
            "123456",
            "ABC   ",
            "      "
            };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void Clip_all__WriteLine_text_that_overflows_X_width()
        {
            // need to test PrintAt, Write, WriteLine
            var c = new MockConsole(6, 3);
            var w = c.Open(new WindowSettings { Width = 6, Height = 2, Clipping = true });
            w.WriteLine("one two three");
            w.Write("four");
            var expected = new[]{
                "one tw",
                "four  ",
                "      ",
            };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void Not_advance_CursorY()
        {
            // need to test PrintAt, Write, WriteLine
            var c = new MockConsole(6, 3);
            var w = c.Open(new WindowSettings { Width = 6, Height = 2, Clipping = true });
            w.Write("123");
            // should not advance the line (Y) if it's a write...
            w.Write("AAAAA");
            w.Write("BBB");
            // writeLine should not print, but finally move cursor to new line after everything is said and done.
            w.WriteLine("CCCCCCC");
            w.Write("ABC");
            var expected = new[]{
                "123AAC",  // the C is written here because of a rare edge case where if the cursor to a Write, writes to the last character of the screen we don't advance it 1 
                "ABC   ",  // character, otherwise that would cause a scroll, and we'd never be able to neatly print to the bottom of the screen.
                "      ",  // will require a bit of tweaking to sort properly, but is acceptable small edge case for now until I change this.
                };
            c.Buffer.ShouldBe(expected);
        }

    }


}
