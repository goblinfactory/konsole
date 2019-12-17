using Konsole.Platform.Windows;
using System;
using System.Runtime.InteropServices;

namespace Konsole.Platform
{
    public class PlatformStuff : IPlatformStuff
    {
        private WindowsPlatformStuff _platformStuff;
        public PlatformStuff()
        {
            _platformStuff = new WindowsPlatformStuff();
        }

        public void EnableNativeRendering(int width, int height, bool allowClose = true, bool allowMinimize = true)
        {
            EnsureRunningWindows();
            new WindowsPlatformStuff().LockResizing(width, height, allowClose, allowMinimize);
        }

        private static bool? _isWindows;
        public static bool IsWindows
        {
            get
            {
                return (_isWindows ?? (_isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows))).Value;
            }
        }

        internal static void EnsureRunningWindows()
        {
            if (!IsWindows)
            {
                throw new InvalidOperationException($"Only windows platforms are currently supported using the HighSpeedWriter. Please use new Window(), without the high speed writer for Linux and Mac.");
            }
        }

        public void LockResizing(int width, int height, bool allowClose = true, bool allowMinimize = true)
        {
            EnsureRunningWindows();
            _platformStuff.LockResizing(width, height, allowClose, allowMinimize);
        }
    }
}
