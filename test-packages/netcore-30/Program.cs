using Konsole;
using Konsole.Diagnostics;

namespace netcore_30
{
    class Program
    {
        private static void Main(string[] args)
        {
            using (var writer = new HighSpeedWriter())
            {
                var window = new Window(writer);
                SelfTest.Test(window, writer.Flush);
            }
        }
    }
}

