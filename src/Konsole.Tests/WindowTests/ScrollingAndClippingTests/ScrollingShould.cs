using System;
using ApprovalTests.Reporters;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class ScrollingShould
    {        
        // Nesting SplitWindows is when you call w.SplitLeft() or w.SplitRight() on a window already created by calling split.
        // for example w1 = new Window(); w2 = w1.SplitLeft(); w3 = w2.SplitRight();
        // in the example above, w3 is a nested SplitWindow and w1. w2 is not a nested split window.
        [Test]
        public void when_nesting_split_windows_printing_that_causes_scrolling_SHOULD_scroll_the_nested_portion_of_the_screen_only()
        {
            var w = new MockConsole(20, 12);

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
            w.Buffer.ShouldBe(expected);
        }

        [Test]
        public void WhenFalse_Truncate_all_WriteLine_text_that_overflows_the_bottom_of_the_screen()
        {
            // need to test PrintAt, Write, WriteLine
            var c = new MockConsole(6, 4);
            var w = c.Open(new WindowSettings { Width = 6, Height = 4, Scrolling = false });
            w.WriteLine("one");
            w.WriteLine("two");
            w.WriteLine("three");
            w.WriteLine("four");
            w.WriteLine("five");
            w.WriteLine("six");
            var expected = new[]
            {
            "one   ",
            "two   ",
            "three ",
            "four  "
            };
            w.Buffer.ShouldBe(expected);
        }

        [Test]
        public void Scroll_screen_up_1_line_for_each_line_that_overflows()
        {
            var c = new MockConsole(6, 2);
            var w = c.Open(new WindowSettings { Width = 6, Height = 2, Scrolling = true });
            w.WriteLine("cat");
            w.WriteLine("dog");
            var expected = new[]
            {
                    "dog   ",
                    "      "
                };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void WhenScrolling_Write_then_WriteLine_without_overflowing_width_SHOULD_scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
        {
            var c = new MockConsole(6, 2);
            var w = c.Open(new WindowSettings { Width = 6, Height = 2, Scrolling = true });
            w.WriteLine("cat");
            w.WriteLine("dog");
            w.Write("mouse");
            var expected = new[]
            {
                    "dog   ",
                    "mouse "
                };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void Cause_a_scroll_when_on_last_line_and_Write_overflows_width()
        {
            var c = new MockConsole(6, 2);
            var w = c.Open(new WindowSettings { Width = 6, Height = 2, Scrolling = true });
            w.CursorLeft = 0;
            w.CursorTop = 1;
            w.Write("123456abc");
            var expected = new[]
            {
                    "123456",
                    "abc   "
            };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_And_on_last_line_and_overflowing_width_Write_and_WriteLine_SHOULD_cause_a_scroll()
        {
            var c = new MockConsole(6, 3);
            var w = c.Open(new WindowSettings { Width = 6, Height = 3, Scrolling = true });
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
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_And_on_last_line_and_not_overflowing_width_Write_SHOULD_not_cause_a_scroll()
        {
            var c = new MockConsole(6, 2);
            var w = c.Open(new WindowSettings { Width = 6, Height = 2, Scrolling = true });
            w.CursorLeft = 0;
            w.CursorTop = 1;
            w.Write("abcdef");
            var expected = new[]
            {
                    "      ",
                    "abcdef"
                };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_And_on_last_line_Write_SHOULD_not_cause_a_scroll()
        {
            var c = new MockConsole(6, 2);
            var w = c.Open(new WindowSettings { Width = 6, Height = 2, Scrolling = true });
            w.CursorLeft = 0;
            w.CursorTop = 1;
            w.Write("abc");
            var expected = new[]
            {
                    "      ",
                    "abc   "
                };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_Write_then_WriteLine_and_overflowing_width_SHOULD_scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
        {
            var c = new MockConsole(6, 4);
            var w = c.Open(new WindowSettings { Width = 6, Height = 4, Scrolling = true });
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
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_WriteLine_SHOULD_scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
        {
            var c = new MockConsole(6, 4);
            var w = c.Open( new WindowSettings { Width = 6, Height = 4, Scrolling = true });
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
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void When_scrolling_is_enabled_Write_SHOULD_scroll_the_screen_up_1_line_for_each_line_that_overflows_the_screen_height()
        {
            var c = new MockConsole(6, 4);
            var w = c.Open(new WindowSettings { Width = 6, Height = 4, Scrolling = true });
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
            c.Buffer.ShouldBe(expected);
        }


    }


}
