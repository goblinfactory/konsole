using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Konsole.Samples
{
    public static class ProgressBarWithNewWindowConcurrencyTests
    {
        private static void slowBuildStep(IConsole con, bool twoLine, string title, int delay)
        {

            var pb = twoLine 
                ? new ProgressBar(con, PbStyle.DoubleLine, 70) 
                : new ProgressBar(con, 70);
            
            for (int i = 1; i < 71; i++)
            {
                Thread.Sleep(delay);
                pb.Refresh(i, $"{title} {i}");
            }
        }

        public static void Test(IConsole console, int pausesMs = 500, int subTaskDelay = 100)
        {
            // pauses allow for us to run this in a shop window demo mode
            // set pauses to 0 when running in acceptance test
            var inlines = new List<IConsole>();
            var backgrounds = new List<Task>();
            for (int i = 1; i < 5; i++)
            {
                if (i % 2 == 0)
                {
                    var inline = console.Open(90, 3, "uncompressing");
                    inlines.Add(inline);
                    backgrounds.Add(Task.Run(() => slowBuildStep(inline, false, "unzipping", subTaskDelay)));
                }
                if (i % 3 == 0)
                {
                    var inline = console.Open(90, 4, "decrypting");
                    inlines.Add(inline);
                    backgrounds.Add(Task.Run(() => slowBuildStep(inline, true, "calculating rsa token", subTaskDelay)));
                }

                Thread.Sleep(pausesMs);
                console.WriteLine($"reading file {i}");
            }
            console.Write("waiting for compress to finish;");
            Task.WaitAll(backgrounds.ToArray());
            console.WriteLine(" finished.");
            Thread.Sleep(pausesMs);
            foreach (var inline in inlines) { 
                inline.Clear(); 
                inline.WriteLine("good bye!"); 
            }
            Thread.Sleep(pausesMs);
        }

    }
}
