using System;
using FluentAssertions;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.WindowTests
{
    class WriteLineShould
    {
        // for now, if we want to write to a box to fill it completely simply enable clipping instead.
        //[Test]
        //public void when_WriteLine_to_last_char_on_screen_and_on_last_row_dont_scroll_until_more_printing_happens()
        //{
        //    Assert.Fail("this is not a bug, this is a new feature! so need to fix all the other bugs first..before implementing this.");
        //    var con = new MockConsole(6, 4);
        //    con.WriteLine("123");
        //    con.WriteLine("abc");
        //    con.WriteLine("456");
        //    con.WriteLine("defghi");
        //    var expected = new[]
        //    {
        //        "123   ",
        //        "abc   ",
        //        "456   ",
        //        "defghi"
        //    };
        //    con.Buffer.ShouldBe(expected);
        //    con.Write("123");

        //    expected = new[]
        //    {
        //        "456   ",
        //        "defghi",
        //        "      ",
        //        "123   "
        //    };
        //    con.Buffer.ShouldBe(expected);
        //}

        [Test]
        public void when_writeLine_ends_on_last_char_on_screen_move_cursor_forward_one_line_only()
        {
            // so that we can support writing neatly to fill a small window exactly without
            // causing scrolling. Otherwise we can't fill windows, required for creating controls!

            var con = new MockConsole(6, 4);
            con.WriteLine("123456");
            con.WriteLine("XY");

            var expected = new[]{
                "123456",
                "XY    ",
                "      ",
                "      ",
                };
            con.Buffer.ShouldBe(expected);
        }
        [Test]
        public void allow_embedded_interpolations_without_exception()
        {
            var con = new MockConsole(6, 4);
            con.WriteLine("{0}");
            con.WriteLine("{0}", "cat");
            con.WriteLine("{json}");
            var expected = new[]
            {
                "{0}   ",
                "cat   ",
                "{json}",
                "      "
            };
            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void write_relative_to_the_window_being_printed_to_not_the_parent()
        {
            var c = new MockConsole(6, 4);
            c.WriteLine("------");
            c.WriteLine("------");
            c.WriteLine("------");
            c.Write("------");
            var w = c.Open(new WindowSettings { SX = 1, SY = 1, Width = 4, Height = 2, Transparent = true });
            w.WriteLine("X");
            w.Write("Y");
            var expected = new[]
            {
                "------",
                "-X----",
                "-Y----",
                "------"
            };
            Console.WriteLine(c.BufferWrittenString);
            c.Buffer.ShouldBe(expected);
        }


        [Test]
        public void write_using_the_currently_set_fore_and_background_colors()
        {
            var console = new MockConsole(3, 3);
            Window.HostConsole = console;
            console.ForegroundColor = Red;
            console.BackgroundColor = White;
            console.PrintAt(0, 0, "X");
     
            console.Peek(0, 0).Colors.Should().BeEquivalentTo(new Colors(Red, White));
            var expectedBefore = new[]
            {
                "Xrw wk wk",
                " wk wk wk",
                " wk wk wk"
            };

            Assert.AreEqual(expectedBefore, console.BufferWithColor);

            var w = new Window(new WindowSettings
            {
                SX = 1,
                SY = 1,
                Width = 2,
                Height = 2,
                Theme = new StyleTheme(Black, White),
                Transparent = true
            });
            w.ForegroundColor = Yellow;
            w.BackgroundColor = Black;
            w.WriteLine("Y");
            w.Write("Z");

            var expectedAfter = new[]
            {
                "Xrw wk wk",
                " wkYyk wk",
                " wkZyk wk"
            };

            Assert.AreEqual(expectedAfter, console.BufferWithColor);
        }

        [Test]
        public void increment_cursortop_position_of_calling_window()
        {
            var console = new MockConsole(80, 20);
            Assert.AreEqual(0, console.CursorTop);
            console.WriteLine("line1");
            Assert.AreEqual(1, console.CursorTop);
            console.Write("This ");
            Assert.AreEqual(1, console.CursorTop);
            console.Write("is ");
            Assert.AreEqual(1, console.CursorTop);
            console.WriteLine("a test line.");
            Assert.AreEqual(2, console.CursorTop);
            console.WriteLine("line 3");
            Assert.AreEqual(3, console.CursorTop);
        }

        [Test]
        public void not_change_state_of_parent_console()
        {
            var parent = new MockConsole(80, 20);
            var state = parent.State;

            var console = new Window(parent);

            console.WriteLine("This");
            state.Should().BeEquivalentTo(parent.State);
        }

    }
}
