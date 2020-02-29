using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public class WhenEchoFalse
    {
        public class EchoTrueShould
        {
            [Test]
            public void when_EchoFalse_SHOULD_not_translate_any_writes_to_the_parent()
            {
                Assert.Inconclusive("test needs to be written, this is a copy of When_EchoTrue");
                var parent = new MockConsole(4, 4);
                var window = parent.Open(1, 1, 2, 2);
                window.WriteLine("12");
                window.Write("34");

                var expected = new[]
                {
                "    ",
                " 12 ",
                " 34 ",
                "    "
            };
                Assert.AreEqual(expected, parent.Buffer);
            }
        }
    }
}
