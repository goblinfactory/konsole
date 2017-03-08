using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Konsole.Tests.WindowTests;
using NUnit.Framework;

namespace Konsole.Tests.CrossCuttingConcerns
{
    /// <summary>
    /// I'm using the convention of "RequirementXYZ" when a requirement relates to the class itself and not to a single method, where MethodXShould is the naming convention.
    /// </summary>
    class ScrollingAndClippingRequirements
    {
        public class WhenClippingEnabled_printing_should
        {
            [Test]
            public void When_clipping_enabled_printing_should_clip_all_text_that_overflows_the_bottom_of_the_screen_and_not_scroll_the_screen()
            {
                // need to test PrintAt, Write, WriteLine
                var c = new MockConsole(6, 4);
                Assert.True(c.Clipping);
                c.WriteLine("one");
                c.WriteLine("two");
                c.WriteLine("three");
                c.WriteLine("four");
                c.WriteLine("five");
                var expected = new[]
                {
            "one   ",
            "two   ",
            "three ",
            "four  "
            };
                c.Buffer.ShouldBeEquivalentTo(expected);
            }

        }

        public class WhenScrollingEnabled_printing_should
        {
            [Test]
            public void scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
            {
                Assert.Inconclusive("new requirement");
            }

        }


    }
}
