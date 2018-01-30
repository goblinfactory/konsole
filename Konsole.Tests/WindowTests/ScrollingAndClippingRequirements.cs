using System;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Konsole.Layouts;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    [UseReporter(typeof(DiffReporter))]
    class ScrollingAndClippingRequirements
    {
        // Nesting SplitWindows is when you call w.SplitLeft() or w.SplitRight() on a window already created by calling split.
        // for example w1 = new Window(); w2 = w1.SplitLeft(); w3 = w2.SplitRight();
        // in the example above, w3 is a nested SplitWindow and w1. w2 is not a nested split window.

        [Test]
        public void when_nesting_split_windows_printing_that_causes_scrolling_SHOULD_scroll_the_nested_portion_of_the_screen_only()
        {
            var w = new MockConsole(20,12);

            var left = w.SplitLeft("left");
            var right = w.SplitRight("right");
            var nestedTop = left.SplitTop("ntop");
            var nestedBottom = left.SplitBottom("nbot");
            void Writelines(IConsole con)
            {
                con.WriteLine("one");
                con.WriteLine("two");
                con.WriteLine("three");
                con.Write("four");
            }

            Writelines(nestedTop);
            Writelines(nestedBottom);
            Writelines(right);

            var expected = new[]
            {
                "┌─ left ─┐┌─ right ┐",
                "│┌ ntop ┐││one     │",
                "││two   │││two     │",
                "││three │││three   │",
                "││four  │││four    │",
                "│└──────┘││        │",
                "│┌ nbot ┐││        │",
                "││two   │││        │",
                "││three │││        │",
                "││four  │││        │",
                "│└──────┘││        │",
                "└────────┘└────────┘"
            };

            Console.WriteLine(w.BufferString);
            w.Buffer.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void when_clipping_is_enabled_WriteLine_SHOULD_clip_all_text_that_overflows_the_bottom_of_the_screen_and_not_scroll_the_screen()
        {
            // need to test PrintAt, Write, WriteLine
            var c = new MockConsole(6, 4);
            var w = new Window(c, 6, 4, K.Clipping);
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
        public void when_clipping_is_enabled_Write_SHOULD_clip_all_text_that_overflows_the_bottom_of_the_screen_and_not_scroll_the_screen()
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

        [Test]
        public void When_scrolling_is_enabled_WriteLine_when_on_bottom_line_SHOULD_Scroll_screen_up_1_line_for_each_line_that_overflows()
        {
            var c = new MockConsole(6, 2);
            var w = new Window(c, 6, 2, K.Scrolling);
            w.WriteLine("cat");
            w.WriteLine("dog");
            var expected = new[]
            {
                    "dog   ",
                    "      "
                };
            c.Buffer.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_Write_then_WriteLine_without_overflowing_width_SHOULD_scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
        {
            var c = new MockConsole(6, 2);
            var w = new Window(c, 6, 2, K.Scrolling);
            w.WriteLine("cat");
            w.WriteLine("dog");
            w.Write("mouse");
            var expected = new[]
            {
                    "dog   ",
                    "mouse "
                };
            c.Buffer.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_And_on_last_line_and_overflowing_width_Write_SHOULD_cause_a_scroll()
        {
            var c = new MockConsole(6, 2);
            var w = new Window(c, 6, 2, K.Scrolling);
            w.CursorLeft = 0;
            w.CursorTop = 1;
            w.Write("abcdefg");
            var expected = new[]
            {
                    "abcdef",
                    "g     "
                };
            c.Buffer.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_And_on_last_line_and_overflowing_width_Write_and_WriteLine_SHOULD_cause_a_scroll()
        {
            var c = new MockConsole(6, 3);
            var w = new Window(c, 6, 3, K.Scrolling);
            w.CursorLeft = 0;
            w.CursorTop = 1;
            w.Write("abcd");
            w.WriteLine("efg");
            var expected = new[]
            {
                    "abcdef",
                    "g     ",
                    "      "
                };
            c.Buffer.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_And_on_last_line_and_not_overflowing_width_Write_SHOULD_not_cause_a_scroll()
        {
            var c = new MockConsole(6, 2);
            var w = new Window(c, 6, 2, K.Scrolling);
            w.CursorLeft = 0;
            w.CursorTop = 1;
            w.Write("abcdef");
            var expected = new[]
            {
                    "      ",
                    "abcdef"
                };
            c.Buffer.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_And_on_last_line_Write_SHOULD_not_cause_a_scroll()
        {
            var c = new MockConsole(6, 2);
            var w = new Window(c, 6, 2, K.Scrolling);
            w.CursorLeft = 0;
            w.CursorTop = 1;
            w.Write("abc");
            var expected = new[]
            {
                    "      ",
                    "abc   "
                };
            c.Buffer.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_Write_then_WriteLine_and_overflowing_width_SHOULD_scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
        {
            var c = new MockConsole(6, 4);
            var w = new Window(c, 6, 4, K.Scrolling);
            Assert.True(w.Scrolling);
            w.Write("111");
            w.WriteLine("aaaa");
            w.Write("222");
            w.WriteLine("bbbb");
            w.Write("333");
            w.WriteLine("cccc");
            w.Write("444");
            w.WriteLine("dddd");

            var expected = new[]
            {
                    "c     ",
                    "444ddd",
                    "d     ",
                    "      "
                };
            c.Buffer.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_WriteLine_SHOULD_scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
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
                    "three ",
                    "four  ",
                    "five  ",
                    "      "
                };
            c.Buffer.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_Write_SHOULD_scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
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
            c.Buffer.ShouldBeEquivalentTo(expected);
        }


    }


}
