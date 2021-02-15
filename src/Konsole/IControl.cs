using System;

namespace Konsole
{
    public interface IControl
    {
        string Caption { get; set; }
        int CaptionWidth { get; set; }
        XY? Cursor { get; }
        bool HasCaption { get; }
        int Height { get; }
        bool LayoutSuspended { get; }
        ControlStatus Status { get; set; }
        Style Style { get; }
        int SX { get; }
        int SY { get; }
        StyleTheme Theme { get; set; }
        int Width { get; }
        int X { get; }
        int Y { get; }
        void Blur();
        (bool isDirty, bool handled) HandleKeyPress(ConsoleKeyInfo info, char key);
        void HandleKeyPresses(string keys);
        void HandleKeyPresses(string keys, bool shift, bool alt, bool control);
        void Refresh();
        void Refresh(ControlStatus status);
        void Render();
        void ResumeLayout();
        void SuspendLayout();

        /// <summary>
        /// controls will default tab order to 0 indicating it will inherit tab order from the sequence of creation.
        /// Set to null, to not have a tab order, i.e. will be bypassed during tabbing.
        /// </summary>
        int? TabOrder { get; set;  }
    }
}