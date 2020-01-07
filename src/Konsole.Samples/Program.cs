using Konsole.Internal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static System.ConsoleColor;

namespace Konsole.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            // see if  I can create some kind of source code explorer, find all static methods with attribute Foo then browse them and display the source code
            // and then run the examples

            // OpenBoxSamples.Opening_windows_without_a_parent();
            //InputSamples.Sample();
            var w = Window.OpenBox("test", new BoxStyle() { Title = new Colors(White, Red) });
            var child = w.OpenBox("child");
            child.WriteLine("hello hello!");
            Console.ReadKey(true);
        }
    }
}
