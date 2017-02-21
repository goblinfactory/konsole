using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Konsole.Tests.ConsoleTests
{
    [UseReporter(typeof(DiffReporter))]
    public class OverflowTests
    {
        [Test]
        public void Overflowing_buffer_should_scroll_buffer_up()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void overflow_x_should_wrap_to_next_line()
        {
            Assert.Inconclusive();
        }

        [Test]
        public void overflow_x_and_y_should_wrap_and_scroll()
        {
            Assert.Inconclusive();
        }
    }
}
