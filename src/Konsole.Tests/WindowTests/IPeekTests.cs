using FluentAssertions;
using Konsole.Tests.Internal;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.WindowTests
{
    [TestFixture]
    public partial class IPeekTests
    {
        private MockConsole _console;

        [SetUp]
        public void Setup()
        {
            _console = new MockConsole(20, 5);
            Window.HostConsole = _console;
            var window = Window.OpenBox("users", 20, 5);
            window.ForegroundColor = DarkBlue;
            window.BackgroundColor = White;
            var view = new ListView<User>(window, ()=> TestUsers.CreateUsers(2), (u) => new[] { u.Name, u.Credits.ToString("00000") },
                new Column("Name", 0),
                new Column("Credits", 0)
            );
            view.Render();
            var expected = new[]{
                "┌────── users ─────┐",
                "│  Name  │ Credits │",
                "│Graham  │00100    │",
                "│Kendall │00250    │",
                "└──────────────────┘",
                };
            _console.Buffer.ShouldBe(expected);
        }

        [Test]
        public void PeekCell_should_return_cell()
        {
            _console.Peek(8, 0).Char.Should().Be('u');
            _console.Peek(0, 0).Char.Should().Be('┌');
            _console.Peek(0, 4).Char.Should().Be('└');
            _console.Peek(9, 3).Char.Should().Be('│');
            // todo check the colors, after I've upgraded ListView to use parent colors and themes!
        }

        [Test]
        public void PeekRow_should_return_row()
        {
            _console.Peek(1, 2, 6).ToString().Should().Be("Graham");
            _console.Peek(10, 3, 5).ToString().Should().Be("00250");
            _console.Peek(0, 1, 20).ToString().Should().Be("│  Name  │ Credits │");
        }

        [Test]
        public void PeekRegion_should_return_region()
        {

        }
    }
}
