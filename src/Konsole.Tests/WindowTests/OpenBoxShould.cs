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
        public void WhenNestedShould_open_a_window_with_border_using_default_values()
        {
            _console = new MockConsole(10, 7);
            Window.HostConsole = _console;
            var parent = Window.OpenBox("parent");
            var child = parent.OpenBox("child");
            child.WriteLine("......");
            child.WriteLine("......");
            child.Write("......");
            var expected = new[]
            {
                "┌ parent ┐",
                "│┌ child┐│",
                "││......││",
                "││......││",
                "││......││",
                "│└──────┘│",
                "└────────┘"
            };

            _console.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WhenNestedShould_print_relative_to_the_window_being_printed_to_not_the_parent()
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
        public void open_a_Floating_window_using_provided_sizes()
        {
            Window.OpenBox("x", 1, 1, 5, 3);
            var expected = new[]
            {
                "          ",
                " ┌ x ┐    ",
                " │   │    ",
                " └───┘    ",
                "          "
            };

            //"          ",
            //" ┌ x┐     ",
            //" └        ",
            //"          ",
            //"          "



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
        public void WhenOpeningInlineShould_open_window_at_current_cursorTop()
        {
            var console = new MockConsole(10, 9);
            Window.HostConsole = console;
            console.WriteLine("one");
            var box1 = Window.OpenBox("A", 5, 3);
            console.WriteLine("two");
            var box2 = Window.OpenBox("B", 5, 3);
            console.Write("Under B");
            var expected = new[]
            {
                "one       ",
                "┌ A ┐     ",
                "│   │     ",
                "└───┘     ",
                "two       ",
                "┌ B ┐     ",
                "│   │     ",
                "└───┘     ",
                "Under B   "
            };
            console.Buffer.Should().BeEquivalentTo(expected);
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

        // needs new feature for box, i.e. that box clips parent and draws box inside parent boundary.

        //[Test]
        //public void clip_child_window_to_not_exceed_parent_boundaries_test1()
        //{
        //    var con = new MockConsole(40, 10);
        //    Window.HostConsole = con;
        //    var parent = Window.OpenBox("test", new BoxStyle() { ThickNess = LineThickNess.Double, Line = new Colors(White, Black) });
        //    var child = parent.OpenBox("child", 20, 0, 30, 6, new BoxStyle() { ThickNess = LineThickNess.Double });
        //    child.WriteLine("test");

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
        //    };
        //    con.Buffer.Should().BeEquivalentTo(expected);
        //}

        [Test]
        public void should_not_move_parent_cursor_when_box_is_not_inline()
        {
            var con = new MockConsole(20, 9);
            Window.HostConsole = con;
            var parent = Window.OpenBox("parent", 0, 0, 20, 8, new BoxStyle() { ThickNess = LineThickNess.Double });
            // write the child, and then check if parent cursor still 
            // at 0,0 by writing two lines to parent
            var child = parent.OpenBox("c1", 7, 2, 8, 4, new BoxStyle() { ThickNess = LineThickNess.Single });
            parent.WriteLine("line1");
            parent.WriteLine("line2");

            var expected = new[]
            {
                        "╔═════ parent ═════╗",
                        "║line1             ║",
                        "║line2             ║",
                        "║       ┌─ c1 ─┐   ║",
                        "║       │      │   ║",
                        "║       │      │   ║",
                        "║       └──────┘   ║",
                        "╚══════════════════╝",
                        "                    "
                    };

            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void WhenNested_draw_a_box_around_the_scrollable_window_with_a_centered_title_and_return_a_live_window_at_the_correct_screen_location()
        {
            var con = new MockConsole(20, 9);
            Window.HostConsole = con;
            var parent = Window.OpenBox("parent", 0, 0, 20, 8, new BoxStyle() { ThickNess = LineThickNess.Double });
            var child = parent.OpenBox("c1", 7, 2, 8, 4);
            parent.WindowWidth.Should().Be(18);
            parent.WindowHeight.Should().Be(6);
            //var child = parent.OpenBox("c1", 7, 2, 8, 4);

            parent.WriteLine("line1");
            parent.WriteLine("line2");

            var expected = new[]
            {
                        "╔═════ parent ═════╗",
                        "║line1             ║",
                        "║line2             ║",
                        "║       ┌─ c1 ─┐   ║",
                        "║       │      │   ║",
                        "║       │      │   ║",
                        "║       └──────┘   ║",
                        "╚══════════════════╝",
                        "                    "
            };

            con.Buffer.Should().BeEquivalentTo(expected);

            child.WriteLine("cats");
            child.Write("dogs");
            
            expected = new[]
            {
                        "╔═════ parent ═════╗",
                        "║line1             ║",
                        "║line2             ║",
                        "║       ┌─ c1 ─┐   ║",
                        "║       │cats  │   ║",
                        "║       │dogs  │   ║",
                        "║       └──────┘   ║",
                        "╚══════════════════╝",
                        "                    "
            };

            con.Buffer.Should().BeEquivalentTo(expected);

            // should not interfere with original window cursor position so should still be able to continue writing as 
            // if no new child window had been created.

            parent.WriteLine("line3");
            parent.WriteLine("line4");

            expected = new[]
{
                        "╔═════ parent ═════╗",
                        "║line1             ║",
                        "║line2             ║",
                        "║line3  ┌─ c1 ─┐   ║",
                        "║line4  │cats  │   ║",
                        "║       │dogs  │   ║",
                        "║       └──────┘   ║",
                        "╚══════════════════╝",
                        "                    "
            };

            con.Buffer.Should().BeEquivalentTo(expected);
        }
    }
}
