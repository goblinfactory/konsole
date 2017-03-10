using System;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
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
                var w = new Window(6, 4, c, K.Clipping);
                Assert.True(w.Clipping);
                w.WriteLine("one");
                w.WriteLine("two");
                w.WriteLine("three");
                w.WriteLine("four");
                w.WriteLine("five");
                var expected = new[]
                {
            "one   ",
            "two   ",
            "three ",
            "four  "
            };
                w.Buffer.ShouldBeEquivalentTo(expected);
            }

        }

        public class WhenScrollingEnabled_printing_should
        {
            [Test]
            public void scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
            {
                var c = new MockConsole(6, 4);
                var w = new Window(6, 4, c, K.Scrolling);
                Assert.True(w.Scrolling);
                w.WriteLine("one");
                w.WriteLine("two");
                w.WriteLine("three");
                w.WriteLine("four");
                w.WriteLine("five");
                var expected = new[]
                {
            "two   ",
            "three ",
            "four  ",
            "five  "
            };

                Console.WriteLine("---");
                Console.WriteLine(c.BufferWrittenString);
                Console.WriteLine("---");
                //w.Buffer.ShouldBeEquivalentTo(expected);

                // this test is faulty because we're assering on the Window and not the MockConsole
                c.Buffer.ShouldBeEquivalentTo(expected);
            }

        }

    }


}
