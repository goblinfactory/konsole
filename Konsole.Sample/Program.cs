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
            var pb = new ProgressBar(50, con);
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

                case '1':
                    new WindowDemo(con).SimpleDemo();
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

                case 'd':
                    ProgressBarDemos.ProgressivelyFasterDemo();
                    break;

                case 's':
                    new WindowDemo(con).ScrollingDemo();
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
            var con = new Window();
            var output = new Window(con, 35,0, 70, 20, ConsoleColor.White, ConsoleColor.DarkCyan);

            con.WriteLine("this test should be above the menu");
            con.WriteLine("");

            var menu = new Menu(con, output, "DEMO SAMPLES", 'q', 30, 

                new MenuItem('f',"FORMS",  FormDemos.Run),
                new MenuItem('r',"RANDOM", RandomStuff),
                new MenuItem('p',"PROGRESSBAR 1", ProgressBarDemos.ParallelDemo),
                new MenuItem('b',"BOXES",BoxeDemos.Run),
                new MenuItem('t',"TESTDATA", TestDataDemo.Run),
                new MenuItem('q',"EXIT", null)

            );

            // todo, make this the default behavior?
            menu.BeforeMenu = output.Clear;
            menu.AfterMenu = output.Clear;

            menu.Run();
            con.WriteLine("this test should appear below the menu");
        }

        private static void printMenu(IConsole con)
        {
            con.WriteLine("");
            con.WriteLine("w : windows");
            con.WriteLine("1 : simple window demo");
            con.WriteLine("r : random test stuff. Changes often on each checkin.");
            con.WriteLine("    (Open a dialog window and run one of the other tests 'f' inside it.)");
            con.WriteLine("h : hiliting");
            con.WriteLine("s : scrolling demo");
            con.WriteLine("d : progressively faster 'd'emo");
            con.WriteLine("p : progress bars");
            con.WriteLine("l : Parallel Demo passing ProgressBars to threads");
            con.WriteLine("2 : Parallel Demo creating ProgressBar inside thread");
  
            con.WriteLine("q : quit");
            con.WriteLine("");
        }

    }
}

