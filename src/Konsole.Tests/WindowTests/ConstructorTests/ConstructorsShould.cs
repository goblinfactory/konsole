using System;
using FluentAssertions;
using Konsole.Tests.Internal;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    class ConstructorsShould
    {
        [Test]
        /// <summary>
        /// An "INLINE" (non floating window) is a window that does not have a top and left property set
        /// and will be created at the current cursor y + 1, and left set to 0
        /// and the cursor should be moved to below the newly created window
        /// </summary>
        public void WhenCreatingInlineWindows_cursor_should_be_moved_to_below_the_newly_created_window1()
        {
            IConsole window;
            IConsole inline;

            window = new MockConsole(20, 6);
            window.WriteLine("line1");
            window.Write("1234");
            Assert.AreEqual(1, window.CursorTop);
            Assert.AreEqual(4, window.CursorLeft);
            // create an inline window by only specifying a width and a height.
            inline = window.Open(5, 2);
            Assert.AreEqual(3, window.CursorTop);
            Assert.AreEqual(0, window.CursorLeft);
            window.WriteLine("foo");
        }

        public void when_creating_a_new_inline_window_withoutspecifying_height_or_width_should_use_balance_of_screen_and_keep_cursor_top()
        {
            var console = new MockConsole(20, 6);
            console.WriteLine("line1");
            console.Write("1234");
            Assert.AreEqual(1, console.CursorTop);
            Assert.AreEqual(4, console.CursorLeft);
            // create an inline window without specifying anything
            var restOfWindow = new Window(console);
            Assert.AreEqual(3, console.CursorTop);
            Assert.AreEqual(0, console.CursorLeft);
            console.WriteLine("foo");
        }


        [Test]
        public void WhenCreatingInlineWindows_cursor_should_be_moved_to_below_the_newly_created_window2()
        {
            var console = new MockConsole(10, 9);
            Window.HostConsole = console;
            console.WriteLine("one");
            var box1 = new Window(5, 3);
            box1.WriteLine("aaaaa");
            box1.WriteLine("bbbbb");
            box1.Write("ccccc");
            console.WriteLine("two");
            var box2 = new Window(5, 3);
            box2.WriteLine("aaaaa");
            box2.WriteLine("bbbbb");
            box2.Write("ccccc");
            console.Write("Under B");
            var expected = new[]
            {
                "one       ",
                "aaaaa     ",
                "bbbbb     ",
                "ccccc     ",
                "two       ",
                "aaaaa     ",
                "bbbbb     ",
                "ccccc     ",
                "Under B   "
            };
            console.Buffer.ShouldBe(expected);
        }


        //[Test]
        //public void should_clip_child_window_to_not_exceed_parent_boundaries()
        //{
        //    // this currently does not get clipped, it simply does not render at all? mmm...needs investigating and clipping added.
        //    Assert.Inconclusive("Not yet implemented.");
            
        //    var c = new MockConsole(20, 10);
        //    var w2 = c.Open( new WindowSettings { SX = 10, SY = 5, Width = 20, Height = 10, Theme = new StyleTheme(ConsoleColor.Red, ConsoleColor.White) });
        //    Assert.AreEqual(10, w2.WindowWidth);
        //    //Assert.AreEqual(5, w2.WindowHeight);
        //}

        [Test]
        public void not_allow_start_x_y_values_outside_of_parent_window()
        {
            //Assert.Inconclusive();
        }


        [Test]
        public void not_allow_negative_values()
        {
            //Assert.Inconclusive("new requirements");
        }

        //[Test]
        //public void use_parent_height_and_width_as_defaults()
        //{
        //    Assert.Inconclusive();
        //    //var c = new MockConsole(10,10);
        //    //var state = c.State;

        //    //var w1 = new Window(c);
        //    //w1.WindowHeight()

        //    //var w2 = new Window(0, 0, c);
        //    //state.Should().BeEquivalentTo(c.State);

        //    //var w3 = new Window(0, 0, 10, 10, true, c);
        //    //state.Should().BeEquivalentTo(c.State);
        //}

        [Test]
        public void set_scrolling_as_default_if_nothing_specified()
        {
            var c = new MockConsole();
            var w = c.Open(10, 10);
            Assert.True(w.Scrolling);
            Assert.False(w.Clipping);
        }

        [Test]
        public void set_correct_height_and_width()
        {
            var c = new MockConsole();
            var w = c.Open(10, 8, 6, 4);
            w.WindowWidth.Should().Be(6);
            w.WindowHeight.Should().Be(4);
        }

        [Test]
        public void not_change_host_cursor_position()
        {
            var c = new MockConsole(20, 20);
            c.Open(10, 8, 6, 4);
            c.CursorLeft.Should().Be(0);
            c.CursorTop.Should().Be(0);
        }

        //[Test]
        //public void offset_the_new_window()
        //{
        //    Assert.Inconclusive("Not yet implemented");
        //    // Not implemented yet
        //    // currently the only window that get's offset (called after the constructor returns instance) is a floating window.
        //    // via _CreateFloatingWindow which calls w.SetWindowOffset(x ?? 0, y ?? 0);
        //    var c = new MockConsole(20, 20);
        //    var w = new Window(c, 10, 8, 6, 4);
        //    w.AbsoluteX.Should().Be(10);
        //    w.AbsoluteY.Should().Be(8);
        //}

        [TestCase(0, 0, 10, 10)]
        [TestCase(5, 5, 10, 5)]
        [TestCase(0, 5, 10, 5)]
        [TestCase(5, 0, 10, 10)]
        public void when_no_values_set_should_use_balance_of_parent_screen_defaults_and_set_x_y_to_0_0_and_not_change_parent_window_top_or_left(int parentCurrentX, int parentCurrentY, int expectedWidth, int expectedHeight)
        { 
            var con = new MockConsole(10, 10);
            con.CursorLeft = parentCurrentX;
            con.CursorTop = parentCurrentY;
            var win = con.Open();
            win.WindowWidth.Should().Be(expectedWidth);
            win.WindowHeight.Should().Be(expectedHeight);
            win.CursorLeft.Should().Be(0);
            win.CursorTop.Should().Be(0);

            // and not change parent window top
            con.CursorTop.Should().Be(parentCurrentY);

            // or left
            con.CursorLeft.Should().Be(parentCurrentX);
        }
    }
}
