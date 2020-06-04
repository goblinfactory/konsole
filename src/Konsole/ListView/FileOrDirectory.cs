using System;
using System.IO;
using System.Linq;
using Konsole.Internal;
using static Konsole.DirectoryListView;

namespace Konsole
{
    public class FileOrDirectory
    {
        public static string DefaultDateTimeFormat = "dd MMM yyyy hh:mm"; 
        public enum Me {  File, Directory }
        
        /// <summary>
        /// file or directory extension
        /// </summary>
        public string Ext { get; }

        /// <summary>
        /// full name including path
        /// </summary>
        public string FullName { get; }
        
        /// <summary>
        /// name only, no path
        /// </summary>
        public String Name { get; }
        
        /// <summary>
        /// size in bytes
        /// </summary>
        public long Size { get; }

        /// <summary>
        /// display text for size, in bytes, Kb, Mb, Terabytes etc.
        /// </summary>
        public string SizeText { get; }
        public DateTime LastModifiedUTC { get; }

        /// <summary>
        /// 17 char display text
        /// </summary>
        /// <example>25 May 2020 21:41</example>
        public string LastModifiedText { get; }

        public Me Is { get; }
        public FileOrDirectory(FileInfo file, DirectoryInfo dir)
        {
            if (file == null && dir == null) throw new ArgumentNullException("Please supply either a file or a directory.");
            if (file != null)
            {
                Ext = file.Extension;
                FullName = file.FullName;
                Name = file.Name;
                Size = file.Length;
                SizeText = Size.BytesToSize();
                LastModifiedUTC = file.LastWriteTimeUtc;
                LastModifiedText = DefaultDateTimeFormat;
                Is = Me.File;
            }
            else
            {
                Ext = dir.Extension;
                FullName = dir.FullName;
                Name = dir.Name;
                Size = dir.GetFiles("*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t.FullName).Length));
                SizeText = Size.BytesToSize();
                LastModifiedUTC = dir.LastWriteTimeUtc;
                LastModifiedText = DefaultDateTimeFormat;
                Is = Me.Directory;
            }
        }

        public FileOrDirectory(Me @is, string ext, string fullName, string name, long size, DateTime lastModifiedUTC)
        {
            Ext = ext;
            FullName = fullName;
            Name = name;
            Size = size;
            SizeText = Size.BytesToSize();
            LastModifiedUTC = lastModifiedUTC;
            LastModifiedText = DefaultDateTimeFormat;
            Is = @is;
        }

        public static FileOrDirectory[] ReadDir(DirectoryInfo path, DirectorySortBy sort, string fileSearchPattern = "*", string dirSearchPattern = "*", Func<FileInfo, bool> filterFiles = null, Func<DirectoryInfo, bool> filterDirs = null, bool recursive = false)
        {
            var searchOptions = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var allFiles = path.GetFiles(fileSearchPattern ?? "*", searchOptions);
            var files = (filterFiles == null ? allFiles : allFiles.Where(f => filterFiles(f))).Select(f => new FileOrDirectory(f, null));
            var allDirs = path.GetDirectories(dirSearchPattern ?? "*", searchOptions);
            var dirs = (filterDirs == null ? allDirs : allDirs.Where(d => filterDirs(d))).Select(d => new FileOrDirectory(null, d));
            var all = files.Concat(dirs);
            switch (sort)
            {
                case DirectorySortBy.Size:
                    return all.OrderBy(o => o.Size).ToArray();
                case DirectorySortBy.Name:
                    return all.OrderBy(o => o.Name).ToArray();
                case DirectorySortBy.DirSize:
                    return all.OrderBy(o => o.Is.ToString()).ThenBy(o=> o.Size).ToArray();
                case DirectorySortBy.DirName:
                    return all.OrderBy(o => o.Is.ToString()).ThenBy(o => o.Name).ToArray();
                case DirectorySortBy.FileSize:
                    return all.OrderByDescending(o => o.Is.ToString()).ThenBy(o => o.Size).ToArray();
                case DirectorySortBy.FileName:
                    return all.OrderByDescending(o => o.Is.ToString()).ThenBy(o => o.Name).ToArray();
                default:
                    throw new ArgumentOutOfRangeException(nameof(sort), sort.ToString());
            }
        }
    }
}

