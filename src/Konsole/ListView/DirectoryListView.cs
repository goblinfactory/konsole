//using System;
//using System.Collections.Generic;
//using System.IO;
//using static System.ConsoleColor;


//namespace Konsole
//{
//    public interface IFileOrDirectoryProvider
//    {
//        IEnumerable<FileOrDirectory> ReadDir(
//            DirectoryInfo di, 
//            string fileSearchPattern = "*.*", 
//            string dirSearchPattern = "*.*", 
//            Func<FileInfo, bool> filterFiles = null, 
//            Func<DirectoryInfo, bool> filterDirs = null);

//        IEnumerable<FileOrDirectory> ReadDir(string path);
//    }


//    public class DirectoryListView : ListView<FileOrDirectory>
//    {

        
//        public class Settings
//        {
//            public IConsole Console { get; set; } = null;
//            public string Path { get; set; } = null;
//            public string FileSearchPattern { get; set; } = null;
//            public string DirSearchPattern { get; set; } = null;
//            public Func<FileInfo, bool> FilterFiles = null;
//            public Func<DirectoryInfo, bool> FilterDirs = null;
//            public Func<FileOrDirectory[]> Reader = null;
//            public Func<FileOrDirectory, int, Colors> BusinessRuleColors = (item, columnNo) => null;
//            public Theme Theme { get; set; } = new ListView<FileOrDirectory>.Theme();
//            public DirTheme StyleExtras { get; set; } = new DirTheme();
//        }

//        public class DirTheme
//        {
//            public Colors Directories = new Colors(Green, Black);
//        }

//        public 

//        public DirectoryListView(Settings settings) : base(settings.Console, getData: null, getRow: null, ("Name", 0), ("Size", 12), ("Modified", 17))
//        {
//            settings.Column1 = settings.Column1 ?? new ColumnSetting("Name", 0, Colors.WhiteOnBlack, true);
//            public ColumnSetting Column2 = new ColumnSetting("Size", 12, Colors.WhiteOnBlack, true);
//            public ColumnSetting Column3 = new ColumnSetting("Modified", 17, Colors.WhiteOnBlack, true);

//        Path = path;
//            FileSearchPattern = fileSearchPattern;
//            DirSearchPattern = dirSearchPattern;
//            FilterFiles = filterFiles;
//            FilterDirs = filterDirs;
//            //TODO: inject FileOrDirectory dependancy, create a dependencies optional last, as well as overload where the file types are passed in.
//            _getData = () => FileOrDirectory.ReadDir(new DirectoryInfo(Path), FileSearchPattern, DirSearchPattern, FilterFiles, FilterDirs);
//            _getRow = (i) => new[] {
//                $"{Slash(i.Item)}{i.Name}",
//                BytesToSize(i.Size),
//                i.LastModifiedUTC.ToString("dd MMM yyyy hh:mm")
//            };
            
//            BusinessRuleColors = (item, column) =>
//            {
//                if (column != 1) return null;
//                if (item.Item is DirectoryInfo) return StyleExtras.Directories;
//                return null;
//            };
//        }

//        private static string Slash(object i)
//        {
//            return i is DirectoryInfo ? "/" : "";
//        }
//    }
//}
