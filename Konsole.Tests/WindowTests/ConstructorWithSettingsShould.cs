using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    class ConstructorWithSettingsShould
    {
        [Test]
        public void support_optional_title_and_border()
        {
            // nest a child window inside the parent window to support the offsetting
            Assert.Inconclusive("New requirement");
        }

        [Test]
        public void allow_you_to_fill_the_new_window_with_background()
        {
            Assert.Inconclusive("New behavior");
            var con = new MockConsole(5, 5);
            var expected1 = new[]
            {
                " wk wk wk wk wk",
                " wk wk wk wk wk",
                " wk wk wk wk wk",
                " wk wk wk wk wk",
                " wk wk wk wk wk"
            };
            con.BufferWithColor.ShouldBeEquivalentTo(expected1);
            var w = new Window(1, 1, 3, 3, ConsoleColor.Red, ConsoleColor.DarkYellow, true, con);
            //w.Write("12");

            var expected2 = new[]
            {
                " wk wk wk wk wk",
                " wk rY rY rY wk",
                " wk rY rY rY wk",
                " wk rY rY rY wk",
                " wk wk wk wk wk"
            };

            con.BufferWithColor.ShouldBeEquivalentTo(expected2);

        }

    }
}
