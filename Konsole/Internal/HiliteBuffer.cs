using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Internal
{
    /// <summary>
    /// interface for class that is able to provide a suitable human readable text representation of a buffer console.
    /// </summary>
    public interface IReadableBuffer
    {
        string ToApprovableText(Row[] rows);
    }

    /// <summary>
    /// focus on showing which lines of text have been highlighted, e.g. have a specific background color
    /// </summary>
    public class HiliteBuffer : IReadableBuffer
    {
        private readonly ConsoleColor _highliteColor;
        private readonly char _hiChar;
        private readonly char _normal;

        public HiliteBuffer(ConsoleColor highliteColor, char hiChar, char normal)
        {
            _highliteColor = highliteColor;
            _hiChar = hiChar;
            _normal = normal;
        }
        public string ToApprovableText(Row[] rows)
        {
            var rowTexts = rows.Select(RowText);
            var text = string.Join("\r\n", rowTexts);
            return text;
        }

        private string RowText(Row row)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var cell in row.Cells)
            {
                var c = cell.Value;
                // #ADH - #optimise - Since we know the width of the console, we can optimise this using fixed cell positions instead of string builder
                sb.Append(c.Background == _highliteColor 
                    ? $"{_hiChar}{c.Char}"
                    : $"{_normal}{c.Char}"
                );
            }
            return sb.ToString();
        }
    }
}
