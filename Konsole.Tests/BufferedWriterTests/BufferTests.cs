using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{

    public class BufferTests
    {
        [Test]
        public void ListText_should_return_lines_written_to()
        {
            var console = new BufferedWriter(10, 4);
            console.WriteLine("1");
            console.WriteLine("2");
            var lines = console.LinesText;
            //Assert.That(console.LinesText, Is.EqualTo(buffer));
            Assert.AreEqual(new []
            {
                "1         ",
                "2         ",
            }, lines);
        }
    }
}
