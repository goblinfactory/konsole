using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Konsole.Tests.ProgressBarSlimTests
{
    public class RefreshShould
    {
        [UseReporter(typeof(DiffReporter))]
        [Test]
        public void show_percentage_correctly()
        {
            var console = new MockConsole(80, 60);
            for (int i = 1; i < 21; i++)
            {
                var pb1 = new ProgressBarSlim(20, console);
                pb1.Refresh(i, "cats");
            }
            Approvals.VerifyAll(console.BufferWritten, "");
        }


        [Test]
        [TestCase(100,0,   "1234567890 (0  %)          ", "                           ")]
        [TestCase(101,50,  "1234567890 (50 %) ####     ", "                           ")]
        [TestCase(102,100, "1234567890 (100%) #########", "                           ")]
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
    } 
}