using FluentAssertions;
using Konsole.Tests.Helpers;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    class ScrollDownShould
    {
        [Test]
        public void When_Child_OffsetIsZero_ScrollTheWindow()
        {
            var con = new MockConsole(4, 4);
            var window = new Window(con);
            window.WriteLine("1234");
            window.WriteLine("5678");
            window.WriteLine("90ab");
            window.Write("cdef");
            window.ScrollDown();
            var expected = new[]
            {
                "5678",
                "90ab",
                "cdef",
                "    "
            };
            con.Buffer.Should().BeEquivalentTo(expected);
        }


        [Test]
        public void When_child_offset_is_none_zero_scroll_only_the_offset_region()
        {
            var con = new MockConsole(8, 4);
            con.WriteLine("aaaaaaaa");
            con.WriteLine("bbbbbbbb");
            con.WriteLine("cccccccc");
            con.Write("dddddddd");
            var window = new Window(con, 1, 1, 6, 2);

            // are we good at this point?
            var expected = new[]
            {
                "aaaaaaaa",
                "b      b",
                "c      c",
                "dddddddd"
            };

            Precondition.Check(() => con.Buffer.Should().BeEquivalentTo(expected));

            window.WriteLine("123456");
            window.Write("7890AB");
            expected = new[]
            {
                "aaaaaaaa",
                "b123456b",
                "c7890ABc",
                "dddddddd"
            };
            Precondition.Check(() => con.Buffer.Should().BeEquivalentTo(expected));

            window.ScrollDown();
            expected = new[]
            {
                "aaaaaaaa",
                "b7890ABb",
                "c      c",
                "dddddddd"
            };
            con.Buffer.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ResetCursorToBottomOfWindow()
        {

        }

        [Test]
        public void fill_bottom_line_with_default_window_char()
        {

        }

        [Test]
        public void echo_the_scroll_to_the_parent_at_the_offset_location()
        {

        }
    }
}
