using static System.ConsoleColor;

namespace Konsole
{
    public class BoxStyle
    {
        public LineThickNess ThickNess { get; set; } = LineThickNess.Single;
        public Colors Title { get; set; } = new Colors(White, Black);
        public Colors Line { get; set; } = new Colors(White, Black);
        public Colors Body { get; set; } = new Colors(White, Black);
    }
}
