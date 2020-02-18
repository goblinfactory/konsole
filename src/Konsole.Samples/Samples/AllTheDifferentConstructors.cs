using System;
using System.Collections.Generic;
using System.Text;
using static System.ConsoleColor;

namespace Konsole.Samples
{
    public static class AllTheDifferentConstructors
    {
        public static void Demo()
        {
            void pause() {
                Console.ReadKey(true);
            }

            string status(IConsole con)
            {
                return $"width:{con.WindowWidth}, height:{con.WindowHeight}";
            }
            
            Console.Clear();
            
            // new window from existing window should share the window region, and allow you to independantly write to the same screen region.
            // you should share independant window state's.
            // dimensions should be the same
            // themes should be inherited, or overridden if provided
            // new cursor should be 0,0
            // existing window cursor should remain where it was.

            var w2 = new Window(10, 10, 70, 10, "floating", LineThickNess.Single, White, Blue);
            w2.WriteLine($"w2: hello:{status(w2)}");

            // something wrong here, inside window is same size as parent yet when clear does not clear the top line.
            // possibly offset by 1? and clipps?

            var w3 = w2.SplitLeft();
            w3.WriteLine("hello from left");
            var w4 = w2.SplitRight("right");
            w4.WriteLine("hello from right");
            pause();
            

        }
    }
}
