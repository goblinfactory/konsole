namespace Konsole
{
    // (K) Konsole shortcut options
    public enum K
    {
        // currently under development
        // ---------------------------
        Transparent, // window background color is transparent until you start writing then will print using the configured fore and background color i.e. initial window will not clear the background
        FullScreen, // window background color is transparent until you start writing then will print using the configured fore and background color i.e. initial window will not clear the background
        Clipping, // printing off the screen is clipped, no scrolling. Clipping is the default behavior for a window.
        Scrolling, // printing off the bottom of the window causes the window to scroll. (cannot be used in conjunction with Clipping) Scrolling is the default window behavior.
    }
}
