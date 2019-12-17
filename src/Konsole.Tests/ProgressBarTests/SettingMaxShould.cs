using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Konsole.Tests.ProgressBarTests
{
    public class SettingMaxShould
    {
        [Test]
        public void update_the_display_using_the_new_max_when_number_decreases()
        {
            var console = new MockConsole(80, 20);
            var pb = new ProgressBar(console, PbStyle.DoubleLine, 20);
            pb.Refresh(2, "cats");
            var expected1 = new[]
{
                "Item 2     of 20   . (10 %) #####                                               ",
                "cats                                                                            ",
            };

            Assert.AreEqual(expected1, console.BufferWritten);

            pb.Max = 10;
            var expected2 = new[]
            {
                "Item 2     of 10   . (20 %) ##########                                          ",
                "cats                                                                            ",
            };
            Assert.AreEqual(expected2, console.BufferWritten);

        }


        [Test]
        public void update_the_display_using_the_new_max_when_number_increases()
        {
            var console = new MockConsole(80, 20);
            var pb = new ProgressBar(console, PbStyle.DoubleLine, 10);
            pb.Refresh(2, "cats");
            var expected1 = new[]
            {
                "Item 2     of 10   . (20 %) ##########                                          ",
                "cats                                                                            ",
            };
            Assert.AreEqual(expected1, console.BufferWritten);

            pb.Max = 20;
            var expected2 = new[]
            {
                "Item 2     of 20   . (10 %) #####                                               ",
                "cats                                                                            ",
            };
            Assert.AreEqual(expected2, console.BufferWritten);

        }

    }
}
