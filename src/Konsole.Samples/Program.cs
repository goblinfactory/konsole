using System;
using static System.ConsoleColor;
using Bogus;

namespace Konsole.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello there");
            Console.ReadKey(true);
            int y = Console.CursorTop;
            while(true)
            {
                Render(y + 2, Blue, White, Yellow, new Colors(White, Red));
                Console.ReadKey(true);
                Render(y + 2, DarkBlue, White, Yellow, new Colors(White, Red));
                Console.ReadKey(true);
            }
            //Console.CursorVisible = false;
            //ListViewSampleFileBrowser.Main(null);

        }

        static void Render(int sy, ConsoleColor bg, ConsoleColor fg, ConsoleColor line, Colors title)
        {
            var window = Window.OpenBox("users", 5, sy,  25, 5, new BoxStyle { Body = new Colors(fg, bg), Line = new Colors(line, bg), Title = title });
            var view = new ListView<(string Name, int Credits)>(
                window,
                () => new[] { ("Fred", 250), ("Sally", 100) }, (u) => new[] { u.Name, u.Credits.ToString("00000") },
                new Column("Name", 0),
                new Column("Credits", 0)
            );
            view.Refresh();
        }
    }
}
