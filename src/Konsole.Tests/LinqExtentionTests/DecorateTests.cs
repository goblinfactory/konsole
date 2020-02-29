using FluentAssertions;
using NUnit.Framework;
using System;
using Konsole.Internal;

namespace Konsole.Tests.LinqExtentionTests
{
    public class DecorateTests
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

        // *****************************************
        // **                                     **
        // **  return item and decoration tuple   **
        // **                                     **
        // *****************************************
        [Test]
        public void Return_ItemAndDecorationTuple()
        {
            var actual = _return_ItemAndDecorationTuple();
            var expected = new (Column, int)[]
            {
                        (new Column("name1", 1, Colors.WhiteOnBlack, true),1),
                        (new Column("name2", 2, Colors.WhiteOnBlack, false),0),
                        (new Column("name3", 4, Colors.WhiteOnBlack, true),4)
            };
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        [Category("performance")]
        public void Return_ItemAndDecorationTuple_10µs()
        {
            TestTimer.RunTest_µs(microSeconds: 7, _return_ItemAndDecorationTuple);
        }

        private static (Column, int)[] _return_ItemAndDecorationTuple()
        {
            var actual = new[]
            {
                    new Column("name1", 1, Colors.WhiteOnBlack, true),
                    new Column("name2", 2, Colors.WhiteOnBlack, false),
                    new Column("name3", 4, Colors.WhiteOnBlack, true)
                }.Decorate((c, first, last) => c.Visible ? c.Width : 0);
            for (int i = 0; i < 40; i++) Console.WriteLine("crash it!");
            return actual;
        }

        // ***************************************************
        // **                                               **
        // **  Safely work with closure captured variables  **
        // **                                               **
        // ***************************************************
        private static (Column, int)[] _safelyWorkWithClosurecapturedLocalVariables()
        {
            var columns = new[] {
                        new Column("name1", 10, Colors.WhiteOnBlack, true),
                        new Column("name2", 5, Colors.WhiteOnBlack, false),
                        new Column("name3", 20, Colors.WhiteOnBlack, true)
                    };
            int balance = 40;
            var actual = columns.Decorate((c, first, last) => {
                balance -= c.Width;
                return balance;
            });
            return actual;
        }

        [Test]
        public void SafelyWorkWithClosurecapturedLocalVariables()
        {
            var actual = _safelyWorkWithClosurecapturedLocalVariables();
            var expected = new (Column, int)[]
            {
                    (new Column("name1", 10, Colors.WhiteOnBlack, true),30),
                    (new Column("name2", 5, Colors.WhiteOnBlack, false),25),
                    (new Column("name3", 20, Colors.WhiteOnBlack, true),5)
            };
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        [Category("performance")]
        public void SafelyWorkWithClosurecapturedLocalVariables_2ms()
        {
            TestTimer.RunTest_ms(milliSeconds: 1.5, _safelyWorkWithClosurecapturedLocalVariables);
        }

    }


}
