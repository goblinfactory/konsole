using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using FluentAssertions;
using Konsole.Menus;
using NUnit.Framework;

namespace Konsole.Tests.MenuTests
{
    public class RunShould
    {
        [UseReporter(typeof(DiffReporter))]
        [Test]
        public void render_the_menu_with_default_item_selected_and_with_correct_colours()
        {
            var con = new MockConsole(15, 7);
            var output = new MockConsole();
            
            var m = new Menu(con, output, "MENU", ConsoleKey.Escape, 10,
                new MenuItem('a', "item 1", () => { }),
                new MenuItem('b', "item 2", () => { }),
                new MenuItem('c', "item 3", () => { })
                );
            m.Keyboard = new MockKeyboard(ConsoleKey.Escape);
            m.Run();

            var expected = new[]
            {
                "               ",
                "    MENU       ",
                "    ------     ",
                "    item 1     ",
                "    item 2     ",
                "    item 3     ",
                "               "
            };

            Console.WriteLine(con.BufferString);
            con.Buffer.ShouldBeEquivalentTo(expected);
            Approvals.VerifyAll(con.BufferWithColor,"menu");
        }


        [Test]
        public void render_the_menu_with_correct_theme_colours()
        {
            var con = new MockConsole(15, 7);
            var output = new MockConsole();

            var m = new Menu(con, output, "MENU", ConsoleKey.Escape, 10,
                new MenuItem('a', "item a", () => { }),
                new MenuItem('b', "item b", () => { }),
                new MenuItem('q', "item c", () => { })
                );
            m.Keyboard = new MockKeyboard(ConsoleKey.Escape);
            m.Run();

            // text below looks funny, that's because I've encoded the color of each character in the strings
            // so that I can verify that the foreground and background colors are correct. 
            // which i cannot do using standard C# strings.
            var expected = new[]
            {
                 " wk wk aB aB aB aB aB aB aB aB aB aB wk wk wk",
                 " wk wk aB aBMaBEaBNaBUaB aB aB aB aB wk wk wk",
                 " wk wk aB aB-aB-aB-aB-aB-aB-aB aB aB wk wk wk",
                 " wk wk aB aBiBatBaeBamBa Baara aB aB wk wk wk",
                 " wk wk aB aBiaBtaBeaBmaB aBbwB aB aB wk wk wk",
                 " wk wk aB aBiaBtaBeaBmaB aBcaB aB aB wk wk wk",
                 " wk wk aB aB aB aB aB aB aB aB aB aB wk wk wk"
            };
            
            foreach(var line in con.BufferWithColor) Console.WriteLine(line);
            con.BufferWithColor.ShouldBeEquivalentTo(expected);
        }

    }
}
