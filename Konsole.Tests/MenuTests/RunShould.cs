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
            
            var m = new Menu(con, output, "MENU", 'q', 10,
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
    }
}
