using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Konsole.Drawing;
using Konsole.Forms;
using Konsole.Testing;

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
            Console.WriteLine("press enter to start");
            Console.ReadLine();
            TestBoxes();
            Console.WriteLine("Press enter to close");
            Console.ReadLine();
        }

        public static void TestBoxes()
        {
            Console.Clear();
            var console = new ConsoleWriter();
            var line = new Line(console, LineThickNess.Double);
            line
                .Box(10, 10, 60, 20, "my test box")
                .DrawHorizontal(10, 12, 60, LineThickNess.Single)
                .DrawHorizontal(10, 14, 60, LineThickNess.Single);
            
            console.PrintAt(12, 11, "DEMO INVOICE -");
            console.CursorTop = 21;
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
                var console = new TestConsole(80, 20);
                var cat = new Cat(console);
                cat.Greet();
                Assert.AreEqual(console.TrimmedLines, new[] {"Prrr!", "Meow!"});
            }
            {
                // create an instance of a cat that will purr to the real Console
                // ==============================================================
                var cat = new Cat(new ConsoleWriter());
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
                if (Console.KeyAvailable)
                {
                    if (Console.ReadKey().Key == ConsoleKey.P)
                    {
                        Thread.Sleep(2000);
                        continue;
                    }
                    Console.WriteLine("key press detected, stopped before end.");
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
            Console.WriteLine("ready press enter.");
            Console.ReadLine();

            foreach (var t in tasks) t.Start();
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("done.");
            Console.ReadLine();

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

