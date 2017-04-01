using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Konsole.Menus;
using NUnit.Framework;

namespace Konsole.Tests.MenuTests
{
    public class RunShould
    {
        [Test]
        public void render_the_menu_with_default_item_selected()
        {
            var con = new MockConsole(10, 5);
            var output = new MockConsole();
            
            var m = new Menu(con, output, "MENU", ConsoleKey.Escape, 10,
                new MenuItem('a', "item 1", c => { }),
                new MenuItem('b', "item 2", c => { }),
                new MenuItem('c', "item 3", c => { })
                );
            m.Keyboard = new MockKeyboard('q');
            m.Run();

            var expected = new[]
            {
                "MENU      ",
                "--------- ",
                "item 1    ",
                "item 2    ",
                "item 3    "
            };

            Console.WriteLine(con.BufferString);
            con.Buffer.ShouldBeEquivalentTo(expected);
        }


        [Test]
        public void render_the_menu_with_correct_theme_colours()
        {
            var con = new MockConsole(15, 7);
            var output = new MockConsole();

            var m = new Menu(con, output, "MENU", ConsoleKey.Escape, 10,
                new MenuItem('a', "item a", c => { }),
                new MenuItem('b', "item b", c => { }),
                new MenuItem('c', "item c", c => { })
                );
            m.Keyboard = new MockKeyboard('q');
            m.Run();

            // text below looks funny, that's because I've encoded the color of each character in the strings
            // so that I can verify that the foreground and background colors are correct. 
            // which i cannot do using standard C# strings.
            var expected = new[]
            {
                 " wk wk aB aB aB aB aB aB aB aB aB aB wk wk wk",
                 " wk wk aB aBMaBEaBNaBUaB aB aB aB aB wk wk wk",
                 " wk wk aB aB-aB-aB-aB-aB-aB-aB aB aB wk wk wk",
                 " wk wk aB aBiBatBaeBamBa Ba1Ba aB aB wk wk wk",
                 " wk wk aB aBiaBtaBeaBmaB aB2aB aB aB wk wk wk",
                 " wk wk aB aBiaBtaBeaBmaB aB3aB aB aB wk wk wk",
                 " wk wk aB aB aB aB aB aB aB aB aB aB wk wk wk"
            };
            
            con.BufferWithColor.ShouldBeEquivalentTo(expected);
        }

    }
}
