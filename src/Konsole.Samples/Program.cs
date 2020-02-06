using System;
using static System.ConsoleColor;
using static Konsole.ControlStatus;

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

            var theme = new StyleTheme(
                 Style.BlueOnWhite.WithThickness(LineThickNess.Double),
                 Style.BlueOnWhite
            );

            while (true)
            {
                RenderGames(y + 2, theme, Active);
                RenderUsers(y + 2, theme, Inactive);
                Console.ReadKey(true);
                
                RenderGames(y + 2, theme, Inactive);
                RenderUsers(y + 2, theme, Active);
                Console.ReadKey(true);
            }
            
            //ListViewSampleFileBrowser.Main(null);

        }

        static void RenderUsers(IConsole parent, int sy, StyleTheme theme, ControlStatus status)
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
            view.Refresh(status);
        }

        static void RenderGames(int sy, StyleTheme theme, ControlStatus status)
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
            view.BusinessRuleColors = (i, col) =>
            {
                if (col == 3 && i.result == "lose") return new Colors(White, Red);
                return null;
            };
            view.Refresh(status);
        }

    }
}
