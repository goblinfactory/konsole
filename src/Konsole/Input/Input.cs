//using Konsole;
//using Konsole.Internal;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Konsole
//{
//    //public enum Display {
//    //    /// <summary>
//    //    /// no CR after the element
//    //    /// </summary>
//    //    Inline, 
//    //    /// <summary>
//    //    /// CR after the element
//    //    /// </summary>
//    //    Block, 
//    //}
    
//    public class InputSettings
//    {
//        public string Value { get; set; }
//        public int? SelectionStart { get; set; }
//        public int? SelectionLength { get; set; }
//        public int? MaxLength { get; set; }
//        public string DefaultValue { get; set; }

//        /// <summary>
//        /// set to null for inline (left to right) inputs
//        /// </summary>
//        public int? CaptionWidth { get; set; }

//        //public CaptionPlacement Placement { get; set; } = CaptionPlacement.Left;

//        /// <summary>
//        /// Whether to draw a line seperator under the control 
//        /// </summary>
//        public bool Underline { get; set; } = false;
//    }

//    public class Input : Control<Input, string>
//    {
//        public Input(IConsole console, InputSettings settings) : base(console ?? Window.HostConsole)
//        {

//            lock (Window._locker)
//            {

//            }
//        }

//        public Input(int maxlength) : this(Window.HostConsole, new InputSettings { MaxLength = maxlength })
//        {
//        }

//        public Input(string caption, int maxlength) : this(Window.HostConsole, new InputSettings { Caption = caption, MaxLength = maxlength })
//        {
//        }

//        public Input(IConsole console, int maxlength) : this(console, new InputSettings { MaxLength = maxlength })
//        {
//        }
//        public Input(IConsole console, int x, int y, int maxlength) : this(console, new InputSettings { MaxLength = maxlength, X = x, Y = y })
//        {

//        }

//        public Input(string caption, int maxlength, int captionWidth) : this(Window.HostConsole, new InputSettings { Caption = caption, MaxLength = maxlength, CaptionWidth = captionWidth })
//        {
//        }

//        public Input(IConsole console, string caption, int maxlength) : this(console, new InputSettings { Caption = caption, MaxLength = maxlength })
//        {
//        }

//        public Input(IConsole console, string caption, int maxlength, int captionWidth) : this(console, new InputSettings { Caption = caption, MaxLength = maxlength, CaptionWidth = captionWidth })
//        {
//        }

//        private string _value;

//        private int _cursorPosition = 0;
        

//        public int MaxLength { get; }

//        /// <summary>
//        /// return where we want the flashing cursor to be set when this control has focus
//        /// </summary>
//        public override XY? Cursor => new XY(_x + _cursorPosition, _y);

//        public override string Value => throw new NotImplementedException();

//        public override void Render()
//        {
//            // start all controls as inactive
//            switch (Status)
//            {
//                case ControlStatus.Active:

//                    break;

//                case ControlStatus.Undefined:
//                case ControlStatus.Inactive:
//                    break;
//                case ControlStatus.Disabled:
//                    break;
//            }
//        }

//        public override bool HandleKeyPress(ConsoleKeyInfo info, char key)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
