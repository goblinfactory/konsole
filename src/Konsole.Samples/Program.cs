using System;

namespace Konsole.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var writer = new HighSpeedWriter())
            {
                var window = new Window(writer);
                Diagnostics.SelfTest.Test(window, () => writer.Flush());
            }
        }
    }
}
