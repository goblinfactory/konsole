using FluentAssertions;
using Konsole.Platform;
using Konsole.Platform.Windows;
using NUnit.Framework;
using System;
using System.Linq;

namespace Konsole.Tests.PlatformTests
{
    [TestFixture]
    public class ScrollerTests
    {

        [Test]
        public void WhenScrollingWholeScreen_ScrollDownBy1_Should_ScrollTheWholeScreenDown1Line()
        {
            var screen = new TestScreenBuffer(7, 5, '.', Colors.WhiteOnBlack,
                "the1234",
                "gray[=]",
                "cats567",
                "howled8",
                "9012345"
            );
            var scroller = new Scroller(screen.Buffer, 7, 4, '.', Colors.WhiteOnBlack);
            scroller.ScrollDown(1, 0, 0, 7, 4);

            screen.ToString().Should().Be(
                "gray[=]" +
                "cats567" +
                "howled8" +
                "9012345" +
                "......."
            );
        }

        [Test]
        public void WhenScrollingWholeScreen_ScrollDownBy2_Should_ScrollTheWholeScreenDown2Lines()
        {
            var screen = new TestScreenBuffer(7, 5, '.', Colors.WhiteOnBlack,
                "the....",
                "gray...",
                "cats...",
                "howled.",
                "crazily"
            );
            var scroller = new Scroller(screen.Buffer, 7, 4, '.', Colors.WhiteOnBlack);
            scroller.ScrollDown(2, 0, 0, 7, 4);

            screen.ToString().Should().Be(
                "cats..." +
                "howled." +
                "crazily" +
                "......." +
                "......."
            );

        }

        [Test]
        public void BufferBuilderTests()
        {
            var buffer = new TestScreenBuffer(7, 4, '.', Colors.WhiteOnBlack, "the", "cats", "howled");
            var actual = buffer.ToString();
            actual.Should().Be(
                "the...." +
                "cats..." +
                "howled." +
                ".......");
        }

        public class TestScreenBuffer
        {
            public override string ToString()
            {
                return new String(Buffer.Select(b => b.Char.UnicodeChar).ToArray());
            }

            public CharAndColor[] Buffer { get;  }
            public TestScreenBuffer(int width, int height, char defaultChar, Colors colors, params string[] lines)
            {
                int size = height * width;
                var buffer = new CharAndColor[size];

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        int offset = y * width + x;
                        buffer[offset] = ColorExtensions.Set(colors, defaultChar);
                    }

                for (int i = 0; i < lines.Length; i++)
                    for (int x = 0; x < lines[i].Length; x++)
                    {
                        int offset = i * width + x;
                        buffer[offset] = ColorExtensions.Set(colors, lines[i][x]);
                    }

                Buffer = buffer;
            }
        }
    }
}
