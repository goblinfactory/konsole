//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Konsole.Forms;

//namespace Konsole.Drawing
//{

//    /// <summary>
//    /// responsible for mapping the correct char to return when two lines intersect
//    /// </summary>
//    internal class AsciiLineMerger
//    {
//        public enum Position { First, Middle, Last }

//        // printing horizontal lines, from left to right, printing [═]'s
//        // ---------------------------------------------------
//        //  - If it's the first character, printing  [═] 
//        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ┘ ┌ ╪		
//        //  becomes     ╞ ╪ ╪ ╬ ╦ ╦ ╬ ╠ ╦ ╩ ╩ ╧ ╤ ╘ ╧ ╤ ╞ ═ ╪ ╞ ╠ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╩ ╤ ╦ ╚ ╘ ╒ ╔ ╬ ╧ ╒ ╪		

//        //
//        //  - If it's a middle character, printing  [═] 
//        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ┘ ┌ ╪		
//        //  becomes     ╪ ╪ ╪ ╬ ╦ ╦ ╬ ╬ ╦ ╩ ╩ ╧ ╤ ╧ ╧ ╤ ╪ ═ ╪ ╪ ╬ ╩ ╦ ╩ ╦ ╬ ═ ╬ ╧ ╩ ╤ ╦ ╩ ╧ ╤ ╦ ╬ ╧ ╤ ╪		

//        //  - If last char , printing  [═] 
//        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ┘ ┌ ╪		
//        //  becomes     ╡ ╡ ╡ ╣ ╗ ╕ ╣ ╣ ╗ ╝ ╝ ╛ ╕ ╧ ╧ ╤ ╪ ═ ╪ ╪ ╠ ╩ ╦ ╩ ╦ ╬ ═ ╬ ╧ ╩ ╤ ╦ ╩ ╧ ╤ ╦ ╬ ╛ ╤ ╪		

//        private static readonly LineCharMapper HorizontalDoubleLine = new LineCharMapper('═',
//            "│ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌",new CharMap(
//            "╞ ╪ ╪ ╬ ╦ ╦ ╬ ╠ ╦ ╩ ╩ ╧ ╤ ╘ ╧ ╤ ╞ ═ ╪ ╞ ╠ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╩ ╤ ╦ ╚ ╘ ╒ ╔ ╬ ╪ ╧ ╒",
//            "╪ ╪ ╪ ╬ ╦ ╦ ╬ ╬ ╦ ╩ ╩ ╧ ╤ ╧ ╧ ╤ ╪ ═ ╪ ╪ ╬ ╩ ╦ ╩ ╦ ╬ ═ ╬ ╧ ╩ ╤ ╦ ╩ ╧ ╤ ╦ ╬ ╪ ╧ ╤",
//            "╡ ╡ ╡ ╣ ╗ ╕ ╣ ╣ ╗ ╝ ╝ ╛ ╕ ╧ ╧ ╤ ╪ ═ ╪ ╪ ╠ ╩ ╦ ╩ ╦ ╬ ═ ╬ ╧ ╩ ╤ ╦ ╩ ╧ ╤ ╦ ╬ ╪ ╛ ╤")
//        );

        
//        // printing horizontal lines, from left to right, printing [─]'s
//        // ---------------------------------------------------
//        //  - If it's the first character, printing  [─] 
//        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
//        //  becomes     ├ ┼ ┼ ╫ ╥ ╥ ╫ ╟ ╥ ╨ ╨ ┴ ┬ └ ┴ ┬ ├ ─ ┼ ├ ╟ ╙ ╓ ╨ ╥ ╟ ─ ╫ ┴ ╨ ┬ ╥ ╙ └ ┌ ╓ ╫ ┼ ┴ ┌ ┼	

//        //
//        //  - If it's a middle character, printing  [─] 
//        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
//        //  becomes     ┼ ┼ ┼ ╫ ╥ ╥ ╫ ╫ ╥ ╨ ╨ ┴ ┬ ┴ ┴ ┬ ┼ ─ ┼ ┼ ╫ ╨ ╥ ╨ ╥ ╫ ─ ╫ ┴ ╨ ┬ ╥ ╨ ┴ ┬ ╥ ╫ ┼ ┴ ┬ ┼	

//        //  - If last char 
//        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐└ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
//        //  becomes     ┤ ┤ ┤ ╢ ╖ ╕ ╣ ║ ┐ ╜ ╜ ┘ ┐┴ ┴ ┬ ┼ ─ ┼ ┼ ╢ ╨ ╥ ╨ ╥ ╫ ─ ╫ ┴ ╨ ┬ ╥ ╨ ┴ ┬ ╥ ╫ ┼ ┘ ┬ ┼		

//        private static readonly LineCharMapper HorizontalSingleLine = new LineCharMapper('─',
//            "│ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌",new CharMap(
//            "├ ┤ ╡ ┤ ┐ ╕ ╡ │ ┐ ╡ ┤ ╡ ┬ ├ ┼ ┬ ├ ─ ┼ ╞ ├ ╞ ╒ ╪ ╤ ╞ ╪ ╪ ╪ ┼ ╤ ┬ ├ ╞ ╒ ┌ ┼ ┼ ┤ ┌",
//            "┼ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╡ ┤ ╡ ┬ ├ ┼ ┼ ├ ─ ┼ ╞ ├ ╞ ╞ ╪ ╪ ╞ ╪ ╪ ╪ ┼ ╪ ┼ ├ ╞ ╞ ├ ┼ ┼ ┤ ├",
//            "┤ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╛ ┘ ╛ ┐ ├ ┴ ┼ ├ ─ ┼ ╞ ├ ╘ ╞ ╧ ╪ ╞ ╧ ╪ ╧ ┴ ╪ ┼ └ ╘ ╞ ├ ┼ ┼ ┘ ├")
//        );

//        // printing vertical lines, from top to bottom, printing [│]'s
//        // ---------------------------------------------------
//        //  - If it's the first character, printing  [│] 
//        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
//        //  becomes     │ ┤ ╡ ┤ ┐ ╕ ╡ │ ┐ ╡ ┤ ╡ ┐ ├ ┼ ┬ ├ ┬ ┼ ╞ ├ ╞ ╒ ╪ ╤ ╞ ╪ ╪ ╪ ┼ ╤ ┬ ├ ╞ ╒ ┌ ┼ ┼ ┤ ┌ ╪	

//        //  - If it's a middle character, printing  [│] 
//        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
//        //  becomes     │ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╡ ┤ ╡ ┤ ├ ┼ ┼ ├ ┼ ┼ ╞ ├ ╞ ╞ ╪ ╪ ╞ ╪ ╪ ╪ ┼ ╪ ┼ ├ ╞ ╞ ├ ┼ ┼ ┤ ├ ╪	

//        //  - If it's the bottom character, printing  [│] 
//        //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪
//        //  becomes     │ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╛ ┘ ╛ ┤ ├ ┴ ┼ ├ ┼ ┼ ╞ ├ ╘ ╞ ╧ ╪ ╞ ╧ ╪ ╧ ┴ ╪ ┼ └ ╘ ╞ ├ ┼ ┼ ┘ ├ ╪            

//        private static readonly LineCharMapper VerticalSingleLine = new LineCharMapper('│',
//            "│ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ┘ ┌ ╪", new CharMap(
//            "│ ┤ ╡ ┤ ┐ ╕ ╡ │ ┐ ╡ ┤ ╡ ┐ ├ ┼ ┬ ├ ┬ ┼ ╞ ├ ╞ ╒ ╪ ╤ ╞ ╤ ╪ ╪ ┼ ╤ ┬ ├ ╞ ╒ ┌ ┼ ┤ ┌ ╪",
//            "│ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╡ ┤ ╡ ┤ ├ ┼ ┼ ├ ┼ ┼ ╞ ├ ╞ ╞ ╪ ╪ ╞ ╪ ╪ ╪ ┼ ╪ ┼ ├ ╞ ╞ ├ ┼ ┤ ├ ╪",
//            "│ ┤ ╡ ┤ ┤ ╡ ╡ │ ┤ ╛ ┘ ╛ ┤ ├ ┴ ┼ ├ ┼ ┼ ╞ ├ ╘ ╞ ╧ ╪ ╞ ╧ ╪ ╧ ┴ ╪ ┼ └ ╘ ╞ ├ ┼ ┘ ├ ╪")
//        );

//            // printing vertical lines, from top to bottom, printing [║]'s
//            // ---------------------------------------------------
//            //  - If it's the first character, printing  [║] 
//            //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪
//            //  becomes     ║ ╢ ╣ ╣ ╗ ╗ ╣ ║ ╗ ╣ ╢ ╣ ╖ ╟ ╫ ╥ ╟ ╫ ╫ ╠ ╟ ╠ ╔ ╬ ╦ ╠ ╦ ╬ ╬ ╫ ╦ ╥ ╟ ╠ ╔ ╓ ╫ ╬ ╢ ╓ ╬

//            //  - If it's a middle character, printing  [║] 
//            //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪		
//            //  becomes     ║ ╢ ╣ ╣ ╣ ╣ ╣ ║ ╣ ╣ ╢ ╣ ╢ ╟ ╫ ╫ ╟ ╫ ╫ ╠ ╟ ╠ ╠ ╬ ╬ ╠ ╬ ╬ ╬ ╫ ╬ ╫ ╟ ╠ ╠ ╟ ╫ ╬ ╢ ╟ ╬

//            //  - If it's the bottom character, printing  [║] 
//            //  onto        │ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌ ╪
//            //  becomes     ║ ╢ ╣ ╣ ╣ ╣ ╣ ║ ╣ ╝ ╜ ╝ ╢ ╙ ╨ ╫ ╟ ╫ ╫ ╠ ╟ ╚ ╠ ╩ ╬ ╠ ╩ ╬ ╩ ╨ ╬ ╫ ╙ ╚ ╠ ╟ ╫ ╬ ╜ ╟ ╬

//        private static readonly LineCharMapper VerticalDoubleLine = new LineCharMapper('║',
//            "│ ┤ ╡ ╢ ╖ ╕ ╣ ║ ╗ ╝ ╜ ╛ ┐ └ ┴ ┬ ├ ─ ┼ ╞ ╟ ╚ ╔ ╩ ╦ ╠ ═ ╬ ╧ ╨ ╤ ╥ ╙ ╘ ╒ ╓ ╫ ╪ ┘ ┌", new CharMap(
//            "║ ╢ ╣ ╣ ╗ ╗ ╣ ║ ╗ ╣ ╢ ╣ ╖ ╟ ╫ ╥ ╟ ╫ ╫ ╠ ╟ ╠ ╔ ╬ ╦ ╠ ╦ ╬ ╬ ╫ ╦ ╥ ╟ ╠ ╔ ╓ ╫ ╬ ╢ ╓",
//            "║ ╢ ╣ ╣ ╣ ╣ ╣ ║ ╣ ╣ ╢ ╣ ╢ ╟ ╫ ╫ ╟ ╫ ╫ ╠ ╟ ╠ ╠ ╬ ╬ ╠ ╬ ╬ ╬ ╫ ╬ ╫ ╟ ╠ ╠ ╟ ╫ ╬ ╢ ╟",
//            "║ ╢ ╣ ╣ ╣ ╣ ╣ ║ ╣ ╝ ╜ ╝ ╢ ╙ ╨ ╫ ╟ ╫ ╫ ╠ ╟ ╚ ╠ ╩ ╬ ╠ ╩ ╬ ╩ ╨ ╬ ╫ ╙ ╚ ╠ ╟ ╫ ╬ ╜ ╟")
//            );

//        // references 
//        private static IBoxStyle thin = new ThinBoxStyle();
//        private static IBoxStyle thick = new ThickBoxStyle();

//        public char Merge(char existing, Position position, char printing)
//        {
//            if (printing== thick.R) return VerticalDoubleLine[position, existing];
//            if (printing == thin.R) return VerticalSingleLine[position, existing];
//            if (printing == thin.T) return HorizontalSingleLine[position, existing];
//            if (printing == thick.T) return HorizontalDoubleLine[position, existing];
//            throw new ArgumentOutOfRangeException("existing", "Unsupported line char " + printing);
//        }


//    }
//}
