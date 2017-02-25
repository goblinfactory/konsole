using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    public class ClearTests
    {
        [Test]
        public void Should_reset_y_position()
        {
            var con = new BufferedWriter(10,2);
            con.WriteLine("one");
            con.WriteLine("two");
            Assert.AreEqual(new [] { "one", "two"},con.WholeBuffer);
            con.Clear();
            Assert.AreEqual(new[] { "one", "two" }, con.WholeBuffer);
        }

        public void Should_clear_the_buffer()
        {
            
        }
    }

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
