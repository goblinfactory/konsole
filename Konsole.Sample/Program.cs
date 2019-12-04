using System;
using Konsole;
using Konsole.Drawing;
using Konsole.Menus;
using Konsole.Sample.Demos;

namespace Konsole.Sample
{


    class Program
    {
        private static void TestNestedWindows(IConsole w)
        {
            var left = w.SplitLeft("left", ConsoleColor.Black);
            var right = w.SplitRight("right", ConsoleColor.Black);
            var nestedTop = left.SplitTop("importing", ConsoleColor.White);
            var nestedBottom = left.SplitBottom("exporting", ConsoleColor.DarkRed);
            void Writelines(IConsole con)
            {
                for (int i = 1; i < 11; i++)
                {
                    con.WriteLine($"Batch number :{i}");
                    con.WriteLine($"----------------");
                    con.WriteLine("one");
                    con.WriteLine("two");
                    con.WriteLine("three");
                    con.WriteLine("four");
                    con.WriteLine("-----------------");
                }
            }

            for (int i = 0; i < 100; i++) right.WriteLine(i.ToString());

            Writelines(nestedTop);
            Writelines(nestedBottom);
        }

        private static void QuickTest1()
        {
            Console.CursorVisible = false;
            var c = new Window();
            var consoles = c.SplitRows(
                    new Split(4, "headline", LineThickNess.Single, ConsoleColor.Yellow),
                    new Split(0, "content", LineThickNess.Single),
                    new Split(4, "status", LineThickNess.Single, ConsoleColor.Red)
            );

            var headline = consoles[0];
            var content = consoles[1];
            var status = consoles[2];

            headline.Write("my headline");
            content.WriteLine("content goes here");
            status.Write("System offline!");
            Console.ReadLine();
        }

        private static void QuickTest2()
        {
            Console.CursorVisible = false;
            var c = new Window();
            var consoles = c.SplitRows(
                    new Split(4, "heading", LineThickNess.Single),
                    new Split(0),
                    new Split(4, "status", LineThickNess.Single)
            ); ; ;

            var headline = consoles[0];
            var status = consoles[2];

            var contents = consoles[1].SplitColumns(
                    new Split(20),
                    new Split(0, "content") { Foreground = ConsoleColor.White, Background = ConsoleColor.Cyan },
                    new Split(20)
            );
            var menu = contents[0];
            var content = contents[1];
            var sidebar = contents[2];

            headline.Write("my headline");
            content.WriteLine("content goes here");

            menu.WriteLine("Options A");
            menu.WriteLine("Options B");

            sidebar.WriteLine("20% off all items between 11am and midnight tomorrow!");

            status.Write("System offline!");
            Console.ReadLine();
        }

        private static void Main(string[] args)
        {
            QuickTest2();
        }
        
        private static void Mainzz(string[] args)
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
                new MenuItem('9', "Nested window-scroll", () => TestNestedWindows(con)),
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

