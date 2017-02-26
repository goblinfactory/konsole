using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    [UseReporter(typeof (DiffReporter))]
    public class WindowTests
    {

        public void Should_scroll_when_printing_overflows_window()
        {
            //var console = new BufferedWriter(200, 20);
            //Approvals.Verify(console.Buffer);
            //System.Console.WriteLine(console.Buffer);

        }

        [Test]
        public void Should_translate_all_WriteLine_to_relative_positions_on_parent_console()
        {
            var console = new BufferedWriter(10, 3);

            var c1 = new Window(console, 0, 0, 5, 3);
            var c2 = new Window(console, 5, 0, 5, 3);

            c1.WriteLine("123");
            c2.WriteLine("234");
            c1.WriteLine("345");
            c2.WriteLine("456");

            Assert.AreEqual(new[]
            {
                "123  234  ",
                "345  456  ",
                "          "
            }, console.Buffer);
        }

        [Test]
        public void Overflow_in_window_should_wrap_to_next_line_in_the_window()
        {
            var console = new BufferedWriter(10, 3);

            var c1 = new Window(console, 5, 0, 5, 3);

            c1.WriteLine("123456");

            Assert.AreEqual(new[]
            {
                "     12345",
                "     6    ",
                "          "
            }, console.Buffer);
        }

        // need tests proving we leave the cursor position exactly where it was before we did any command


        [Test]
        public void Mix_and_match_normal_console_writing_with_window_test()
        {
            Assert.Inconclusive();
            //var console = new BufferedWriter(10, 5);
            //console.WriteLine("line1");
            //var w1 = new Window(console, 0, 0, 5, 3);
            //w1.WriteLine("X");
            //w1.WriteLine("Y");
            //w1.WriteLine("Z");

            //Assert.AreEqual(new[]
            //{
            //    "123  234  ",
            //    "345  456  ",
            //    "          "
            //}, console.Buffer);
        }

    }

}