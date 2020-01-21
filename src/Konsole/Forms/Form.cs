using System;
using Konsole.Internal;

namespace Konsole.Forms
{
    // https://en.wikipedia.org/wiki/Box-drawing_character
    //0	1	2	3	4	5	6	7	8	9	A	B	C	D	E	F
    //B				│	┤	╡	╢	╖	╕	╣	║	╗	╝	╜	╛	┐
    //C	└	┴	┬	├	─	┼	╞	╟	╚	╔	╩	╦	╠	═	╬	╧
    //D	╨	╤	╥	╙	╘	╒	╓	╫	╪	┘	┌			╪		

    // http://www.theasciicode.com.ar/extended-ascii-code/black-square-ascii-code-254.html

    // http://www.edlazorvfx.com/ysu/html/ascii.html
    // some useful ascii codes for rendering bools
    //[ ✓ ] check mark [number: &#10003;]
    //[ ✔ ] heavy check mark [number: &#10004;]
    //[ ✕ ] multiplication sign X [number: &#100005;]
    //[ ✖ ] heavy multiplication sign X [number: &#10006;]
    //[ ✗ ] ballot X [number: &#10007;]
    //[ ✘ ] heavy ballot X [number: &#10008;]

    public class Form 
    {
        private int _width;
        private readonly IBoxStyle _boxStyle;
        private readonly IConsole _console;
        
        public Form(IConsole console = null) : this(console, console?.WindowWidth-1 ?? 80, null) { }
        public Form(int width) : this(null, width, null) {}
        public Form() : this(null, 80, null) { }

        public int Width
        {
            get {  return _width;}
            set
            {
                _width = value;
            }
        }

        public Form(int width, IBoxStyle boxStyle) : this(null, width, boxStyle)
        { }

        public Form(IConsole console, int width) : this(console, width, new ThinBoxStyle())
        {
            
        }

        public Form(IConsole console, int width, IBoxStyle boxStyle)
        {
            _width = width;
            _boxStyle = boxStyle ?? new ThinBoxStyle();
            _console = console ?? new Writer();
        }

        /// <summary>
        /// form is written (inline) at current cursor position, and cursor is updated to next line below form, with left=0
        /// </summary>
        public void Write<T>(T item, string title = null)
        {
            var t = typeof (T);
            var boxtitle = title ?? t.Name;
            if(item == null)
            {
                var nullBox = new BoxWriter(_boxStyle, _width, 10, 1);
                _console.WriteLine(nullBox.Header(boxtitle));
                _console.WriteLine(nullBox.Write(" Null"));
                _console.WriteLine(nullBox.Footer);
                return;
            }
            var fl = new FieldReader(item).ReadFieldList();
            var box = new BoxWriter(_boxStyle, _width, fl.CaptionWidth, 1);
            _console.WriteLine(box.Header(boxtitle));
            foreach (var f in fl.Fields)
            {
                var text = string.Format("{0} : {1}", f.Caption.FixLeft(fl.CaptionWidth), f.Value ?? "Null");
                _console.WriteLine(box.Write(text));    
            }
            _console.WriteLine(box.Footer);
        }

        public void Edit<T>(T item, string title = null)
        {
            throw new NotImplementedException();
            //var t = typeof(T);
            //var boxtitle = title ?? t.Name;
            //var fl = new FieldReader(item).ReadFieldList();
            //var box = new BoxWriter(_boxStyle, _width, fl.CaptionWidth, 1);
            //_console.WriteLine(box.Header(boxtitle));
            //foreach (var f in fl.Fields)
            //{
            //    var text = string.Format("{0} : {1}", f.Caption.FixLeft(fl.CaptionWidth), f.Value);
            //    _console.WriteLine(box.Write(text));
            //}
            //_console.WriteLine(box.Footer);
        }

    }
}