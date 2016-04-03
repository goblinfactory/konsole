using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Drawing
{
    public static class LineCharTypes
    {
        private const string AllAsciiLineChars = "┤╡╢╖╕╣╗╝╜╛┐└┴┬─┼╞╟╚╔╩╦╠╬╧╤╥╙╘╒╓╫╪┌╪";

        public static bool HasLeftSingle(this char src)
        {
            return "┤╢╖╜┐┴┬─┼╥╫".Contains(src);
        }  

        public static bool HasLeftDouble(this char src)
        {
            return "╡╕╣╗╝╛╩╦╬╧╤╪".Contains(src);
        }

        public static bool HasBottomDouble(this char src)
        {
            return "╢╖╣║╗╟╔╦╠╬╥╓╫".Contains(src);
        }
        

        public static bool HasBottomSingle(this char src)
        {
            return "│┤╡╕┐┬├┼╞╤╒╪╪┌".Contains(src);
        }

        public static bool HasTopSingle(this char src)
        {
            return "┤╡╛└┴┼╞╧╘╪".Contains(src);
        }

        public static bool HasTopDouble(this char src)
        {
            return "╢╣╝╜╟╚╩╠╬╙╫".Contains(src);
        }

        public static bool HasRightSingle(this char src)
        {
            return "└┴┬├─┼╟╥╙╓╫┌".Contains(src);
        }
        public static bool HasRightDouble(this char src)
        {
            return "╞╚╔╩╦╠═╬╧╤╘╒╪".Contains(src);
        }

    }
}
