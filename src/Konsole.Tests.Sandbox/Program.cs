using System;
using static System.ConsoleColor;

namespace Konsole.Tests.RunOne
{
    class Program
    {
        //unit test is simply to replace \n \r \n\r or \r\n with 

        static void Main(string[] args)
        {
            var w = new Window();
            var feed = w.SplitLeft("feed");
            var status  = w.SplitRight("status");

            // now try to break the layout!

            var longText = @"
        a ver wide text block ...  aqsdasds            asad as                 q   k    j  h   jq  q   jkh jk  hjk         j   j   h   jkh kh  k   j       kjh jkh     kj   k  jh      hjk 
                   
        line 1
    line2
line 3
    line 4
        line5

";

            var shortTexts = "adas\n\r das askladsdfs dfsklfdsadlfj slashN\n  asdklfas sdfasl slashr-slashn\r\n;djsdfj asadsklfja  adsfasdlkf slashN-slashR\n\r ajsklf jsdfklsklfadsf adsfl ds";
            Console.ReadKey();
            feed.WriteLine(Green, longText);
            feed.WriteLine(Yellow, shortTexts);

            status.WriteLine(Yellow, "Press enter to quit");
            Console.ReadLine();
        }
    }
}
