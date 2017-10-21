using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    [TestFixture]
    public class BufferTests
    {
        public class BufferWrittenShould
        {
            [Test]
            public void not_return_lines_not_written_to()
            {
                var console = new MockConsole(10, 10);
                console.WriteLine("1");
                console.WriteLine("2");
                var lines = console.BufferWritten;
                Assert.AreEqual(new[]
                {
                "1         ",
                "2         ",
            }, lines);
            }

            [Test]
            public void return_all_lines_in_between_any_lines_written_to()
            {
                var console = new MockConsole(10, 10);
                console.PrintAt(1, 1, "A");
                console.PrintAt(3, 3, "B");
                var lines = console.BufferWritten;
                Assert.AreEqual(new[]
                {
                "          ",
                " A        ",
                "          ",
                "   B      ",
                }, lines);
            }

        }

        public class BufferShould
        {
            [Test]
            public void return_all_lines()
            {
                var con = new MockConsole(10, 2);
                con.WriteLine("one");

                // This is a write and not a writeLine to avoid window scrolling
                con.Write("two");

                Assert.AreEqual(new[] { "one       ", "two       " }, con.Buffer);
            }

            [Test]
            public void not_be_trimmed()
            {
                var con = new MockConsole(10, 2);
                Assert.AreEqual(new[] { "          ", "          " }, con.Buffer);
            }

        }




    }
}