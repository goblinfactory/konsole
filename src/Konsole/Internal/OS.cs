using System;
using System.Runtime.InteropServices;

namespace Konsole.Internal
{
    public static class OS
    {
        private static bool? _isWindows;
        ///<summary>
        ///allow us to override this in unit tests.
        ///</summary>
        public static Func<bool> IsWindows = () =>
        {
            return _isWindows ?? (_isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows)).Value;
        };

        private static bool? _isOSX;
        ///<summary>
        ///allow us to override this in unit tests.
        ///</summary>
        public static Func<bool> IsOSX = () =>
        {
            return _isOSX ?? (_isOSX = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX)).Value;
        };

    }
}
