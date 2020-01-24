using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bogus;
using Konsole.Forms;
using User = System.ValueTuple<string, string, string, System.ValueTuple<string, string, string>>;

namespace Konsole.Samples
{
    public static class ListViewSampleFileBrowser
    {
        public class FileOrDirectory
        {
            public static IEnumerable<FileOrDirectory> GetItems(DirectoryInfo di)
            {
                var files = di.GetFiles();
                var dirs = di.GetDirectories();
                var items = files.Select(f => new FileOrDirectory(f, null)).Concat(dirs.Select(d => new FileOrDirectory(null, d)));
                return items;
            }
            public FileOrDirectory(FileInfo file, DirectoryInfo dir)
            {
                if (file == null && dir == null) throw new ArgumentNullException("Please supply either a file or a directory.");
                if(file!=null)
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

        public static void Main(string[] args)
        {
            var window = new Window();
            var left = window.SplitLeft("left");
            var right = window.SplitRight("right");

            var cd = new DirectoryInfo(Environment.CurrentDirectory);
            var dir = cd.Parent.Parent;

            var items = FileOrDirectory.GetItems(dir);
            var listview = new ListView<FileOrDirectory>(left, items, item => new [] { 
                item.Name,                                  
                item.Size.ToString(), 
                item.LastModifiedUTC.ToShortDateString() + " " + item.LastModifiedUTC.ToShortTimeString() 
            }, "Name", "Size", "Modified");

            listview.Refresh();
            
            Console.ReadKey(true);

            // list needs to scroll
            // needs to have a current item
            // needs to highlight items
            // needs to react to arrow keys up and down (to start with)

            // window.Keyboard.OnUp(()=> )
            // window.Keyboard.OnDown(()=> );

            // will need to define exit? to stop
            // window.clear to remove list?

        }


        public class ListViewOptions
        {
            public int[] ColumnWidths { get; set; } // use 0 to indicate a wildcard
            public BoxStyle Theme { get; set; } = new BoxStyle();
            public Colors Selected { get; set; } = Colors.BlackOnWhite;
        }
        public class ListView<T>
        {
            public int selectedItemIndex { get; set; }
            public string[] ColumnNames { get; set; }
            
            private Func<T, string[]> _getColumns;
            private int startRecord = 0;
            private readonly IConsole _console;

            public ListView(IConsole console, IEnumerable<T> src, Func<T, string[]> columns, params string[] columnNames)
            {
                _console = console;
                _getColumns = columns;
                ColumnNames = columnNames;
            }

            public  void Refresh()
            {

            }
        }
    }
}
