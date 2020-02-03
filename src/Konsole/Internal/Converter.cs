using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole.Internal
{
    public static class Converter
    {
        public static string BytesToSize(this long bytes)
        {
            if (bytes < Math.Pow(2, 10)) return $"{bytes} bytes";
            if (bytes < Math.Pow(2, 20)) return $"{(bytes >> 10)} KB";
            if (bytes < Math.Pow(2, 30)) return $"{(bytes >> 20)} MB";
            if (bytes < Math.Pow(2, 40)) return $"{(bytes >> 30)} GB";
            return $"{(bytes >> 40)} TB";
        }
    }
}
