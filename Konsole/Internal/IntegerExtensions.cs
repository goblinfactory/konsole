using System.Linq;

namespace Konsole.Internal
{
    public static class IntegerExtensions
    {
        public static int Max(this int src, int max)
        {
            return src > max ? max : src;
        }

        public static int Min(this int src, int min1, int min2)
        {
            return new int[] { src, min1, min2 }.Min();
        }

        public static int Min(this int src, int min1)
        {
            return new int[] { src, min1 }.Min();
        }

    }
}
