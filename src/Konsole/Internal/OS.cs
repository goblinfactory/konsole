using System;
using System.Runtime.InteropServices;

namespace Konsole.Internal
{
    internal class OS
    {
        static OS()
        {
            isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }
        
        public static bool isWindows;
    }
}
