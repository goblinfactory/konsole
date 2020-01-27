using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Konsole.IO
{
    public class FileOrDirectory
    {
        public static IEnumerable<FileOrDirectory> ReadDir(string path)
        {
            var di = new DirectoryInfo(path);
            return ReadDir(di);
        }
        public static IEnumerable<FileOrDirectory> ReadDir(DirectoryInfo di, string filter)
        {
            var files = di.GetFiles();
            var dirs = di.GetDirectories();
            var items = files.Select(f => new FileOrDirectory(f, null)).Concat(dirs.Select(d => new FileOrDirectory(null, d)));
            return items;
        }

        /// <summary>
        /// get the contents of a directory, optionally filtering directories and files. Pass null for either filter to exclude filtering. 
        /// </summary>
        public static IEnumerable<FileOrDirectory> ReadDir(DirectoryInfo di, string fileSearchPattern = "*.*", string dirSearchPattern = "*.*", Func<FileInfo, bool> filterFiles = null, Func<DirectoryInfo, bool> filterDirs = null)
        {
            var allfiles = di.GetFiles(fileSearchPattern);
            var files = filterFiles == null ? allfiles : allfiles.Where(filterFiles);

            var allDirs = di.GetDirectories(dirSearchPattern);
            var dirs = filterDirs == null ? allDirs : allDirs.Where(filterDirs);

            var items = files.Select(f => new FileOrDirectory(f, null)).Concat(dirs.Select(d => new FileOrDirectory(null, d)));
            return items;
        }

        public FileOrDirectory(FileInfo file, DirectoryInfo dir)
        {
            if (file == null && dir == null) throw new ArgumentNullException("Please supply either a file or a directory.");
            if (file != null)
            {
                Name = file.Name;
                Size = file.Length;
                Item = file;
                LastModifiedUTC = file.LastWriteTimeUtc;
            }
            else
            {
                Name = dir.Name;
                Size = dir.GetFiles("*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t.FullName).Length));
                Item = dir;
                LastModifiedUTC = dir.LastWriteTimeUtc;
            }
        }

        public object Item { get; }
        public String Name { get; }
        public long Size { get; }
        public DateTime LastModifiedUTC { get; }
    }
}

