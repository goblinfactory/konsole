using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    class ForegroundColorShould
    {
        [Test]
        public void not_change_parent_state()
        {
            var c = new MockConsole();
            var state = c.State;
            var w = new Window(c);
            w.ForegroundColor = ConsoleColor.DarkGray;
            state.Should().BeEquivalentTo(c.State);
        }


    }
}
