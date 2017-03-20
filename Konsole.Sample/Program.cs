using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Konsole.Drawing;
using Konsole.Forms;
using Konsole.Internal;
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
            var c1 = new MockConsole();
            var pb = new ProgressBar(50);
            pb.Refresh(25,"cats");
            return;


            // what happens with
            // ... small sizes? (0 and 1?)
            // ... nested windows?

            var w1 = new Window(20, 10, ConsoleColor.Black, ConsoleColor.DarkYellow);
            w1.WriteLine("hello");
            var w2 = new Window(w1, 5, 0, 20, 35, ConsoleColor.Red, ConsoleColor.White);
            w2.PrintAt(5,2,"X");
            w1.WriteLine("this will overwrite");

            return;

            //var w = Window.Open(0, 0, 10, 5, "title", LineThickNess.Double, ConsoleColor.DarkYellow, ConsoleColor.Yellow, con);
            //w.WriteLine("one");
            //w.WriteLine("two");
            //w.WriteLine("three");
            //w.WriteLine("four");
            //w.WriteLine("five");

            //var expected = new[]
            //{
            //    "╔════════╗",
            //    "║three   ║",
            //    "║four    ║",
            //    "║five    ║",
            //    "╚════════╝"
            //};
            //return;

            //c.BufferWritten.ShouldBeEquivalentTo(expected);

            //con.WriteLine("testing opening a dialog window and running one of the demos");
            //con.WriteLine(" ('f' progressively faster) inside it");
            //var w = Window.Open(5,5,60,10,"random", LineThickNess.Double, ConsoleColor.Black, ConsoleColor.DarkCyan);
            //w.WriteLine("new window");
            //ProgressBarDemos.ProgressivelyFasterDemo(50,w);

        }


        private static void RunCommand(IConsole con, char cmd)
        {
            switch (cmd)
            {
                case 'f':
                    FormDemos.Run(con);
                    break;

                case '1':
                    WindowDemo.SimpleDemo(con);
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

                case 'l':
                    ProgressBarDemos.ParallelDemo();
                    break;

                case '2':
                    ProgressBarDemos.ParallelConstructorDemo();
                    break;

                case 'd':
                    ProgressBarDemos.ProgressivelyFasterDemo();
                    break;

                case 's':
                    WindowDemo.ScrollingDemo(con);
                    break;

                case 'p':
                    ProgressBarDemos.SimpleDemo(con);
                    break;

                case 'b':
                    BoxeDemos.Run();
                    break;

                case 't':
                    TestDataDemo.Run(con);
                    break;

                default:
                    break;
            }

        }

        private static void Main(string[] args)
        {
            var con = new Window();
            char cmd = ' ';
            while (cmd != 'q')
            {
                con.Clear();
                printMenu(con);
                cmd = Console.ReadKey(true).KeyChar;
                if (cmd == 'q') break;
                var state = con.State;
                con.Clear();
                Console.Clear();
                RunCommand(con, cmd);
                Console.ReadKey(true);
                con.State = state;
            }
        }

        private static void printMenu(IConsole con)
        {
            con.WriteLine("Misc demo and test projects - press key to run demo");
            con.WriteLine("press key again to return to menu"); 
            con.WriteLine("---------------------------------------------------");
            
            con.WriteLine("");
            con.WriteLine("f : Forms : auto forms from objects");
            con.WriteLine("w : windows");
            con.WriteLine("1 : simple window demo");
            con.WriteLine("r : random test stuff. Changes often on each checkin.");
            con.WriteLine("    (Open a dialog window and run one of the other tests 'f' inside it.)");
            con.WriteLine("h : hiliting");
            con.WriteLine("b : boxes");
            con.WriteLine("s : scrolling demo");
            con.WriteLine("d : progressively faster 'd'emo");
            con.WriteLine("p : progress bars");
            con.WriteLine("l : Parallel Demo passing ProgressBars to threads");
            con.WriteLine("2 : Parallel Demo creating ProgressBar inside thread");
            con.WriteLine("t : test data demo");
            con.WriteLine("q : quit");
            con.WriteLine("");
        }

    }
}

