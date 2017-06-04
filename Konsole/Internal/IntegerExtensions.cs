namespace Konsole.Internal
{
    public static class IntegerExtensions
    {
        public static int Max(this int src, int max)
        {
            return src > max ? max : src;
        }
    }
}
