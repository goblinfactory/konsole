using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole.Tests.IntegrationTests
{
    class SplitLeftRightFullscreen
    {

        public void PritingLargeFormattedTextBlocksWithEmbeddedCrLfShouldNotBleedAcrossWindowFrames()
        {
            // how to run this test using integration
            // set useRealWindow = true to open a real window instead of a mock console.
            // use process start to open a remote console
            // and talk to it using netMQ or akka?
            var con = new MockConsole(15, 7);
        }
        

    }
}
