using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konsole.Menus;
using NUnit.Framework;

namespace Konsole.Tests.MenuTests
{
    public class PressingExitKeyShould
    {
        [Test]
        public void WhenSpecialKeyIsConfiguredAsExitKey_should_exit_the_menu()
        {
            var con = new MockConsole(15, 7);
            var seq = new List<char>();

            var m = new Menu(con, "MENU", ConsoleKey.Escape, 10,
                new MenuItem('a', "item 1", () => { seq.Add('a'); })
            );

            m.Keyboard = new MockKeyboard(ConsoleKey.Escape);
            m.Run();
        }

        [Test]
        public void WhenShortcutKeyIsConfiguredAsExitKey_should_exit_the_menu()
        {
            var con = new MockConsole(15, 7);
            var seq = new List<char>();

            var m = new Menu(con, "MENU", ConsoleKey.Escape, 10,
                MenuItem.Quit('x',"Exit")
            );

            m.Keyboard = new MockKeyboard('x');
            m.Run();
        }


        [Test]
        public void WhenEmptyMenuItemIsConfiguredAsExitKey_should_exit_the_menu()
        {
            var con = new MockConsole(15, 7);

            var m = new Menu(con, "MENU", ConsoleKey.F10, 10,
                MenuItem.Quit("Quit")
            );

            m.Keyboard = new MockKeyboard(ConsoleKey.Enter);
            m.Run();
        }


    }
}
