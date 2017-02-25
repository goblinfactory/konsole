using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
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


        [Test]
        public void overflow_text_should_wrap_onto_next_line()
        {
            var console = new BufferedWriter(8, 20);
            console.WriteLine("1234567890");
            console.WriteLine("---");
            console.WriteLine("12345678901234567890");
            System.Console.WriteLine(console.Buffer);
            System.Console.WriteLine();
            Approvals.Verify(console.Buffer);
        }

    }
}