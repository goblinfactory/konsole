using System.Diagnostics;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class ClearShould
    {
        [Test]
        public void clear_the_buffer_test2()
        {
            var con = new MockConsole(10,4);
            var w1 = new Window(5, 2, con);
            w1.WriteLine("one");
            w1.WriteLine("two");
            Assert.AreEqual(new[] { "one", "two" }, w1.BufferWrittenTrimmed);
            Assert.AreEqual(new[] { "one", "two" }, con.BufferWrittenTrimmed);
            w1.Clear();
            w1.WriteLine("three");
            Assert.AreEqual(new string[] { "three"}, w1.BufferWrittenTrimmed);
            Assert.AreEqual(new string[] { "three"}, con.BufferWrittenTrimmed);
        }

        [Test]
        public void only_clear_the_overlapping_portion_of_window_in_the_parent_window()
        {
            //NB! Current implementation clears the parent at the same time as the window! Not cool!
            Assert.Inconclusive();
        }

        [Test]
        public void resetting_buffer_many_times_should_not_cause_a_problem()
        {
            var sw = new Stopwatch();
            sw.Start();
            var con = new Window(10, 4, false);
            for (int i = 0; i < 100; i++)
            {
                con.WriteLine("one");
                con.WriteLine("two");
                con.WriteLine("three");
                con.WriteLine("four");
                con.Clear();
            }
            con.WriteLine("five");
            con.WriteLine("six");
            con.WriteLine("seven");
            con.WriteLine("eight");
            var expected = new[]
            {
                "five",
                "six",
                "seven",
                "eight",

            };

            Assert.AreEqual(expected,con.BufferWrittenTrimmed);
        }


        [Test]
        public void reset_the_y_position()
        {
            var con = new Window(10, 2, false);
            Assert.AreEqual(0,con.CursorTop);
            con.WriteLine("one       ");
            con.WriteLine("two       ");
            Assert.AreEqual(2, con.CursorTop);
            con.Clear();
            Assert.AreEqual(0, con.CursorTop);
        }
    }
}
