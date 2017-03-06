using System;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    class WriteLineShould
    {
        [Category(CrossCuttingConcerns.Scrolling)]
        public class WhenScrollingEnabled
        {
            public void scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
            {
                Assert.Inconclusive("new requirement");
            }
        }

        [Category(CrossCuttingConcerns.Scrolling)]
        public class WhenScrollingDisabled
        {
            [Test]
            public void clip_all_text_that_overflows_the_bottom_of_the_screen_and_not_scroll_the_screen()
            {
                var c = new MockConsole(6, 4);
                c.WriteLine("one");
                c.WriteLine("two");
                c.WriteLine("three");
                c.WriteLine("four");
                c.WriteLine("five");
                var expected = new[]
                {
                "one   ",
                "two   ",
                "three ",
                "four  "    
                };
                c.Buffer.ShouldBeEquivalentTo(expected);
            }


        }



        [Test]
        public void write_relative_to_the_window_being_printed_to_not_the_parent()
        {
            var c = new MockConsole(6, 4);
            c.WriteLine("------");
            c.WriteLine("------");
            c.WriteLine("------");
            c.WriteLine("------");
            var w = new Window(1, 1, 4, 2, true, c, K.Transparent);
            w.WriteLine("X");
            w.WriteLine("Y");
            var expected = new[]
            {
                "------",
                "-X----",
                "-Y----",
                "------"
            };
            Console.WriteLine(c.BufferWrittenString);
            c.Buffer.ShouldBeEquivalentTo(expected);
        }


        [Test]
        public void write_using_the_currently_set_fore_and_background_colors()
        {
            var console = new MockConsole(3, 3);
            console.ForegroundColor = ConsoleColor.Red;
            console.BackgroundColor = ConsoleColor.White;
            console.PrintAt(0, 0, "X");

            var expectedBefore = new[]
            {
                "Xrw wk wk",
                " wk wk wk",
                " wk wk wk"
            };

            Assert.AreEqual(expectedBefore, console.BufferWithColor);

            var w = new Window(1, 1, 2, 2, ConsoleColor.Black, ConsoleColor.White, true, console, K.Transparent);
            w.ForegroundColor = ConsoleColor.Yellow;
            w.BackgroundColor = ConsoleColor.Black;
            w.WriteLine("Y");
            w.WriteLine("Z");

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
            var console = new Window(80, 20, false);
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
            var parent = new Window(80, 20, false);
            var state = parent.State;

            var console = new Window(parent);

            console.WriteLine("This");
            state.ShouldBeEquivalentTo(parent.State);
        }

    }
}
