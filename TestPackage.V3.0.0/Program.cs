using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Konsole;
using Konsole.Drawing;
using Konsole.Internal;
using Konsole.Layouts;
using Konsole.Menus;

namespace TestPackage.V3._0._3
{
    class Program
    {

        static void Main(string[] args)
        {


            var c = new Window(100, 30);
            var mw = c.SplitLeft();
            //var aw = c.RightHalf("app window", WindowTheme.Inherit(c, false));
            var aw = c.SplitRight();
            var clientConsole1 = aw.SplitTop("client", ConsoleColor.Red);
            var serverConsole = aw.SplitBottom("server", ConsoleColor.Blue);

            //Q: Why can't I see right half and right half's top and bottom?


            Console.ReadKey(true);
            return;

            //var w = Window.Open(10, 10, 80, 20, "test scrolling");
            //var t = w.TopHalf("game");
            //for(int i =0; i<10;i++) t.WriteLine(i.ToString());

            var w = new Window(100, 32,ConsoleColor.DarkRed, ConsoleColor.DarkYellow);
            //w.WriteLine("starting");
            //var w2 = new Window(w, 80, 10);
            var w2 = TopBox(w, "top");
            var w3 = BottomBox(w, "bottom");


            for (int i = 0; i < 20; i++)
            {
                w2.WriteLine("foo " + i.ToString());
                w3.WriteLine("foo " + i.ToString());
            }

            //var b = w.BottomHalf("players");
            //for (int i = 0; i < 10; i++) b.WriteLine(i.ToString());
            Console.ReadKey(true);
        }

        static void Main2(string[] args)
        {
            var w = Window.Open(10, 10, 80, 20, "Single line progress bar demo");
            var t = w.SplitTop("game");
            var b = w.SplitBottom("players");
            t.WriteLine("extracting...");
            FakeExtractWithProgress(t, "sounds     ", 10);
            FakeExtractWithProgress(t, "characters ", 5);
            FakeExtractWithProgress(b, "skins      ", 20);
            b.WriteLine("done. ");
        }


        public static void FakeExtractWithProgress(IConsole window, string packageName, int numItems)
        {
            var pb = new ProgressBar(window, numItems, 25);
            var fakePackageParts = TestData.MakeObjectNames(numItems);
            for (int i = 0; i < numItems; i++) {
                Thread.Sleep(new Random().Next(1000));
                var part = fakePackageParts[i];
                pb.Refresh(i+1, $"{packageName} : {part}");
            }
        }

        public static IConsole TopBox(IConsole c, string title)
        {
            int h = c.WindowHeight/2;
            int w = c.WindowWidth;
            new Draw(c).Box(0, 0, w-1 , h-1, title, LineThickNess.Single);
            return  Window._CreateWindow(1, 1, w - 2, h - 2, c.ForegroundColor, c.BackgroundColor, true, c, null);
        }

        public static IConsole BottomBox(IConsole c, string title)
        {
            int h = c.WindowHeight / 2;
            int offset = c.WindowHeight - h;
            int w = c.WindowWidth;
            new Draw(c).Box(0, offset, w - 1, (h - 1)+offset, title, LineThickNess.Single);
            return Window._CreateWindow(1, 1+offset, w - 2, h - 2, c.ForegroundColor, c.BackgroundColor, true, c, null);
        }

    }
}
