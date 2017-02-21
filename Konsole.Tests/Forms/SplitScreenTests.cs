using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Konsole.Forms;
using Konsole.Internal;
using Konsole.Tests.TestClasses;
using NUnit.Framework;

namespace Konsole.Tests.Forms
{
    [UseReporter(typeof(DiffReporter))]
    class SplitScreenTests
    {
       // [Test]
        public void two_forms_side_by_side_test()
        {
            Assert.Inconclusive("Not yet implemented");
            //// only have 1 test console
            //// and two windows side by side
            //// two windows side each 50x by 10y
            //var console = new TestConsole(80, 10);
            //var window1 = new Window(0, 0, 40, 10, console);
            //var window2 = new Window(41, 0, 40, 10, console);

            //var form1 = new Form(window1);
            //var form2 = new Form(window2);
            //var person = new Person()
            //{
            //    FirstName = "Freddy",
            //    LastName = "Astair",
            //    AFieldWithAMuchLongerName = "22 apples",
            //    FavouriteMovie =
            //        "Night"//" of the Day of the Dawn of the Son of the Bride of the Return of the Revenge of the Terror of the Attack of the Evil, Mutant, Hellbound, Flesh-Eating Subhumanoid Zombified Living Dead, Part 2: In Shocking 2-D"
            //};
            //window1.WriteLine("line1");
            //form1.Show(person); 
            //window1.WriteLine("line2");

            //window2.WriteLine("line3");
            //form2.Show(person);
            //window2.WriteLine("line4");

            //Approvals.Verify(console.Buffer);
        }
    }

}
