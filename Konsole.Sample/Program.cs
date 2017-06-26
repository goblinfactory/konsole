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
        
        private static void RandomStuff(IConsole con)
        {
            var pb = new ProgressBar(con, PbStyle.DoubleLine, 50);
            pb.Refresh(25,"cats");
            Console.ReadKey(true);
            pb.Max = 40;
            Console.ReadKey(true);
        }

        private static void Main(string[] args)
        {
            var con = Window.Open(67, 0, 50, 25, "server", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkYellow);

            var output1 = con;

            var menu = new Menu("Samples", ConsoleKey.Escape, 30,

                new MenuItem('f', "Forms", ()=> FormDemos.Run(output1)),
                new MenuItem('b', "Boxes", ()=> BoxeDemos.Run(output1)),
                new MenuItem('s', "Scrolling", ()=> WindowDemo.Run2(con)),
                new MenuItem('p', "ProgressBar 1 line demo", ()=> ProgressBarDemos.ParallelDemo(con)),
                new MenuItem('q', "ProgressBar 2 line demo", () => ProgressBarDemos.ParallelDemo(con)),
                new MenuItem('t', "Test data", () => TestDataDemo.Run(con)),
                new MenuItem('c', "clear screen", () => con.Clear()),
                new MenuItem('r', "RANDOM", () => RandomStuff(con))

            );

            menu.OnBeforeMenuItem += (i) => con.Clear();
            menu.Run();

        }



    }
}

