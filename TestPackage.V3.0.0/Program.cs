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

        private static void GreenAction(IConsole con, char c)
        {
            con.WriteLine("pressed " + c);
        }

        private static void YellowAction(IConsole con, char c)
        {
            con.WriteLine("pressed " + c);
        }



        static void Main(string[] args)
        {
            var con = new Window(Console.WindowWidth, 20);

            var green = con.SplitLeft("left");
            green.ForegroundColor = ConsoleColor.Green;
            
            var yellow = con.SplitRight("right");
            yellow.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 0; i < 30; i++)
            {
                green.Write("PART ONE - ");
                green.WriteLine(" - PART TWO");
            }
                
            Console.ReadLine();
            return;
            var k = new Keyboard();
            bool stop = false;
            var t1 = Task.Run(() =>
            {
                k.OnCharPressed('1', c => GreenAction(green, c));
                k.OnCharPressed('2', c =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        green.Write(".");
                        Thread.Sleep(500);
                    }
                    green.WriteLine("pressed " + c);
                });
                while(!stop) Thread.Sleep(10);
            });

            var t2 = Task.Run(() =>
            {
                k.OnCharPressed('2', c =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        yellow.Write(".");
                        Thread.Sleep(500);
                    }

                    YellowAction(yellow, c);
                });
                k.OnCharPressed('3', c => yellow.WriteLine("pressed " + c));
                while (!stop) Thread.Sleep(10);
            });

            k.WaitForKeyPress('q');
            stop = true;
            Task.WaitAll(t1, t2);


            var menu = con.SplitLeft("menu", ConsoleColor.White);
            var aw = con.SplitRight();
            var client = aw.SplitTop("client", ConsoleColor.White);
            var server = aw.SplitBottom("server", ConsoleColor.White);

            menu.WriteLine("option 1");
            menu.WriteLine("option 2");
            menu.WriteLine("option 3");
                
            client.WriteLine("waiting for server");
            server.WriteLine("starting server");
            client.WriteLine("sending");
            server.WriteLine("recieved, replying");
            client.WriteLine("got it!");
            Console.ReadKey(true);
            Console.WriteLine("finished");
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
