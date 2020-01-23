using System.Runtime.InteropServices;
using Konsole;

namespace Konsole.Platform.Windows
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CharAndColor
    {
        [FieldOffset(0)] public CharInfo Char;
        [FieldOffset(2)] public short Attributes;

    }

    public static class ColorExtensions
    {
        public static CharAndColor Set(this Colors src, char c)
        {
            var @char = new CharAndColor() {
                Char = new CharInfo() { UnicodeChar = c, Attributes = src.ToAttributes() },
                Attributes = src.ToAttributes()
            };            
            return @char;
        }

        public static short ToAttributes(this Colors src)
        {
            int backAtt = (int)src.Foreground + (short)((int)src.Background << 4);
            return (short)backAtt;
        }

    }
}