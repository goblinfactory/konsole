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
            var w = Window.OpenInline(c, 2);
            w.WriteLine("cats");
            w.WriteLine("dogs");
            w.Write("fruit");
            c.Write("line2");
            var expected = new[]
            {
                "line1     ",
                "dogs      ",
                "fruit     ",
                "line2     "
            };
            c.Buffer.Should().BeEquivalentTo(expected);
        }


    }
}
