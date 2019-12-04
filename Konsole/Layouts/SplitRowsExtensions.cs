using System;
using System.Linq;


namespace Konsole
{
    public static class SplitRowsExtensions
    {
        public static IConsole[] SplitRows(this Window c, params Split[] splits)
        {
            int height = c.WindowHeight;
            int splitHeight = splits.Sum(s => s.Size);

            if (splitHeight + 1 > height)
            {
                throw new ArgumentOutOfRangeException($"Console window is not tall enought to support that many rows. Console height:{height}, Sum of split rows:{splitHeight}");
            }
            bool hasWildcard = splits.Any(s => s.Size == 0);
            int wildCardHeight = height - splitHeight;
            if(wildCardHeight > 0 && !hasWildcard)
            {
                throw new ArgumentOutOfRangeException("The sum of your splits must equal the height of the window if you do not have any wildcard splits.");
            }

            var rows = new IConsole[splits.Length];
            int row = 0;
            for(int i = 0; i < splits.Length; i++)
            {
                var split = splits[i];
                var size = (split.Size == 0) ? wildCardHeight : split.Size;
                var color = split.Foreground ?? c.ForegroundColor;
                rows[i] = LayoutExtensions._RowSlice(c, split.Title, row, size, split.Thickness != null, split.Thickness, color);
                row += size;
            }
            return rows;
        }
    }
}
