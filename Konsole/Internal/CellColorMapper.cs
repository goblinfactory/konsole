using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Internal
{
    /*

    k  Black,
    B  DarkBlue,
    G  DarkGreen,
    C  DarkCyan,
    R  DarkRed,
    M  DarkMagenta,
    Y  DarkYellow,
    a  Gray,
    A  DarkGray,
    b  Blue,
    g  Green,
    c  Cyan,
    r  Red,
    m  Magenta,
    y  Yellow,
    w  White,    

    */

    public static class CellColorMapper
    {
        private static string colors = "kBGCRMYaAbgcrmyw";

        private static char ToChar(this ConsoleColor color)
        {
            int c = (int) color;
            return colors[c];
        }

        /// <summary>
        /// returns a char representation of the Cell as an array of the Char, Foreground and Background.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static char[] ToChars(this Cell cell)
        {
            return new[]
            {
                cell.Char,
                cell.Foreground.ToChar(),
                cell.Background.ToChar()
            };
        }

    }
}
