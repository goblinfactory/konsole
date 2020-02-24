//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Konsole.Tests.InputControlTests
//{
//    public class ConstructorTests
//    {

//        [Test]
//        public void Console_Caption_MaxLength_CaptionWidth_tests()
//        {
//            var console = new MockConsole(30, 5);
//            Window.HostConsole = console;
            
//            // given the console cursor is not at X = 0
//            console.Write("123");

//            // when caption width IS given, then the caption width is used 
//            var name = new Input(console, "name", maxlength: 10, captionWidth: 15);

//            // when no caption width given, then the width of the supplied caption is used
//            var line1 = new Input(console, "line1:", maxlength: 12);
//            var line2 = new Input(console, "line2: ", maxlength: 12);
//            var line3 = new Input(console, "line3:  ", maxlength: 12);

//            // when an input is on  the last line of the screen, do not add lineFeed after control is rendered.

//            var expected = new[] { 
//                "123                           ",
//                "name           [          ]   ",
//                "line1:[            ]          ",
//                "line2: [            ]         ",
//                "line3:  [            ]        "
//            };

//            console.Buffer.ShouldBe(expected);

//            // then the control is rendered using the Inactive status theme
//            var expectedWithColor = new[] {
//                "123                           ",
//                "addr1: [            ]         ",
//                "addr2:   [            ]       ",
//                "                              ",
//                "                              "
//            };

//            console.BufferWithColor.ShouldBe(expectedWithColor);
//        }
//    }
//}
