using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.ConsoleColor;

namespace Konsole.Samples
{
    public static class InputSamples
    {

        public static void NoBackgroundThread()
        {
            static void Compress(IConsole status, string file) {
                status.Write($"compressing {file}");
                Thread.Sleep(2000);
                status.WriteLine(Green, " finished.");
            }

            static void Index(IConsole status, string file)
            {
                status.Write($"indexing {file}");
                Thread.Sleep(2000);
                status.WriteLine(Green, " finished.");
            }

            //var window = new Window(50, 20);
            var compressWindow = Window.OpenBox("compress", 50, 10);
            var encryptWindow = Window.OpenBox("encrypt", 50, 10);

            while(true)
            {
                // no background threads so can use Console
                Console.Write("Enter name of file to process (quit) to exit:");
                var file = Console.ReadLine();
                if (file == "quit") break;
                Compress(compressWindow, file);
                Index(encryptWindow, file);
            }
            Console.WriteLine("done.");
        }

        public static void Foo()
        {
            // NB this is the demo code, and it doesnt work as advertised! need to fix immediately.


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

            var console = new ConcurrentWriter();  // < -- NOTE THE ConcurrentWriter to replace Console

            var compressWindow = Window.OpenBox("compress", 50, 10);
            var encryptWindow = Window.OpenBox("encrypt", 50, 10);

            var tasks = new List<Task>();

            while (true)
            {
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
