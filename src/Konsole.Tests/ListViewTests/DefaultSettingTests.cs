using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole.Tests.ListViewTests
{
    public class DefaultSettingTests
    {
        [Test]
        public void NewListViewShouldHaveADefaultStyle()
        {
            var con = new MockConsole(20, 6);
            var view = new ListView<int>(con, () => new[] { 1, 2, 3 },
                (i) => new[] { "sdd" },
                new Column("c1", 0),
                new Column("c2", 0)
            );
            view.Style.Should().NotBeNull();
        }
    }
}
