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

        public static void Sample()
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

            var window = new Window(50, 20);
            var compressWindow = window.SplitTop("compress");
            var encryptWindow = window.SplitBottom("encrypt");

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

    }

}
