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
                new MenuItem('a', "item 1", () => { }),
                new MenuItem('b', "item 2", () => { }),
                new MenuItem('c', "item 3", () => { })
                );
            m.Keyboard = new MockKeyboard(ConsoleKey.Escape);
            m.Run();

            var expected = new[]{
                "     MENU      ",
                "               ",
                "   a. item     ",
                "   b. item     ",
                "   c. item     ",
                "               ",
                "               ",
                };

            con.Buffer.ShouldBe(expected);

            expected = new[]{
                " wk wk yr yr yrMyrEyrNyrUyr yr yr yr wk wk wk",
                " wk wk wB wB wB wB wB wB wB wB wB wB wk wk wk",
                " wk wk wBawB.wB wBiBatBaeBamBa wB wB wk wk wk",
                " wk wk wBbwB.wB wBiwBtwBewBmwB wB wB wk wk wk",
                " wk wk wBcwB.wB wBiwBtwBewBmwB wB wB wk wk wk",
                " wk wk wB wB wB wB wB wB wB wB wB wB wk wk wk",
                " wk wk wk wk wk wk wk wk wk wk wk wk wk wk wk",
                };

            con.BufferWithColor.ShouldBe(expected);
        }


        [Test]
        public void render_the_menu_with_correct_theme_colours()
        {
            var con = new MockConsole(15, 7);

            var m = new Menu(con, "MENU", ConsoleKey.Escape, 10,
                new MenuItem('a', "item a", () => { }),
                new MenuItem('b', "item b", () => { }),
                new MenuItem('q', "item c", () => { })
                );
            m.Keyboard = new MockKeyboard(ConsoleKey.Escape);
            m.Run();

            var expected = new[]{
                " wk wk yr yr yrMyrEyrNyrUyr yr yr yr wk wk wk",
                " wk wk wB wB wB wB wB wB wB wB wB wB wk wk wk",
                " wk wk wBawB.wB wBiBatBaeBamBa wB wB wk wk wk",
                " wk wk wBbwB.wB wBiwBtwBewBmwB wB wB wk wk wk",
                " wk wk wBqwB.wB wBiwBtwBewBmwB wB wB wk wk wk",
                " wk wk wB wB wB wB wB wB wB wB wB wB wk wk wk",
                " wk wk wk wk wk wk wk wk wk wk wk wk wk wk wk",
                };

            con.BufferWithColor.ShouldBe(expected);
        }

    }
}
