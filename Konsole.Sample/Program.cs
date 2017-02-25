using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Konsole.Drawing;
using Konsole.Forms;
using Konsole.Internal;

namespace Konsole.Sample
{
    class Program
    {
        // #1 - strings
        // #2 - integers
        // #3 - look for model attribute data for length
        // #4 -   "  for required
        // navigation keys to move between fields
        // enter and tab to move to next
        // current field to highlight when editing (reverse video)
        // Support Changesets
        // support copy and paste?

        internal class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FieldWithLongerName { get; set; }
            public string FavouriteMovie { get; set; }
        }

        private static void Main(string[] args)
        {
            var con = new BufferedWriter(true);
            char cmd = ' ';
            while (cmd != 'q')
            {
                Console.WriteLine(Console.ForegroundColor);
                Console.WriteLine();

                printMenu(con);
                cmd = Console.ReadKey(true).KeyChar;
                switch (cmd)
                {
                    case 'c':
                        con.Clear();
                        printMenu(con);
                        break;

                    case 'w':
                        TestWindowsNormalUsage(con);
                        break;

                    case 'W':
                        TestWindowsTestUsage(con);
                        break;

                    case 'h':
                        TestHilite();
                        break;
                    case 'b':
                        TestBoxes();
                        break;

                    default:
                        break;
                }

            }
        }

        private static void printMenu(BufferedWriter con)
        {
            con.WriteLine("");
            con.WriteLine("W : TestWindowsTestUsage");
            con.WriteLine("w : TestWindowsNormalUsage");
            con.WriteLine("h : hiliting");
            con.WriteLine("b : boxes");
            con.WriteLine("c : clear screen");
            con.WriteLine("q : quit");
            con.WriteLine("");
        }

        private static void TestWindowsNormalUsage(IConsole con)
        {
            con.WriteLine("'w' TestWindowsNormalUsage");
            con.WriteLine("--------------------------");
            var console = new Writer();
            var w1 = new Window(console, 0,0,40,20);
            var w2 = new Window(console, 0, 0, 40, 20);
            
            //var window1 = new Window(buffer, 0,0, 20,20, true);  // zorder = 1, show = true
            //var window2 = new Window(40,10, false); // zorder = 2
        }

        // this is how we would use a buffered writer instead of normal writer so that we can prove the windows are working!
        private static void TestWindowsTestUsage(IConsole con)
        {
            con.WriteLine("'W' TestWindowsTestUsage");
            con.WriteLine("------------------------");
            var console = new BufferedWriter(80,20,true);
            var w1 = new Window(console, 0, 0, 40, 20); // for now perhaps not allow the windows to overlap?
            var w2 = new Window(console, 0, 0, 40, 20);

            //var window1 = new Window(buffer, 0,0, 20,20, true);  // zorder = 1, show = true
            //var window2 = new Window(40,10, false); // zorder = 2
        }


        private static void TestHilite()
        {
            var normal = ConsoleColor.Black;
            var hilite = ConsoleColor.White;

            var console = new BufferedWriter(40, 10, true);
            var fore = console.ForegroundColor;
            try
            {
                console.ForegroundColor = ConsoleColor.Red;

                console.BackgroundColor = normal;
                console.WriteLine("menu item 1");
                console.WriteLine("menu item 2");
                console.Write("menu ");

                console.BackgroundColor = hilite;
                console.Write("item");

                console.BackgroundColor = normal;
                console.WriteLine(" 3");
                console.WriteLine("menu item 4");
                console.WriteLine("menu item 5");
            }
            finally
            {
                console.ForegroundColor = fore;
            }
        }

        public static void TestBoxes()
        {
            System.Console.Clear();
            var console = new Writer();
            
            int height = 18;
            int sy = 2;
            int sx = 2;
            int width = 60;
            int ex = sx + width;
            int ey = sy + height;
            int col1 = 20;

            var draw = new Draw(console, LineThickNess.Double);
            draw
                .Box(sx, sy, ex, ey, "my test box")
                .Line(sx, sy + 2, ex, sy + 2)
                .Line(sx + col1, sy, sx + col1, sy + 2, LineThickNess.Single)
                //.Box(sx + 35, ey - 4, ex - 5, ey - 2); faulty! need to fix
                .Line(sx + 35, ey - 4, ex - 5, ey - 4, LineThickNess.Double)
                .Line(sx + 35, ey - 2, ex - 5, ey - 2, LineThickNess.Double)
                .Line(sx + 35, ey - 4, sx + 35, ey - 2, LineThickNess.Single) // faulty! need to fix
                .Line(ex - 5, ey - 4, ex - 5, ey - 2, LineThickNess.Single);  // faulty! need to fix
                            
            console.PrintAt(sx+2, sy+1, "DEMO INVOICE");
            console.CursorTop = ey+1;
        }

        public static void TestForms()
        {
            var form1 = new Form(80,new ThickBoxStyle());
            var person = new Person()
            {
                FirstName = "Fred",
                LastName = "Astair",
                FieldWithLongerName = "22 apples",
                FavouriteMovie = "Night of the Day of the Dawn of the Son of the Bride of the Return of the Revenge of the Terror of the Attack of the Evil, Mutant, Hellbound, Flesh-Eating Subhumanoid Zombified Living Dead, Part 2: In Shocking 2-D"
            };
            form1.Show(person);

            // works with anonymous types
            new Form().Show(new {Height = "40px", Width = "200px"}, "Demo Box");

            // change the box style, and width, thickbox
            new Form(40, new ThickBoxStyle()).Show(new { AddUser= "true", CloseAccount = "false", OpenAccount = "true"}, "Permissions");
        }

        public class Cat
        {
            private readonly IConsole _console;
            public Cat(IConsole console) { _console = console; }
            public void Greet()
            {
                _console.WriteLine("Prrr!");
                _console.WriteLine("Meow!");
            }
        }

        public void TestConsole_ConsoleWriter_and_IConsole_example_usage()
        {
            {
                // test the cat
                // ============
                var console = new BufferedWriter(80, 20);
                var cat = new Cat(console);
                cat.Greet();
                Assert.AreEqual(console.BufferWrittenTrimmed, new[] {"Prrr!", "Meow!"});
            }
            {
                // create an instance of a cat that will purr to the real Console
                // ==============================================================
                var cat = new Cat(new Writer());
                cat.Greet(); // prints Prrr! aand Meow! to the console
            }
        }

        public static void ProgressivelyFasterDemo(int startingPauseMilliseconds = 50)
        {
            var pb = new ProgressBar(300);
            var names = TestData.MakeNames(300);
            int cnt = names.Count();
            int i = 1;
            foreach (var name in names)
            {
                pb.Refresh(i++, name);
                int pause = startingPauseMilliseconds - (1 * (i * (startingPauseMilliseconds - 1) / cnt));
                if (pause > 0) Thread.Sleep(pause);
                if (System.Console.KeyAvailable)
                {
                    if (System.Console.ReadKey().Key == ConsoleKey.P)
                    {
                        Thread.Sleep(2000);
                        continue;
                    }
                    System.Console.WriteLine("key press detected, stopped before end.");
                    break;
                }
            }

        }


        static void ParallelDemo()
        {
            // demo; take the first 10 directories that have files from c:\windows, and then pretends to process (list) them.
            // processing of each directory happens on a different thread, to simulate multiple background tasks, 
            // e.g. file downloading.
            // ==============================================================================================================
            var dirs = Directory.GetDirectories(@"c:\windows").Where(d=> Directory.GetFiles(d).Any()).Take(7);

            var tasks = new List<Task>();
            var bars = new List<ProgressBar>();
            foreach (var d in dirs)
            {
                var dir = new DirectoryInfo(d);
                var files = dir.GetFiles().Take(50).Select(f=>f.FullName).ToArray();
                if (files.Length==0) continue;
                var bar = new ProgressBar(files.Count());
                bars.Add(bar);
                bar.Refresh(0, d);
                tasks.Add(new Task(() => ProcessFiles(d, files, bar)));
            }
            System.Console.WriteLine("ready press enter.");
            System.Console.ReadLine();

            foreach (var t in tasks) t.Start();
            Task.WaitAll(tasks.ToArray());
            System.Console.WriteLine("done.");
            System.Console.ReadLine();

        }

        public void SimplestUsage()
        {
            System.Console.WriteLine("Simplest usage");
            var pb = new ProgressBar(50);
            pb.Refresh(0, "connecting to server to download 50 files sychronously.");
            System.Console.ReadLine();
            pb.Refresh(5, "downloading file 5");
            System.Console.ReadLine();
            pb.Refresh(50, "finished.");
            return;
        }


        public static void ProcessFiles(string directory, string[] files, ProgressBar bar)
        {
            var cnt = files.Count();
            foreach (var file in files)
            {
                bar.Next(new FileInfo(file).Name);
                Thread.Sleep(150);
            }
        } 

    }

    // fake Assert class so that I can have an Assert without referencing nunit in the sample project
    public static class Assert
    {
        public static void That(bool result) { }
        public static void AreEqual<T>(IEnumerable<T> a, IEnumerable<T> b) { }
    }

}

