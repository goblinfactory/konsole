using Konsole.Internal;
using System;
using System.Threading.Tasks;

namespace Konsole
{

    public enum Display {

        /// <summary>
        /// control is rendered at the absolute X,Y or SX, SY if there is a caption, cursor is left where it was.
        /// </summary>
        Absolute,

        /// <summary>
        /// control is rendered inline at current X,Y or SX, SY if there is a caption, cursor is left at SX + Width
        /// </summary>
        Inline, 

        /// <summary>
        /// control is rendered inline to the right of the previous control and an CRLF is added to the end so that next controls start on left.
        /// </summary>
        InlineEnd,
        Block,
    }
    // Form (controls) lifecycle ....
    // new () ... constructor sets all properties 
    // then calls Refresh -> Render(Inactive, inactiveSstyle) 

    // next you call console.RunForms()
    // does each console need an ID?
    // RunForms ... finds first console that can focus, then finds first "next" input in that console 
    // calls Focus
    //   ... which calls OnEnter (first time)
    //   .... then OnFocus() each time
    // then calls Refresh()
    // then calls XY to see if we need a blinking cursor, then sets Cursor at XY

    // do while key != exit key
    //  control.HandleKey()

    // calls OnLostFocus

    // RunForm handles move key, to move focus between controls
    // 



    // then sends all keystrokes to your control until exit key... 


    // need a test for Activated, so that we can make sure that controls are only initialised once, e.g. fetching cached lookup data/ reading from disk.
    // need a sample that shows how to use that!


    public abstract class Control<T, TItem>
    {
        private int _captionWidth;
        private string _caption;

        public int Width { get; }
        public int Height { get; }
        public int X { get; }
        public int Y { get; }

        /// <summary>
        /// start X position of the input control, without the caption
        /// </summary>
        public int SX { get; }

        /// <summary>
        /// start Y position of the input control, without the caption
        /// </summary>
        public int SY { get; }

        /// <summary>
        /// Enter event happens when the control gets focus for the first time. 
        /// </summary>
        /// <remarks>Useful place to put lazy load, or initialisers you dont want to put in constructors.</remarks>
        public Action<Control<T, TItem>> OnEnter = (c) => { };

        /// <summary>
        /// every time the control gets focus for editing, even the first time
        /// </summary>
        /// <remarks>Is triggered by .Focus()</remarks>
        public Action<Control<T, TItem>> OnGotFocus = (c) => { };

        /// <summary>
        /// Occurs when the input focus leaves the control. 
        /// </summary>
        /// <remarks>Is triggered by Blur()</remarks>
        public Action<Control<T, TItem>> OnLostFocus = (c) => { };

        /// <summary>
        /// returns the cursor position to set the cursor to when this control has focus. Return null if the control does not require a blinking cursor. e.g. listview (no cursor) vs (input box, has cursor)
        /// </summary>
        public abstract XY? Cursor { get; }

        // not doing validation here, can create a validating control that derives from this!

        public bool HasCaption { get; }
        public class ControlSettings
        {
            public int? x { get; set; }
            public int? y { get; set; }

            public int? Width { get; set; }

            public int? Height { get; set; }
            public IConsole console { get; set; }
            public string caption { get; set; }
            public int? captionWidth { get; set; }

        }

        public Control(IConsole console, int? x, int? y, string caption, int? captionWidth, int? width, int? height)
        {
            lock(Window._locker)
            {
                _captionWidth = captionWidth ?? caption?.Length ?? 0;
                HasCaption = _captionWidth != 0;
                _caption = caption;
                _console = console ?? Window._HostConsole;
            }

            // layout
            // Absolute  : fixed X and Y, and does not impact console's cursor X or Y
            // Inline    : uses current Y, and increments X
            // Block     : start on newline, and adds newline to end of current line.

            // caption position
            // Left, Top, Right, Bottom

            // needs a style, or styletheme and-or foreground/background, and inherits from console if not provided
        }


        public int CaptionWidth
        {
            get
            {
                return _captionWidth;
            }
            set
            {
                _captionWidth = value;
                Refresh();
            }
        }

        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                lock (Window._locker)
                {
                    _caption = value.FixLeft(CaptionWidth);
                    Refresh();
                }

            }
        }

        public virtual void Refresh()
        {
            lock (Window._locker)
            {
                if (!_layoutSuspended)
                {
                    _console.DoCommand(_console, Render);
                }
            }
        }

        /// <summary>
        /// render the control based on all the property settings. Override this method and implement all the code necessary to render the control based on it's current state
        /// </summary>
        /// <remarks>
        /// Do not call this property yourself, simply set IsDirty to true and your control will be renderded ONCE after it's finished handling any events or if it's focus changes.
        /// If your control responds to non keyboard events then call Refresh, which will check to see if layout has been suspended 
        /// if layout has been suspended then render will not be called until Layout resumes with a call to ResumeLayout.
        /// </remarks>
        public abstract void Render(ControlStatus status, Style style);

        public void Render()
        {
            if (!LayoutSuspended)
            {
                Render(Status, Style);
            }
        }

        public void Refresh(ControlStatus status)
        {
            lock (Window._locker)
            {
                Status = status;
            }
        }
        
        /// <summary>
        /// helpful automation method. Especially useful for automated testing. This delegates to HandleKeyPress.
        /// </summary>
        /// <param name="keys"></param>
        public void HandleKeyPresses(string keys, bool shift, bool alt, bool control)
        {
            foreach(char c in keys) {
                var press = c.ToKeypress(shift, alt, control);
                HandleKeyPress(press, c);
            }
        }

        /// <summary>
        /// helpful automation method. Especially useful for automated testing. This delegates to HandleKeyPress.
        /// </summary>
        /// <param name="keys"></param>
        public void HandleKeyPresses(string keys)
        {
            HandleKeyPresses(keys, false, false, false);
        }

        private bool _layoutSuspended = false;
        public void SuspendLayout()
        {
            _layoutSuspended = true;
        }

        public bool LayoutSuspended
        {
            get { return _layoutSuspended; }
        }

        public void ResumeLayout()
        {
            _layoutSuspended = false;
            Render();
        }

        /// <summary>
        /// handle keypress
        /// </summary>
        /// <returns>true if you handled the keypress, false if you did not and it needs to bubble up.</returns>
        public abstract (bool isDirty, bool handled) HandleKeyPress(ConsoleKeyInfo info, char key);

        private bool activated = false;

        void Focus()
        {
            lock (Window._locker)
            {
                if (Status == ControlStatus.Active) return;
                if (!activated)
                {
                    activated = true;
                    OnEnter(this);
                }
                OnGotFocus(this);
            }
        }

        public abstract TItem Value { get; }

        public void Blur()
        {
            // if nothing has changed
            if (Status != ControlStatus.Active) return;

            lock (Window._locker)
            {
                Status = ControlStatus.Inactive;
                OnLostFocus(this);
                Refresh();
            }
        }

        protected readonly IConsole _console;

        private ControlStatus _status = ControlStatus.Active;
        public ControlStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != Status)
                {
                    _status = value;
                    Refresh();
                }
            }
        }
        public StyleTheme Theme { get; set; } = null;

        public Style Style
        {
            get
            {
                return Theme?.GetActive(Status) ?? _console?.Theme.GetActive(Status) ?? Style.Default;
            }
        }
    }
}
