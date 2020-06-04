using System;
using static System.ConsoleColor;

namespace Konsole
{
    public class Style
    {
        public static Func<Style> GetDefault = () => Style.WhiteOnBlack;
        public static Style Default
        {
            get
            {
                return GetDefault();
            }
        }

        public static Style[] GetStyles()
        {
            return new [] {
                WhiteOnRed,
                WhiteOnBlue,
                GrayOnBlack,
                WhiteOnBlack, 
                BlackOnGray,
                BlackOnWhite,
                
                
               // BlueOnWhite,
               // WhiteOnDarkBlue,
               //DarkBlueOnWhite,
            };
        }
        public static Style WhiteOnBlue
        {
            get { return new Style(LineThickNess.Single, Colors.WhiteOnBlue, Colors.WhiteOnBlue, Colors.WhiteOnDarkBlue, Colors.WhiteOnBlue, Colors.BlueOnWhite, Colors.CyanOnBlue); } 
        }

        public static Style WhiteOnBlack
        {
            get { return new Style(LineThickNess.Single, Colors.WhiteOnBlack, Colors.WhiteOnBlack, Colors.YellowOnBlack, Colors.WhiteOnBlack, Colors.DarkBlueOnGray, Colors.GreenOnBlack); }
        }

        public static Style BlackOnWhite
        {
            get { return new Style(LineThickNess.Single, Colors.BlackOnWhite, Colors.BlackOnWhite, Colors.BlueOnWhite, Colors.BlackOnWhite, Colors.WhiteOnDarkGrey, Colors.RedOnWhite); }
        }

        public static Style BlackOnGray  // one of my favourite styles!
        {
            get { return new Style(LineThickNess.Single, Colors.BlackOnGray, Colors.BlackOnGray, Colors.DarkBlueOnGray, Colors.BlackOnGray, Colors.GrayOnBlue, Colors.MagentaOnGray); }
        }

        public static Style GrayOnBlack
        {
            get { return new Style(LineThickNess.Single, Colors.GrayOnBlack, Colors.GrayOnBlack, Colors.YellowOnBlack, Colors.GrayOnBlack, Colors.BlackOnGray, Colors.GreenOnBlack); }
        }

        public static Style WhiteOnRed
        {
            //                          thickNess               body                title               columnHeaders           line            selectedItem        bold 
            get { return new Style(LineThickNess.Single, Colors.WhiteOnRed, Colors.WhiteOnRed, new Colors(White, DarkRed), Colors.WhiteOnRed, new Colors(Red, Gray), new Colors(Gray, Red)); }
        }

        public static Style BlueOnWhite
        {
            get { return new Style(LineThickNess.Single, Colors.WhiteOnBlue, Colors.BlueOnWhite, Colors.BlueOnWhite, Colors.BlueOnWhite); }
        }

        public static Style WhiteOnDarkBlue
        {
            get { return new Style(LineThickNess.Single, Colors.DarkBlueOnGray, Colors.WhiteOnDarkBlue, Colors.WhiteOnDarkBlue, Colors.WhiteOnDarkBlue); }
        }

        public static Style DarkBlueOnWhite
        {
            get { return new Style(LineThickNess.Single, Colors.GrayOnDarkBlue, Colors.DarkBlueOnWhite, Colors.DarkBlueOnWhite, Colors.DarkBlueOnWhite); }
        }

        public Style(LineThickNess thickNess, Colors body)
        {
            ThickNess = thickNess;
            Body = body;
        }

        public Style(LineThickNess? thickNess = null, Colors body = null, Colors title = null, Colors columnHeaders = null, Colors line = null, Colors selectedItem = null, Colors bold = null)
        {
            ThickNess = thickNess ?? LineThickNess.Single;
            Title = title;
            ColumnHeaders = columnHeaders;
            Line = line;
            Body = body;
            SelectedItem = selectedItem ?? body?.ToSelectedItem();
            Bold = bold ?? columnHeaders ?? body;
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
                thickNess: style.ThickNess,
                body: style.Body ?? colors,
                title: style.Title ?? colors,
                columnHeaders: style.ColumnHeaders ?? colors,
                line: style.Line ?? colors,
                bold: style.Bold ?? colors,
                selectedItem: style.SelectedItem ?? colors.ToSelectedItem()
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
        public Colors ColumnHeaders { get; } = null;

        public Colors Title { get; } = null;
        public Colors Line { get; } = null;

        public Colors SelectedItem { get; } = null;
        public Colors Body { get; } = null;
        public Colors Bold { get; } = null;

        public Style WithBackGround(ConsoleColor background)
        {
            return new Style(
                ThickNess,
                SelectedItem
,
                new Colors(Title.Foreground, background),
                new Colors(Line.Foreground, background),
                new Colors(Body.Foreground, background));
        }

        public Style WithForeground(ConsoleColor foreground)
        {
            return new Style(
                ThickNess,
                SelectedItem
,
                new Colors(Title.Foreground, Title.Background),
                new Colors(Line.Foreground, Line.Background),
                new Colors(foreground, Body.Background));
        }

        public Style WithTitle(Colors title)
        {
            return new Style(
                ThickNess,
                SelectedItem
,
                title,
                Line,
                Body);
        }

        public Style WithLine(Colors line)
        {
            return new Style(
                ThickNess,
                SelectedItem
,
                Title,
                line,
                Body);
        }

        public Style WithColors(Colors colors)
        {
            return new Style(
                ThickNess,
                SelectedItem
,
                colors,
                colors,
                colors);
        }

        public Style WithThickness(LineThickNess thickNess)
        {
            return new Style(
                thickNess,
                SelectedItem
,
                Title,
                Line,
                Body);
        }

    }
}
