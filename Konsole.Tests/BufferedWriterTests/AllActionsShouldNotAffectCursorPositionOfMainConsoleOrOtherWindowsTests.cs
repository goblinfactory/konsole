using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    public class AllActionsShouldNotAffectCursorPositionOfMainConsoleOrOtherWindowsTests
    {
        [Test]
        public void Print_and_WriteLine_tests()
        {
            var parent = new Window(10, 3, false);

            parent.WriteLine("one");
            parent.WriteLine("two");
            parent.WriteLine("three");
            Assert.AreEqual(new[]
            {
                "one       ",
                "two       ",
                "three     "
            }, parent.BufferWritten);
            
            var child = new Window(5,2, true, parent);
            child.WriteLine("XX");
            child.WriteLine("YY");
            Console.WriteLine(parent.BufferWrittenString);
            
        }
    }
}
