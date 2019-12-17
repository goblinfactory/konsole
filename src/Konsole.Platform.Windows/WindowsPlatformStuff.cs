using System;
using System.Runtime.InteropServices;

namespace Konsole.Platform.Windows
{
    // all the files in the Windows folder may have to be made into a DLL that get's referenced. (loaded) at runtime? (need some thought on this.)
    public class WindowsPlatformStuff
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        private static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();


        public void LockResizing(int width, int height, bool allowClose = true, bool allowMinimize = true)
        {
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);

            if (handle != IntPtr.Zero)
            {
                if(!allowClose)
                {
                    DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
                }
                
                if(!allowMinimize)
                {
                    DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                }
                DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
                DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }
        }
    }
}
