using Goblinfactory.Konsole.Platform;

namespace Konsole
{
    public static class PlatformExtensions
    {
        public static Window LockConsoleResizing(this Window window, bool allowClose = true, bool allowMinimize = true)
        {
            new PlatformStuff().LockResizing(allowClose, allowMinimize);
            return window;
        }

    }
}
