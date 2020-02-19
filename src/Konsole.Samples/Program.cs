using System;
using static System.ConsoleColor;
using static Konsole.ControlStatus;

namespace Konsole.Samples
{
    public class Foo
    {

        public static void InlineWindowMovesParentConsoleCursorTopToBelow()
        {
            Console.WriteLine("this is the top line");
            var w1 = new Window(6, White, Red);
            Console.ReadKey(true);

            w1.WriteLine("line1");
            w1.WriteLine("line2");
            w1.WriteLine("line3");
            w1.WriteLine("line4");
            w1.WriteLine("line5");
            Console.ReadKey(true);
            w1.WriteLine("line6");
            w1.WriteLine("line7");
            Console.ReadKey(true);

            Console.WriteLine("line2");
            Console.ReadKey(true);
            var w2 = w1.Open(3, Black, White);
            Console.ReadKey(true);
            w2.WriteLine(Red, "w2: hello1");
            w2.WriteLine(Red, "w2: hello2");
            Console.ReadKey(true);
            
            w2.WriteLine(Red, "w2: hello3");
            w2.WriteLine(Red, "w2: hello4");
            Console.ReadKey(true);

            w1.WriteLine(Yellow, "w1: hello1");
            w1.WriteLine(Yellow, "w1: hello2");
            w1.Write(Yellow, "w1: hello3");
            Console.ReadKey(true);
        }

        public static void MenuWithoutHotKeys()
        {
            var win = new Window();
            var left = win.SplitLeft("menu");
            var output = win.SplitRight("output");
            // menu should appear below the 8 line "output" window which should be inline!
            // there's a bug here! need to fix...

            int cats = 0;
            int dogs = 0;
            int mice = 0;

            Menu menu1 = null;

            menu1 = new Menu(left, "ANIMALS", ConsoleKey.Escape, 20, MenuLine.none,
                new MenuItem('1', "CATS", m => { output.WriteLine("cats"); m.Title = $"CATS {++cats}"; }),
                new MenuItem('2', "DOGS", m => { output.WriteLine("doggies"); m.Title = $"DOGS {++dogs}"; }),
                new MenuItem('3', "MICE", m => { output.WriteLine("mouses"); m.Title = $"MICE {++mice}"; }),
                MenuItem.Quit("QUIT")
            );

            int apples = 0;
            int bananas = 0;
            int grapes = 0;

            menu1.Run();
            
            left.WriteLine("");

            Menu menu2 = null;
            menu2 = new Menu(left, "FRUIT", ConsoleKey.Escape, 30, MenuLine.none,
                new MenuItem('1', "apples", m => { output.WriteLine("apples"); m.Title = $"apples [{++apples}] £ {(apples * 0.45): 0.00}"; }),
                new MenuItem('2', "bananas", m => { output.WriteLine("bananas"); m.Title = $"bananas [{++bananas}] £ {(bananas * 0.1): 0.00}"; }),
                new MenuItem('3', "grapes", m => { output.WriteLine("grapes"); m.Title = $"grapes [{++grapes}] £ {(grapes * 0.05): 0.00}"; }),
            MenuItem.Quit("QUIT")
            );
            
            menu2.Run();

        }

        public static void ListViewThemeTest()
        {
            var window = new Window(50, 10, new StyleTheme(Yellow, DarkMagenta).WithTitle(White, Red));
            var incoming = window.SplitLeft("INCOMING");
            var outgoing = window.SplitRight("OUTGOING");

            var view = new ListView<(string Name, int Credits)>(incoming, 
                () => new[] { ("Graham", 100), ("Kendall", 250) }, 
                (u) => new[] { u.Name, u.Credits.ToString("00000") },
                new Column("Name", 0),
                new Column("Credits", 0)
            );
            view.Refresh();
            Console.ReadKey(true);
        }
        
        private static void MenuTest()
        {
            int linesNo = 0;
            var lines = MenuLine.naked;

            while(true)
            {
                var win = new Window(100, 50);
                var cols = win.SplitColumns(new Split(24), new Split(0));
                var menuCon = cols[0];
                Menu menu = null;
             
                menu = new Menu("Accounts", ConsoleKey.Q, 20, MenuLine.naked,
                    new MenuItem('1', $"cycle menuLines [{lines}]", m => {
                        lines = (MenuLine)(linesNo++ % 5);
                        menu.Refresh();
                    }),
                    new MenuItem('2', "None", m => { }),
                    new MenuItem('3', "Customers", m => { }),
                    new MenuItem('q', "Quit", m => { })
                );

                menu.Run();
            }

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Foo.MenuWithoutHotKeys();
            
            return;

            AllTheDifferentConstructors.Demo();
            return;
            
            int y = Console.CursorTop;
            //Console.ForegroundColor = White;
            //Console.BackgroundColor = DarkBlue;

            var window = new Window(100, 20, Style.BlueOnWhite);

            while (true)
            {
                RenderGames(window, y + 2, Active);
                RenderUsers(window, y + 2, Inactive);
                Console.ReadKey(true);
                
                RenderGames(window, y + 2, Inactive);
                RenderUsers(window, y + 2, Active);
                Console.ReadKey(true);
            }
            
            //ListViewSampleFileBrowser.Main(null);

        }

        static void RenderUsers(IConsole console, int sy, ControlStatus status)
        {

            var window = console.OpenBox("players", new WindowSettings { SX = 7, SY = sy, Width = 35, Height = 7 } );
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
            view.Status = status;
            view.Refresh();
        }

        static void RenderGames(IConsole console, int sy, ControlStatus status)
        {
            var window = console.OpenBox("openings", 50, sy, 35, 12);
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
            view.Status = status;
            view.Refresh();
        }

    }
}
