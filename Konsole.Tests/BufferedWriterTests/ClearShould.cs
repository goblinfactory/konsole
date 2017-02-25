using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    public class ClearShould
    {
        [Test]
        public void clear_the_buffer()
        {
            var con = new BufferedWriter(10,4);
            con.WriteLine("one");
            con.WriteLine("two");
            Assert.AreEqual(new [] { "one", "two"}, con.BufferWrittenTrimmed);
            con.Clear();
            Assert.AreEqual(new string[] {}, con.BufferWritten);
            con.WriteLine("three");
        }

        [Test]
        public void reset_the_y_position()
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
