using System;

namespace Konsole.Samples
{
    public static class MenuDemos
    {
        public static void MenuTest()
        {
            int linesNo = 0;
            var lines = MenuLine.naked;

            while (true)
            {
                var win = new Window(100, 50);
                var cols = win.SplitColumns(new Split(24), new Split(0));
                var menuCon = cols[0];
                Menu menu = null;

                menu = new Menu("Accounts", ConsoleKey.Q, 20, MenuLine.naked,
                    new MenuItem('1', $"cycle menuLines [{lines}]", m => {
                        lines = (MenuLine)(linesNo++ % 5);
                        menu.Render();
                    }),
                    new MenuItem('2', "None", m => { }),
                    new MenuItem('3', "Customers", m => { }),
                    new MenuItem('q', "Quit", m => { })
                );

                menu.Run();
            }
        }

        public static void MenuWithoutHotKeys()
        {
            var win = new Window();
            var left = win.SplitLeft("menu");
            win.PrintAt(Colors.GrayOnDarkBlue, 55, 0, "[f1]");
            var output = win.SplitRight("output");
            win.PrintAt(Colors.GrayOnDarkBlue, 115, 0, "[f2]");
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

            left.WriteLine("");

            Menu menu2 = null;
            menu2 = new Menu(left, "FRUIT", ConsoleKey.Escape, 30, MenuLine.none,
                new MenuItem('1', "apples", m => { output.WriteLine("apples"); m.Title = $"apples [{++apples}] £ {(apples * 0.45): 0.00}"; }),
                new MenuItem('2', "bananas", m => { output.WriteLine("bananas"); m.Title = $"bananas [{++bananas}] £ {(bananas * 0.1): 0.00}"; }),
                new MenuItem('3', "grapes", m => { output.WriteLine("grapes"); m.Title = $"grapes [{++grapes}] £ {(grapes * 0.05): 0.00}"; }),
            MenuItem.Quit("QUIT")
            );

            menu2.Render();

            menu1.Run();

            Console.CursorTop = 8;
            Console.CursorLeft = 3;
            Console.CursorVisible = true;
            var name = Console.ReadLine();
            Console.CursorVisible = false;
            left.WriteLine(name);

            menu2.Run();

        }

    }
}
