using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class OpenInlineShould
    {
        [Test]
        public void set_cursor_position_to_below_the_window()
        {
            var c = new MockConsole(10, 4);
            c.WriteLine("line1");
            var w = c.Open(2);
            w.WriteLine("cats");
            w.Write("dogs");
            c.Write("fruit1");
            var expected = new[]
            {
                "line1     ",
                "cats      ",
                "dogs      ",
                "fruit1    "
            };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void clip_the_height_to_fit_within_parent()
        {
            Assert.Inconclusive("Not yet implemented");
            //var c = new MockConsole(10, 10);
            //c.CursorLeft = 5;
            //c.CursorTop = 5;
            //var w = Window.OpenInline(c, 10);
            //w.WindowHeight.Should().Be(5);
        }

        [Test]
        public void use_the_full_screen_width_if_no_width_provided_and_move_cursor_of_host_to_below_inline_window_and_reset_x_position_to_left()
        {
            var c = new MockConsole(12, 10);
            c.CursorLeft = 5;
            c.CursorTop = 5;
            var w = c.Open(2);
            w.WindowWidth.Should().Be(12);
            c.CursorTop.Should().Be(8);
            c.CursorLeft.Should().Be(0);
        }

        [Test]
        [TestCase(0, 0, 4, 4)]
        [TestCase(0, 0, 7, 7)] // balance is 3
        [TestCase(0, 0, 15, 10)] 
        [TestCase(5, 2, 15, 8)] // clip the height to 8 (balance)
        public void use_balance_of_parent_height_as_defaults(int parentCurrentX, int parentCurrentY, int heightRows, int expectedHeight)
        {
            var c = new MockConsole(10, 10);
            c.CursorLeft = parentCurrentX;
            c.CursorTop = parentCurrentY;
            var w = c.Open(heightRows);
            w.WindowWidth.Should().Be(10);
            w.WindowHeight.Should().Be(expectedHeight);
        }
    }
}
