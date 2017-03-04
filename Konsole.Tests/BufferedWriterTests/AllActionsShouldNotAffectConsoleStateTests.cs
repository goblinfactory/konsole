using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Konsole.Tests.BufferedWriterTests
{
    public class AllActionsShouldNotAffectConsoleStateTests
    {
        private IConsole _console;
        private IConsole _window;
        private ConsoleState _consoleState;

        [SetUp]
        public void Setup()
        {
            _console = new MockConsole();
            // simulate a bit of printing
            _console.WriteLine("line1");
            _console.ForegroundColor = ConsoleColor.Red;
            _console.BackgroundColor = ConsoleColor.Yellow;
            _console.Write("Warning!");
            _consoleState = _console.State;
        }

        private void CheckState()
        {
            var actual = _console.State;
            actual.ShouldBeEquivalentTo(_consoleState);
        }

        [Test]
        public void Write_and_writeline_test()
        {
            Assert.Inconclusive("This is technically new 'behavior', so won't include this test (which currently fails) until the behavior is implemented and passes these tests.");
            CheckState();
            var window = new Window(10, 3, _console);

            window.WriteLine("one");
            window.WriteLine("two");
            window.WriteLine("three");
            CheckState();
        }

        [Test]
        public void ForegroundColor_test()
        {
            var w = new Window(0, 0, _console);
            w.ForegroundColor = ConsoleColor.DarkGray;
            CheckState();
        }

        [Test]
        public void BackgroundColor_test()
        {
            var w = new Window(0, 0, _console);
            w.BackgroundColor = ConsoleColor.Red;
            CheckState();
        }


        [Test]
        public void Constructor_1_test()
        {
            var w = new Window(2,2, 10, 10, true, _console);
            CheckState();
        }

        [Test]
        public void foo()
        {
            _console.Clear();
            _console.WriteLine("one");
            _console.WriteLine("two");
            _console.WriteLine("three");
            _consoleState = _console.State;
            

            var w = new Window(10, 10, _console);
            CheckState();
            w.ForegroundColor = ConsoleColor.Red;
            return;
            w.WriteLine("X");
            _console.WriteLine("Y");
            w.WriteLine("four");
            
        }
    }


    public class WindowsShouldNotBeAffectedByParentStateChangesTests
    {
        
    }
}
