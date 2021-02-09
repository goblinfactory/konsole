using System.Collections.Generic;
using System.Linq;

namespace Konsole.Internal
{
    public static class StringExtensions
    {
        internal static bool ContainsEncodedCrLf(this string src)
        {
            if (_crlfs.Any(c => src.Contains(c))) return true;
            return false;
        }
        private static string[] _crlfs = new[]
        {
            "\n\r",
            "\r\n",
            "\n",
            "\r"
        };
        internal static string[] SplitByCrLfOrNull(this string text)
        {
            if (!text.ContainsEncodedCrLf()) return null;
            return text.Split(_crlfs, System.StringSplitOptions.None);
        }
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