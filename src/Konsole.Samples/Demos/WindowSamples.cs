using System;
using System.Collections.Generic;
using System.Text;
using static System.ConsoleColor;

namespace Konsole.Samples
{
    public static class OpenBoxSamples
    {
        private static void wait() => Console.ReadKey(true);

        private static void Fill(IConsole con){
            con.WriteLine("one");
            con.WriteLine("two");
            con.WriteLine("three");
            con.WriteLine("four");
            con.WriteLine("five");
        }
        public static void Opening_windows_without_a_parent()
        {
            // full screen with defaults
            // -------------------------
            var win = Window.OpenBox("full screen");
            Fill(win);

            wait();

            // full screen with style
            // ----------------------
            win = Window.OpenBox("full screen", new WindowSettings
            {
                Theme = new Style(
                    thickNess:      LineThickNess.Single,
                    body:           new Colors(Yellow, DarkBlue),
                    title:          new Colors(White, DarkRed),
                    columnHeaders:  new Colors(Yellow, Red),
                    line:           new Colors(Yellow, Black),
                    selectedItem:   new Colors(White,Red),
                    bold:           new Colors(White, Red)
            ).ToTheme()
            }
            );
            
            Fill(win);

            wait();


            // inline at current cursor
            // with style
            // ----------------------
            Console.Clear();
            Console.WriteLine("line 1");
            Console.WriteLine("line 2");
            // should continue at current line 3!! ie Inline.
            win = Window.OpenBox("full screen", 60, 10, new Style(
                thickNess: LineThickNess.Double,
                body: new Colors(Yellow, Black),
                title: new Colors(White, Red),
                columnHeaders: new Colors(Yellow, Red),
                line: new Colors(Red, Black),
                selectedItem: new Colors(White, Red),
                bold: new Colors(White, Red)
            ));
            Fill(win);

            throw new Exception("manual test above does not start the window at the current inline line, expect line 3! expect to preserve existing console CursorTop!");
            

            // inline


            // inline full width


            // multiple boxed windows with titles -> use Splits


            // without a split, with a title -> use OpenBox

            // ovveridding colors
        }

        public static void Opening_windows_inside_a_window()
        {
            Console.WriteLine("examples of simplest and fastest way to open windows with least amount of fuss.");

            // floating with title


            // floating without box


            // inline


            // inline full width


            // multiple boxed windows with titles -> use Splits


            // without a split, with a title -> use OpenBox

            // ovveridding colors
        }

        public static void deep_nesting_of_windows()
        {
            Console.WriteLine("examples of simplest and fastest way to open windows with least amount of fuss.");

            // floating with title


            // floating without box


            // inline


            // inline full width


            // multiple boxed windows with titles -> use Splits


            // without a split, with a title -> use OpenBox

            // ovveridding colors
        }

    }
}
