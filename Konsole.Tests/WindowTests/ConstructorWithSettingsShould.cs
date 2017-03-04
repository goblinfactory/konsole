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
            // The constructor has an indirect dependancy on PrintAt, used by Init() in constructor. bit dangerous! ;-o
            // i.e. if PrintAt fails, then this test will most likely also fail with a false negative.

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
            var settings = new WindowSettings() {
                X = 1,
                Y = 1,
                Height = 3,
                Width =  3,
                ForegroundColor = ConsoleColor.Red,
                BackgroundColor = ConsoleColor.DarkYellow,
                Echo = true,
                EchoConsole = con,
                FillBackground = true
            };
            var w = new Window(settings);

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
