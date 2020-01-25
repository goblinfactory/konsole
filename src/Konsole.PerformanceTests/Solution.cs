using System;
using System.IO;
using System.Linq;

namespace Konsole.PerformanceTests
{
    public static class Solution
    {
        private static string _root = null;
        public static string Root
        {
            get
            {
                static string GetRoot(DirectoryInfo di, string marker)
                {
                    di = di ?? new DirectoryInfo(Environment.CurrentDirectory);
                    if (di.GetFiles(marker).Count() == 1) return di.FullName;
                    if (di.Parent == null) return "NULL";
                    return GetRoot(di.Parent, marker);
                }
                return _root ?? (_root = GetRoot(null, "root.txt"));
            }
        }

        public static string Path(string path)
        {
            return System.IO.Path.Combine(Root, path);
        }

        public static string Path(string path1, string path2)
        {
            return System.IO.Path.Combine(Root, path1, path2);
        }

    }
}
