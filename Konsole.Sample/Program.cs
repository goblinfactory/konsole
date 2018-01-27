using System;
using Konsole.Menus;
using Konsole.Sample.Demos;

namespace Konsole.Sample
{


    class Program
    {
        private static void Main(string[] args)
        {
            var con = new Window(28, 1, 70, 30, ConsoleColor.Yellow, ConsoleColor.DarkGreen, K.Clipping);

            PrintNumberedBox(con);
            Console.WriteLine();
            var menu = new Menu("Konsole Samples", ConsoleKey.X, 25,

                new MenuItem('1', "Forms", () => FormDemos.Run(con)),
                new MenuItem('2', "Boxes", () => BoxeDemos.Run(con)),
                new MenuItem('3', "Scrolling", () => WindowDemo.Run2(con)),
                new MenuItem('4', "ProgressBar", () => ProgressBarDemos.ProgressBarDemo(con)),
                new MenuItem('5', "ProgressBarTwoLine", () => ProgressBarDemos.ProgressBarTwoLineDemo(con)),
                new MenuItem('6', "Test data", () => TestDataDemo.Run(con)),
                new MenuItem('7', "SplitLeft, SplitRight", () =>  SplitDemo.DemoSplitLeftRight(con)),
                new MenuItem('8', "SplitTop, SplitBottom", () =>  SplitDemo.DemoSplitTopBottom(con)),
                new MenuItem('c', "clear screen", () => con.Clear()),
                new MenuItem('x', "Exit", () => { })

            );
            
            menu.OnBeforeMenuItem += (i) => { con.Clear();  };

            PrintNumberedBox(con);

            Console.WriteLine("\nPress 'X' to exit the demo.");

            menu.Run();
        }

        private static void PrintNumberedBox(IConsole c)
        {
            c.ForegroundColor = ConsoleColor.Green;
            // print a numbered box so that I can see where the menu is being printed
            for (int y = 0; y < 30; y += 5)
            {
                c.PrintAt(0,y, y.ToString());
                c.PrintAt(10,y, "10");
                c.PrintAt(20,y, "20");
                c.PrintAt(30,y, "30");
                c.PrintAt(40,y, "40");
                c.PrintAt(50,y, "50");
                c.PrintAt(60,y, "60");
            }
        }

    }
}

