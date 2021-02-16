using Konsole.Layouts;
using System;
using System.Linq;


namespace Konsole
{
    public static class SplitRowsExtensions
    {
        public static IConsole[] SplitRows(this Window w, params Split[] splits)
        {
            return _SplitRows(w, splits);
        }

        public static IConsole[] SplitRows(this Window w, params string[] splits)
        {
            var _splits = splits.Select(s => new Split(s)).ToArray();
            return _SplitRows(w, _splits);
        }

        public static IConsole[] SplitRows(this IConsole c, params Split[] splits)
        {
            return _SplitRows(c, splits);
        }

        private static IConsole[] _SplitRows(IConsole c, params Split[] splits)
        {
            var sizes = Splitter.GetSplitSizes(splits, c.WindowHeight, Splitter.SplitType.Row);
            var rows = new IConsole[splits.Length];
            int row = 0;
            int i = 0;
            foreach(var split in splits)
            {
                int size = sizes[i];
                var foregroundColor = split.Foreground ?? c.ForegroundColor;
                var backgroundColor = split.Background ?? c.BackgroundColor;
                rows[i] = LayoutExtensions._RowSlice(c, split.Title, row, size, split.Thickness != null, split.Thickness, foregroundColor, backgroundColor);
                row += size;
                i++;
            }
            return rows;
        }
    }
}
