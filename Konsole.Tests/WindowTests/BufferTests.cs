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
                var console = new Window(10, 10, false);
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
                var console = new Window(10, 10, false);
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
                var con = new Window(10, 2, false);
                con.WriteLine("one");
                con.WriteLine("two");
                Assert.AreEqual(new[] { "one       ", "two       " }, con.Buffer);
            }

            [Test]
            public void not_be_trimmed()
            {
                var con = new Window(10, 2, false);
                Assert.AreEqual(new[] { "          ", "          " }, con.Buffer);
            }

        }




    }
}