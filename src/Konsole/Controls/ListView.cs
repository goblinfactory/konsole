using System;
using System.Collections.Generic;
using System.Linq;
using Konsole.Internal;
using static System.ConsoleColor;

namespace Konsole
{
    public class ListView<T> : IListView
    {
        public int selectedItemIndex { get; set; }
        public (string name, int width)[] Columns { get; set; }

        protected Func<T, string[]> _getRow;
        private int startRecord = 0;
        private readonly IConsole _console;
        protected Func<IEnumerable<T>> _getData;

        public class Theme
        {
            public Colors Header { get; set; } = new Colors(Yellow, Black);
            public Colors Col1 { get; set; } = new Colors(White, Black);
            public Colors Col2 { get; set; } = new Colors(Gray, Black);
            public Colors Col3 { get; set; } = new Colors(Gray, Black);
            public Colors Col4 { get; set; } = new Colors(Gray, Black);

        }
        public Theme Style { get; set; } = new Theme();

        /// <summary>
        /// /// set this to a function that will override the default theme for a column
        /// if you need to customise the colour of a specific item. e.g. 
        /// highlighting a specific account that is ON_HOLD.
        /// </summary>
        /// <remarks>columnNo is unusually '1' based for these rules.</remarks>
        public Func<T, int, Colors> BusinessRuleColors = (item, columnNo) =>  null;

        public ListView(IConsole console, Func<IEnumerable<T>> getData, Func<T, string[]> getRow, params (string name, int width)[] columns)
        {
            _console = console;
            _getData = getData;
            _getRow = getRow;
            Columns = columns;
        }
        
        public void Refresh()
        {
            // TODO: write concurrency test that proves that we need this lock here! Test must fail if I take it out !
            lock(Window._staticLocker)
            {
                int cnt = Columns.Count();
                var columns = GetResizedColumns();
                int width = _console.WindowWidth;
                var colLen = columns.Sum(c => c.width);

                // certain that we can do better than
                // clearing each time. Make sure we always write the full width
                // then we can simply clear below where we write to, if we can 
                // tell if it was empty, then no need.

                _console.Clear();
                var items = _getData().ToArray();

                PrintColumnHeadings(columns);
                foreach (var item in items)
                {
                    int i = 0;
                    var row = _getRow(item);
                    foreach (var columnText in row)
                    {
                        var column = columns[i];
                        bool lastColumn = (++i == cnt);
                        if (lastColumn)
                        {
                            var colors = getColors(item, i);
                            _console.WriteLine(colors, columnText.FixLeft(column.width));
                        }
                        else
                        {
                            var colors = getColors(item, i);
                            _console.Write(colors, columnText.FixLeft(column.width));
                            // TODO: the bar needs to come from the parent frame
                            _console.Write("│");
                        }
                    }
                }
            }
        }

        private Colors getColors(T item, int column)
        {
            // allow users to override the colouring for a column and a row
            // based on a set of business rules

            if (BusinessRuleColors!= null)
            {
                var colors = BusinessRuleColors(item, column);
                if (colors != null) return colors;
            }

            switch(column)
            {
                case 1: return Style.Col1;
                case 2: return Style.Col2;
                case 3: return Style.Col3;
                default: return Style.Col3;
            }
        }

        private void PrintColumnHeadings((string name, int width)[] columns)
        {
            int i = 0;
            int len = columns.Length;
            foreach (var col in columns)
            {
                bool last = (++i == len);
                if (last)
                {
                    _console.WriteLine(Style.Header.Foreground, col.name.FixCenter(col.width));
                }
                else
                {
                    _console.Write(Style.Header.Foreground, col.name.FixCenter(col.width));
                    _console.Write("│");
                }
            }

        }



        /// <summary>
        /// returns the actual size of the columns that are or will be used based on the requested column sizes, and the actual window width.
        /// if the window is too small to accommodate the requested sizes then all the fields are reduced pro ratio to fit the window.
        /// all content will be clipped to fit.
        /// Conversly if the window is larger then the columns are resized proportionately.
        /// if any columns are 0, then the other columns will be fixed, and the 0 columns (wildcards) will receive the balance, split evenly between 0's.
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
            int numWildCards = Columns.Count(i => i.width == 0);
            bool hasWildCards = numWildCards > 0;

            double ratio = hasWildCards ? 1.0 : (double)size / (double)requestedSize;

            int wildSize = 0;
            if(numWildCards > 0)
            {
                wildSize = (balance - requestedSize) / numWildCards;
            }

            for (int i = 0; i < cnt; i++)
            {
                var col = Columns[i];
                // if last column
                if (i == cnt - 1)
                {
                    items.Add((col.name, balance));
                }
                else
                {
                    if(col.width == 0)
                    {
                        items.Add((col.name, wildSize));
                        balance -= wildSize;
                    }
                    else
                    {
                        int newSize = (int)((double)col.width * ratio);
                        items.Add((col.name, newSize));
                        balance -= newSize;
                    }
                }
            }
            return items.ToArray();
        }

    }
}
