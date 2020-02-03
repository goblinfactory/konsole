using FluentAssertions;
using NUnit.Framework;
using static System.ConsoleColor;

namespace Konsole.Tests.MockConsoleTests
{
    [TestFixture]
    public class ColorTests
    {
        [Test]
        public void setting_color_using_either_ForeGroundBackground_or_Colors_should_set_the_color()
        {
            var c1 = new Colors(Green, Yellow);
            var c2 = new Colors(Red, Blue);
            
            var console = new MockConsole(20, 5);
            console.Colors = c1;
            console.Colors.Should().BeEquivalentTo(c1);
            console.ForegroundColor.Should().Be(c1.Foreground);
            console.BackgroundColor.Should().Be(c1.Background);

            console.Colors = c2;
            console.Colors.Should().BeEquivalentTo(c2);
            console.ForegroundColor.Should().Be(c2.Foreground);
            console.BackgroundColor.Should().Be(c2.Background);

            console.ForegroundColor = Cyan;
            console.Colors.Should().BeEquivalentTo(new Colors(Cyan, Blue));

            console.BackgroundColor = Black;
            console.Colors.Should().BeEquivalentTo(new Colors(Cyan, Black));
        }

    }
}
