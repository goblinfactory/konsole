using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole.Menus;
using NUnit.Framework;

namespace Konsole.Tests.MenuTests
{
    public class PressingEnterShould
    {
        [Test]
        public void run_selected_menu_item()
        {
            var c = new MockConsole(10,4);
            //var m = new Menu(c,)
        }


        [Test]
        public void exit_menu_if_item_keybinding_is_quit_key()
        {
            Assert.Inconclusive();
        }

    }
}
