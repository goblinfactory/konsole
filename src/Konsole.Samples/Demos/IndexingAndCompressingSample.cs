using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.ConsoleColor;

namespace Konsole.Samples.Samples
{
    public static class IndexingAndCompressingSample
    {
        public static void Run()
        {
            static void Compress(IConsole status, string file)
            {
                status.WriteLine($"compressing {file}");
                Thread.Sleep(new Random().Next(10000));
                status.WriteLine(Green, $"{file} (OK)");
            }

            static void Index(IConsole status, string file)
            {
                status.WriteLine($"indexing {file}");
                Thread.Sleep(new Random().Next(10000));
                status.WriteLine(Green, " finished.");
            }

            // seems to keep the cursor on top! (not showing up as an inline window)
            //var window = new Window(50, 20);

            var console = new Window();
            console.WriteLine(console.CursorTop.ToString());

            var compressWindow = console.OpenBox("compress", 40, 4);
            console.WriteLine(console.CursorTop.ToString());

            var encryptWindow = console.OpenBox("encrypt", 40, 4);
            console.WriteLine(console.CursorTop.ToString());


            var tasks = new List<Task>();

            while (true)
            {
                // no background threads so can use Console
                console.Write("Enter name of file to process (quit) to exit:");
                var file = Console.ReadLine();
                if (file == "quit") break;
                tasks.Add(Task.Run(() => Compress(compressWindow, file)));
                tasks.Add(Task.Run(() => Index(encryptWindow, file)));
                console.WriteLine($"processing {file}");
            }

            console.WriteLine("waiting for background tasks");
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("done.");
        }

    }
}
