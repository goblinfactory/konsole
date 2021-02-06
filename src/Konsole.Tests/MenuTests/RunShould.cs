using System;
using System.Linq;
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
            
            var m = new Menu(con, "MENU", ConsoleKey.Escape, 10,
                new MenuItem('a', "item 1", m => { }),
                new MenuItem('b', "item 2", m => { }),
                new MenuItem('c', "item 3", m => { })
                );
            m.Keyboard = new MockKeyboard(ConsoleKey.Escape);
            m.Run();

            var expected = new[]{
                "    MENU       ",
                "               ",
                " a. item 1     ",
                " b. item 2     ",
                " c. item 3     ",
                "               ",
                "               ",
                };

            con.Buffer.ShouldBe(expected);

            // idea : write colour tests using two colours that can be represented using solid and space.
            // then assert that the colors are ..and show the layout for foreground, and background.
            // finally assert the text, independantly of the foreground or background colour.
            // so that you can visually "see" the colouring.

            expected = new[]{
                " yr yr yr yrMyrEyrNyrUyr yr yr yr yr wk wk wk",
                " Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba wk wk wk",
                " BaaBa.Ba BaiwBtwBewBmwB wB1wB Ba Ba wk wk wk",
                " BabBa.Ba BaiBatBaeBamBa Ba2Ba Ba Ba wk wk wk",
                " BacBa.Ba BaiBatBaeBamBa Ba3Ba Ba Ba wk wk wk",
                " Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba wk wk wk",
                " wk wk wk wk wk wk wk wk wk wk wk wk wk wk wk",
                };

            con.BufferWithColor.ShouldBe(expected);
        }


        [Test]
        public void render_the_menu_with_correct_theme_colours()
        {
            var con = new MockConsole(15, 7);

            var m = new Menu(con, "MENU", ConsoleKey.Escape, 10,
                new MenuItem('a', "item a", m => { }),
                new MenuItem('b', "item b", m => { }),
                new MenuItem('q', "item c", m => { })
                );
            m.Keyboard = new MockKeyboard(ConsoleKey.Escape);
            m.Run();

            var expected = new[]{
                " yr yr yr yrMyrEyrNyrUyr yr yr yr yr wk wk wk",
                " Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba wk wk wk",
                " BaaBa.Ba BaiwBtwBewBmwB wBawB Ba Ba wk wk wk",
                " BabBa.Ba BaiBatBaeBamBa Babba Ba Ba wk wk wk",
                " BaqBa.Ba BaiBatBaeBamBa BacBa Ba Ba wk wk wk",
                " Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba Ba wk wk wk",
                " wk wk wk wk wk wk wk wk wk wk wk wk wk wk wk",
                };
            con.BufferWithColor.ShouldBe(expected);
        }

    }
}
