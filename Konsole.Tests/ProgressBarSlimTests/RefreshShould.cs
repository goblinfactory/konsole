using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.ProgressBarSlimTests
{
    public class RefreshShould
    {
        
        [Test]
        [TestCase(100,0,   "1234567890 (0  %)         ")]
        [TestCase(101,50,  "1234567890 (50 %) ####    ")]
        [TestCase(102,100, "1234567890 (100%) ########")]
        public void at_xx_pc_the_progress_bar_should_fill_the_balance_of_the_console_width_pro_ratio(int seq, int i, string progressBarText)
        {
            var console = new MockConsole(26,2);
            var pb = new ProgressBarSlim(100,PbStyle.SingleLine, 10,console);
            pb.Refresh(i,"1234567890");
            var expected = new string[]
            {
                progressBarText,
                "                          "
            };
            Console.WriteLine(console.BufferWrittenString);
            CollectionAssert.AreEqual(expected,console.Buffer);
        }

        public void clip_text_to_fit_text_width()
        {
            
        }

    }
}
