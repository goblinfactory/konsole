using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static System.ConsoleColor;

namespace Konsole.Tests.CharBarTests
{
    public class SmokeTests
    {
        [Test]
        public void bars_should_print_at_the_relative_x_y_position()
        {
            var console = new MockConsole(10, 5);
            var box = console.OpenBox("test");
            box.Write("1.\n2.\n3.");
            // test clipping

            var bar1 = new CharBar(box, x:2, y:0, width: 6, max: 100, barChar: '#', color: Red);
            var bar2 = new CharBar(box, 2, 2, 4, 100, 'X', Green);

            bar1.Refresh(100);
            bar2.Refresh(100);

            console.Buffer.Should().BeEquivalentTo(new[] {
                "┌─ test ─┐",
                "│1.######│",
                "│2.      │",
                "│3.XXXX  │",
                "└────────┘"
            });

            bar1.Refresh(50);
            bar2.Refresh(50);

            console.Buffer.Should().BeEquivalentTo(new[] {
                "┌─ test ─┐",
                "│1.###   │",
                "│2.      │",
                "│3.XX    │",
                "└────────┘"
            });

        }

        [Test]
        public void bars_that_overlap_bounding_window_should_be_clipped()
        {
            var console = new MockConsole(10, 4);
            var box = console.OpenBox("test");
            box.Write("1.\n2.");
            // test clipping

            var bar1 = new CharBar(box, x: 2, y: 0, width: 8, max: 100, barChar: '#', color: Red);

            bar1.Refresh(50);

            console.Buffer.Should().BeEquivalentTo(new[] {
                "┌─ test ─┐",
                "│1.####  │",
                "│2.      │",
                "└────────┘"
            });

            bar1.Refresh(100);

            console.Buffer.Should().BeEquivalentTo(new[] {
                "┌─ test ─┐",
                "│1.######│",
                "│2.      │",
                "└────────┘"
            });
        }

    }
}
