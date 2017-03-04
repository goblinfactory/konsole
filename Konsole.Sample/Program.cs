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

        private static void RandomStuff(IConsole con)
        {
            con.Clear();
            con.WriteLine("one");

            var w = new Window(10,10,20,15, ConsoleColor.Black,ConsoleColor.DarkYellow, true, con);
            w.WriteLine("new window");

            con.WriteLine("two");
            con.WriteLine("three");
            Console.ReadKey();
        }


        private static void Main(string[] args)
        {
            var con = new Window();
            char cmd = ' ';
            while (cmd != 'q')
            {
                printMenu(con);
                cmd = Console.ReadKey(true).KeyChar;
                switch (cmd)
                {
                    case 'c':
                        con.Clear();
                        Console.Clear();
                        printMenu(con);
                        break;

                        
                    case 'l':
                        Console.Clear();
                        ParallelDemo();
                        break;

                    case 'f':
                        Console.Clear();
                        ProgressivelyFasterDemo();
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

                    case 'r':
                        RandomStuff(con);
                        break;

                    default:
                        break;
                }

            }
        }

        private static void printMenu(Window con)
        {
            con.WriteLine("");
            con.WriteLine("w : windows");
            con.WriteLine("h : hiliting");
            con.WriteLine("b : boxes");
            con.WriteLine("f : progressively faster demo");
            con.WriteLine("p : progress bars");
            con.WriteLine("l : Parallel Demo");
            con.WriteLine("c : clear screen");
            con.WriteLine("r : random test stuff (changes often on each checkin)");
            con.WriteLine("q : quit");
            con.WriteLine("");
        }

        private static void TestWindows(IConsole con)
        {


            con.Clear();
            Console.WriteLine("starting client server");
            var height = 30;
            int width = (Console.WindowWidth / 2) - 2;
            var client = new Window(1, 2, width, height);
            var server = new Window(width + 2, 2, width, height);
            Console.CursorTop = 32;
            // simulate a bunch of messages from my fake REST server

            var messages = new[]
            {
                new { Request = "put foo", Response = "404|Not Found|foo|"},
                new { Request = "post animals/cat", Response = "201|Created|animals/cat|"},
                new { Request = "post animals/dog", Response = "201|Created|animals/dog|"},
                new { Request = "get animals", Response = "206 | Partial content | animals |[`animals/cat`, `animals/dog`]"}

            };
            client.WriteLine("CLIENT");
            client.WriteLine("------");
            server.WriteLine("SERVER");
            server.WriteLine("------");

            foreach (var m in messages)
            {
                client.Send(m.Request);
                server.Recieve(m.Request);
                server.Send(m.Response);
                client.Recieve(m.Response);
                client.WriteLine("");
                server.WriteLine("");
            }

            Console.WriteLine("");
            Console.WriteLine("finished, press any key to continue");
            Console.ReadKey();
            Console.Clear();
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

            var console = new Window(40, 10, true);
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
                var console = new Window(80, 20);
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
            // demo; take the first 10 directories that have files from solution root and then pretends to process (list) them.
            // processing of each directory happens on a different thread, to simulate multiple background tasks, 
            // e.g. file downloading.
            // ==============================================================================================================
            var dirs = Directory.GetDirectories(@"..\..\..\..").Where(d=> Directory.GetFiles(d).Any()).Take(7);

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
            var c = con.BackgroundColor;
            con.BackgroundColor = ConsoleColor.DarkYellow;
            con.WriteLine(text);
            con.BackgroundColor = c;
        }

    }
}

