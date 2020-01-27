using System;
using System.IO;
using static System.ConsoleColor;


namespace Konsole
{
    public class DirectoryListView : ListView<FileOrDirectory>
    {
        public string Path { get; set; }
        public string FileSearchPattern { get; set; } = null;
        public string DirSearchPattern { get; set; } = null;
        Func<FileInfo, bool> FilterFiles = null;
        Func<DirectoryInfo, bool> FilterDirs = null;

        public class DirTheme
        {
            public Colors Directories = new Colors(Green, Black);
        }

        public DirTheme StyleExtras = new DirTheme();

        public DirectoryListView(
            IConsole console, 
            string path, 
            string fileSearchPattern = "*.*", 
            string dirSearchPattern = "*.*", 
            Func<FileInfo, bool> filterFiles = null, 
            Func<DirectoryInfo, bool> filterDirs = null) 
                : base(console, getData: null, getRow: null, ("Name", 0), ("Size", 12), ("Modified", 17)
        )
        {
            Path = path;
            // this implementation relies on the base class never using the source until refresh is called.
            FileSearchPattern = fileSearchPattern;
            DirSearchPattern = dirSearchPattern;
            FilterFiles = filterFiles;
            FilterDirs = filterDirs;
            //TODO: inject FileOrDirectory dependancy, create a dependencies optional last, as well as overload where the file types are passed in.
            _getData = () => FileOrDirectory.ReadDir(new DirectoryInfo(Path), FileSearchPattern, DirSearchPattern, FilterFiles, FilterDirs);
            _getRow = (i) => new[] {
                $"{Slash(i.Item)}{i.Name}",
                BytesToSize(i.Size),
                i.LastModifiedUTC.ToString("dd MMM yyyy hh:mm")
            };
            
            BusinessRuleColors = (item, column) =>
            {
                if (column != 1) return null;
                if (item.Item is DirectoryInfo) return StyleExtras.Directories;
                return null;
            };
        }

        private static string Slash(object i)
        {
            return i is DirectoryInfo ? "/" : "";
        }
    }
}
