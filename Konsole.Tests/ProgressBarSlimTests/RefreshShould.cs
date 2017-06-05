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
        ////Item {0,-5} of {1,-5}. ({2,-3}%) 
        [Test]
        [TestCase(100,0,   "1234567890 (0  %)          ", "                           ")]
        [TestCase(101,50,  "1234567890 (50 %) ####     ", "                           ")]
        [TestCase(102,100, "1234567890 (100%) #########", "                           ")]
        //[TestCase(103, 0,  "Item     0 of   100. (  0%)", "                           ")]
        //[TestCase(104, 50, "Item    50 of   100. ( 50%)", "                           ")]
        //[TestCase(105, 100,"Item   100 of   100. (100%)", "                           ")]
        public void at_xx_pc_the_progress_bar_should_fill_the_balance_of_the_console_width_pro_ratio(int seq, int i, string line1, string line2)
        {
            var console = new MockConsole(27,2);
            var pb = new ProgressBar(console, 100, 10);
            pb.Refresh(i,"1234567890");
            var expected = new string[]
            {
                line1,
                line2
            };
            Console.WriteLine(console.BufferWrittenString);
            CollectionAssert.AreEqual(expected,console.Buffer);
        }




        // todo : write tests for progress bar wider than console window.

    } 
}