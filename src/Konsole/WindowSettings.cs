using static Konsole.ControlStatus;

namespace Konsole
{
    public class WindowSettings
    {
        public WindowSettings() { }
        public WindowSettings(WindowSettings settings)
        {
            if (settings != null)
            {
                Status = settings.Status;
                Title = settings.Title;
                SX = settings.SX;
                SY = settings.SY;
                Width = settings.Width;
                Height = settings.Height;
                Theme = settings.Theme;
            }
        }

        /// <summary>
        /// Starting X position of window. 
        /// </summary>
        public int SX { get; set; } = 0;

        /// <summary>
        /// starting Y position of the window. Leave null to use the current CursorTop position.
        /// </summary>
        public int? SY { get; set; } = null;

        /// <summary>
        /// Left padding, to open an inline window slightly offset from the left by a few characters. Only applies to OpenInline.
        /// </summary>
        public int PadLeft { get; set; } = 0;

        /// <summary>
        /// Window background color is transparent until you start writing then will print using the configured fore and background color i.e. initial window will not clear the background
        /// </summary>
        public bool Transparent { get; set; } = false;

        /// <summary>
        /// When true, printing off the right hand edge of the screen is clipped, no wrapping.
        /// </summary>
        public bool Clipping { get; set; } = false;
        
        /// <summary>
        /// Printing off the bottom of the window causes the window to scroll. 
        /// </summary>
        public bool Scrolling { get; set; } = true;   
        
        /// <summary>
        /// defines the active status of the window, either active, inactive or disabled. this status is used to determining the currently active style fom the style theme.
        /// </summary>
        public ControlStatus Status { get; set; } = Active;

        /// <summary>
        /// The title of the screen. Will be centered and at the top of the window.
        /// </summary>
        public string Title { get; set; } = null;
        public bool hasTitle => Title != null;

        /// <summary>
        /// The width of the screen. Leave black to use whole width.
        /// </summary>
        public int? Width { get; set; } = null;

        /// <summary>
        /// The height of the window. Leave black to use the whole height, or if creating an inline window, will be capped by the remaining lines in the screen. (this is a known issue in OSX at the moment that you can cannot cause the screen to scroll to accommodate inline windows to grow the buffer. Can be fixed, check for a later release.)
        /// </summary>
        public int? Height { get; set; } = null;

        /// <summary>
        /// The style of the body, title, line, selected item, headings, linkthickness, both foreground and background colors, for the active, inactive and disabled states of the window.
        /// </summary>
        public StyleTheme Theme { get; set; } = null;

        /// <summary>
        /// Set to false if you want all the behavior of a window without actually rendering (echo-ing) to the final operating system console. This is a system setting and is used by MockConsole. Default is true.
        /// </summary>
        public bool _echo { get; set; } = true;

        /// <summary>
        /// Internal use only. do not set this. This value is set by the extension method of the 
        /// </summary>
        public IConsole _parentWindow { get; set; } = null;
    }
}
