using System;
using System.Linq;

namespace Konsole
{
    public static class SplitColumnsExtensions
    {
        public static IConsole[] SplitColumns(this Window w, params Split[] splits)
        {
            return _SplitColumns(w, splits);
        }

        public static IConsole[] SplitColumns(this IConsole c, params Split[] splits)
        {
            return _SplitColumns(c, splits);
        }

        private static IConsole[] _SplitColumns(IConsole c, params Split[] splits)
        {
            int width = c.WindowWidth;
            int splitWidth = splits.Sum(s => s.Size);

            if (splitWidth + 1 > width)
            {
                throw new ArgumentOutOfRangeException($"Console window is not wide enought to support that many columns. Console width:{width}, Sum of split columns:{splitWidth}");
            }
            bool hasWildcard = splits.Any(s => s.Size == 0);
            int wildCardWidth = width - splitWidth;
            if (wildCardWidth > 0 && !hasWildcard)
            {
                throw new ArgumentOutOfRangeException("The sum of your splits must equal the width of the window if you do not have any wildcard splits.");
            }

            var cols = new IConsole[splits.Length];
            int left = 0;
            for (int i = 0; i < splits.Length; i++)
            {
                var split = splits[i];
                var size = (split.Size == 0) ? wildCardWidth : split.Size;
                var foregroundColor = split.Foreground ?? c.ForegroundColor;
                var backgroundColor = split.Background ?? c.BackgroundColor;
                cols[i] = LayoutExtensions._ColumnSlice(c, split.Title, left, size, split.Thickness != null, split.Thickness, foregroundColor, backgroundColor);
                left += size;
            }
            return cols;
        }
    }
}
