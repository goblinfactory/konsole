using System;
using static System.ConsoleColor;
using Bogus;

namespace Konsole.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            int y = Console.CursorTop;
            Console.ForegroundColor = White;
            Console.BackgroundColor = DarkBlue;

            var normal = new BoxStyle(
                LineThickNess.Single,
                body: new Colors(Gray, DarkBlue),
                line: new Colors(Gray, Black),
                title: new Colors(Gray, Black)
            );

            var active = new BoxStyle(
                LineThickNess.Single,
                body: new Colors(White, DarkBlue),
                line: new Colors(White, DarkBlue),
                title: new Colors(White, DarkBlue)
            );

            while (true)
            {
                RenderGames(y + 2, active);
                RenderUsers(y + 2, normal);
                Console.ReadKey(true);
                
                RenderGames(y + 2, normal);
                RenderUsers(y + 2, active);
                Console.ReadKey(true);
            }
            
            //ListViewSampleFileBrowser.Main(null);

        }

        static void RenderUsers(int sy, BoxStyle style)
        {
            var window = Window.OpenBox("players", 7, sy,  35, 7, style);
            var view = new ListView<(string Name, int Credits, string IPAddress)>(
                window,
                () => new[] { 
                    ("Fred", 250, "80.56.12.11"), 
                    ("Sally", 100, "74.11.23.44"), 
                    ("Michael", 0, "63.11.11.40"), 
                    ("Dennis", 0, "88.22.23.12")  }, (u) => new[] { 
                        u.Name, 
                        u.Credits.ToString("00000"),
                        u.IPAddress
                    },
                new Column("Name", 0),
                new Column("Credits", 0),
                new Column("IP Address", 0)
            );
            view.Refresh();
        }

        static void RenderGames(int sy, BoxStyle style)
        {
            var window = Window.OpenBox("openings", 50, sy, 35, 12, style);
            var view = new ListView<(string opening, int moves, string result)>(
                window,
                () => new[] {
                    ("Kings Gambit", 39, "win"),
                    ("Sicilian Defence", 35, "draw"),
                    ("French Defence", 22, "win"),
                    ("Alekhine Defence", 19, "win"),
                    ("Kings Gambit", 33, "win"),
                    ("Kings Indian", 21, "draw"),
                    ("Ruy Lopaz", 82, "lose")  }, (u) => new[] {
                        u.opening,
                        u.moves.ToString(),
                        u.result,
                    },
                new Column("Opening", 0),
                new Column("Moves", 7),
                new Column("Result", 7)
            );
            view.Refresh();
        }

    }
}
