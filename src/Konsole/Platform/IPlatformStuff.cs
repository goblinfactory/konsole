namespace Konsole.Platform
{
    public interface IPlatformStuff
    {
        void LockResizing(int width, int height, bool allowClose, bool allowMinimize);

        /// <summary>
        /// Enabled platform rendering (currently only windows is supported), and uses the native high speed platform renderer. e.g. in windows that's Kernel32.dll,
        /// also immediately locks resizing, so you do not need to call that afterwards.
        /// </summary>
        void EnableNativeRendering(int width, int height, bool allowClose, bool allowMinimize);
    }
}
