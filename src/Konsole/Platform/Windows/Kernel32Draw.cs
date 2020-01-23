using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Konsole.Platform.Windows
{
    //TODO Make this private later.
    internal partial class Kernel32Draw
    {

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern SafeFileHandle CreateFile(
            string consoleFileHandle,
            [MarshalAs(UnmanagedType.U4)] 
            uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] 
            uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] 
            FileMode fileMode,
            [MarshalAs(UnmanagedType.U4)] 
            int flags,
            IntPtr template
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool WriteConsoleOutputW(
          SafeFileHandle consoleFileHandle,
          CharAndColor[] buffer,
          COORD bufferWidthHeight,
          COORD offsetXY,
          ref ConsoleRegion consoleRegion
        );

        internal static SafeFileHandle OpenConsole()
        {
            SafeFileHandle h = CreateFile("CONOUT$", FILE_FLAG_OVERLAPPED, FILE_SHARE_WRITE, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            return h;
        }

        const uint FILE_FLAG_OVERLAPPED = 0x40000000;
        const uint FILE_SHARE_WRITE     = 0x00000002;

        //TODO: work out if STATHread is necessary?
        [STAThread]
        public static void SelfTest()
        {
            SafeFileHandle h = OpenConsole();
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            if (!h.IsInvalid)
            {
                CharAndColor[] buf = new CharAndColor[width * height];
                ConsoleRegion rect = new ConsoleRegion() { StartX = 0, StartY = 0, EndX = (short)width, EndY = (short)height };

                var chars = Encoding.ASCII.GetBytes("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
                var r = new Random(123);
                for(int background = 550; background>= 0; background--)
                {
                    for (short x = 0; x < width; x++)
                    {
                        for (short y = 0; y < height; y++)
                        {
                            int xy = y * width + x;
                            buf[xy].Attributes = SetColors(r.Next(16), 0);
                            buf[xy].Char.AsciiChar = chars[r.Next(chars.Length)];
                        }
                    }
                    // this is the flush, to flush a region of a buffer to the screen
                    bool b = WriteConsoleOutputW(h, buf,
                        new COORD() { X = (short)width, Y = (short)height }, // buffer size
                        new COORD() { X = 0, Y = 0 },  // where on the actual console will we be writing.
                        ref rect);
                }
            }
        }

        private static short SetColors(int foreground, int background)
        {
            return SetColors((short)foreground, (short)background);
        }

        private static short SetColors(short foreground, short background)
        {
            return (short)(foreground + (background << 4));
        }


    }
}