using Goblinfactory.Konsole.Layouts.Internal;
using System;
using System.Linq;

namespace Konsole
{
    public static class SplitColumnsExtensions
    {

        public static IConsole[] SplitColumns(this IConsole c, params Split[] splits)
        {
            return _SplitColumns(c, splits);
        }

        public static IConsole[] SplitColumns(this IConsole c, params string[] splits)
        {
            var _splits = splits.Select(s => new Split(s)).ToArray();
            return _SplitColumns(c, _splits);
        }
        private static IConsole[] _SplitColumns(IConsole c, params Split[] splits)
        {
            var sizes = Splitter.GetSplitSizes(splits, c.WindowWidth, Splitter.SplitType.Column);
            var columns = new IConsole[splits.Length];
            int col = 0;
            int i = 0;
            foreach (var split in splits)
            {
                int size = sizes[i];
                var foregroundColor = split.Foreground ?? c.ForegroundColor;
                var backgroundColor = split.Background ?? c.BackgroundColor;
                columns[i] = LayoutExtensions._ColumnSlice(c, split.Title, col, size, split.Thickness != null, split.Thickness, foregroundColor, backgroundColor);
                col += size;
                i++;
            }
            return columns;
        }
    }
}
