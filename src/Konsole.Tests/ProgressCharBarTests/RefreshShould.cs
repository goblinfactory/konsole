using FluentAssertions;
using NUnit.Framework;
using System;
using static System.ConsoleColor;

namespace Konsole.Tests.ProgressCharBarTests
{
    public class ProgressCharBarTests_RefreshShould
    {
        private MockConsole _console;

        [SetUp]
        public void Setup()
        {
            _console = new MockConsole(22, 3);
            Window.HostConsole = _console;
        }

        [Test]
        public void show_percentage_correctly_0_perecent()
        {
            // progress bar index is 1 based. if max is 4, then refresh(1)=25%, 2=50%, 3=75% >= 4 is 100%
            var box = Window.OpenBox("test progress");
            var pb = new ProgressCharBar(box, max: 4, barChar: '*', color: Green);

            pb.Refresh(0);
            _console.Buffer.Should().BeEquivalentTo(new[]
            {
                "┌─── test progress ──┐",
                "│                    │",
                "└────────────────────┘"
            });
        }

        [Test]
        public void show_percentage_correctly_25_perecent()
        {
            //begin-snippet: ProgressCharBar
            // 'OpenBox' opens to fill the entire parent window.
            // this test runs inside a mock console 22 chars wide, by 3 lines tall.
            // hence the 22x3 box shown in the buffer below.
            // -------------------------------------------
            var box = Window.OpenBox("test progress");
            
            // default color is green, default char is #
            var pb = new ProgressCharBar(box, max: 4); 
            pb.Refresh(1);
            _console.Buffer.Should().BeEquivalentTo(new[]
            {
                "┌─── test progress ──┐",
                "│#####               │",
                "└────────────────────┘"
            });
            //end-snippet: ProgressCharBar
        }

        [Test]
        public void show_percentage_correctly_50_perecent()
        {

            var box = Window.OpenBox("test progress");

            // default color is green, default char is #
            var pb = new ProgressCharBar(box, max: 4, barChar:'$');

            pb.Refresh(2);
            _console.Buffer.Should().BeEquivalentTo(new[]
            {
                "┌─── test progress ──┐",
                "│$$$$$$$$$$          │",
                "└────────────────────┘"
            });
        }

        [Test]
        public void show_percentage_correctly_100_perecent()
        {

            var box = Window.OpenBox("test progress");

            // default color is green, default char is #
            var pb = new ProgressCharBar(box, max: 4, barChar: '#', ConsoleColor.Red);
            pb.Refresh(4);
            _console.Buffer.Should().BeEquivalentTo(new[]
            {
                "┌─── test progress ──┐",
                "│####################│",
                "└────────────────────┘"
            });
        }

        [Test]
        public void clip_any_values_greater_than_max()
        {
            var box = Window.OpenBox("test progress");
            var pb = new ProgressCharBar(box, max: 10);
            pb.Refresh(40);
            _console.Buffer.Should().BeEquivalentTo(new[]
            {
                "┌─── test progress ──┐",
                "│####################│",
                "└────────────────────┘"
            });
        }

        [Test]
        public void print_the_bar_in_the_chosen_color()
        {

            var box = Window.OpenBox("test progress");
            var pb = new ProgressCharBar(box, max: 4, barChar: '#', ConsoleColor.Red);
            pb.Refresh(4);
            _console.BufferWithColor[1][4].Should().Be('r');
        }
    }
}