using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole.Tests.WindowTests
{
    class VerySmallWindowConstructors
    {
        [Test]
        public void When_0_lines_tall_test()
        {
            var con = new MockConsole(10, 3);
            con.WriteLine("line1");
            Window.HostConsole = con;
            var win = new Window(5, 0);
            win.WriteLine("win");
            con.WriteLine("line2");

            var expected = new[]{
                "line1     ",
                "line2     ",
                "          "
                };
            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void When_0_chars_wide()
        {
            var con = new MockConsole(10, 3);
            con.WriteLine("line1");
            Window.HostConsole = con;
            var win = new Window(0, 5);
            win.WriteLine("win");
            con.WriteLine("line2");

            var expected = new[]{
                "line1     ",
                "line2     ",
                "          "
                };
            con.Buffer.ShouldBe(expected);
        }

        public void Should_not_callScrollBuffer_if_window_only_1_character_tall()
        {
            // nothing to scroll then!
        }

        [Test]
        public void When_1_lines_tall_test()
        {
            var con = new MockConsole(10, 4);
            con.WriteLine("line1");
            Window.HostConsole = con;
            var win = new Window(5, 1);
            win.Write("win");
            con.WriteLine("line2");

            var expected = new[]{
                "line1     ",
                "win       ",
                "line2     ",
                "          "
                };
            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void when_2_lines_tall_test()
        {

        }

        [Test]
        public void When_3_lines_tall_test()
        {

        }
    }

}
