using Konsole.Diagnostics;
using Konsole.Samples;
using System;
using static System.ConsoleColor;

namespace Konsole.Tests.RunOne
{
    class Program
    {
        static void Main(string[] args)
        {
            var cwin = new Window();
            ListViewDemos.ListViewRenderChessGamesSwapFocus();
            Console.ReadKey(true);
        }
    }
}
