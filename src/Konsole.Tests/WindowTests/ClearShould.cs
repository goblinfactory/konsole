using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.WindowTests
{
    namespace ClearTests
    {

        public class WhenClearingWithDifferentBackgroundColor_should
        {
            private ConsoleState _state;
            private Window _con;
            private Window _window;

            [SetUp]
            public void setup()
            {
                _con = new MockConsole(5, 3);
                _state = _con.State;
                _window = _con.Open(3, 0, 2, 2, White, DarkCyan);
                _window.Clear();
            }

            [Test]
            public void set_the_background_color_of_the_window()
            {
                _con.BufferWithColor.ShouldBe(new[] {
                    " wk wk wk wC wC",
                    " wk wk wk wC wC",
                    " wk wk wk wk wk"
                });
            }

            [Test]
            public void not_affect_console_state()
            {
                _con.State.Should().BeEquivalentTo(_state);
            }

            [Test]
            public void keep_own_cursor_at_top_left()
            {
                _con.CursorTop.Should().Be(0);
                _con.CursorLeft.Should().Be(0);
            }

        }


        public class ClearShould
        {

            [Test]
            public void clear_the_window_and_buffer_of_any_written_lines()
            {
                var con = new MockConsole(10, 4);
                Window.HostConsole = con;
                var w1 = new Window(7, 2);
                w1.WriteLine("one");
                w1.Write("two");
                Assert.AreEqual(new[] { "one", "two" }, w1.BufferWrittenTrimmed);
                Assert.AreEqual(new[] { "one", "two" }, con.BufferWrittenTrimmed);
                w1.Clear();
                w1.WriteLine("three");
                Assert.AreEqual(new string[] { "three" }, w1.BufferWrittenTrimmed);
                Assert.AreEqual(new string[] { "three", "" }, con.BufferWrittenTrimmed);
            }

            [Test]
            public void only_clear_the_overlapping_portion_of_window_in_the_parent_window()
            {
                //NB! Current implementation clears the parent at the same time as the window! Not cool!
                //Assert.Inconclusive();
            }

            [Test]
            public void resetting_buffer_many_times_should_not_cause_a_problem()
            {
                var con = new MockConsole(10, 4);
                for (int i = 0; i < 100; i++)
                {
                    con.WriteLine("one");
                    con.WriteLine("two");
                    con.WriteLine("three");
                    con.Write("four");
                    con.Clear();
                }
                con.WriteLine("five");
                con.WriteLine("six");
                con.WriteLine("seven");
                con.Write("eight");
                var expected = new[]
                {
                "five",
                "six",
                "seven",
                "eight",

            };

                Assert.AreEqual(expected, con.BufferWrittenTrimmed);
            }

            [Test]
            public void reset_the_y_position()
            {
                var con = new MockConsole(10, 2);
                Assert.AreEqual(0, con.CursorTop);
                con.WriteLine("one       ");
                con.WriteLine("two       ");
                Assert.AreEqual(1, con.CursorTop);
                con.Clear();
                Assert.AreEqual(0, con.CursorTop);
            }

            [Test]
            public void should_not_change_the_current_colors()
            {
                var con = new MockConsole(10, 2);
                var colors = new Colors(Red, Green);
                con.Colors = colors;
                con.Clear();
                con.Colors.Should().BeEquivalentTo(colors);
            }
        }
    }
}