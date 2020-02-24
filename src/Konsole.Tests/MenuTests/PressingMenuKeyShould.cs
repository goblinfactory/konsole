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
        }


        [Test]
        public void execute_the_matching_menu_item()
        {
            
            var con = new MockConsole(15, 7);
            var seq = new List<char>();

            var m = new Menu(con, "MENU", ConsoleKey.Q, 10,
                new MenuItem('A', "item 1", m => seq.Add('1')),
                new MenuItem('B', "item 2", m => seq.Add('2')),
                new MenuItem('C', "item 3", m => seq.Add('3')),
                new MenuItem('D', "item 4", m => seq.Add('4')),
                new MenuItem('E', "item 5", m => seq.Add('5'))
                );

            m.Keyboard = new MockKeyboard('A','F','A','D','D','Q');
            
            m.Run();
            
            var actual = new string(seq.ToArray());

            Console.WriteLine();
            Console.WriteLine(con.BufferString);

            Assert.AreEqual("1144",actual);

        }

        [Test]
        public void menu_with_only_one_menu_item_must_not_be_case_sensitive()
        {
            var con = new MockConsole(15, 10);

            var m1 = new Menu(con, "MENU", ConsoleKey.Q, 10,
                new MenuItem('A', "item 1", m => { })
                );
            Assert.AreEqual(false, m1.CaseSensitive);

            var m2 = new Menu(con, "MENU", ConsoleKey.Q, 10,
                new MenuItem('a', "item 1", m => { })
                );

            Assert.AreEqual(false, m2.CaseSensitive);
        }

        [Test]
        public void menu_with_menu_items_differing_only_by_case_must_be_case_sensitive()
        {
            var con = new MockConsole(15, 10);

            var m1 = new Menu(con, "MENU", ConsoleKey.Q, 10,
                new MenuItem('A', "item 1", m => { }),
                new MenuItem('a', "item 1", m => { })
                );
            Assert.AreEqual(true, m1.CaseSensitive);

            var m2 = new Menu(con, "MENU", ConsoleKey.Q, 10,
                new MenuItem('A', "item 1", m => { }),
                new MenuItem('B', "item 1", m => { })
                );

            Assert.AreEqual(false, m2.CaseSensitive);


            var m3 = new Menu(con, "MENU", ConsoleKey.Q, 10,
                new MenuItem('A', "item 1", m => { }),
                new MenuItem('b', "item 1", m => { })
            );
            Assert.AreEqual(false, m3.CaseSensitive);

        }

        [Test]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(false, true, true)]
        [TestCase(false, false, true)]
        public void WhenDisableWhenRunning_and_already_running_should_not_activate_selected_menu_item(bool disableWhenRunning, bool alreadyRunning, bool expectActivation)
        {
            Assert.Inconclusive();
        }

        [Test]
        [TestCase(true, "12")]
        [TestCase(false, "1")]
        public void when_menu_item_is_disabled_it_must_not_be_activated_if_user_presses_corresponding_menu_key(bool enabled, string expected)
        {
            var con = new MockConsole(15, 7);
            var seq = new List<char>();

            var m = new Menu(con, "MENU", ConsoleKey.Q, 10,
                new MenuItem('A', "item 1", m => seq.Add('1')) { Enabled = true },
                new MenuItem('B', "item 2", m => seq.Add('2')) { Enabled = enabled }
                );

            m.Keyboard = new MockKeyboard('A', 'B', 'Q');
            m.Run();

            var actual = new string(seq.ToArray());

            Console.WriteLine(actual);
            Console.WriteLine(con.BufferString);

            Assert.AreEqual(expected, actual);

        }

    }
}
