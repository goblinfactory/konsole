using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Maintenance;
using ApprovalTests.Reporters;
using Konsole.Forms;
using Konsole.Internal;
using Konsole.Tests.TestClasses;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Konsole.Tests
{
    [UseReporter(typeof (DiffReporter))]
    public class WindowTests
    {
        //[Test]
        public void Should_restore_background_when_window_closes()
        {
            //var main = new Window(0,0,10,4)
            //.WriteLine("1111111111")
            //.WriteLine("2222222222")
            //.WriteLine("3333333333")
            //.WriteLine("4444444444");
            //Assert.AreEqual(main.Buffer, new [] { "1111111111","2222222222","3333333333", "4444444444" });

            //var pop = main.newWindow(1, 1, 4, 2)
            //    .WriteLine("xx")
            //    .WriteLine("yy");


            //Assert.AreEqual(pop.Buffer, new[] { "xx", "yy"});
            //Assert.AreEqual(pop);



            ////Approvals.Verify(console.Buffer);
            ////System.Console.WriteLine(console.Buffer);

        }

        public void Should_scroll_when_printing_overflows_window()
        {
            //var console = new BufferedWriter(200, 20);
            //Approvals.Verify(console.Buffer);
            //System.Console.WriteLine(console.Buffer);

        }



    }

}