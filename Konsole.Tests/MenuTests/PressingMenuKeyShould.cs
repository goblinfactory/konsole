using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using FluentAssertions;
using Konsole.Menus;
using NUnit.Framework;

namespace Konsole.Tests.MenuTests
{
    public class PressingMenuKeyShould
    {
        [Test]
        public void hilight_current_menu_item()
        {
            Assert.Inconclusive();
            Assert.Fail("todo : write this test");
        }


        [TestCase(1,false,false, "abc")]
        [TestCase(1,false,true, "abc")]
        [TestCase(1,true,false, "abc")]
        [TestCase(1,true,true, "abc")]
        [Test]
        public void execute_the_matching_menu_item(int sort, bool enabled, bool disableWhenRunning, string expected)
        {

            //TODO: need to write a test for the menu that pressing q, actually exits the menu!!
            // todo write .. PressingExitKeyShould.cs

            var con = new MockConsole(15, 7);
            var seq = new List<char>();

            var m = new Menu(con, "MENU", ConsoleKey.Q, 10,
                new MenuItem('a', "item 1", () => seq.Add('a')) { Enabled = true },
                new MenuItem('b', "item 2", () => seq.Add('b')) { Enabled = enabled, DisableWhenRunning = disableWhenRunning },
                new MenuItem('c', "item 3", () => seq.Add('c')) { Enabled = true }
                );

            m.Keyboard = new MockKeyboard('a','b','c','q');
            m.Run();

            var actual = new string(seq.ToArray());

            Console.WriteLine($"enabled:{enabled,-4}, disableWhenRunning:{disableWhenRunning,-4}   Output:{actual}");
            Console.WriteLine();
            Console.WriteLine(con.BufferString);

            Assert.AreEqual(expected,actual);

            //Approvals.VerifyAll(con.BufferWithColor, "menu");
        }
    }
}
