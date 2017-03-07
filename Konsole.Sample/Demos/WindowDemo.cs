using System;
using Konsole.Internal;

namespace Konsole.Sample.Demos
{
    internal class WindowDemo
    {
        public static void Run(IConsole con)
        {
            con.WriteLine("starting client server demo");
            var height = 20;
            int width = (Console.WindowWidth / 3) - 3;
            var client = new Window(1, 3, width, height, ConsoleColor.Gray, ConsoleColor.DarkBlue);
            var server = new Window(width + 2, 3, width, height);

            Console.CursorTop = 22;
            // simulate a bunch of messages from my fake REST server

            var messages = new[]
            {
                new { Request = "put foo", Response = "404|Not Found|foo|"},
                new { Request = "post animals/cat", Response = "201|Created|animals/cat|"},
                new { Request = "post animals/dog", Response = "201|Created|animals/dog|"},
                new { Request = "get animals", Response = "206 | Partial content | animals |[`animals/cat`, `animals/dog`]"}

            };
            client.WriteLine("CLIENT");
            client.WriteLine("------");
            server.WriteLine("SERVER");
            server.WriteLine("------");

            foreach (var m in messages)
            {
                client.Send(m.Request);
                server.Recieve(m.Request);
                server.Send(m.Response);
                client.Recieve(m.Response);
                client.WriteLine("");
                server.WriteLine("");
            }

            client.WriteLine("this is a long line that will wrap over the end of the window.");


            var nameWindow = Window.Open(width * 2 + 3, 3, width, height + 1); // HACK is this an error? shouldn't have to add 1 to height?

            // let's print 300 names (300 WriteLine statements) that will roll right off 
            // the bottom of the screen, and should be clipped.
            var names = TestData.MakeNames(300);
            con.ForegroundColor = ConsoleColor.DarkGray;
            con.WriteLine("If you see ??? in any of the names, that's because Windows terminal does not print all UTF8 characters. (This prints correctly in Linux and Mac).");
            foreach (var name in names) nameWindow.WriteLine(name);
        }

    }

    public static class SendRec
    {
        public static void Send(this Window con, string text)
        {
            con.WriteLine(text);
        }

        public static void Recieve(this Window con, string text)
        {
            // awful, but will do...for rudimentary testing!
            var c = con.BackgroundColor;
            con.BackgroundColor = ConsoleColor.DarkYellow;
            con.WriteLine(text);
            con.BackgroundColor = c;
        }

    }

}
