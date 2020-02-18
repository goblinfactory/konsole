using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole.Tests.WindowTests
{
    public class ConstructorOverloadTests
    {
        [Test]
        public void AllTheConstructorOverloadsShouldWork()
        {
            var console = new MockConsole();
            Window.HostConsole = console;
            // link to sample programs
        }
    }
}
