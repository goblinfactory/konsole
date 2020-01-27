namespace Konsole.Internal
{
    public static class StringExtensions
    {
        /// <summary>
        /// Align to the left, padd with spaces, and truncate any oveflow characters.
        /// </summary>
        public static string FixLeft(this string src, int len)
        {
            int slen = src.Length;
            if (slen > len) return src.Substring(0, len);
            return string.Format("{0}{1}", src, new string(' ', len-slen));
        }

        /// <summary>
        /// Align to the left, padd with spaces, and truncate any oveflow characters.
        /// </summary>
        public static string FixCenter(this string src, int len)
        {
            int slen = src.Length;
            if (slen + 1> len) return src.Substring(0, len);
            int pad1 = (len - slen) / 2;
            int pad2 = len - (pad1 + slen);
            return string.Format("{0}{1}{2}", new string(' ', pad1), src, new string(' ', pad2));
        }

    }
}