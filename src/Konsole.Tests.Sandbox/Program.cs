using System;
using static System.ConsoleColor;

namespace Konsole.Tests.RunOne
{
    class Program
    {
        //unit test is simply to replace \n \r \n\r or \r\n with 

        static void Main(string[] args)
        {
            var c = Window.OpenBox("test", 40, 6);
            var left = c.SplitLeft();
            var right = c.SplitRight();
            left.WriteLine("one");
            left.WriteLine("two");
            left.WriteLine("three");
            left.WriteLine("four");
            // used write here so that last line does not add aditional scroll
            left.Write("five");

            right.WriteLine("foo");
            right.WriteLine("cats");
            right.WriteLine("dogs");
            // last line is already scrolling ie at the bottom of the screen so this adds an additional scroll
            right.WriteLine("dots");

            Console.ReadLine();
        }
    }
}
