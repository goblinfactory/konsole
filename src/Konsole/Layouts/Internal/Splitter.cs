using Konsole;
using System;
using System.Linq;

namespace Goblinfactory.Konsole.Layouts.Internal
{
    internal static class Splitter
    {
        public enum SplitType { Row, Column };
        public static int[] GetSplitSizes(Split[] splits, int size, SplitType type)
        {
            int splitsTotal = splits.Sum(s => s.Size);

            if (splitsTotal + 1 > size)
            {
                if(type == SplitType.Column)
                {
                    throw new ArgumentOutOfRangeException($"Parent window is not wide enought to support that many columns. Console width:{size}, Sum of split columns:{splitsTotal}");
                }
                else
                {
                    throw new ArgumentOutOfRangeException($"Parent window is not tall enought to support that many rows. Console height:{size}, Sum of split rows:{splitsTotal}");
                }
            }

            int cntWildCards = splits.Count(s => s.Size == 0);
            bool hasWildcard = cntWildCards > 0;           
            int wildcardSize = hasWildcard ? (size - splitsTotal) / cntWildCards : 0;
            int totalSize = splits.Sum(s => s.Size) + (wildcardSize * cntWildCards);
            bool needsExtraLine = totalSize != size;
            if (wildcardSize > 0 && !hasWildcard)
            {
                throw new ArgumentOutOfRangeException($"The sum of your splits must equal the {(type == SplitType.Column ? "width" : "height")} of the window if you do not have any wildcard splits.");
            }

            var newsizes = new int[splits.Length];
            for (int i = 0; i < splits.Length; i++)
            {
                bool lastSplit = (i == splits.Length - 1);
                int extra = (lastSplit && needsExtraLine) ? 1 : 0;
                var split = splits[i];
                var newsize = ((split.Size == 0) ? wildcardSize : split.Size) + extra;
                newsizes[i] = newsize;
            }
            return newsizes;

        }
    }
}
