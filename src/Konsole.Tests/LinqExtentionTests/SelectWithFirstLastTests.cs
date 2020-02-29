//using FluentAssertions;
//using NUnit.Framework;
//using System;
//using Konsole.Internal;

//namespace Konsole.Tests.LinqExtentionTests
//{
//    public class SelectWithFirstLastTests
//    {
//        [Test]
//        public void WhenEmpty_ShouldReturn_EmptyArray()
//        {
//            var columns = new Column[0];
//            Column[] decorations = columns.SelectWithFirstLast((c, first, last) => c);
//            decorations.Length.Should().Be(0);
//        }

//        [Test]
//        public void WhenNull_ShouldReturn_EmptyArray()
//        {
//            Column[] columns = null;
//            Column[] decorations = columns.SelectWithFirstLast((c, first, last) => c);
//            decorations.Length.Should().Be(0);
//        }

//        // ***************************
//        // **                       **
//        // **  return modified item **
//        // **                       **
//        // ***************************
//        [Test]
//        public void ReturnModifiedItem()
//        {
//            var before = new[]
//            {
//                    new Column("name1", 1, Colors.WhiteOnBlack, true),
//                    new Column("name2", 2, Colors.WhiteOnBlack, false),
//                    new Column("name3", 4, Colors.WhiteOnBlack, true)
//            };
            
//            var actual = before.SelectWithFirstLast((c, first, last) => c.Visible ? c : c.WithWidth(0));

//            var expected = new Column[]
//            {
//                        new Column("name1", 1, Colors.WhiteOnBlack, true),
//                        new Column("name2", 0, Colors.WhiteOnBlack, false),
//                        new Column("name3", 4, Colors.WhiteOnBlack, true)
//            };
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Test]
//        [Category("performance")]
//        public void Return_ItemAndDecorationTuple_10µs()
//        {
//            var items = new[]
//            {
//                    new Column("name1", 1, Colors.WhiteOnBlack, true),
//                    new Column("name2", 2, Colors.WhiteOnBlack, false),
//                    new Column("name3", 4, Colors.WhiteOnBlack, true)
//            };

//            TestTimer.RunTest_µs(microSeconds: 7, ()=> items.SelectWithFirstLast((c, first, last) => c.Visible ? c : c.WithWidth(0)));
//        }

//        // ***************************************************
//        // **                                               **
//        // **  Safely work with closure captured variables  **
//        // **                                               **
//        // ***************************************************
//        private static Column[] _safelyWorkWithClosurecapturedLocalVariables()
//        {
//            var columns = new[] {
//                        new Column("name1", 10, Colors.WhiteOnBlack, true),
//                        new Column("name2", 5, Colors.WhiteOnBlack, false),
//                        new Column("name3", 20, Colors.WhiteOnBlack, true)
//                    };
//            int balance = 40;
//            var actual = columns.SelectWithFirstLast((c, first, last) => {
//                balance -= c.Width;
//                return c.WithWidth(balance);
//            });
//            return actual;
//        }

//        [Test]
//        public void SafelyWorkWithClosurecapturedLocalVariables()
//        {
//            var actual = _safelyWorkWithClosurecapturedLocalVariables();
//            var expected = new Column[]
//            {
//                    new Column("name1", 30, Colors.WhiteOnBlack, true),
//                    new Column("name2", 25, Colors.WhiteOnBlack, false),
//                    new Column("name3", 5, Colors.WhiteOnBlack, true)
//            };
//            actual.Should().BeEquivalentTo(expected);
//        }

//        [Test]
//        [Category("performance")]
//        public void SafelyWorkWithClosurecapturedLocalVariables_2ms()
//        {
//            TestTimer.RunTest_ms(milliSeconds: 1.5, _safelyWorkWithClosurecapturedLocalVariables);
//        }

//    }


//}
