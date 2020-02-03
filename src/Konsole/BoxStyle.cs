using System;
using static System.ConsoleColor;

namespace Konsole
{
    public class BoxStyle
    {
        public BoxStyle(LineThickNess thickNess, Colors title, Colors line, Colors body)
        {
            ThickNess = thickNess;
            Title = title;
            Line = line;
            Body = body;
        }

        public BoxStyle()
        {

        }

        public LineThickNess ThickNess { get; set; } = LineThickNess.Single;
        public Colors Title { get; set; } = new Colors(White, Black);
        public Colors Line { get; set; } = new Colors(White, Black);
        public Colors Body { get; set; } = new Colors(White, Black);

        public BoxStyle WithBackGround(ConsoleColor background)
        {
            return new BoxStyle(
                ThickNess,
                new Colors(Title.Foreground, background),
                new Colors(Line.Foreground, background),
                new Colors(Body.Foreground, background)
                );
        }

        public BoxStyle WithThickness(LineThickNess thickNess)
        {
            return new BoxStyle(
                thickNess,
                Title,
                Line,
                Body
                );
        }

    }
}
