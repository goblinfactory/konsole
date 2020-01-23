using System.Runtime.InteropServices;

namespace Konsole.Platform.Windows
{
    [StructLayout(LayoutKind.Explicit, CharSet=CharSet.Unicode)]
    public struct CharInfo
    {
        [FieldOffset(0)] 
        public char UnicodeChar;
        [FieldOffset(0)]
        public byte AsciiChar;
        [FieldOffset(2)]
        public short Attributes;
    }
}