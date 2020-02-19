using FluentAssertions;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.WindowTests
{
    class BackgroundColorShould
    {
        [Test]
        public void not_change_parent_state_background_color()
        {
            var c = new MockConsole(White, Red);
            var w = new Window(c);
            w.BackgroundColor = DarkGray;
            c.BackgroundColor.Should().Be(Red);
        }

    }
}
