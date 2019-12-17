using Konsole;
using Konsole.Diagnostics;

namespace netcore_21
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var writer = new HighSpeedWriter())
            {
                var window = new Window(writer);
                SelfTest.Test(window, writer.Flush);
            }
        }
    }
}
