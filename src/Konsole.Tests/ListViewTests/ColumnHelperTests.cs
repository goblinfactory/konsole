//using FluentAssertions;
//using NUnit.Framework;
//using System.Linq;

//namespace Konsole.Tests.ListViewTests
//{
//    [TestFixture]
//    [Ignore("ignore")]
//    public class ColumnHelperTests
//    {
//        [Test]
//        [TestCase(12, new[] { 2, 3, 5 }, new[] { 2, 3, 5 })] // 12 minus 2 characters for the spacer bars!
//        [TestCase(13, new[] { 2, 2, 3, 3 }, new[] { 2, 2, 3, 3 })] // 13 minus 3 characters for the spacer bars!
//        [TestCase(5, new[] { 5 }, new[] { 5 })]
//        public void A_WhenNoWildCards_AndExactMatchShould_ReturnColumns(int width, int[] current, int[] expected)
//        {
//            var columns = current.Select(w => new Column("name", w, Colors.BlackOnWhite, true)).ToArray();
//            var decorated = columns.ResizeColumns(width);
//            var newWidths = decorated.Select(d => d.Width).ToArray();
//            //newWidths.Should().BeEquivalentTo(expected);
//        }

//        [Test]
//        [TestCase(12, new[] { 2, 0, 5 }, new[] { 2, 3, 5 })] // 12 minus 2 characters for the spacer bars!
//        [TestCase(13, new[] { 2, 2, 0, 3 }, new[] { 2, 2, 3, 3 })] // 13 minus 3 characters for the spacer bars!
//        [TestCase(5, new[] { 0 }, new[] { 5 })]
//        public void B_WhenSingleWildCard_AndExactMatchShould_ReturnColumns(int width, int[] current, int[] expected)
//        {
//            var columns = current.Select(w => new Column("name", w, Colors.BlackOnWhite, true)).ToArray();
//            var decorated = columns.ResizeColumns(width);
//            var newWidths = decorated.Select(d => d.Width).ToArray();
//            //newWidths.Should().BeEquivalentTo(expected);
//        }

//        [Test]
//        [TestCase(13, new[] { 2, 0, 0, 3 }, new[] { 2, 2, 2, 4 })] // 13 minus 3 characters for the spacer bars, last character picks up the overflow (columns can't be 2.5 chars wide)
//        [TestCase(13, new[] { 0, 2, 0, 0 }, new[] { 2, 2, 2, 4 })] // 13 minus 3 characters for the spacer bars!
//        [TestCase(14, new[] { 0, 0, 2, 0, 0 }, new[] { 2, 2, 2, 2, 2 })] // 14 minus 4 characters for the spacer bars!
//        public void C_WhenMultipleWildCards_AndExactMatchShould_ReturnColumnsAndEqualWildCards(int width, int[] current, int[] expected)
//        {
//            var columns = current.Select(w => new Column("name", w, Colors.BlackOnWhite, true)).ToArray();
//            var decorated = columns.ResizeColumns(width);
//            var newWidths = decorated.Select(d => d.Width).ToArray();
//            //newWidths.Should().BeEquivalentTo(expected);
//        }

//        [Test]
//        [TestCase(13, new[] { 2, 0, 0, 3 }, new[] { 2, 2, 2, 4 })] // even though column 3 is fixed width, it's the last column so has to take up extra chars to ensure a perfect fit!
//        public void D_WhenMultipleWildCards_AndRoundingDifferencesShould_ReturnColumnsAndEqualWildCardsExceptForLastColumnContainingSpare(int width, int[] current, int[] expected)
//        {
//            var columns = current.Select(w => new Column("name", w, Colors.BlackOnWhite, true)).ToArray();
//            var decorated = columns.ResizeColumns(width);
//            var newWidths = decorated.Select(d => d.Width).ToArray();
//            //newWidths.Should().BeEquivalentTo(expected);
//        }

//        [Test]
//        [TestCase(18, new[] { 2, 0, 0, 0, 3 }, new[] { 2, 3, 3, 3, 3 })]
//        public void E_WhenMultipleWildCards_AndExactColumnMatchShould_ReturnColumnsAndEqualWildCards(int width, int[] current, int[] expected)
//        {
//            var columns = current.Select(w => new Column("name", w, Colors.BlackOnWhite, true)).ToArray();
//            var decorated = columns.ResizeColumns(width);
//            var newWidths = decorated.Select(d => d.Width).ToArray();
//            //newWidths.Should().BeEquivalentTo(expected);
//        }

//    }
//}
