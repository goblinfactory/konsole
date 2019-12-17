namespace Konsole.Forms
{
    public class ThinBoxStyle : IBoxStyle
    {
        public char TL { get { return '┌'; } }
        public char T { get { return '─'; } }
        public char TR { get { return '┐'; } }
        public char L { get { return '│'; } }
        public char R { get { return '│'; } }
        public char BL { get { return '└'; } }
        public char BR { get { return '┘'; } }
        public char B { get { return '─'; } }
        public char LJ { get { return '├'; } }
        public char RJ { get { return '┤'; } }
    }

    // https://en.wikipedia.org/wiki/Box-drawing_character
}