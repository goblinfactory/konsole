using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Konsole.Samples;

namespace Konsole.Tests.Slow
{
    public class MenuConcurrencyTests
    {
        private List<Task> _tasks = new List<Task>();

        [Test]
        public void SeperateThreadsForMenuAndTwoWindows()
        {
            var console = new MockConsole();
            Window.HostConsole = console;
            MenuConcurrencyTestDemo.SeperateThreadsForMenuAndTwoWindows(console, 8000);

            var expected = new[]{
                "line 1      [4005]                 ┌─────────────── client ───────────────┐  ┌─────────────── server ───────────────┐   ",
                "line 2                             │cats 7978                             │  │dogs 7978                             │   ",
                "          Test samples             │cats 7979                             │  │dogs 7979                             │   ",
                "                                   │cats 7980                             │  │dogs 7980                             │   ",
                " 1. cats                           │cats 7981                             │  │dogs 7981                             │   ",
                " 2. dogs                           │cats 7982                             │  │dogs 7982                             │   ",
                " 3. item 1                         │cats 7983                             │  │dogs 7983                             │   ",
                " 4. item 2                         │cats 7984                             │  │dogs 7984                             │   ",
                " 5. item 3                         │cats 7985                             │  │dogs 7985                             │   ",
                " 6. item 4                         │cats 7986                             │  │dogs 7986                             │   ",
                " 7. item 5                         │cats 7987                             │  │dogs 7987                             │   ",
                "                                   │cats 7988                             │  │dogs 7988                             │   ",
                "line 3                             │cats 7989                             │  │dogs 7989                             │   ",
                "                                   │cats 7990                             │  │dogs 7990                             │   ",
                "                                   │cats 7991                             │  │dogs 7991                             │   ",
                "                                   │cats 7992                             │  │dogs 7992                             │   ",
                "                                   │cats 7993                             │  │dogs 7993                             │   ",
                "                                   │cats 7994                             │  │dogs 7994                             │   ",
                "                                   │cats 7995                             │  │dogs 7995                             │   ",
                "                                   │cats 7996                             │  │dogs 7996                             │   ",
                "                                   │cats 7997                             │  │dogs 7997                             │   ",
                "                                   │cats 7998                             │  │dogs 7998                             │   ",
                "                                   │cats 7999                             │  │dogs 7999                             │   ",
                "                                   │cats 8000                             │  │dogs 8000                             │   ",
                "                                   └──────────────────────────────────────┘  └──────────────────────────────────────┘   ",
                };

            var actual = console.BufferWritten;
            actual.ShouldBe(expected);
        }
    }
}
