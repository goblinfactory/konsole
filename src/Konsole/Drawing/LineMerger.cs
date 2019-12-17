using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole.Forms;

namespace Konsole.Drawing
{

    /// <summary>
    /// responsible for mapping the correct char to return when two lines intersect
    /// </summary>
    internal class LineMerger
    {
        private class CharMap
        {
            public CharMap(string first, string middle, string last)
            {
                // allow for specifying the values with spaces in them so that strings in code with ascii map line chars is easier to read.
                First = first.Replace(" ","");
                Middle = middle.Replace(" ","");
                Last = last.Replace(" ", "");
            }
            public string First { get; private set; }
            public string Last { get; private set; }
            public string Middle { get; private set; }
        }

        private class CharMapKey
        {
            public Char Char { get; private set; }
            public Position Position { get; private set; }

            public CharMapKey(Char c, Position p)
            {
                Char = c;
                Position = p;
            }

            public override int GetHashCode()
            {
                return Char.GetHashCode() + 1000*(int)Position;
            }

            public override bool Equals(object obj)
            {
                var item = obj as CharMapKey;
                if (item == null) return false;
                return (item.Char == this.Char && item.Position == this.Position);
            }
        }

        private class LineCharMapper
        {
            public char PrintChar { get; private set; }
            private Dictionary<CharMapKey, char> _map = new Dictionary<CharMapKey, char>();

            public LineCharMapper(char printChar, string onto, CharMap becomes)
            {
                PrintChar = printChar;
                var keys = onto.Replace(" ", "");

                int len = keys.Length;
                int m1 = becomes.First.Length;
                int m2 = becomes.Middle.Length;
                int m3 = becomes.Last.Length;

                bool areAllEqualLengths = (((len*3) - (m1 + m2 + m3)) == 0);
                if (!areAllEqualLengths || len==0 || m1 ==0 || m2==0 || m3==0) throw new ArgumentOutOfRangeException("onto and becomes cannot be blank, and must contain the same number of characters.");

                var firstItems = keys.Select((c, i) => new {Key = new CharMapKey(c, Position.First), Value = becomes.First[i]}).ToList();
                var middleItems  = keys.Select((c, i) => new {Key = new CharMapKey(c, Position.Middle), Value = becomes.Middle[i]}).ToList();
                var lastItems = keys.Select((c, i) => new {Key = new CharMapKey(c, Position.Last), Value = becomes.Last[i]}).ToList();

                firstItems.ForEach(i => _map.Add(i.Key, i.Value));
                middleItems.ForEach(i => _map.Add(i.Key, i.Value));
                lastItems.ForEach(i => _map.Add(i.Key, i.Value));
            }

            public char this[Position position, char c]
            {
                get { return _map[new CharMapKey(c,position) ]; }
            }
        }
         

            
        public enum Position { First, Middle, Last }

        // printing horizontal lines, from left to right, printing [═]'s
        // ---------------------------------------------------
        //  - If it's the first character, printing  [═] 
        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ┘ ┌ ╪		
        //  becomes     ╞ ╪ ╪ ╬ ╦ ╦ ╬ ╠ ╦ ╩ ╩ ╧ ╤ ╘ ╧ ╤ ╞ ═ ╪ ╞ ╠ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╩ ╤ ╦ ╚ ╘ ╒ ╔ ╬ ╧ ╒ ╪		

        //
        //  - If it's a middle character, printing  [═] 
        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ┘ ┌ ╪		
        //  becomes     ╪ ╪ ╪ ╬ ╦ ╦ ╬ ╬ ╦ ╩ ╩ ╧ ╤ ╧ ╧ ╤ ╪ ═ ╪ ╪ ╬ ╩ ╦ ╩ ╦ ╬ ═ ╬ ╧ ╩ ╤ ╦ ╩ ╧ ╤ ╦ ╬ ╧ ╤ ╪		

        //  - If last char , printing  [═] 
        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ┘ ┌ ╪		
        //  becomes     ╡ ╡ ╡ ╣ ╗ ╕ ╣ ╣ ╗ ╝ ╝ ╛ ╕ ╧ ╧ ╤ ╪ ═ ╪ ╪ ╠ ╩ ╦ ╩ ╦ ╬ ═ ╬ ╧ ╩ ╤ ╦ ╩ ╧ ╤ ╦ ╬ ╛ ╤ ╪		

        private static readonly LineCharMapper HorizontalDoubleLine = new LineCharMapper('═',
            "│ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌",new CharMap(
            "╞ ╪ ╪ ╬ ╦ ╦ ╬ ╠ ╦ ╩ ╩ ╧ ╤ ╘ ╧ ╤ ╞ ═ ╪ ╞ ╠ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╩ ╤ ╦ ╚ ╘ ╒ ╔ ╬ ╪ ╧ ╒",
            "╪ ╪ ╪ ╬ ╦ ╦ ╬ ╬ ╦ ╩ ╩ ╧ ╤ ╧ ╧ ╤ ╪ ═ ╪ ╪ ╬ ╩ ╦ ╩ ╦ ╬ ═ ╬ ╧ ╩ ╤ ╦ ╩ ╧ ╤ ╦ ╬ ╪ ╧ ╤",
            "╡ ╡ ╡ ╣ ╗ ╕ ╣ ╣ ╗ ╝ ╝ ╛ ╕ ╧ ╧ ╤ ╪ ═ ╪ ╪ ╠ ╩ ╦ ╩ ╦ ╬ ═ ╬ ╧ ╩ ╤ ╦ ╩ ╧ ╤ ╦ ╬ ╪ ╛ ╤")
        );

        
        // printing horizontal lines, from left to right, printing [─]'s
        // ---------------------------------------------------
        //  - If it's the first character, printing  [─] 
        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
        //  becomes     ├ ┼ ┼ ╫ ╥ ╥ ╫ ╟ ╥ ╨ ╨ ┴ ┬ └ ┴ ┬ ├ ─ ┼ ├ ╟ ╙ ╓ ╨ ╥ ╟ ─ ╫ ┴ ╨ ┬ ╥ ╙ └ ┌ ╓ ╫ ┼ ┴ ┌ ┼	

        //
        //  - If it's a middle character, printing  [─] 
        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
        //  becomes     ┼ ┼ ┼ ╫ ╥ ╥ ╫ ╫ ╥ ╨ ╨ ┴ ┬ ┴ ┴ ┬ ┼ ─ ┼ ┼ ╫ ╨ ╥ ╨ ╥ ╫ ─ ╫ ┴ ╨ ┬ ╥ ╨ ┴ ┬ ╥ ╫ ┼ ┴ ┬ ┼	

        //  - If last char 
        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐└ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
        //  becomes     ┤ ┤ ┤ ╢ ╖ ╕ ╣ ║ ┐ ╜ ╜ ┘ ┐┴ ┴ ┬ ┼ ─ ┼ ┼ ╢ ╨ ╥ ╨ ╥ ╫ ─ ╫ ┴ ╨ ┬ ╥ ╨ ┴ ┬ ╥ ╫ ┼ ┘ ┬ ┼		

        private static readonly LineCharMapper HorizontalSingleLine = new LineCharMapper('─',
            "│ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌",new CharMap(
            "├ ┤ ╡ ┤ ┐ ╕ ╡ │ ┐ ╡ ┤ ╡ ┬ ├ ┼ ┬ ├ ─ ┼ ╞ ├ ╞ ╒ ╪ ╤ ╞ ╪ ╪ ╪ ┼ ╤ ┬ ├ ╞ ╒ ┌ ┼ ┼ ┤ ┌",
            "┼ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╡ ┤ ╡ ┬ ├ ┼ ┼ ├ ─ ┼ ╞ ├ ╞ ╞ ╪ ╪ ╞ ╪ ╪ ╪ ┼ ╪ ┼ ├ ╞ ╞ ├ ┼ ┼ ┤ ├",
            "┤ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╛ ┘ ╛ ┐ ├ ┴ ┼ ├ ─ ┼ ╞ ├ ╘ ╞ ╧ ╪ ╞ ╧ ╪ ╧ ┴ ╪ ┼ └ ╘ ╞ ├ ┼ ┼ ┘ ├")
        );

        // printing vertical lines, from top to bottom, printing [│]'s
        // ---------------------------------------------------
        //  - If it's the first character, printing  [│] 
        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
        //  becomes     │ ┤ ╡ ┤ ┐ ╕ ╡ │ ┐ ╡ ┤ ╡ ┐ ├ ┼ ┬ ├ ┬ ┼ ╞ ├ ╞ ╒ ╪ ╤ ╞ ╪ ╪ ╪ ┼ ╤ ┬ ├ ╞ ╒ ┌ ┼ ┼ ┤ ┌ ╪	

        //  - If it's a middle character, printing  [│] 
        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
        //  becomes     │ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╡ ┤ ╡ ┤ ├ ┼ ┼ ├ ┼ ┼ ╞ ├ ╞ ╞ ╪ ╪ ╞ ╪ ╪ ╪ ┼ ╪ ┼ ├ ╞ ╞ ├ ┼ ┼ ┤ ├ ╪	

        //  - If it's the bottom character, printing  [│] 
        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪
        //  becomes     │ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╛ ┘ ╛ ┤ ├ ┴ ┼ ├ ┼ ┼ ╞ ├ ╘ ╞ ╧ ╪ ╞ ╧ ╪ ╧ ┴ ╪ ┼ └ ╘ ╞ ├ ┼ ┼ ┘ ├ ╪            

        private static readonly LineCharMapper VerticalSingleLine = new LineCharMapper('│',
            "│ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ┘ ┌ ╪", new CharMap(
            "│ ┤ ╡ ┤ ┐ ╕ ╡ │ ┐ ╡ ┤ ╡ ┐ ├ ┼ ┬ ├ ┬ ┼ ╞ ├ ╞ ╒ ╪ ╤ ╞ ╤ ╪ ╪ ┼ ╤ ┬ ├ ╞ ╒ ┌ ┼ ┤ ┌ ╪",
            "│ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╡ ┤ ╡ ┤ ├ ┼ ┼ ├ ┼ ┼ ╞ ├ ╞ ╞ ╪ ╪ ╞ ╪ ╪ ╪ ┼ ╪ ┼ ├ ╞ ╞ ├ ┼ ┤ ├ ╪",
            "│ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╛ ┘ ╛ ┤ ├ ┴ ┼ ├ ┼ ┼ ╞ ├ ╘ ╞ ╧ ╪ ╞ ╧ ╪ ╧ ┴ ╪ ┼ └ ╘ ╞ ├ ┼ ┘ ├ ╪")
        );

            // printing vertical lines, from top to bottom, printing [║]'s
            // ---------------------------------------------------
            //  - If it's the first character, printing  [║] 
            //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪
            //  becomes     ║ ╢ ╣ ╣ ╗ ╗ ╣ ║ ╗ ╣ ╢ ╣ ╖ ╟ ╫ ╥ ╟ ╫ ╫ ╠ ╟ ╠ ╔ ╬ ╦ ╠ ╦ ╬ ╬ ╫ ╦ ╥ ╟ ╠ ╔ ╓ ╫ ╬ ╢ ╓ ╬

            //  - If it's a middle character, printing  [║] 
            //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
            //  becomes     ║ ╢ ╣ ╣ ╣ ╣ ╣ ║ ╣ ╣ ╢ ╣ ╢ ╟ ╫ ╫ ╟ ╫ ╫ ╠ ╟ ╠ ╠ ╬ ╬ ╠ ╬ ╬ ╬ ╫ ╬ ╫ ╟ ╠ ╠ ╟ ╫ ╬ ╢ ╟ ╬

            //  - If it's the bottom character, printing  [║] 
            //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪
            //  becomes     ║ ╢ ╣ ╣ ╣ ╣ ╣ ║ ╣ ╝ ╜ ╝ ╢ ╙ ╨ ╫ ╟ ╫ ╫ ╠ ╟ ╚ ╠ ╩ ╬ ╠ ╩ ╬ ╩ ╨ ╬ ╫ ╙ ╚ ╠ ╟ ╫ ╬ ╜ ╟ ╬

        private static readonly LineCharMapper VerticalDoubleLine = new LineCharMapper('║',
            "│ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌", new CharMap(
            "║ ╢ ╣ ╣ ╗ ╗ ╣ ║ ╗ ╣ ╢ ╣ ╖ ╟ ╫ ╥ ╟ ╫ ╫ ╠ ╟ ╠ ╔ ╬ ╦ ╠ ╦ ╬ ╬ ╫ ╦ ╥ ╟ ╠ ╔ ╓ ╫ ╬ ╢ ╓",
            "║ ╢ ╣ ╣ ╣ ╣ ╣ ║ ╣ ╣ ╢ ╣ ╢ ╟ ╫ ╫ ╟ ╫ ╫ ╠ ╟ ╠ ╠ ╬ ╬ ╠ ╬ ╬ ╬ ╫ ╬ ╫ ╟ ╠ ╠ ╟ ╫ ╬ ╢ ╟",
            "║ ╢ ╣ ╣ ╣ ╣ ╣ ║ ╣ ╝ ╜ ╝ ╢ ╙ ╨ ╫ ╟ ╫ ╫ ╠ ╟ ╚ ╠ ╩ ╬ ╠ ╩ ╬ ╩ ╨ ╬ ╫ ╙ ╚ ╠ ╟ ╫ ╬ ╜ ╟")
            );

        // references 
        private static IBoxStyle thin = new ThinBoxStyle();
        private static IBoxStyle thick = new ThickBoxStyle();

        public char Merge(char existing, Position position, char printing)
        {
            if (printing== thick.R) return VerticalDoubleLine[position, existing];
            if (printing == thin.R) return VerticalSingleLine[position, existing];
            if (printing == thin.T) return HorizontalSingleLine[position, existing];
            if (printing == thick.T) return HorizontalDoubleLine[position, existing];
            throw new ArgumentOutOfRangeException("existing", "Unsupported line char " + printing);
        }


    }
}
