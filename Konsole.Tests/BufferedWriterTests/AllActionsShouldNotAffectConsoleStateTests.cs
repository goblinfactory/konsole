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
            _consoleState = _console.GetState();
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


        private void CheckState()
        {
            var actual = _console.GetState();
            actual.ShouldBeEquivalentTo(_consoleState);
        }
    }

    public class WindowsShouldNotBeAffectedByParentStateChangesTests
    {
        
    }
}
