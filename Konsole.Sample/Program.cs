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
            // experiment to see if we can read a block of the screen by using move and redirecting input and output
            // see http://stackoverflow.com/questions/12355378/read-from-location-on-console-c-sharp

            // windows specific 
            // more information https://msdn.microsoft.com/en-us/library/windows/desktop/ms681913(v=vs.85).aspx

            con.WriteLine("testing opening a dialog window and running one of the demos");
            con.WriteLine(" ('f' progressively faster) inside it");
            var w = Window.Open(5,5,60,10, LineThickNess.Double, ConsoleColor.Black, ConsoleColor.DarkCyan);
            w.WriteLine("new window");
            ProgressivelyFasterDemo(50,w);

        }


        private static void RunCommand(IConsole con, char cmd)
        {
            switch (cmd)
            {
                case 'f':
                    TestForms();
                    break;

                case 'w':
                    TestWindows(con);
                    break;

                case 'r':
                    RandomStuff(con);
                    break;

                case 'h':
                    TestHilite(con);
                    break;

                case 'l':
                    ParallelDemo();
                    break;

                case 'd':
                    ProgressivelyFasterDemo();
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
            con.WriteLine("r : random test stuff. Changes often on each checkin.");
            con.WriteLine("    (Open a dialog window and run one of the other tests 'f' inside it.)");
            con.WriteLine("h : hiliting");
            con.WriteLine("b : boxes");
            con.WriteLine("d : progressively faster 'd'emo");
            con.WriteLine("p : progress bars");
            con.WriteLine("l : Parallel Demo");
            con.WriteLine("q : quit");
            con.WriteLine("");
        }

        private static void TestWindows(IConsole con)
        {
            con.WriteLine("starting client server demo");
            var height = 20;
            int width = (Console.WindowWidth / 3) - 3;
            var client = new Window(1, 3, width, height, ConsoleColor.Gray, ConsoleColor.DarkBlue);
            var server = new Window(width + 2, 3, width, height);
            
            Console.CursorTop = 22;
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

            client.WriteLine("this is a long line that will wrap over the end of the window.");

            
            var nameWindow = Window.Open(width * 2 + 3, 3, width, height + 1); // HACK is this an error? shouldn't have to add 1 to height?
            var names = TestData.MakeNames(height-2);
            con.ForegroundColor = ConsoleColor.DarkGray;
            con.WriteLine("If you see ??? in any of the names, that's because Windows terminal does not print all UTF8 characters. (This prints correctly in Linux and Mac).");
            foreach(var name in names) nameWindow.WriteLine(name);
        }


        private static void Progress(IConsole con)
        {
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
        }


        private static void TestHilite(IConsole con)
        {
            var normal = ConsoleColor.Black;
            var hilite = ConsoleColor.White;

            var console = new Window(40, 10, true);
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

        public static void TestBoxes()
        {
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

        public static void ProgressivelyFasterDemo(int startingPauseMilliseconds = 50, Window window = null)
        {
            var pb = window?.ProgressBar(300) ?? new ProgressBar(300);
            var names = TestData.MakeNames(300);
            int cnt = names.Count();
            int i = 1;
            foreach (var name in names)
            {
                pb.Refresh(i++, name);
                int pause = startingPauseMilliseconds - (1 * (i * (startingPauseMilliseconds - 1) / cnt));
                if (pause > 0) Thread.Sleep(pause);
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
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
            Console.WriteLine("ready press enter.");
            Console.ReadLine();

            foreach (var t in tasks) t.Start();
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("done.");
        }

        public void SimplestUsage()
        {
            Console.WriteLine("Simplest usage");
            var pb = new ProgressBar(50);
            pb.Refresh(0, "connecting to server to download 50 files sychronously.");
            Console.ReadLine();
            pb.Refresh(5, "downloading file 5");
            Console.ReadLine();
            pb.Refresh(50, "finished.");
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

