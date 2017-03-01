using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    public class EchoOntoAnotherConsoleTests
    {
        [Test]
        public void When_echo_is_true_should_translate_all_writes_to_the_parent()
        {
            var parent = new Window(0,0, 4, 4, false);
            var window = new Window(1, 1, 2, 2, true, parent);
            window.WriteLine("12");
            window.WriteLine("34");

            var expected = new[]
            {
                "    ",
                " 12 ",
                " 34 ",
                "    "
            };
            Assert.AreEqual(expected,parent.Buffer);
        }

        [Test]
        public void Wrapping_text_during_echo_to_parent_should_also_overlap_on_echoed_parent()
        {
            var parent = new Window(0, 0, 4, 4, false);
            var window = new Window(1, 1, 2, 2, true, parent);
            window.WriteLine("1234");

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
