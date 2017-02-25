using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    public class WholeBufferTests
    {
        [Test]
        public void Should_return_all_lines()
        {
            var con = new BufferedWriter(10, 2);
            con.WriteLine("one");
            con.WriteLine("two");
            Assert.AreEqual(new[] { "one       ", "two       " }, con.WholeBuffer);
        }

        [Test]
        public void Lines_should_not_be_trimmed()
        {
            var con = new BufferedWriter(10, 2);
            Assert.AreEqual(new[] { "          ", "          " }, con.WholeBuffer);
        }



    }
}