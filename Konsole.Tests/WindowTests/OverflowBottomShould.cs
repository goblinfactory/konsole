using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{

    class OverflowBottomShould
    {
        public void indicate_when_printing_has_gone_off_the_bottom_of_the_screen()
        {
            var c = new MockConsole(6,4);
            
            // default is clipping
            Assert.True(c.Clipping);
            Assert.False(c.OverflowBottom);
            c.WriteLine("one");
            c.WriteLine("two");
            c.WriteLine("three");
            c.Write("four");
            c.OverflowBottom.Should().BeFalse();

            var expected1 = new[]
            {
                "one   ",
                "two   ",
                "three ",
                "four  "
                };
            c.Buffer.ShouldBeEquivalentTo(expected1);

            // now this next print should overflow the bottom

            c.Write("5678");

            Assert.True(c.OverflowBottom);

            var expected2 = new[]
{
                "one   ",
                "two   ",
                "three ",
                "four56"
                };
            c.Buffer.ShouldBeEquivalentTo(expected2);
        }

    }
}
