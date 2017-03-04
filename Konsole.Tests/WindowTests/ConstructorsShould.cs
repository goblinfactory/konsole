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
        [Test]
        public void Not_change_parent_state()
        {
            var c = new MockConsole();
            var state = c.State;

            var w1 = new Window(c);
            state.ShouldBeEquivalentTo(c.State);

            var w2 = new Window(0,0,c);
            state.ShouldBeEquivalentTo(c.State);

            var w3 = new Window(0,0,10,10,true,c);
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







    }
}
