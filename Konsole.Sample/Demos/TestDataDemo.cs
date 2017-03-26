using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Konsole.Drawing;
using Konsole.Internal;
using Konsole.Menus;

namespace Konsole.Sample.Demos
{
    public static class TestDataDemo
    {
        // bug - Window.Open with Single line thickness opens with double!
        public static void Run(IConsole con)
        {
            var w1 = Window.Open(1, 1, 40, 28, "TestData : Filenames", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkBlue);
            var fileNames = TestData.MakeFileNames(100, ".txt", ".com", ".exe", ".png");
            foreach (var file in fileNames)
                w1.WriteLine(file);

            var w2 = Window.Open(43, 1, 40, 28, "TestData : Filenames", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkYellow);
            var objNames = TestData.MakeObjectNames(100);
            foreach (var oname in objNames)
                w2.WriteLine(oname);
            Console.ReadKey(true);
        }


        // code below shows a possible error  with Windows  : ends up scrolling the wrong portion of the window
        // need to decide if we want this behavior or if that simply overcomplicates things and simple usage above is recommended.

        //public static void Run(IConsole con)
        //{
        //    var w1 = new Window(con, 0, 0, 40, 30);
        //    w1.WriteLine(ConsoleColor.Yellow, "TestData : Filenames");
        //    w1.WriteLine(ConsoleColor.Yellow, "--------------------");
        //    var w1b = new Window(w1, 0, 2, 40, 27);
        //    var fileNames = TestData.MakeFileNames(100, ".txt", ".com", ".exe", ".png");
        //    foreach (var file in fileNames)
        //    {
        //        w1b.WriteLine(file);
        //        Thread.Sleep(20);
        //    }

        //    var w2 = new Window(con, 42, 0, 40, 30);
        //    w2.WriteLine(ConsoleColor.Yellow, "TestData : Object Names");
        //    w2.WriteLine(ConsoleColor.Yellow, "-----------------------");
        //    var w2b = new Window(w2, 0, 2, 40, 27);
        //    var objNames = TestData.MakeObjectNames(100);
        //    foreach (var oname in objNames)
        //    {
        //        w2b.WriteLine(oname);
        //        Thread.Sleep(20);
        //    }
        //}

    }
}
