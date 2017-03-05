using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Konsole.Drawing;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    class ConstructorWithSettingsShould
    {
        public class FillBackgroundShould
        {
            [Test]
            public void not_change_parent_state()
            {
                var con = new MockConsole(5, 5);
                var state = con.State;
                var settings = new WindowSettings()
                {
                    X = 1,
                    Y = 1,
                    Height = 3,
                    Width = 3,
                    ForegroundColor = ConsoleColor.Red,
                    BackgroundColor = ConsoleColor.DarkYellow,
                    Echo = true,
                    EchoConsole = con,
                    FillBackground = true
                };
                var w = new Window(settings);
                con.State.ShouldBeEquivalentTo(state);
            }
        }

        [Test]
        public void support_optional_title_and_border()
        {
            Assert.Inconclusive();
            //var con = new MockConsole(8, 7);
            //var expected1 = new[]
            //{
            //    "        ",
            //    "        ",
            //    "        ",
            //    "        ",
            //    "        ",
            //    "        ",
            //    "        "
            //};

            //con.Buffer.ShouldBeEquivalentTo(expected1);
            //var settings = new WindowSettings()
            //{
            //    X = 1,
            //    Y = 1,
            //    Height = 5,
            //    Width = 5,
            //    ForegroundColor = ConsoleColor.Red,
            //    BackgroundColor = ConsoleColor.DarkYellow,
            //    Echo = true,
            //    EchoConsole = con,
            //    FillBackground = true,
            //    Title = "test",
            //    BorderThickness = LineThickNess.Single,
            //    Border = true
            //};
            //var w = new Window(settings);

            //var expected2 = new[]
            //{
            //    "        ",
            //    " ┌test┐ ",
            //    " │    │ ",
            //    " │    │ ",
            //    " │    │ ",
            //    " └────┘ ",
            //    "        ",
            //};

            //con.Buffer.ShouldBeEquivalentTo(expected2);
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
