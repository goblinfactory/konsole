using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Konsole.Drawing;
using Konsole.Forms;
using Konsole.Internal;
using Konsole.Menus;
using Konsole.Sample.Demos;

namespace Konsole.Sample
{


    class Program
    {
        
        // random notes and links

        // experiment to see if we can read a block of the screen by using move and redirecting input and output
        // see http://stackoverflow.com/questions/12355378/read-from-location-on-console-c-sharp

        // windows specific 
        // more information https://msdn.microsoft.com/en-us/library/windows/desktop/ms681913(v=vs.85).aspx


        private static void RandomStuff(IConsole con)
        {
            var pb = new ProgressBar(con, PbStyle.DoubleLine, 50);
            pb.Refresh(25,"cats");
            Console.ReadKey(true);
            pb.Max = 40;
            Console.ReadKey(true);
        }


        private static void RunCommand(IConsole con, char cmd)
        {
            switch (cmd)
            {
                case 'f':
                    FormDemos.Run(con);
                    break;

                case 'w':
                    WindowDemo.Run(con);
                    break;

                case 'r':
                    RandomStuff(con);
                    break;

                case 'h':
                    HiliteDemo.Run(con);
                    break;


                case '2':
                    ProgressBarDemos.ParallelConstructorDemo();
                    break;


                case 'p':
                    ProgressBarDemos.SimpleDemo(con);
                    break;



                default:
                    break;
            }

        }

        private static void Main(string[] args)
        {

            var mo = Menu.WithOutput(28, 25, "Samples", "output",

                new MenuItem('f', "Forms", FormDemos.Run),
                new MenuItem('b', "Boxes", BoxeDemos.Run),
                new MenuItem('s', "Scrolling", WindowDemo.Run2),
                new MenuItem('p', "ProgressBar1", ProgressBarDemos.ParallelDemo),
                new MenuItem('q', "ProgressBar2", ProgressBarDemos.ParallelDemo),
                new MenuItem('t', "Test data", TestDataDemo.Run),
                new MenuItem('c', "clear screen", c => c.Clear()),
                new MenuItem('r', "RANDOM", RandomStuff)

            );

            var menu = mo.Menu;
            var output = mo.Output;
            menu.BeforeMenu = () => output.Clear();
            menu.Run();

        }



    }
}

