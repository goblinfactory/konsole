using System;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
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

        /// <summary>
        /// Create a style from a compact code consisting of six 2 letter color codes for each of the styled elements plus 1 character for single or double line.
        /// </summary>
        /// <remarks>
        /// {Body}{Title}{ColumnHeaders}{Line}{SelectedItem}{Bold} + {lineThinkness} : each element takes 2 letters from the list below, representing foreground and background.
        /// k = Black = 0,              
        /// B = DarkBlue = 1,           
        /// G = DarkGreen = 2,      
        /// C = DarkCyan = 3,
        /// R = DarkRed = 4,
        /// M = DarkMagenta = 5,
        /// Y = DarkYellow = 6,
        /// a = Gray = 7,
        /// A = DarkGray = 8,
        /// b = Blue = 9,
        /// g = Green = 10,
        /// c = Cyan = 11,
        /// r = Red = 12,
        /// m = Magenta = 13,
        /// y = Yellow = 14,
        /// w = White = 15
        /// </remarks>
        /// <example>
        /// kgkgBGkgyrmgs => Body:black on green, title:black on green, columnHeader:DarkBlue on DarkGreen, Line:black on green, selected item:yellow on red, bold:magenta on green, Line = single
        /// </example>
        /// <returns></returns>

        public Style(string code)
        {
            if (code == null || code.Length != 13) throw new ArgumentOutOfRangeException($"code must be 13 characters. :{code ?? "" }");
            if (!code.All(c => allowed.Contains(c))) throw new ArgumentOutOfRangeException($"not a valid code. Contains invalid color code.");

            var thickness = GetThickness(code[12]);
            ThickNess = thickness;
            Body            = new Colors(code[0], code[1]);
            Title           = new Colors(code[2], code[3]);
            ColumnHeaders   = new Colors(code[4], code[5]);
            Line            = new Colors(code[0], code[7]);
            SelectedItem    = new Colors(code[8], code[9]);
            Bold            = new Colors(code[10], code[11]);
        }

        private static readonly char[] allowed = new char[19] { 'k', 'B', 'G', 'C', 'R', 'M', 'Y', 'a', 'A', 'b', 'g', 'c', 'r', 'm', 'y', 'w', 'd', 's', 'd' };

        private static LineThickNess GetThickness(char t)
        {
            switch (t)
            {
                case 's': return LineThickNess.Single;
                case 'd': return LineThickNess.Double;
                default: throw new ArgumentOutOfRangeException($"'{t}' is not a valid line thickness.");
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
            get { return new Style(LineThickNess.Single, Colors.WhiteOnBlue, Colors.BlueOnWhite, Colors.BlueOnWhite, Colors.BlueOnWhite, Colors.BlueOnWhite, Colors.BlueOnWhite); }
        }

        public static Style WhiteOnDarkBlue
        {
            get { return new Style(LineThickNess.Single, Colors.DarkBlueOnGray, Colors.WhiteOnDarkBlue, Colors.WhiteOnDarkBlue, Colors.WhiteOnDarkBlue, Colors.WhiteOnDarkBlue, Colors.WhiteOnDarkBlue); }
        }

        public static Style DarkBlueOnWhite
        {
            get { return new Style(LineThickNess.Single, Colors.GrayOnDarkBlue, Colors.DarkBlueOnWhite, Colors.DarkBlueOnWhite, Colors.DarkBlueOnWhite, Colors.DarkBlueOnWhite, Colors.DarkBlueOnWhite); }
        }

        public Style(LineThickNess thickNess, Colors body)
        {
            ThickNess = thickNess;
            Body = body;
        }

        public Style(
            LineThickNess? thickNess, 
            Colors body, 
            Colors title, 
            Colors columnHeaders, 
            Colors line, 
            Colors selectedItem, 
            Colors bold )
        {
            ThickNess = thickNess ?? LineThickNess.Single;
            Body = body;
            Title = title;
            ColumnHeaders = columnHeaders;
            Line = line;
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
            ColumnHeaders = color;
            Bold = color;
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
                new Colors(Body.Foreground, background),
                new Colors(Title.Foreground, background),
                new Colors(ColumnHeaders.Foreground, background),
                new Colors(Line.Foreground, background),
                new Colors(SelectedItem.Foreground, background),
                new Colors(Bold.Foreground, background));
        }

        public Style WithForeground(ConsoleColor foreground)
        {
            return new Style(
                ThickNess,
                new Colors(foreground, Body.Background),
                new Colors(foreground, Title.Background),
                new Colors(foreground, ColumnHeaders.Background),
                new Colors(foreground, Line.Background),
                new Colors(foreground, SelectedItem.Background),
                new Colors(foreground, Bold.Background));
        }

        public Style WithTitle(Colors title)
        {
            return new Style(
                ThickNess,
                Body,
                title,
                ColumnHeaders,
                Line,
                SelectedItem,
                Bold);
        }

        public Style WithLine(Colors line)
        {
            return new Style(
                ThickNess,
                Body,
                Title,
                ColumnHeaders,
                line,
                SelectedItem,
                Bold);
        }

        public Style WithColors(Colors colors)
        {
            return new Style(
                ThickNess,
                colors,
                colors,
                colors,
                colors,
                colors,
                colors);
        }

        public Style WithThickness(LineThickNess thickNess)
        {
            return new Style(
                thickNess,
                Body,
                Title,
                ColumnHeaders,
                Line,
                SelectedItem,
                Bold);
        }

        private string _code = null;
        public override string ToString()
        {
            return _code ?? (_code = $"{Body.Code}{Title.Code}{ColumnHeaders.Code}{Line.Code}{SelectedItem.Code}{Bold.Code}");
        }
    }
}
