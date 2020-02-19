using FluentAssertions;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.WindowTests
{
    class ForegroundColorShould
    {
        [Test]
        public void not_change_parent_state()
        {
            var c = new MockConsole(White, Red);
            var w = new Window(c);
            w.ForegroundColor = DarkGray;
            c.ForegroundColor.Should().Be(White);
        }


    }
}
