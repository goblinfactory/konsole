using System;

namespace Konsole.Drawing
{
    internal class CellMap
    {
        public char Top { get; private set; }
        public char Left { get; private set; }
        public char Right { get; private set; }
        public char Bottom { get; private set; }
        public char Centre { get; private set; }

        public CellMap(char[] map)
        {
            if (map.Length != 5) throw new ArgumentOutOfRangeException("Expected 5 chars.");
            Centre = map[0];
            Top = map[1];
            Right = map[2];
            Bottom = map[3];
            Left = map[4];
        }

        public CellMap(char centre, char top, char right, char bottom, char left)
        {
            Centre = centre;
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public char[] Horizontal
        {
            get
            {
                return new [] { Left, Centre, Right };
            }   
        }

        public char[] Vertical
        {
            get { return new[] {Top, Centre, Bottom}; }
        }

        public char[] Chars
        {
            get {  return new [] { Centre, Top, Right, Bottom, Left};}
        }
    }
}