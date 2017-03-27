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
        public void only_create_helptext_window_when_output_console_p()
        {
            var con = new MockConsole(10,10);
            var output = new MockConsole();
            
            var m = new Menu(con, output, 'q', 10, new MenuItem('a', "item 1", c => { }));


            var expected = new[]
            {
                ""
            };
            // create without console
        }

        [Test]
        public void display_menu_inline_and_move_cursor_below_menu()
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
        public void render_one_line_per_menu_item()
        {
            var con = new MockConsole(10, 10);
            var output = new MockConsole();
            var m = new Menu(con, output, 'q', 10, 
                new MenuItem('a', "item 1", c => { }),
                new MenuItem('b', "item 2", c => { }),
                new MenuItem('c', "item 3", c => { })
                );
            var expected = new[]
            {
                "item 1",
                "item 2",
                "item 3"
            };

            Console.WriteLine(con.BufferString);
            con.Buffer.ShouldBeEquivalentTo(expected);
        }

    }
}
