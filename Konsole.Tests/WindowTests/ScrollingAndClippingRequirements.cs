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
        public class WhenClippingEnabled
        {
            [Test]
            public void WriteLine_SHOULD_clip_all_text_that_overflows_the_bottom_of_the_screen_and_not_scroll_the_screen()
            {
                // need to test PrintAt, Write, WriteLine
                var c = new MockConsole(6, 4);
                var w = new Window(c, 6, 4, K.Clipping);
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

            [Test]
            public void Write_SHOULD_clip_all_text_that_overflows_the_bottom_of_the_screen_and_not_scroll_the_screen()
            {
                // need to test PrintAt, Write, WriteLine
                var c = new MockConsole(6, 4);
                var w = new Window(c, 6, 4, K.Clipping);
                Assert.True(w.Clipping);
                w.WriteLine("one");
                w.WriteLine("two");
                w.WriteLine("three"); 
                w.Write("44444444"); // NB! there are 8 x 4's here, so we expect the last 2 four's to be clipped and not cause scrolling
                w.Write("555");     // should be ingored since scrolling is clipped.
                var expected = new[]
                {
                    "one   ",
                    "two   ",
                    "three ",
                    "444444"
                };
                w.Buffer.ShouldBeEquivalentTo(expected);
            }

        }

        public class WhenScrollingEnabled
        {
            [Test]
            public void WriteLine_SHOULD_scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
            {
                var c = new MockConsole(6, 4);
                var w = new Window(c, 6, 4, K.Scrolling);
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


            [Test]
            public void Write_SHOULD_scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
            {
                var c = new MockConsole(6, 4);
                var w = new Window(c, 6, 4, K.Scrolling);
                Assert.True(w.Scrolling);
                w.WriteLine("one");
                w.WriteLine("two");
                w.WriteLine("three");
                w.Write("44444444"); // there are 8 fours here, so we expect next line to start with 2 fours
                w.Write("55555555"); // there are 8 fives here, so we expect this line to start with two fours from the previous line, followed by 4 x fives.
                var expected = new[]
                {
                    "three ",
                    "444444",
                    "445555",
                    "5555  ",
                };

                Console.WriteLine("------");
                Console.WriteLine(c.BufferWrittenString);
                Console.WriteLine("------");
                c.Buffer.ShouldBeEquivalentTo(expected);
            }

        }

    }


}
