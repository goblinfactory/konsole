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
            var window = Window.OpenBox("users", 20, 5);
            var view = new ListView<(string Name, int Credits)>(
                window,
                () => new[] { ("Fred", 250), ("Sally", 100) }, (u) => new[] { u.Name, u.Credits.ToString("00000") },
                new Column("Name", 0),
                new Column("Credits", 0)
            );
            view.Refresh();
            Console.ReadKey(true);
            var c = new Colors(Red, White);
            //window.ForegroundColor = Red;
            //window.BackgroundColor = White;
            window.Colors = c;
            view.Refresh();
            Console.WriteLine("should now be Red on White?");
            Console.ReadKey(true);


            //Console.CursorVisible = false;
            //ListViewSampleFileBrowser.Main(null);

        }

    }
}
