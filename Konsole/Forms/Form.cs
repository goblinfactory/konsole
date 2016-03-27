using System;

namespace Konsole.Forms
{
    // https://en.wikipedia.org/wiki/Box-drawing_character
    //0	1	2	3	4	5	6	7	8	9	A	B	C	D	E	F
    //B				│	┤	╡	╢	╖	╕	╣	║	╗	╝	╜	╛	┐
    //C	└	┴	┬	├	─	┼	╞	╟	╚	╔	╩	╦	╠	═	╬	╧
    //D	╨	╤	╥	╙	╘	╒	╓	╫	╪	┘	┌					

    public class Form 
    {
        private readonly int _width;
        private readonly IBoxStyle _boxStyle;
        private readonly IConsole _console;
        
        public Form(IConsole console = null) : this(80, null, console) { }
        public Form(int width) : this(width, null, null) {}
        public Form() : this(80, null, null) { }

        public Form(int width, IBoxStyle boxStyle, IConsole console = null)
        {

            if (width<40) throw new ArgumentOutOfRangeException("width","Minimum width of 40.");
            _width = width;
            _boxStyle = boxStyle ?? new ThinBoxStyle();
            _console = console ?? new ConsoleWriter();
        }

        public void Show<T>(T item, string title = null)
        {
            var t = typeof (T);
            var boxtitle = title ?? t.Name;
            var fl = new FieldReader<T>(item).ReadFields();
            var box = new BoxWriter(_boxStyle, _width, fl.CaptionWidth, 1);
            _console.WriteLine(box.Header(boxtitle));
            foreach (var f in fl.Fields)
            {
                var text = string.Format("{0} : {1}", f.Caption.FixLeft(fl.CaptionWidth), f.Value);
                _console.WriteLine(box.Write(text));    
            }
            
            _console.WriteLine(box.Footer);
        }
    }
}

public static class StringExtensions
{
    /// <summary>
    /// Align to the left, padd with spaces, and truncate any oveflow characters.
    /// </summary>
    public static string FixLeft(this string src, int len)
    {
        int slen = src.Length;
        if (slen > len) return src.Substring(0, len);
        return string.Format("{0}{1}", src, new string(' ', len-slen));
    }
}
