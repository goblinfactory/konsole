namespace Konsole.Forms
{
    // this styling is possibly broken, i.e. you can't style a box using these.
    // the style is currently only being used to pick up the top or left characters.

    public class ThickBoxStyle : IBoxStyle
    {
        public char TL { get { return '╔'; } }
        public char T { get { return '═'; } }
        public char TR { get { return '╗'; } }
        public char L { get { return '║'; } }
        public char R { get { return '║'; } }
        public char BL { get { return '╚'; } }
        public char BR { get { return '╝'; } }
        public char B { get { return '═'; } }
        public char LJ { get { return '╟'; } }
        public char RJ { get { return '╢'; } }
        public char Box { get { return '╢'; } }
    }
}