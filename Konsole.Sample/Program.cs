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
                        Console.Clear();
                        printMenu(con);
                        break;

                    case 'w':
                        TestWindows(con);
                        break;

                    case 'h':
                        TestHilite();
                        break;
                    case 'p':
                        Progress(con);
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
            con.WriteLine("w : windows");
            con.WriteLine("h : hiliting");
            con.WriteLine("b : boxes");
            con.WriteLine("p : progress bars");
            con.WriteLine("c : clear screen");
            con.WriteLine("q : quit");
            con.WriteLine("");
        }

        private static void TestWindows(IConsole con)
        {
            con.Clear();
            var console = new Writer();
            var height = 30;
            int width = Console.WindowWidth / 2;
            var client = new Window(console, 0, 0, width, height);
            var server = new Window(console, width, 0, width, height);
            
            // simulate a bunch of messages from my fake REST server

            server.WriteLine("Server started, listening on 'tcp://*:10001'.");
            client.WriteLine("enter commands, exit to quit");

            client.Send("put foo");
            server.Recieve("put foo");
            server.Send("404|Not Found|foo|");
            client.Recieve("404|Not Found|foo|");
            
            client.Send("post animals/cat");
            server.Recieve("post animals/cat");
            server.Send("201|Created|animals/cat|");
            client.Recieve("201|Created|animals/cat|");

            client.Send("post animals/dog");
            server.Recieve("post animals/dog");
            server.Send("201|Created|animals/dog|");
            client.Recieve("201|Created|animals/dog|");

            client.Send("get animals");
            server.Recieve("get animals");
            server.Send("206 | Partial content | animals |[`animals/cat`, `animals/dog`]");
            server.Send("200 | OK | animals / cat |");
            server.Send("200 | OK | animals / dog |");

            client.Recieve("206 | Partial content | animals |[`animals/cat`, `animals/dog`]");
            client.Recieve("200 | OK | animals / cat |");
            client.Recieve("200 | OK | animals / dog |");

            server.WriteLine("");
            server.WriteLine("finished, press enter to continue");
            Console.ReadLine();
        }


        private static void Progress(IConsole con)
        {
            Console.Clear();
            Console.WriteLine("'p' Test Progress bars");
            Console.WriteLine("----------------------");

            var pb = new ProgressBar(10);
            var pb2 = new ProgressBar(30);
            Console.Write("loading...");
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                Console.Write($" {i}");
                pb.Refresh(i, $"loading cat {i}");
                pb2.Refresh(i, $"loading dog {i}");

            }
            pb.Refresh(10, "All cats loaded.");
            Console.WriteLine(" Done!");
            Console.ReadKey(true);
            Console.Clear();
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

    public static class SendRec
    {
        public static void Send(this Window con, string text)
        {
            con.WriteLine(text);
        }

        public static void Recieve(this Window con, string text)
        {
            // awful, but will do...for rudimentary testing!
            var c = con.ForegroundColor;
            con.ForegroundColor = ConsoleColor.DarkGreen;
            con.WriteLine(text);
            con.ForegroundColor = c;
        }

    }
}

