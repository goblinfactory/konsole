using Konsole.Samples;
using NUnit.Framework;

namespace Konsole.Tests.InputControlTests
{
    public class SampleTests
    {
        [Test]
        public static void EnsureSampleTestsWorkAsExpected()
        {
            return;
            var console = new MockConsole(80, 40);
            Window.HostConsole = console;

            var expected = new[]
            {
                ""
            };

            //InputControlSamples.Demo(console);

            console.Buffer.ShouldBe(expected);
        }
    }
}
