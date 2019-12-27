using System;
using FluentAssertions;
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
        public void WhenCreatingInlineWindows_cursor_should_be_moved_to_below_the_newly_created_window()
        {
            IConsole _window;
            IConsole _inline;

            _window = new MockConsole(20,6);
            _window.WriteLine("line1");
            _window.Write("1234");
            Assert.AreEqual(1, _window.CursorTop);
            Assert.AreEqual(4, _window.CursorLeft);
            // create an inline window by only specifying a width and a height.
            _inline = new Window(_window,5,2);
            Assert.AreEqual(3, _window.CursorTop);
            Assert.AreEqual(0, _window.CursorLeft);
            _window.WriteLine("foo");
        }


        [Test]
        public void should_clip_child_window_to_not_exceed_parent_boundaries()
        {
            var c = new MockConsole(20,10);
            var w2 = new Window(c, 10, 5, 20, 10, ConsoleColor.Red, ConsoleColor.White);
            Assert.AreEqual(10, w2.WindowWidth);
            //Assert.AreEqual(5, w2.WindowHeight);
        }

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

        [Test]
        public void Not_change_parent_state()
        {
            var c = new MockConsole();
            var state = c.State;

            var w1 = new Window(c);
            state.Should().BeEquivalentTo(c.State);

            var w2 = new Window(c, 0, 0);
            state.Should().BeEquivalentTo(c.State);

            var w3 = new Window(0,0,10,10,c);
            state.Should().BeEquivalentTo(c.State);
        }

        [Test]
        public void use_parent_height_and_width_as_defaults()
        {
            Assert.Inconclusive();
            //var c = new MockConsole(10,10);
            //var state = c.State;

            //var w1 = new Window(c);
            //w1.WindowHeight()

            //var w2 = new Window(0, 0, c);
            //state.Should().BeEquivalentTo(c.State);

            //var w3 = new Window(0, 0, 10, 10, true, c);
            //state.Should().BeEquivalentTo(c.State);
        }

        [Test]
        public void set_scrolling_if_specified()
        {
            var c = new MockConsole();
            var w = new Window(c, 10, 10, K.Scrolling);
            Assert.True(w.Scrolling);
            Assert.False(w.Clipping);
        }

        [Test]
        public void set_clipping_if_specified()
        {
            var c = new MockConsole();
            var w = new Window(c, 10, 10, K.Clipping);
            Assert.True(w.Clipping);
            Assert.False(w.Scrolling);
        }

        [Test]
        public void set_scrolling_as_default_if_nothing_specified()
        {
            var c = new MockConsole();
            var w = new Window(c, 10, 10);
            Assert.True(w.Scrolling);
            Assert.False(w.Clipping);
        }

        [Test]
        public void set_correct_height_and_width()
        {
            var c = new MockConsole(20, 20);
            var w = new Window(c, 10, 8, 6, 4);
            w.WindowWidth.Should().Be(6);
            w.WindowHeight.Should().Be(4);
        }

        [Test]
        public void not_change_host_cursor_position()
        {
            var c = new MockConsole(20, 20);
            var w = new Window(c, 10, 8, 6, 4);
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
    }
}
