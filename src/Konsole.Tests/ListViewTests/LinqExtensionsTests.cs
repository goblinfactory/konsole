using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Konsole.Internal;
using FluentAssertions;

namespace Konsole.Tests.ListViewTests
{
    [TestFixture]
    public class LinqExtensionsTests
    {

        public class DecorateShould
        {
            [Test]
            public void WhenEmpty_ShouldReturn_EmptyArray()
            {
                var columns = new Column[0];
                (Column, int)[] decorations = columns.Decorate((c, first, last) => 1);
                decorations.Length.Should().Be(0);
            }

            [Test]
            public void WhenNull_ShouldReturn_EmptyArray()
            {
                Column[] columns = null;
                (Column, int)[] decorations = columns.Decorate((c, first, last) => 1);
                decorations.Length.Should().Be(0);
            }

            [Test]
            public void Return_ItemAndDecorationTuple()
            {
                var columns = new[]
                {
                    new Column("name1", 1, Colors.WhiteOnBlack, true),
                    new Column("name2", 2, Colors.WhiteOnBlack, false),
                    new Column("name3", 4, Colors.WhiteOnBlack, true)
                };

                var expected = new (Column, int)[]
                {
                    (new Column("name1", 1, Colors.WhiteOnBlack, true),1),
                    (new Column("name2", 2, Colors.WhiteOnBlack, false),0),
                    (new Column("name3", 4, Colors.WhiteOnBlack, true),4)
                };

                (Column, int)[] actual = columns.Decorate((c, first, last) => c.Visible ? c.Width : 0);

                actual.Should().BeEquivalentTo(expected);
            }


            [Test]
            public void SafelyWorkWithClosurecapturedLocalVariables()
            {
                var columns = new[]
                {
                    new Column("name1", 10, Colors.WhiteOnBlack, true),
                    new Column("name2", 5, Colors.WhiteOnBlack, false),
                    new Column("name3", 20, Colors.WhiteOnBlack, true)
                };

                var expected = new (Column, int)[]
                {
                    (new Column("name1", 10, Colors.WhiteOnBlack, true),30),
                    (new Column("name2", 5, Colors.WhiteOnBlack, false),25),
                    (new Column("name3", 20, Colors.WhiteOnBlack, true),5)
                };

                // this local variable will be captured in the closure below, it is not threadsafe!
                int balance = 40;
                (Column, int)[] actual = columns.Decorate((c, first, last) =>
                {
                    balance -= c.Width;
                    return balance;
                });

                actual.Should().BeEquivalentTo(expected);
            }

        }

    }
}
