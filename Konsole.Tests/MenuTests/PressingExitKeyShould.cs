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

        // do I need to test a number of conditions ... must X ... AND .. Y ... AND ..Z
        // in which case, put setup in constructor and tests in methods.

            // need to add in tests for special keys e.g. escape!!

            [TestCase('q')]
            [TestCase('Q')]
            [TestCase(ConsoleKey.Escape)]
        public void exit_the_menu(object key)
        {
            var con = new MockConsole(15, 7);
            var seq = new List<char>();

            Menu m = null;
            if (key is ConsoleKey)
            {
                m = new Menu(con, "MENU", (ConsoleKey) key, 10,
                    new MenuItem('a', "item 1", () => { })
                );
                m.Keyboard = new MockKeyboard('Q');
            }
            else
            {
                ConsoleKey c = (ConsoleKey) ((char) key);
                m = new Menu(con, "MENU",c, 10,
                    new MenuItem('a', "item 1", () => { })
                );
                m.Keyboard = new MockKeyboard('Q');
            }

            m.Run();
        }

    }
}
