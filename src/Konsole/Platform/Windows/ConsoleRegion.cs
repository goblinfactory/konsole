using System.Runtime.InteropServices;

namespace Konsole.Platform.Windows
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ConsoleRegion
    {
        public short StartX;
        public short StartY;
        public short EndX;
        public short EndY;
        public ConsoleRegion(short startX, short startY, short endX, short endY)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
        }
    }
}