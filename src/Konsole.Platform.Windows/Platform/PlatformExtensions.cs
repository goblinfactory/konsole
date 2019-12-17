
namespace Konsole.Platform
{
    public static class PlatformExtensions
    {
        public static Window LockConsoleResizing(this Window window, int width, int height, bool allowClose = true, bool allowMinimize = true)
        {
            PlatformStuff.EnsureRunningWindows();
            new PlatformStuff().LockResizing(width, height, allowClose, allowMinimize);
            return window;
        }

    }
}
