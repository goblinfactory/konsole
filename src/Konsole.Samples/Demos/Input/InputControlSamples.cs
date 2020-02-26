//using System;
//using System.Collections.Generic;
//using System.Text;
//using Konsole;

//namespace Konsole.Samples
//{
//    public static class InputControlSamples
//    {
//        // Modes : edit, navigation

//        // keyboard theme [CLASSIC]
//        // ----------------------------------------

//        // INSERT - OVERWRITE mode
//        // when in overwrite mode, when entering new control, highlight the whole text, any typing erases the whole control text?

//        // EDITING MODE
//        // ---------------
//        // tab          : moves forward a control, stay in edit mode, move to end of text, or beginning of non text
//        // shift + tab  : move back a control
//        // arrows       : when in nav mode : move between controls, up, down, left, right
//        // arrows       : when in edit mode : move caret. (cursor position)
//        // shift+left   : move selection start
//        // shift+right  : move selection end
//        // F1           : open built in help for screen
//        // enter        : enter editing mode when in nav mode.
//        // enter        : when in editing mode, finishes editing current control and causes field validation, moves to next, staying in edit mode
//        // escape       : exit edit mode and returns to fast keyboard navigation
//        // control + tab : move to next screen
//        // F1 to F12    : when in navigation mode, moves to console F1 to F12

//        // NAVIGATION MODE
//        // ---------------
//        // tab          : moves forward a control
//        // shift + tab  : move back a control
//        // arrows       : when in nav mode : move between controls, up, down, left, right
//        // arrows       : when in edit mode : move caret. (cursor position)
//        // shift+left   : move selection start
//        // shift+right  : move selection end
//        // F1           : open built in help for screen
//        // enter        : enter editing mode when in nav mode.
//        // enter        : when in editing mode, finishes editing current control and causes field validation, moves to next, staying in edit mode
//        // escape       : exit edit mode and returns to fast keyboard navigation
//        // control + tab : move to next screen
//        // F1 to F12    : when in navigation mode, moves to console F1 to F12


//        public static void Demo(IConsole console)
//        {

//            // on creation the control needs to be rendered as inactive, only when it's activated by the controlling window
//            // must it be re-rendered using "active" style. 

//            // advanced settings can 
//            var client = console.SplitLeft("client", ConsoleKey.F1);
//            var server = console.SplitRight("server", ConsoleKey.F2);

//            // setting hotkey causes the hotkey to be displayed in the frame and binds the key to switch the active window processing keystrokes

//            SetupClientPage(client);
//            SetupServerPage(server);

//            //window.HandleIO(new RunSettings { ExitKey = ConsoleKey.Escape });
//        }

//        private static void SetupClientPage(IConsole client)
//        {
//            client.Write("this write moves the cursor x off from the left a bit so I can make sure non inline controls start on fresh newline.");

//            var address1 = new Input(client, "Address 1 ", 60);
//            var address2 = new Input(client, "Address 2 ", maxlength: 60);
//            var postal = new Input(client, "postal", maxlength: 8, captionWidth: 10);

//            return;
//            // Address 1 [                                                            ]
//            // Address 2 [                                                            ]
//            // postal    [        ]

//            // setting a maxLength, and DisplayWidth smaller than MaxLength causes text to be scrolled when typing.
//            // TBD var address1 = new Input(client, "Address 1 ", 60);

//            //TODO: set a hintText, hintText to appear in nice soft color! As soon as start typing hintText to go away!

//            var name = new Input(client, "Name, Initials, Surname", 25);
//            var initials = new Input(client, 3);
//            var surname = new Input(client, 25);

//            // default must be the simplest possible, then have the exception to the rule with a simple extension.

//            // no caption means controls flow after each other without a newline. The first control does have a caption but leaves the cursor at the current position
//            //
//            // Name, Initials, Surname   [                       ][   ][                       ]
//            //

//            // set caption width to 0 to make the input flow on from current line.
//            var name2 = new Input(client, "Name 2", 25, 0);
//            var initials2 = new Input(client, "Initials 2", 3, 0);
//            var surname2 = new Input(client, "Surname 2", 25, 0);
//            //
//            // Name 2 [                       ] Initials 2 [   ] Surname 2 [                       ]
//            //


//            //var comment1 = new Input(client, "comment 1", 70, Caption.AboveWithUnderline);
//            //var comment2 = new Input(client, "comment 2", 70, Caption.AboveWithUnderline);
//            //var comment3 = new Input(client, "comment 3", 70, Caption.AboveWithUnderline);

//            // comment1 
//            // [                                                                      ]
//            // ------------------------------------------------------------------------
//            // comment2 
//            // [                                                                      ]
//            // ------------------------------------------------------------------------
//            // comment3 
//            // [                                                                      ]
//            // ------------------------------------------------------------------------

//            // following commands should not interfere with console cursorX or Y
//            client.PrintAt(0, 13, " print at samples     X1     X2     X3     X4     X5");
//            int width = 3;
//            var x1 = new Input(client, 25, 13, width);  // control will take 5 characters space [ + 3 + ] = [   ]
//            var x2 = new Input(client, 32, 13, width); // x, y, width
//            var x3 = new Input(client, 39, 13, width);
//            var x4 = new Input(client, 46, 13, width);
//            var x5 = new Input(client, 53, 13, width);


//            client.WriteLine("");
//            client.WriteLine("press F10 to save and create new record");

//            //TODO: add in some sample controls with placement on right, and below

//            //client.OnKeyPress(ConsoleKey.F10, () => {

//            //    var user = new
//            //    {
//            //        name.Value,
//            //        surname.Value,
//            //        address1.Value,
//            //        address2.Value,
//            //        postal.Value
//            //    };
//            //    CreateNewClient(client);
//            //    client.Reset();
//            //});

//        }

//        private static void SetupServerPage(IConsole server)
//        {
//            return;
//            var port = new Input(server, "server Port", maxlength: 8, captionWidth: 15);
//            var IPAddress = new Input(server, "IP4 Address", maxlength: 12, captionWidth: 15);
//        }


//        private static void CreateNewClient(object client)
//        {

//        }
//    }
//}
