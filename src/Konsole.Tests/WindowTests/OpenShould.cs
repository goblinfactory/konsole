using System;
using FluentAssertions;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.WindowTests
{
    public class OpenShould
    {
        [Test]
        public void open_a_window_with_border_using_default_values()
        {
            var c = new MockConsole(10,5);
            var w = Window.Open(0, 0, 10, 5,"title", LineThickNess.Double, ConsoleColor.White, ConsoleColor.Black, c);
            w.WriteLine("one");
            w.WriteLine("two");
            w.Write("three");
            Console.WriteLine(c.BufferString);
            var expected = new[]
            {
                "╔═ title ╗",
                "║one     ║",
                "║two     ║",
                "║three   ║",
                "╚════════╝"
            };
            c.BufferWritten.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void return_an_inside_scrollable_window_that_exactly_fits_inside_the_box_with_the_title()
        {
            var c = new MockConsole(10, 8);
            var w = Window.Open(0, 0, 8, 6, "title", LineThickNess.Double, White, Black, c);
            w.WindowHeight.Should().Be(4);
            w.WindowWidth.Should().Be(6);
        }

        [Test]
        public void open_a_window_that_can_be_scrolled()
        {
            var c = new MockConsole(10, 8);
            var w = Window.Open(0, 0, 10, 5, "title", LineThickNess.Double, ConsoleColor.White, ConsoleColor.Black, c);
            w.WriteLine("one");
            w.WriteLine("two");
            w.WriteLine("three");
            w.WriteLine("four");
            var expected = new[]
            {
                "╔═ title ╗",
                "║three   ║",
                "║four    ║",
                "║        ║",
                "╚════════╝"
            };

            c.BufferWritten.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void draw_a_box_around_the_scrollable_window_with_a_centered_title_()
        {
            var c = new MockConsole(10, 8);
            var w = Window.Open(0, 0, 10, 5, "title", LineThickNess.Double, White, Black, c);
            var expected = new[]
            {
                "╔═ title ╗",
                "║        ║",
                "║        ║",
                "║        ║",
                "╚════════╝"
            };
            c.BufferWritten.Should().BeEquivalentTo(expected);
        }

        [Test]
        [TestCase(LineThickNess.Double)]
        [TestCase(LineThickNess.Single)]
        public void draw_a_box_around_the_scrollable_window(LineThickNess thickness)
        {
            var c = new MockConsole(10, 8);
            var w = Window.Open(0, 0, 10, 5, "title", thickness, White, Black, c);
            var expected = new string[0];
            switch (thickness)
            {
                case LineThickNess.Single:
                    expected = new[]
                    {
                        "┌─ title ┐",
                        "│        │",
                        "│        │",
                        "│        │",
                        "└────────┘"
                    };
                    break;
                case LineThickNess.Double:
                    expected = new[]
                    {
                        "╔═ title ╗",
                        "║        ║",
                        "║        ║",
                        "║        ║",
                        "╚════════╝"
                    };
                    break;

            }
            c.BufferWritten.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void clip_child_window_to_not_exceed_parent_boundaries_test1()
        {
            var con = new MockConsole(40, 10);
            var win = Window.Open(0, 0, 40, 10, "test", LineThickNess.Double, White, Black, con);
            var child = Window.Open(20, 0, 30, 6, "child", LineThickNess.Double, White, Black, win);
            child.WriteLine("test");

            // this is the current behaviour, not ideal, but test needs to be this until
            // the change is made.
            var expected = new[]
            {
            "╔════════════════ test ════════════════╗",
            "║                    ╔═══════════ child║",
            "║                    ║test            ║║",
            "║                    ║                ║║",
            "║                    ║                ║║",
            "║                    ║                ║║",
            "║                    ╚═════════════════║",
            "║                                      ║",
            "║                                      ║",
            "╚══════════════════════════════════════╝"
            };

            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void clip_child_window_to_not_exceed_parent_boundaries_test2()
        {
            var con = new MockConsole(40, 10);
            var win = Window.Open(20, 5, 30, 20, "test", LineThickNess.Double, White, Black, con);
            win.WriteLine("cats and dogs");

            // this is the current behaviour, not ideal, but test needs to be this until
            // the change is made.
            var expected = new[]
            {
            "                                        ",
            "                                        ",
            "                                        ",
            "                                        ",
            "                                        ",
            "                    ╔═══════════ test ══",
            "                    ║cats and dogs     ║",
            "                    ║                  ║",
            "                    ║                  ║",
            "                    ║                  ║"
            };

            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        [TestCase("title")]
        [TestCase("titles")]
        [TestCase("catsandDogs is a long title")]
        [TestCase(null)]
        [TestCase("")]
        public void text_should_be_centered_or_clipped(string title)
        {
            var c = new MockConsole(10, 4);
            var w = Window.Open(0, 0, 10, 4, title, LineThickNess.Double, White, Black, c);

            string[] expected = new string[0];
            switch (title)
            {
                case "title":
                    expected = new[]
                    {
                        "╔═ title ╗",
                        "║        ║",
                        "║        ║",
                        "╚════════╝"
                    };
                    break;
                case "catsandDogs is a long title":
                    expected = new[]
                    {
                        "╔ catsand╗",
                        "║        ║",
                        "║        ║",
                        "╚════════╝"
                    };
                    break;

                case "titles":
                    expected = new[]
                    {
                        "╔ titles ╗",
                        "║        ║",
                        "║        ║",
                        "╚════════╝"
                    };
                    break;
                case null:
                    expected = new[]
                    {
                        "╔════════╗",
                        "║        ║",
                        "║        ║",
                        "╚════════╝"
                    };
                    break;

                case "":
                    expected = new[]
                    {
                        "╔════════╗",
                        "║        ║",
                        "║        ║",
                        "╚════════╝"
                    };
                    break;
            }
            c.BufferWritten.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WhenNested_draw_a_box_around_the_scrollable_window_with_a_centered_title_and_return_a_live_window_at_the_correct_screen_location()
        {
            var con = new MockConsole(20, 8);
            var w = Window.Open(0, 0, 20, 8, "title", LineThickNess.Double, White, Black, con);
            w.WriteLine("line1");
            w.WriteLine("line2");
            var child = Window.Open(7, 2, 8, 4, "c1", LineThickNess.Single, White, Black, w);
            var expected = new[]
            {
                "╔══════ title ═════╗",
                "║line1             ║",
                "║line2             ║",
                "║       ┌─ c1 ─┐   ║",
                "║       │      │   ║",
                "║       │      │   ║",
                "║       └──────┘   ║",
                "╚══════════════════╝"
            };

            con.BufferWritten.Should().BeEquivalentTo(expected);

            child.WriteLine("cats");
            child.Write("dogs");
            expected = new[]
            {
                "╔══════ title ═════╗",
                "║line1             ║",
                "║line2             ║",
                "║       ┌─ c1 ─┐   ║",
                "║       │cats  │   ║",
                "║       │dogs  │   ║",
                "║       └──────┘   ║",
                "╚══════════════════╝"
            };

            con.BufferWritten.Should().BeEquivalentTo(expected);

            // should not interfere with original window cursor position so should still be able to continue writing as 
            // if no new child window had been created.

            w.WriteLine("line3");
            w.WriteLine("line4");

            expected = new[]
{
                "╔══════ title ═════╗",
                "║line1             ║",
                "║line2             ║",
                "║line3  ┌─ c1 ─┐   ║",
                "║line4  │cats  │   ║",
                "║       │dogs  │   ║",
                "║       └──────┘   ║",
                "╚══════════════════╝"
            };

            con.BufferWritten.Should().BeEquivalentTo(expected);
        }
    }
}
