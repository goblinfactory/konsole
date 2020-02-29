using NUnit.Framework;

namespace Konsole.Tests.WindowTests.EchoPropertyTests
{
    public class WhenEchoTrue
    {
        [Test]
        public void When_WriteLine_SHOULD_translate_wrapped_lines_to_parent()
        {
            var parent = new MockConsole(4, 5);
            var window = parent.Open(1, 1, 2, 3);
            window.WriteLine("12345");

            var expected = new[]
            {
                "    ",
                " 34 ",
                " 5  ",
                "    ",
                "    "
            };
            Assert.AreEqual(expected, parent.Buffer);
        }

        [Test]
        public void When_Write_SHOULD_translate_wrapped_lines_to_parent()
        {
            var parent = new MockConsole(4, 5);
            var window = parent.Open(1, 1, 2, 3);
            window.Write("12345");

            var expected = new[]
            {
                    "    ",
                    " 12 ",
                    " 34 ",
                    " 5  ",
                    "    "
                };
            Assert.AreEqual(expected, parent.Buffer);
        }

        [Test]
        public void Translate_all_writes_to_the_parent()
        {
            // the only reason this test is so important, because it's how we simulate writing to the real Console.
            var parent = new MockConsole(4, 4);
            var window = parent.Open(1, 1, 2, 2);
            window.WriteLine("12");
            window.Write("34");

            var expected = new[]
            {
                "    ",
                " 12 ",
                " 34 ",
                "    "
            };
            Assert.AreEqual(expected, parent.Buffer);
        }

    }
}