using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
        public class CursorTopShould
        {
            [Test]
            public void setting_cursor_top_to_a_previously_written_line_should_allow_us_to_overwrite_previously_written_lines()
            {
                var console = new MockConsole(80, 20);
                console.WriteLine("line 0");
                console.WriteLine("line 1");
                console.WriteLine("line 2");
                console.CursorTop = 1;
                console.WriteLine("new line 1");
                var expected = new[] {
                    "line 0",
                    "new line 1",
                    "line 2"
                };
                Assert.That(console.BufferWrittenTrimmed, Is.EqualTo(expected));
            }

        }
}