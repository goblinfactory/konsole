using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using static Konsole.FileOrDirectory;
using static System.ConsoleColor;


namespace Konsole
{
    public interface IFileOrDirectoryProvider
    {
        IEnumerable<FileOrDirectory> ReadDir(
            DirectoryInfo di,
            string fileSearchPattern = "*.*",
            string dirSearchPattern = "*.*",
            Func<FileInfo, bool> filterFiles = null,
            Func<DirectoryInfo, bool> filterDirs = null);

        IEnumerable<FileOrDirectory> ReadDir(string path);
    }

    public enum DirectorySortBy { Size, Name, DirSize, DirName, FileSize, FileName }

    public class DirectoryListView : ListView<FileOrDirectory>
    {
        public Colors DirColors { get; set; } = new Colors(Green, Black);
        public DirectoryListView(IConsole console, string path) : this(console, path, DirectorySortBy.FileSize, null, null, false, null, null) { }
        public DirectoryListView(string path) : this(Window.HostConsole, path, DirectorySortBy.FileSize, null, null, false, null, null) { }
        public DirectoryListView(
            IConsole console,
            string path,
            DirectorySortBy sort,
            string fileSearchPattern,
            string dirSearchPattern,
            bool recursive,
            Func<FileInfo, bool> filterFiles,
            Func<DirectoryInfo, bool> filterDirs
            ) : base(
            console,
            getData: () => FileOrDirectory.ReadDir(new DirectoryInfo(path), sort, fileSearchPattern, dirSearchPattern, filterFiles, filterDirs, recursive),
            getRow: (i) => new[] {
                $"{(i.Is == Me.Directory ? "/" : "")}{i.Name}",
                i.SizeText,
                i.LastModifiedUTC.ToString("dd MMM yyyy hh:mm")
            },
            new Column("Name", 0), 
            new Column("Size", 12), 
            new Column("Modified", 17))
        {
            BusinessRuleColors = (item, column) =>
                {
                    if (column != 1) return null;
                    if (item.Is == FileOrDirectory.Me.Directory) return DirColors;
                    return null;
                };
        }
    }
}
