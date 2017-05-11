using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.ProgressBarSlimTests
{
    public class TextWidthShould
    {
        [TestCase(100, 100, "12345",        "12345      (100%) ########")]
        [TestCase(100, 100, "123456789012", "1234567890 (100%) ########")]
        [TestCase(100, 100, null,           "           (100%) ########")]
        public void clip_or_pad_text_to_fit_text_width(int seq, int i, string text, string progressBarText)
        {
            var console = new MockConsole(26, 2);
            var pb = new ProgressBar(console, 100, 10);
            pb.Refresh(i, text);
            var expected = new string[]
            {
                progressBarText,
                "                          "
            };
            Console.WriteLine(console.BufferWrittenString);
            CollectionAssert.AreEqual(expected, console.Buffer);
        }

    }
}
