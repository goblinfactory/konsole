using FluentAssertions;
using Konsole.Tests.Internal;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static System.ConsoleColor;

namespace Konsole.Tests.ListViewTests
{
    [TestFixture]
    public class ThemeTests
    {
        MockConsole console;
        ListView<User> view;
        IConsole box;

        [SetUp]
        public void Setup()
        {
            console = new MockConsole(20, 5);
            box = console.OpenBox("users", 20, 5);
            view = new ListView<User>(box, () => TestUsers.CreateUsers(2), (u) => new[] { u.Name, u.Credits.ToString("00000") },
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

        // changing parent window theme should result in children being updated when they refresh.
        // might need window to register controls, and call refresh on children, that recursively call refresh on their children.

        [Test]
        [TestCase(Red, Blue)]
        [TestCase(Yellow, DarkMagenta)]
        public void WhenThemeNotOverridden_ShouldUse_ParentConsoleTheme(ConsoleColor foreground, ConsoleColor background)
        {
            console.Peek(1, 2).Colors.Should().BeEquivalentTo(new Colors(White, Black));
            var colors = new Colors(foreground, background);
            box.Colors = colors;
            view.Refresh();

            // list items
            console.Peek(1, 2).Colors.Should().BeEquivalentTo(colors);

            // selected Item
        }
    }
}
