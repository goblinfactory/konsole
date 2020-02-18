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
        [Test]
        [TestCase(Red, Blue)]
        [TestCase(Yellow, DarkMagenta)]
        public void WhenThemeNotOverridden_ShouldUse_ParentConsoleTheme(ConsoleColor foreground, ConsoleColor background)
        {

            var console = new MockConsole(20, 5, foreground, background);
            var box = console.OpenBox("users", 20, 5);
            var view = new ListView<User>(box, () => TestUsers.CreateUsers(2), (u) => new[] { u.Name, u.Credits.ToString("00000") },
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
            console.Peek(1, 2).Colors.Should().BeEquivalentTo(new Colors(foreground, background));
        }
    }
}
