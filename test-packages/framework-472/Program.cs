using Konsole;
using Konsole.Diagnostics;

namespace framework_472
{
    class Program
    {
        private static void Main(string[] args)
        {
            using (var frame = new HighSpeedWriter())
            {
                var window = new Window(frame);
                SelfTest.Test(window, frame.Flush);
            }
        }
    }
}
