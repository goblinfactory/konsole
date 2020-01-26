using System;
using Bogus;

namespace Konsole.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            ListViewSampleFileBrowser.Main(null);
            Console.ReadKey(true);
        }
    }
}
