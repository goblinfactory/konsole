using System;
using static System.ConsoleColor;

namespace Konsole
{
    public class Style
    {
        public static Func<Style> GetDefault = ()=> Style.WhiteOnBlack;
        public static Style Default
        {
            get
            {
                return GetDefault();
            }
        }

        public static Style BlackOnWhite
        {
            get { return new Style(LineThickNess.Single, Colors.BlackOnWhite, Colors.BlackOnWhite, Colors.BlackOnWhite, Colors.WhiteOnBlack); }
        }

        public static Style GrayOnBlack
        {
            get { return new Style(LineThickNess.Single, Colors.GrayOnBlack, Colors.GrayOnBlack, Colors.GrayOnBlack, Colors.BlackOnWhite); }
        }

        public static Style WhiteOnBlack
        {
            get { return new Style(LineThickNess.Single, Colors.WhiteOnBlack, Colors.WhiteOnBlack, Colors.WhiteOnBlack, Colors.BlackOnWhite); }
        }

        public static Style WhiteOnBlue
        {
            get { return new Style(LineThickNess.Single, Colors.WhiteOnBlue, Colors.WhiteOnBlue, Colors.WhiteOnBlue, Colors.BlueOnWhite); }
        }

        public static Style BlueOnWhite
        {
            get { return new Style(LineThickNess.Single, Colors.BlueOnWhite, Colors.BlueOnWhite, Colors.BlueOnWhite, Colors.WhiteOnBlue); }
        }

        public static Style WhiteOnDarkBlue
        {
            get { return new Style(LineThickNess.Single, Colors.WhiteOnDarkBlue, Colors.WhiteOnDarkBlue, Colors.WhiteOnDarkBlue, Colors.DarkBlueOnGray); }
        }

        public static Style DarkBlueOnWhite
        {
            get { return new Style(LineThickNess.Single, Colors.DarkBlueOnWhite, Colors.DarkBlueOnWhite, Colors.DarkBlueOnWhite, Colors.GrayOnDarkBlue); }
        }

        public Style(LineThickNess thickNess, Colors body)
        {
            ThickNess = thickNess;
            Body = body;
        }


        public Style(LineThickNess? thickNess = null, Colors title = null, Colors line = null, Colors body = null, Colors selectedItem = null)
        {
            ThickNess = thickNess ?? LineThickNess.Single;
            Title = title;
            Line = line;
            Body = body;
            SelectedItem = selectedItem ?? body?.ToSelectedItem();
        }

        public Style()
        {

        }

        public StyleTheme ToTheme()
        {
            var altThickness = ThickNess == LineThickNess.Double ? LineThickNess.Single : LineThickNess.Double;

            var style = ReplaceInheritFromParentNullsWithColors(this);
            return new StyleTheme(
                active : style,
                inactive : style.WithThickness(altThickness),
                disabled : style
            );
        }

        private Style ReplaceInheritFromParentNullsWithColors(Style style)
        {
            var colors = style.Body ?? style.Title ?? style.Line;
            if (colors == null) return Style.Default.WithThickness(style.ThickNess);

            return new Style(
                style.ThickNess,
                style.Title ?? colors,
                style.Line ?? colors,
                style.Body ?? colors,
                style.SelectedItem ?? colors.ToSelectedItem()
            );
        }

        public Style(ConsoleColor foreground, ConsoleColor background, LineThickNess thickness)
        {
            var color = new Colors(foreground, background);
            ThickNess = thickness;
            Title = new Colors(background.ToHeading(), background);
            Line = color;
            Body = color;
            SelectedItem = color.ToSelectedItem();
        }

        public Style(ConsoleColor foreground, ConsoleColor background)
        {
            var color = new Colors(foreground, background);
            ThickNess = LineThickNess.Single;
            Title = new Colors(background.ToHeading(), background);
            Line = color;
            Body = color;
            SelectedItem = new Colors(foreground, background.ToSelectedItemBackground());
        }

        public LineThickNess ThickNess { get; } = LineThickNess.Single;
        public Colors Title { get; } = null;
        public Colors Line { get; } = null;

        public Colors SelectedItem { get; } = null;
        public Colors Body { get; } = null;

        public Style WithBackGround(ConsoleColor background)
        {
            return new Style(
                ThickNess,
                new Colors(Title.Foreground, background),
                new Colors(Line.Foreground, background),
                new Colors(Body.Foreground, background),
                SelectedItem
                );
        }

        public Style WithForeground(ConsoleColor foreground)
        {
            return new Style(
                ThickNess,
                new Colors(Title.Foreground, Title.Background),
                new Colors(Line.Foreground, Line.Background),
                new Colors(foreground, Body.Background),
                SelectedItem
                );
        }

        public Style WithTitle(Colors title)
        {
            return new Style(
                ThickNess,
                title,
                Line,
                Body,
                SelectedItem
                );
        }

        public Style WithLine(Colors line)
        {
            return new Style(
                ThickNess,
                Title,
                line,
                Body,
                SelectedItem
                );
        }

        public Style WithColors(Colors colors)
        {
            return new Style(
                ThickNess,
                colors,
                colors,
                colors,
                SelectedItem
                );
        }

        public Style WithThickness(LineThickNess thickNess)
        {
            return new Style(
                thickNess,
                Title,
                Line,
                Body,
                SelectedItem
                );
        }

    }
}
