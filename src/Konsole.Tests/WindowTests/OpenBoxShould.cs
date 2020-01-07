using System;
using FluentAssertions;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.WindowTests
{
    public class OpenBoxShould
    {
        private MockConsole _console;
        
        [SetUp]
        public void Setup()
        {
            _console = new MockConsole(10, 5);
            Window.HostConsole = _console;
        }

        [Test]
        public void open_a_window_with_border_using_default_values()
        {
            Window.OpenBox("title");
            var expected = new[]
            {
                "┌─ title ┐",
                "│        │",
                "│        │",
                "│        │",
                "└────────┘"
            };
            _console.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void when_opening_inside_existing_window_open_a_window_with_border_using_default_values()
        {
            var parent = Window.OpenBox("parent");
            var child = parent.OpenBox("child");
            Fill(child);
            var expected = new[]
            {
                "┌ parent ┐",
                "│┌ child┐│",
                "││five  ││",
                "│└──────┘│",
                "└────────┘"
            };
            _console.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void return_an_inside_scrollable_window_that_exactly_fits_inside_the_box_with_the_title()
        {
            var win = Window.OpenBox("title");
            win.WindowHeight.Should().Be(3);
            win.WindowWidth.Should().Be(8);
        }

        static void Fill(IConsole con)
        {
            con.WriteLine("one");
            con.WriteLine("two");
            con.WriteLine("three");
            con.WriteLine("four");
            con.Write("five");
        }

        [Test]
        public void open_a_window_that_can_be_scrolled()
        {
            var win = Window.OpenBox("title", new BoxStyle() { ThickNess = LineThickNess.Double });
            Fill(win);
            var expected = new[]
            {
                        "╔═ title ╗",
                        "║three   ║",
                        "║four    ║",
                        "║five    ║",
                        "╚════════╝"
            };
            _console.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void open_an_inline_window_using_provided_sizes()
        {
            Window.OpenBox("x", 5, 3);
            var expected = new[]
            {
                "┌ x ┐     ",
                "│   │     ",
                "└───┘     ",
                "          ",
                "          "
            };
            _console.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void move_the_cursor_to_below_the_inline_window()
        {
            Window.OpenBox("x", 5, 3);
            _console.WriteLine("I am under");
            var expected = new[]
            {
                "┌ x ┐     ",
                "│   │     ",
                "└───┘     ",
                "I am under",
                "          "
            };
            _console.Buffer.Should().BeEquivalentTo(expected);
        }

        //[Test]
        //public void clip_child_window_to_not_exceed_parent_boundaries_test1()
        //{
        //    var con = new MockConsole(40, 10);
        //    Window.HostConsole = con;
        //    Window.OpenBox(0, 0, 40, 10, "test", LineThickNess.Double, White, Black, con);
        //    var child = Window.Open(20, 0, 30, 6, "child", LineThickNess.Double, White, Black, win);
        //    child.WriteLine("test");

        //    // this is the current behaviour, not ideal, but test needs to be this until
        //    // the change is made.
        //    var expected = new[]
        //    {
        //            "╔════════════════ test ════════════════╗",
        //            "║                    ╔═══════════ child║",
        //            "║                    ║test            ║║",
        //            "║                    ║                ║║",
        //            "║                    ║                ║║",
        //            "║                    ║                ║║",
        //            "║                    ╚═════════════════║",
        //            "║                                      ║",
        //            "║                                      ║",
        //            "╚══════════════════════════════════════╝"
        //            };
        //}

        //            con.Buffer.Should().BeEquivalentTo(expected);
        //        }

        //        [Test]
        //        public void clip_child_window_to_not_exceed_parent_boundaries_test2()
        //        {
        //            var con = new MockConsole(40, 10);
        //            var win = Window.Open(20, 5, 30, 20, "test", LineThickNess.Double, White, Black, con);
        //            win.WriteLine("cats and dogs");

        //            // this is the current behaviour, not ideal, but test needs to be this until
        //            // the change is made.
        //            var expected = new[]
        //            {
        //            "                                        ",
        //            "                                        ",
        //            "                                        ",
        //            "                                        ",
        //            "                                        ",
        //            "                    ╔═══════════ test ══",
        //            "                    ║cats and dogs     ║",
        //            "                    ║                  ║",
        //            "                    ║                  ║",
        //            "                    ║                  ║"
        //            };

        //            con.Buffer.Should().BeEquivalentTo(expected);
        //        }

        //        [Test]
        //        [TestCase("title")]
        //        [TestCase("titles")]
        //        [TestCase("catsandDogs is a long title")]
        //        [TestCase(null)]
        //        [TestCase("")]
        //        public void text_should_be_centered_or_clipped(string title)
        //        {
        //            var c = new MockConsole(10, 4);
        //            var w = Window.Open(0, 0, 10, 4, title, LineThickNess.Double, White, Black, c);

        //            string[] expected = new string[0];
        //            switch (title)
        //            {
        //                case "title":
        //                    expected = new[]
        //                    {
        //                        "╔═ title ╗",
        //                        "║        ║",
        //                        "║        ║",
        //                        "╚════════╝"
        //                    };
        //                    break;
        //                case "catsandDogs is a long title":
        //                    expected = new[]
        //                    {
        //                        "╔ catsand╗",
        //                        "║        ║",
        //                        "║        ║",
        //                        "╚════════╝"
        //                    };
        //                    break;

        //                case "titles":
        //                    expected = new[]
        //                    {
        //                        "╔ titles ╗",
        //                        "║        ║",
        //                        "║        ║",
        //                        "╚════════╝"
        //                    };
        //                    break;
        //                case null:
        //                    expected = new[]
        //                    {
        //                        "╔════════╗",
        //                        "║        ║",
        //                        "║        ║",
        //                        "╚════════╝"
        //                    };
        //                    break;

        //                case "":
        //                    expected = new[]
        //                    {
        //                        "╔════════╗",
        //                        "║        ║",
        //                        "║        ║",
        //                        "╚════════╝"
        //                    };
        //                    break;
        //            }
        //            c.BufferWritten.Should().BeEquivalentTo(expected);
        //        }

        //        [Test]
        //        public void WhenNested_draw_a_box_around_the_scrollable_window_with_a_centered_title_and_return_a_live_window_at_the_correct_screen_location()
        //        {
        //            var con = new MockConsole(20, 8);
        //            var w = Window.Open(0, 0, 20, 8, "title", LineThickNess.Double, White, Black, con);
        //            w.WriteLine("line1");
        //            w.WriteLine("line2");
        //            var child = Window.Open(7, 2, 8, 4, "c1", LineThickNess.Single, White, Black, w);
        //            var expected = new[]
        //            {
        //                "╔══════ title ═════╗",
        //                "║line1             ║",
        //                "║line2             ║",
        //                "║       ┌─ c1 ─┐   ║",
        //                "║       │      │   ║",
        //                "║       │      │   ║",
        //                "║       └──────┘   ║",
        //                "╚══════════════════╝"
        //            };

        //            con.BufferWritten.Should().BeEquivalentTo(expected);

        //            child.WriteLine("cats");
        //            child.Write("dogs");
        //            expected = new[]
        //            {
        //                "╔══════ title ═════╗",
        //                "║line1             ║",
        //                "║line2             ║",
        //                "║       ┌─ c1 ─┐   ║",
        //                "║       │cats  │   ║",
        //                "║       │dogs  │   ║",
        //                "║       └──────┘   ║",
        //                "╚══════════════════╝"
        //            };

        //            con.BufferWritten.Should().BeEquivalentTo(expected);

        //            // should not interfere with original window cursor position so should still be able to continue writing as 
        //            // if no new child window had been created.

        //            w.WriteLine("line3");
        //            w.WriteLine("line4");

        //            expected = new[]
        //{
        //                "╔══════ title ═════╗",
        //                "║line1             ║",
        //                "║line2             ║",
        //                "║line3  ┌─ c1 ─┐   ║",
        //                "║line4  │cats  │   ║",
        //                "║       │dogs  │   ║",
        //                "║       └──────┘   ║",
        //                "╚══════════════════╝"
        //            };

        //            con.BufferWritten.Should().BeEquivalentTo(expected);
        //        }
    }
}
