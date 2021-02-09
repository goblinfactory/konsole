using System;
using ApprovalTests.Reporters;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class WriteShould
    {
        // eseential change is to only scroll on beginning of print, not end of print, must mark bool (needs scroll)
        // so that we can print neatly to end of screen without always having an empty line at the bottom of the screen
        // even a writeLine should only set "needs scroll".

        // when writing to last char, set "needs CR", and do the CR at the beginning of any write or writeLine

        [Test]
        public void when_writing_to_last_char_on_screen_but_not_last_row_move_cursor_to_next_line()
        {
            Assert.Inconclusive("");
            var con = new MockConsole(6, 4);
            con.Write("123456");
            con.Write("XY    ");
            var expected = new[]
            {
                "123456",
                "XY    ",
                "      ",
                "      "
            };
            con.Buffer.ShouldBe(expected);
        }

        // for now, if we want to write to a box to fill it completely simply enable clipping instead.
        //[Test]
        //public void when_Write_to_last_char_on_screen_and_on_last_row_dont_scroll_until_more_printing_happens()
        //{
        //    Assert.Fail("this is not a bug, this is a new feature! so need to fix all the other bugs first..before implementing this.");
        //    var con = new MockConsole(6, 4);
        //    con.WriteLine("123");
        //    con.WriteLine("abc");
        //    con.WriteLine("456");
        //    con.Write("defghi");
        //    var expected = new[]
        //    {
        //        "123   ",
        //        "abc   ",
        //        "456   ",
        //        "defghi"
        //    };
        //    con.Buffer.ShouldBe(expected);
        //    con.Write("123");

        //    expected = new[]
        //    {
        //        "abc   ",
        //        "456   ",
        //        "defghi",
        //        "123   "
        //    };
        //    con.Buffer.ShouldBe(expected);
        //}

        //[Ignore("fix later after refactoring write and writeline to seperate partial")]
        //[Test]
        //public void when_printing_ends_exactly_on_last_char_of_screen_do_not_automatically_scroll_until_user_starts_printing_again()
        //{
        //    // so that we can allow user to print using all the lines of the window.
        //    var con = new MockConsole(6, 3);
        //    con.Write("123456");
        //    con.Write("ABCDEF");
        //    con.Write("789012");
        //    var expected = new[]
        //    {
        //        "123456",
        //        "ABCDEF",
        //        "789012"
        //    };

        //    con.Buffer.ShouldBe(expected);
        //    con.Write(".");
        //    expected = new[]
        //    {
        //        "ABCDEF",
        //        "789012",
        //        ".     "
        //    };
        //    con.Buffer.ShouldBe(expected);
        //}

        [Test]
        public void allow_embedded_interpolations_without_exception()
        {
            var con = new MockConsole(7, 3);
            con.Write("{0}");
            con.Write("{0}", "cat");
            con.Write("{json}");
            var expected = new[]
            {
                "{0}cat{",
                "json}  ",
                "       "
            };
            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void write_relative_to_the_window_being_printed_to_not_the_parent()
        {
            var c = new MockConsole(6, 4);
            c.WriteLine("------");
            c.WriteLine("------");
            c.WriteLine("------");
            c.Write("------");
            var w = c.Open(new WindowSettings
            {
                SX = 1,
                SY = 1,
                Width = 4,
                Height = 2,
                Transparent = true
            });
            w.Write("X");
            w.Write(" Y");
            var expected = new[]
            {
                "------",
                "-X Y--",
                "------",
                "------"
            };
            Console.WriteLine(c.BufferWrittenString);
            c.Buffer.ShouldBe(expected);
        }

        [Test]
        public void when_on_last_line_and_write_long_lines_causes_wrapping_should_write_to_end_of_line_scrollWindow_and_move_cursor_to_begginning_of_last_line_and_print_balance_foreach_portion_of_line()
        {
            // need to repeat for longer lines as well
            var console = new MockConsole(20, 5);
            console.WriteLine("line1");
            console.WriteLine("line2");
            console.WriteLine("line3");
            console.WriteLine("line4");
                    //     12345678901234567890123456789012345678901234567890123456789012345
            console.Write("line 3 is a sixty five char long line that will wrap three times.");

            var expected = new[]{
                "line4               ",
                "line 3 is a sixty fi",
                "ve char long line th",
                "at will wrap three t",
                "imes.               ",
                }; 
            
            console.Buffer.ShouldBe(expected);
        }

        [Test]
        public void when_nested_and_on_last_line_and_write_long_lines_causes_wrapping_should_write_to_end_of_line_scrollWindow_and_move_cursor_to_begginning_of_last_line_and_print_balance_foreach_portion_of_line()
        {
            // need to repeat for longer lines as well
            var parent = new MockConsole(25, 10);
            var console = parent.Open(20, 5);
            console.WriteLine("line1");
            console.WriteLine("line2");
            console.WriteLine("line3");
            console.WriteLine("line4");
            //     12345678901234567890123456789012345678901234567890123456789012345
            console.Write("line 3 is a sixty five char long line that will wrap three times.");

            var expected = new[]{
                "line4                    ",
                "line 3 is a sixty fi     ",
                "ve char long line th     ",
                "at will wrap three t     ",
                "imes.                    ",
                "                         ",
                "                         ",
                "                         ",
                "                         ",
                "                         ",
                };
            parent.Buffer.ShouldBe(expected);
        }

        [Test]
        public void when_printing_on_last_line_causes_wrap_must_scroll_the_window()
        {
            var con = new MockConsole(5, 4);
            con.WriteLine("abcde");
            con.WriteLine("fghij");
            con.WriteLine("klmno");
            con.Write("pqrst");

            var expected = new[]{
                "abcde",
                "fghij",
                "klmno",
                "pqrst"
                };

            con.Buffer.ShouldBe(expected);

            // nested window without border
            var nested = con.Open(1, 1, 3, 2);
            nested.WriteLine("123");
            nested.Write("456");

            expected = new[]{
                "abcde",
                "f123j",
                "k456o",
                "pqrst",
                };

            con.Buffer.ShouldBe(expected);

            nested.Clear();
            nested.WriteLine("123");
            nested.Write("4567");

            expected = new[]{
                "abcde",
                "f456j",
                "k7  o",
                "pqrst",
                };

            con.Buffer.ShouldBe(expected);
        }

        [Test]
        public void when_nested_with_box_and_on_last_line_and_write_long_lines_causes_wrapping_should_write_to_end_of_line_scrollWindow_and_move_cursor_to_begginning_of_last_line_and_print_balance_foreach_portion_of_line()
        {
            // need to repeat for longer lines as well
            var parent = new MockConsole(25, 10);
            var p2 = parent.Open(22, 7, "parent");
            var console = p2.Open(20, 5);
            console.WriteLine("line1");
            var wrap = "a line ...........that wraps";
            console.WriteLine(wrap);
            console.WriteLine("line3");
            console.WriteLine(wrap);

            var expected = new[]{
                "┌────── parent ──────┐   ",
                "│at wraps            │   ",
                "│line3               │   ",
                "│a line ...........th│   ",
                "│at wraps            │   ",
                "│                    │   ",
                "└────────────────────┘   ",
                "                         ",
                "                         ",
                "                         ",
                };

            parent.Buffer.ShouldBe(expected);
        }

        [Test]
        public void when_on_last_line_and_write_causes_wrapping_should_write_to_end_of_line_scrollWindow_and_move_cursor_to_begginning_of_last_line_and_print_balance()
        {
            // need to repeat for longer lines as well
            var console = new MockConsole(80, 20);
            console.WriteLine("line1");
            console.Write("This ");
            console.Write("is ");
            console.WriteLine("a test line.");
            console.WriteLine("line 3");

            var expected = new[]
            {
                "line1",
                "This is a test line.",
                "line 3"
            };
            System.Console.WriteLine(console.BufferWrittenString);
            Assert.That(console.BufferWrittenTrimmed, Is.EqualTo(expected));
        }

        [Test]
        public void write_to_end_of_line_and_WriteLine_should_write_to_current_line_and_move_cursor_to_beginning_of_next_line()
        {
            var console = new MockConsole(80, 20);
            console.WriteLine("line1");
            console.Write("This ");
            console.Write("is ");
            console.WriteLine("a test line.");
            console.WriteLine("line 3");

            var expected = new[]
            {
                "line1",
                "This is a test line.",
                "line 3"
            };
            System.Console.WriteLine(console.BufferWrittenString);
            Assert.That(console.BufferWrittenTrimmed, Is.EqualTo(expected));
        }

        [Test]
        public void print_to_the_parent_if_echo_set()
        {
            var console = new MockConsole(3, 3);
            console.ForegroundColor = ConsoleColor.Red;
            console.BackgroundColor = ConsoleColor.White;
            console.PrintAt(0, 0, "X");

            var w = new Window(console);
            w.Write("YY");

            var expectedAfter = new[]
            {
                "YY ",
                "   ",
                "   "
            };

            Assert.AreEqual(expectedAfter, console.Buffer);
        }


        [Test]
        public void not_increment_cursortop_or_left_of_parent_window()
        {
            var parent = new MockConsole(80, 20);
            var state = parent.State;

            var console = new Window(parent);
            state.Should().BeEquivalentTo(parent.State);

            console.Write("This is");
            state.Should().BeEquivalentTo(parent.State);

            console.Write(" a test line");
            state.Should().BeEquivalentTo(parent.State);
        }

        [Test]
        public void replace_crlf_cr_lf_lfcr_with_individual_writeLines()
        {
            var w = new MockConsole(20, 5);
            (var left, var right) = w.SplitLeftRight("left", "right");

            left.Write("one\r\ntwo\rthree\nfour");
            right.Write("one\r\ntwo\rthree\nfour");
            var expected = new[]
            {
                "┌─ left ─┬─ right ─┐",
                "│two     │two      │",
                "│three   │three    │",
                "│four    │four     │",
                "└────────┴─────────┘"
            };
            w.Buffer.Should().BeEquivalentTo(expected);
        }


    }
}