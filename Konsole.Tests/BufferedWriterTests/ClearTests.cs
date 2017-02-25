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
        public void Should_clear_the_buffer()
        {
            var con = new BufferedWriter(10,2);
            con.WriteLine("one       ");
            con.WriteLine("two       ");
            Assert.AreEqual(new [] { "one       ", "two       "}, con.WholeBuffer);
            con.Clear();
            Assert.AreEqual(new[] { "          ", "          " }, con.WholeBuffer);
        }

        [Test]
        public void Should_reset_the_y_position()
        {
            var con = new BufferedWriter(10, 2);
            Assert.AreEqual(0,con.Y);
            con.WriteLine("one       ");
            con.WriteLine("two       ");
            Assert.AreEqual(2, con.Y);
            con.Clear();
            Assert.AreEqual(0, con.Y);
        }
    }
}
