using System;
using System.Collections.Generic;
using System.Text;

namespace Konsole
{
    internal class Line
    {
        private readonly int _width;
        public Dictionary<int, Cell> Cells { get; private set; }

        public Line(int width, char c, ConsoleColor color, ConsoleColor background)
        {
            _width = width;
            Cells = new Dictionary<int, Cell>();
            for (int i = 0; i < width; i++)
            {
                Cells.Add(i,new Cell(c, color, background));
            }
        }

        public string WriteFormatted(ConsoleColor color, ConsoleColor background, int x, string format, params object[] args)
        {
            var text = string.Format(format, args);
            return Write(color, background, x, text);
        }

        /// <summary>
        /// Writes text to a line, returns any overflow.
        /// </summary>
        public string Write(ConsoleColor color, ConsoleColor background, int x, string text)
        {
            int len = text.Length;
            int overflow = len + x > _width ? len - (_width - x) : 0;
            // strlen - (width - x)
            // e.g. width 10, x = 1 , string len = 20, overflow = 20-(10-1)=11 
            // e.g. width 20, x = 11, string len = 5, overflow = 5 - (20-11) = -4

            int writeLen = len - overflow;
            // e.g. width 10, x = 6 , string len = 10, overflow = 10-(10-1)=11 

            var writeText = text.Substring(0, writeLen);
            var overflowText = overflow > 0 ? text.Substring(writeLen, overflow) : null;
            // todo; consider asignment overrides? 
            for (int i = 0; i < writeLen; i++) Cells[i + x] = Cells[i + x].WithChar(writeText[i]);
            return overflowText;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var c in Cells.Values)
            {
                sb.Append(c.Char);
            }
            return sb.ToString();
        }
    }
}