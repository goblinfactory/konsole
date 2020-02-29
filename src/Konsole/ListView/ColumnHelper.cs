using Konsole.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konsole
{
    public static class ColumnHelper
    {
        /// <summary>
        /// returns the actual size of the columns that are or will be used based on the requested column sizes, and the actual window width.
        /// if the window is too small to accommodate the requested sizes then all the fields are reduced pro ratio to fit the window.
        /// all content will be clipped to fit.
        /// Conversly if the window is larger then the columns are resized proportionately.
        /// if any columns are 0, then the other columns will be fixed, and the 0 columns (wildcards) will receive the balance, split evenly between 0's.
        /// </summary>
        /// <returns>A new array of columns with updated widths, old source is not affected</returns>
        public static Column[] ResizeColumns(this Column[] columns, int width)
        {
            if (columns == null) return new Column[0];
            if (width == 0) return columns.Select(c => c.WithWidth(0)).ToArray();

            int cnt = columns.Length;
            var items = new (Column column, int resized)[cnt];
            
            int numbBars = cnt - 1;
            int size = width - numbBars;
            int requestedSize = columns.Sum(c => c.Width);
            int numWildCards = columns.Count(c => c.Width == 0);
            bool hasWildCards = numWildCards > 0;

            double ratio = hasWildCards ? 1.0 : (double)size / (double)requestedSize;

            int balance = size;
            int wildSize = 0;
            if (numWildCards > 0)
            {
                wildSize = (balance - requestedSize) / numWildCards;
            }

            var newColumns = columns.SelectWithFirstLast<Column>((c,first, last)  => 
            {
                // if last column
                if (last)
                {
                    return c.WithWidth(balance);
                }
                else
                {
                    if (c.Width == 0)
                    {
                        balance -= wildSize;
                        return c.WithWidth(wildSize);
                    }
                    else
                    {
                        int newSize = (int)((double)c.Width * ratio);
                        balance -= newSize;
                        return c.WithWidth(newSize);
                    }
                }
            });

            return newColumns;
        }
    }

}
