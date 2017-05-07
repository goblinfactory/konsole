using System;
using System.Text;
using ApprovalTests;
using ApprovalTests.Maintenance;
using ApprovalTests.Reporters;
using NUnit.Framework;

namespace Konsole.Tests
{
    public class RefreshShould
    {

        [Test]
        public void still_work_if_console_is_scrolling()
        {
            // i.e. when progress bar position changes because window is scrolling.
            Assert.Inconclusive();
        }

        [Test]
        public void still_work_if_progressbar_has_scrolled_off_and_is_no_longer_visible()
        {
            // this needs some careful though because the console window height and width does not appear to 
            // return the correct values when run from within CONEMU console window.
            // need to test with a no frills windows console window.
            Assert.Inconclusive();
        }

        [Test]
        public void show_progress_title_and_progress_bar()
        {
            var console = new MockConsole(80,20);
            var pb1 = new ProgressBar(10, console);
            var pb2 = new ProgressBar(10, console);
            pb1.Refresh(2,"cats");            
            pb2.Refresh(10,"dogs");

            var expected = new[]
            {
                "Item 2     of 10   . (20 %) ##########                                          ",
                "cats                                                                            ",
                "Item 10    of 10   . (100%) ##################################################  ",
                "dogs                                                                            ",
            };
            Assert.AreEqual(expected, console.BufferWritten);
        }

        
        [Test]
        public void update_progress_even_when_writing_lines_after_progress_bar()
        {
            var console = new MockConsole(40,10);
            console.WriteLine("line 1");
            var pb = new ProgressBar(10, console);
            pb.Refresh(0, "loading");
            console.WriteLine("line 2");
            pb.Refresh(1, "cats");
            console.WriteLine("line 3");
            pb.Refresh(10, "dogs");
            console.WriteLine("line 4");

            Console.WriteLine(console.BufferWrittenString);
            Console.WriteLine();

            var expected = new[]
            {
                "line 1                                  ",
                "Item 10    of 10   . (100%) ##########  ", 
                "dogs                                    ",
                "line 2                                  ",
                "line 3                                  ",
                "line 4                                  "
            };
            Assert.AreEqual(expected, console.BufferWritten);
        }

        [Test]
        public void update_progress_even_when_Writing_text_after_progress_bar()
        {
            var console = new MockConsole(40, 10);
            console.Write("Some text"); // this text gets overwritten because progress bar is a full width screen control.
            var pb = new ProgressBar(10, console);
            console.Write("word 1");
            pb.Refresh(0, "loading");
            console.Write(", word 2");
            pb.Refresh(1, "cats");
            console.Write(", word 3");
            pb.Refresh(10, "dogs");
            console.WriteLine(", word 4!");
            Console.WriteLine(console.BufferWrittenString);
            Console.WriteLine();
            var expected = new[]
            {
                "Item 10    of 10   . (100%) ##########  ",
                "dogs                                    ",
                "word 1, word 2, word 3, word 4!         "
            };

            Assert.AreEqual(expected,console.BufferWritten);
        }
    }


}
