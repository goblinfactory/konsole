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
            var w1 = Window.Open(30, 1, 30, 20, "TestData : Filenames", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkBlue);
            var fileNames = TestData.MakeFileNames(100, ".txt", ".com", ".exe", ".png");
            foreach (var file in fileNames)
                w1.WriteLine(file);

            var w2 = Window.Open(63, 1, 30, 20, "TestData : Filenames", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkYellow);
            var objNames = TestData.MakeObjectNames(100);
            foreach (var oname in objNames)
                w2.WriteLine(oname);    
        }


    }
}
