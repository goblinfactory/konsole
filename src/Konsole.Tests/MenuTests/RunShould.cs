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

            expected = new[]{
                " yr yr yr yrMyrEyrNyrUyr yr yr yr yr wk wk wk",
                " wB wB wB wB wB wB wB wB wB wB wB wB wk wk wk",
                " wBawB.wB wBiBatBaeBamBa Ba1Ba wB wB wk wk wk",
                " wBbwB.wB wBiwBtwBewBmwB wB2wB wB wB wk wk wk",
                " wBcwB.wB wBiwBtwBewBmwB wB3wB wB wB wk wk wk",
                " wB wB wB wB wB wB wB wB wB wB wB wB wk wk wk",
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
                " wB wB wB wB wB wB wB wB wB wB wB wB wk wk wk",
                " wBawB.wB wBiBatBaeBamBa BaaBa wB wB wk wk wk",
                " wBbwB.wB wBiwBtwBewBmwB wBbwB wB wB wk wk wk",
                " wBqwB.wB wBiwBtwBewBmwB wBcwB wB wB wk wk wk",
                " wB wB wB wB wB wB wB wB wB wB wB wB wk wk wk",
                " wk wk wk wk wk wk wk wk wk wk wk wk wk wk wk",
                };
            con.BufferWithColor.ShouldBe(expected);
        }

    }
}
