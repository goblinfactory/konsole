using System;
using FluentAssertions;
using Konsole.Tests.Helpers;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class PrintAtTests
    {
        [Test]
        public void WhenNestedShould_print_relative_to_the_window_being_printed_to_not_the_parent()
        {
            var c = new MockConsole(6, 6);
            Window.HostConsole = c;
            var parent = new Window(1, 1, 4, 4); // create a child window 4 x 4
            parent.WriteLine("....");
            parent.WriteLine("....");
            parent.WriteLine("....");
            parent.Write("....");
            var nested = parent.Open(1, 1, 2, 2);
            var expected = new[]
            {
                "      ",
                " .... ",
                " .  . ",
                " .  . ",
                " .... ",
                "      "
            };
            c.Buffer.ShouldBe(expected);
            nested.PrintAt(0, 0, 'X');
            nested.PrintAt(1, 1, 'Y');

            expected = new[]
            {
                "      ",
                " .... ",
                " .X . ",
                " . Y. ",
                " .... ",
                "      "
            };
            c.Buffer.ShouldBe(expected);
        }

        [Test]
    public void print_relative_to_the_window_being_printed_to_not_the_parent()
    {
        var c = new MockConsole(6, 4);
        Window.HostConsole = c;
        var w = new Window(1, 1, 4, 2);
        w.WriteLine("....");
        w.Write("....");
        w.PrintAt(0, 0, "X");
        w.PrintAt(1, 1, "Y");
        var expected = new[]
        {
                "      ",
                " X... ",
                " .Y.. ",
                "      "
            };
        c.Buffer.ShouldBe(expected);
    }

    [Test]
    public void Not_change_cursor_position_when_printing()
    {
        var c = new MockConsole(5, 3);
        c.PrintAt(4, 1, "O");
        c.WriteLine("one");

        var expected = new[]
        {
                "one  ",
                "    O",
                "     "
            };
        Assert.AreEqual(expected, c.Buffer);
    }


    [Test]
    public void print_the_text_at_the_required_x_y_coordinate()
    {
        var console = new MockConsole(5, 5);
        console.PrintAt(0, 0, "*");
        console.PrintAt(2, 2, "*");
        console.PrintAt(4, 4, "*");
        // all lines are trimmed
        var trimmed = new[]
        {
                "*",
                "",
                "  *",
                "",
                "    *",
            };

        var buffer = new[]
        {
                "*    ",
                "     ",
                "  *  ",
                "     ",
                "    *"
            };
        Assert.That(console.Buffer, Is.EqualTo(buffer));
    }


    [Test]
    public void overflow_any_overflow_text_to_next_line()
    {
        var console = new MockConsole(5, 5);
        console.PrintAt(2, 2, "12345");

        var expected = new[]
        {
                "     ",
                "     ",
                "  123",
                "45   ",
                "     "
            };
        Assert.AreEqual(expected, console.Buffer);
    }


    [Test]
    public void print_to_the_parent()
    {
        var console = new MockConsole(5, 3);
        console.ForegroundColor = ConsoleColor.Red;
        console.BackgroundColor = ConsoleColor.White;
        console.PrintAt(0, 0, "X");
        // if the window was not transparent, then this window would overwrite (blank out) the 'X' just printed above, and test would fail.
        // setting the window to transparent, keeps the underlying text visible. (showing through all non printed areas).
        var w = console.Open(new WindowSettings { Transparent = true });
        w.PrintAt(3, 1, "123");

        var expectedAfter = new[]
        {
                "X    ",
                "   12",
                "3    "
            };

        Assert.AreEqual(expectedAfter, console.Buffer);
    }


    [Test]
    public void echo_printing_to_parent_in_the_right_fore_and_back_colors()
    {
        var console = new MockConsole(3, 3);
        console.ForegroundColor = ConsoleColor.Red;
        console.BackgroundColor = ConsoleColor.White;
        console.PrintAt(0, 0, "X");

        var expectedBefore = new[]
        {
                "Xrw wk wk",
                " wk wk wk",
                " wk wk wk"
            };

        Precondition.Check(() => expectedBefore.ShouldBe(console.BufferWithColor));

        var w = console.Open( new WindowSettings { Transparent = true });
        w.ForegroundColor = ConsoleColor.DarkGreen;
        w.BackgroundColor = ConsoleColor.DarkCyan;
        w.PrintAt(1, 1, "YY");

        var expectedAfter = new[]
        {
                "Xrw wk wk",
                " wkYGCYGC",
                " wk wk wk"
            };

        Assert.AreEqual(expectedAfter, console.BufferWithColor);
    }


    [Test]
    public void not_change_the_parent_state()
    {
        var console = new MockConsole(3, 3);
        console.WriteLine("X");
        var state = console.State;

        var w = new Window(console);
        w.ForegroundColor = ConsoleColor.DarkGreen;
        w.BackgroundColor = ConsoleColor.DarkCyan;
        w.PrintAt(2, 2, "Y");
        console.State.Should().BeEquivalentTo(state);
    }





}


    }
