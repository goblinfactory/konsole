using System;
using System.Text;
using ApprovalTests;
using ApprovalTests.Maintenance;
using ApprovalTests.Reporters;
using Konsole.Internal;
using NUnit.Framework;
using Console = Konsole.Console;

namespace Konsole.Tests
{
    [UseReporter(typeof(DiffReporter))]
    public class ProgressBarTests
    {
        [Test]
        public void EnsureNoAbandonedFiles()
        {
            ApprovalMaintenance.VerifyNoAbandonedFiles();
        }

        [Test]
        public void refresh_should_show_progress_title_and_progress_bar()
        {
            var testoutput = new StringBuilder();
            var console = new Console(80,20);
            var pb = new global::Konsole.ProgressBar(10, console);
            
            for (int i = 1; i < 5; i++)
            {
                System.Console.WriteLine(" --- test " + i + "-----");
                pb.Refresh(i, "ITEM " + i);
                System.Console.WriteLine(console.Buffer);
                testoutput.AppendLine(console.Buffer);
            }
            System.Console.WriteLine();
            Approvals.Verify(testoutput.ToString());
        }

        [Test]
        public void should_still_update_progress_even_when_writing_lines_after_progress_bar()
        {
            var console = new Console(40,10);
            console.WriteLine("line 1");
            var pb = new global::Konsole.ProgressBar(10, console);
            pb.Refresh(0, "loading");
            console.WriteLine("line 2");
            pb.Refresh(1, "cats");
            console.WriteLine("line 3");
            pb.Refresh(10, "dogs");
            console.WriteLine("line 4");

            System.Console.WriteLine(console.Buffer);
            System.Console.WriteLine();
            Approvals.Verify(console.Buffer);
        }


        //public void progress_bar_print_at_should_position_progress_bar_at_required_position()
        //{

        //}

        //public void progress_bar_and_form_objects_should_support_zindex_and_respect_clipped_overlapped_areas_when_rendering()
        //{

        //}

    }
}
