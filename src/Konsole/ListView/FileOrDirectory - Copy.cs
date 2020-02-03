//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace Konsole
//{
//    public class DirectoryReader
//    {
//        private string _path;

//        public class Filter
//        {
//            public string FileSearchPattern { get; set; } = "*.*";
//            public string DirSearchPattern { get; set; } = "*.*";
//            public Func<FileInfo, bool> FilesFilters { get; set; } = null;
//            public Func<DirectoryInfo, bool> DirFilters { get; set; } = null;
//        }

//        public Filter Filters { get; set; } = new Filter();

//        public DirectoryReader(string path)
//        {
//            if (!Directory.Exists(path)) throw new ArgumentOutOfRangeException("path does not exist");
//        }

//        public DirectoryReader(string path, Filter filters)
//        {
            
//        }

//        public FileOrDirectory ReadDir(string path)
//        {
//            var di = new DirectoryInfo(path);
//            return ReadDir(di);
//        }

//        public FileProvider(string path)
//        {

//        }
            
//        public FileOrDirectory[] ReadDir(DirectoryInfo di)
//        {
//            var files = di.GetFiles();
//            var dirs = di.GetDirectories();
//            var items = files.Select(f => new FileOrDirectory(f, null)).Concat(dirs.Select(d => new FileOrDirectory(null, d)));
//            return items.ToArray();
//        }

//        /// <summary>
//        /// get the contents of a directory, optionally filtering directories and files. Pass null for either filter to exclude filtering. 
//        /// </summary>
//        public IEnumerable<FileOrDirectory> ReadDir(DirectoryInfo di, string fileSearchPattern = "*.*", string dirSearchPattern = "*.*", Func<FileInfo, bool> filterFiles = null, Func<DirectoryInfo, bool> filterDirs = null)
//        {
//            var allfiles = di.GetFiles(fileSearchPattern);
//            var files = filterFiles == null ? allfiles : allfiles.Where(filterFiles);

//            var allDirs = di.GetDirectories(dirSearchPattern);
//            var dirs = filterDirs == null ? allDirs : allDirs.Where(filterDirs);

//            var items = files.Select(f => new FileOrDirectory(f, null)).Concat(dirs.Select(d => new FileOrDirectory(null, d)));
//            return items;
//        }
//    }
    
//    public class FileOrDirectory
//    {
//        public FileOrDirectory(FileInfo file, DirectoryInfo dir)
//        {
//            if (file == null && dir == null) throw new ArgumentNullException("Please supply either a file or a directory.");
//            if (file != null)
//            {
//                Name = file.Name;
//                Size = file.Length;
//                Item = file;
//                LastModifiedUTC = file.LastWriteTimeUtc;
//            }
//            else
//            {
//                Name = dir.Name;
//                Size = dir.GetFiles("*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t.FullName).Length));
//                Item = dir;
//                LastModifiedUTC = dir.LastWriteTimeUtc;
//            }
//        }

//        public object Item { get; }
//        public String Name { get; }
//        public long Size { get; }
//        public DateTime LastModifiedUTC { get; }
//    }
//}

