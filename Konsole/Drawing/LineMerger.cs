using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Drawing
{

    public enum Direction
    {
        Horizontal, Vertical
    }

    public class RenderedLine
    {
        public XY Start { get; private set; }
        public XY End { get; private set; }
        public Direction Direction { get; private set; }

        public RenderedLine(XY start, XY end, Direction direction)
        {
            Start = start;
            End = end;
            Direction = direction;
        }
    }

    /// <summary>
    /// responsible for mapping the correct char to return when two lines intersect
    /// </summary>
    internal class LineMerger
    {
    // │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐└ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
        public CellMap Merge(CellMap map, char linechar )
        {

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


            // printing horizontal lines, from left to right, printing [═]'s
            // ---------------------------------------------------
            //  - If it's the first character, printing  [═] 
            //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
            //  becomes     ╞ ╪ ╪ ╬ ╦ ╦ ╬ ╠ ╦ ╩ ╩ ╧ ╤ ╘ ╧ ╤ ╞ ═ ╪ ╞ ╠ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╩ ╤ ╦ ╚ ╘ ╒ ╔ ╬ ╪ ╧ ╒ ╪		

            //
            //  - If it's a middle character, printing  [═] 
            //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
            //  becomes     ╪ ╪ ╪ ╬ ╦ ╦ ╬ ╬ ╦ ╩ ╩ ╧ ╤ ╧ ╧ ╤ ╪ ═ ╪ ╪ ╬ ╩ ╦ ╩ ╦ ╬ ═ ╬ ╧ ╩ ╤ ╦ ╩ ╧ ╤ ╦ ╬ ╪ ╧ ╤ ╪		

            //  - If last char , printing  [═] 
            //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
            //  becomes     ╡ ╡ ╡ ╣ ╗ ╕ ╣ ╣ ╗ ╝ ╝ ╛ ╕ ╧ ╧ ╤ ╪ ═ ╪ ╪ ╠ ╩ ╦ ╩ ╦ ╬ ═ ╬ ╧ ╩ ╤ ╦ ╩ ╧ ╤ ╦ ╬ ╪ ╛ ╤ ╪		

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

            var newmap = new CellMap(map.Chars);

            switch (linechar)
            {
                case '│':
                    
                    // Do I need to change?
                    // --------------------
                    bool leftSingle = (map.Left.HasRightSingle());
                    bool leftDouble = (map.Left.HasLeftDouble());
                    bool rightSingle = (map.Right.HasLeftSingle());
                    bool rightDouble = (map.Right.HasRightDouble());



            }
        }


    }
}
