using FluentAssertions;
using NUnit.Framework;
using System;
using static System.ConsoleColor;

namespace Konsole.Tests.ListViewTests
{
    public class RefreshShould
    {
        internal class User
        {
            public User(string name, string iD, int credits)
            {
                Name = name;
                ID = iD;
                Credits = credits;
            }

            public string Name { get; set; }
            public string ID { get; set; }
            public int Credits { get; set; }
        }

        internal static class TestUsers
        {
            public static User[] Users()
            {
                return new User[]
                {
                    new User("Graham", "GRH01", 100),
                    new User("Kendall", "KEN01", 250)
                };
            }

        }

        [Test]
        public void RenderListViewInRequiredColumns()
        {
            var console = new MockConsole(30, 4);
            var view = new ListView<User>(console, TestUsers.Users, (u) => new[] { u.Name, u.ID, u.Credits.ToString("00000") }, 
                new Column("Name", 10),
                new Column("ID",10), 
                new Column("Credits", 7)
            );
            view.Refresh();
            var expected = new[]{
                "   Name   │    ID    │Credits ",
                "Graham    │GRH01     │00100   ",
                "Kendall   │KEN01     │00250   ",
                "                              "
                };

            console.Buffer.ShouldBe(expected);
        }

        [Test]
        public void WhenWildCardsUsed_RenderListViewInRequiredColumnsAndProRationInAvailableSpace()
        {
            var console = new MockConsole(30, 6);
            Window.HostConsole = console;
            var window = Window.OpenBox("users", 30, 6);
            var view = new ListView<User>(window, TestUsers.Users, (u) => new[] { u.Name, u.ID, u.Credits.ToString("00000") },
                new Column("Name", 0),
                new Column("ID", 8),
                new Column("Credits", 7)
            );
            view.Refresh();
            var expected = new[]{
                "┌─────────── users ──────────┐",
                "│   Name    │   ID   │Credits│",
                "│Graham     │GRH01   │00100  │",
                "│Kendall    │KEN01   │00250  │",
                "│                            │",
                "└────────────────────────────┘",
                };

            console.Buffer.ShouldBe(expected);
        }

        [Test]
        public void When_The_last_line_prints_onto_the_last_line_of_parent_window_should_not_scroll()
        {
            var console = new MockConsole(20, 5);
            Window.HostConsole = console;
            var window = Window.OpenBox("users", 20, 5);
            var view = new ListView<User>(window, TestUsers.Users, (u) => new[] { u.Name, u.Credits.ToString("00000") },
                new Column("Name", 0),
                new Column("Credits", 0)
            );
            view.Refresh();
            var expected = new[]{
                "┌────── users ─────┐",
                "│  Name  │ Credits │",
                "│Graham  │00100    │",
                "│Kendall │00250    │",
                "└──────────────────┘",
                };
            console.Buffer.ShouldBe(expected);
        }
    }
}
