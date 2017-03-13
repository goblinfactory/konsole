using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    class ConstructorsShould
    {
        //todo .. some manual experiments
        // try to break it... using 'random' then  update tests appropriately.

        [Test]
        public void should_clip_child_window_to_not_exceed_parent_boundaries()
        {
            Assert.Inconclusive();
            var c = new MockConsole();
            var w1 = new Window(c, 20, 10, ConsoleColor.Black, ConsoleColor.DarkYellow);
            var w2 = new Window(w1, 11, 6, 20, 10, ConsoleColor.Red, ConsoleColor.White);
            Assert.AreEqual(10, w2.WindowWidth);
            Assert.AreEqual(5, w2.WindowHeight);
        }

        [Test]
        public void not_allow_start_x_y_values_outside_of_parent_window()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void not_allow_negative_values()
        {
            Assert.Inconclusive("new requirements");
        }

        [Test]
        public void Not_change_parent_state()
        {
            var c = new MockConsole();
            var state = c.State;

            var w1 = new Window(c);
            state.ShouldBeEquivalentTo(c.State);

            var w2 = new Window(c, 0, 0);
            state.ShouldBeEquivalentTo(c.State);

            var w3 = new Window(0,0,10,10,c);
            state.ShouldBeEquivalentTo(c.State);
        }

        [Test]
        public void use_parent_height_and_width_as_defaults()
        {
            var c = new MockConsole(10,10);
            var state = c.State;

            var w1 = new Window(c);
            //w1.WindowHeight()

            //var w2 = new Window(0, 0, c);
            //state.ShouldBeEquivalentTo(c.State);

            //var w3 = new Window(0, 0, 10, 10, true, c);
            //state.ShouldBeEquivalentTo(c.State);
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

    }
}
