using System;
using System.Collections.Generic;
using System.Text;
using Konsole.Internal;
using static Konsole.Samples.ControlSample;

namespace Konsole.Tests.Control
{
    class ControlTests
    {

        public void caption_width_provided_tests()
        {
            var console = new MockConsole(50, 5);
            var t1 = new MyInput(console, "Name", 10);
            var t2 = new MyInput(console, "Initials", 10);
            var t3 = new MyInput(console, "Last Name", 10);
            t1.HandleKeyPresses("cats");
            t2.HandleKeyPresses("foo");
            var expected = new[]
            {
                "Name      [cats      ]                            ",
                "Initials  [foo       ]                            ",
                "Last Name [          ]                            ",
                "                                                  ",
                "                                                  ",
            };
            console.Buffer.ShouldBe(expected);
        }

        public void caption_width_not_provided_tests()
        {
            var console = new MockConsole(50, 5);
            var t1 = new MyInput(console, "Name : ");
            var t2 = new MyInput(console, "Initials : ");
            var t3 = new MyInput(console, "Last Name : ");
            var expected = new[]
            {
                "Name : [          ]                               ",
                "Initials : [          ]                           ",
                "Last Name : [          ]                          ",
                "                                                  ",
                "                                                  ",
            };
            console.Buffer.ShouldBe(expected);
        }

        public void default_layout_always_starts_and_ends_with_newLine()
        {
            var console = new MockConsole(50, 5);
            console.Write("123");
            var t1 = new MyInput(console, "Name : ", 10);
            var t2 = new MyInput(console, "Initials : ", 10);
            var t3 = new MyInput(console, "Last Name : ", 10);
            var expected = new[]
            {
                "123                                               ",
                "Name : [          ]                               ",
                "Initials : [          ]                           ",
                "Last Name : [          ]                          ",
                "                                                  ",
            };
            console.Buffer.ShouldBe(expected);
        }

    }
}
