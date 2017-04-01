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
    
    public class ConstructorShould
    {

        [Test]
        public void display_menu_inline_and_move_cursor_below_menu()
        {
            Assert.Inconclusive("new feature");
        }

        [Test]
        public void allow_you_to_assing_function_keys_to_menus()
        {
            Assert.Inconclusive("new feature");
        }

        [Test]
        public void clip_the_menu_items_to_fit_the_width()
        {
            Assert.Inconclusive("new feature");
        }

        [Test]
        public void autosize_the_menu_to_widest_menu_item_if_no_width_provided()
        {
            Assert.Inconclusive("new feature");
        }

        [Test]
        public void support_defining_menu_without_shortcut_keys()
        {
            //Assert.Inconclusive("new feature");

            var con = new MockConsole(40,10);
            var output = new MockConsole(20,20);
            int i = 0;

            var menu = new Menu(con, output, "TITLE", 'q', 20,
                new MenuItem("ONE", (d) => { d.WriteLine("cats"); }),
                new MenuItem("TWO", (d) => { d.WriteLine("dogs"); }),
                new MenuItem("TWO", (d) => { d.WriteLine("mice"); }),
                new MenuItem("QUIT", null )
            );
            menu.Keyboard = new MockKeyboard(ConsoleKey.DownArrow, ConsoleKey.Enter, ConsoleKey.DownArrow, ConsoleKey.DownArrow, ConsoleKey.Enter);
            menu.Run();
            output.BufferWrittenTrimmed.ShouldBeEquivalentTo(new []
                {
                 "dogs"   
                });
        }

    }
}
