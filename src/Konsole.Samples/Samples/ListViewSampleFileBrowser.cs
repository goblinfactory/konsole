using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bogus;
using Konsole.Forms;
using Konsole.Internal;
using User = System.ValueTuple<string, string, string, System.ValueTuple<string, string, string>>;

namespace Konsole.Samples
{
    public static class ListViewSampleFileBrowser
    {
        public class FileOrDirectory
        {
            public static IEnumerable<FileOrDirectory> ReadDir(DirectoryInfo di)
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
            var dir = cd.Parent.Parent.Parent;

            IEnumerable<FileOrDirectory> items = FileOrDirectory.ReadDir(dir);

            var listview = new ListView<FileOrDirectory>(left, items, i => new[] {
                i.Name,
                i.Size.ToString(),
                i.LastModifiedUTC.ToString("dd MMM yyyy hh:mm")
            }, ("Name", 0), ("Size", 20), ("Modified", 22)); // get algorithm from SplitRows and SplitColumns

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

        public static int[] GetColumnSizes()
        {
            // if there are no wildcards then size is fixed with balance going to wildcard.
            // if there Bare multiple wildcards, then sizes are fixed with balance equally split between wildcards
            // if all are fixed, then all columns are sized pro-ratio with extra size due to rounding up or down go to any column that suits keep the algorithm simple and deterministic.
            return null;
        }
        

        public class ListViewOptions
        {
            public int[] ColumnWidths { get; set; } // use 0 to indicate a wildcard
            public BoxStyle Theme { get; set; } = new BoxStyle();
            public Colors Selected { get; set; } = Colors.BlackOnWhite;
            public Char ColumnSeperator { get; set; } = '|';
        }
        public class ListView<T>
        {
            public int selectedItemIndex { get; set; }
            public (string name, int width)[] Columns { get; set; }
            
            private Func<T, string[]> _getRow;
            private int startRecord = 0;
            private readonly IConsole _console;
            private readonly IEnumerable<T> _src;

            public ListView(IConsole console, IEnumerable<T> src, 
                Func<T, string[]> getRow, params (string name, int width)[] columns)
            {

                _console = console;
                _src = src;
                _getRow = getRow;
                Columns = columns;
            }

            public  void Refresh()
            {
                int cnt = Columns.Count();
                var columns = GetResizedColumns();
                int width = _console.WindowWidth;
                var colLen = columns.Sum(c => c.width);
                _console.Clear();
                var items = _src.ToArray();
                // todo print heading
                foreach (var item in items)
                {
                    int i = 0;
                    var row = _getRow(item);
                    foreach(var columnText in row)
                    {
                        var column = columns[i];
                        bool lastColumn = (++i == cnt);
                        if(lastColumn)
                        {
                            _console.WriteLine(columnText.FixLeft(column.width));
                        }
                        else
                        {
                            _console.Write(columnText.FixLeft(column.width));
                            _console.Write("│");
                        }
                    }
                }
            }

            /// <summary>
            /// returns the actual size of the columns that are or will be used based on the requested column sizes, and the actual window width.
            /// if the window is too small to accommodate the requested sizes then all the fields are reduced pro ratio to fit the window.
            /// all content will be clipped to fit.
            /// Conversly if the window is larger then the columns are resized proportionately.
            /// </summary>
            /// <returns></returns>
            public (string name, int width)[] GetResizedColumns()
            {
                var items = new List<(string name, int width)>();
                var width = _console.WindowWidth;
                int cnt = Columns.Count();
                int numbBars = cnt - 1;
                int size = width - numbBars;
                int balance = size;
                int requestedSize = Columns.Sum(c => c.width);
                double ratio = (double) size / (double)requestedSize;
                
                for(int i = 0; i < cnt; i++)
                {
                    var col = Columns[i];
                    // if last column
                    if (i == cnt - 1)
                    {
                        items.Add((col.name, balance));
                    }
                    else
                    {
                        int newSize = (int)((double)col.width * ratio);
                        items.Add((col.name, newSize));
                        balance -= newSize;
                    }
                }
                return items.ToArray();
            }

        }
    }
}
