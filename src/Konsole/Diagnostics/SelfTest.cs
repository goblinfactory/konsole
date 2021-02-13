using System;
using System.Threading;

namespace Konsole.Diagnostics
{
    public static class SelfTest
    {

        public static void Test()
        {
            var window = new Window();
            Test(window, () => { });
        }


        /// <summary>
        /// demonstrates text wrapping, performance, progress bar, splitLeft, SplitRight, SplitRows, SplitColumns, printing in color, highspeed writing with HighSpeedWriter and flush, 
        /// </summary>
        /// <param name="window"></param>
        /// <param name="flush"></param>
        public static void Test(IConsole window, Action flush)
        {
            var consoles = window.SplitRows(
                    new Split(4, "heading", LineThickNess.Single),
                    new Split(10),
                    new Split(0),
                    new Split(4, "status", LineThickNess.Single)
            ); ; ;

            var headline = consoles[0];
            var contentTop = consoles[1];
            var contentBottom = consoles[2];
            var status = consoles[3];

            var longText = "Let's see if this text wraps. Plus some more text to see if this eventually wraps. Here is more text to see if this eventually wraps, plus some more text to see if this eventually wraps? ";

            var menu = contentTop.SplitLeft("menu");
            var content = contentTop.SplitRight("content");

            var splits = contentBottom.SplitColumns(
                    new Split(20, "menu2"),
                    new Split(0),
                    new Split(20, "content3")
                );
            var menu2 = splits[0];
            menu2.WriteLine(longText);
            var content2a = splits[1];
            var content2parts = content2a.SplitRows(
                new Split(0, "content 2"),
                new Split(5, "demo REPL input")
                );
            var content2 = content2parts[0];
            var input = content2parts[1];
            var content3 = splits[2];
            content2.WriteLine(longText);
            content3.WriteLine(longText);
            headline.WriteLine("my headline");
            headline.WriteLine("line2");
            headline.WriteLine("line3");
            content.WriteLine("content goes here");
            content.WriteLine("Do these lines ╢╖╣║╗╟  print?");
            content.WriteLine(longText);
            menu.WriteLine("Options A");
            menu.WriteLine("Options B");
            menu.WriteLine(longText);
            menu.WriteLine("Options D");
            status.Write("System offline!");
            flush();
            input.WriteLine(ConsoleColor.Green, "  press 'esc' to quit");

            //input.CursorVisible = true;
            //input.CursorLeft = 0;
            //input.CursorTop = 0;

            char key = 'x';
            int color = 0;
            var statusProgress = new ProgressBar(status, 100);
            flush();
            while (key != 'q')
            {
                color++;
                content2.WriteLine((ConsoleColor)(color % 15), longText);
                var fruit = $"apples {color}";
                menu2.WriteLine(fruit);
                menu.WriteLine(fruit);
                statusProgress.Refresh(color % 100, fruit);
                flush();
                var readKey = Console.ReadKey(true);
                if (readKey.Key == ConsoleKey.Escape)
                {
                    input.Colors = new Colors(ConsoleColor.White, ConsoleColor.Red);
                    input.WriteLine("   --- GOOD BYE!! ---   ");
                    flush();
                    Thread.Sleep(1000);
                    return;
                }
                if (readKey.Key == ConsoleKey.Enter)
                {
                    input.WriteLine("");
                    continue;
                }
                key = readKey.KeyChar;

                input.Write(new string(new[] { key }));
            }

        }


    }
}
