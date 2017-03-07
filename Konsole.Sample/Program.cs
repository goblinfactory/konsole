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
            ProgressBarDemo.ProgressivelyFasterDemo(50,w);

        }


        private static void RunCommand(IConsole con, char cmd)
        {
            switch (cmd)
            {
                case 'f':
                    FormsDemo.Run();
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
                    ParallelDemo();
                    break;

                case 'd':
                    ProgressBarDemo.ProgressivelyFasterDemo();
                    break;

                case 'p':
                    ProgressBarDemo.SimpleDemo(con);
                    break;

                case 'b':
                    BoxesDemo.Run();
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

