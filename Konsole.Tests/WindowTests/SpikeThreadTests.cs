using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Konsole.Drawing;
using Konsole.Menus;
using NUnit.Framework;

namespace Konsole.Tests.WindowTests
{
    public static class TaskHelper
    {
        public static Task StartTask(this Task task)
        {
            task.Start();
            return task;
        }    
    }

    public class SpikeThreadTests
    {
        [UseReporter(typeof(DiffReporter))]
        [Test]
        public void WindowsAndMenu()
        {
            var console = new MockConsole(40,20);
            var client = Window.Open(0, 0, 20, 20, "client", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkBlue, console);
            var server = Window.Open(20, 0, 20, 20, "server", LineThickNess.Single, ConsoleColor.White, ConsoleColor.DarkYellow, console);

            Task t1 = new Task(() =>
            {
                for(int i=0; i<200; i++) client.WriteLine(i.ToString());
            }).StartTask();
            Task t2 = new Task(() =>
            {
                for (int i = 0; i < 200; i++) server.WriteLine(i.ToString());
            }).StartTask();

            Task.WaitAll(new[] {t1, t2});

            Approvals.Verify(console.BufferWrittenString);


        }
    }
}
