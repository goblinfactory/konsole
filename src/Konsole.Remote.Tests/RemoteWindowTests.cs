using Konsole.Remote;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Konsole.Remote.Tests
{
    public class RemoteWindowTests
    {
        [SetUp]
        public void Setup()
        {
            // make sure spawned processes are not running
        }

        [TearDown]
        public void Teardown()
        {
            // make sure spawned processes are not running
        }

        [Test]
        public void when_we_open_a_remote_window_then_a_console_window_is_opened()
        {
            var args = new RunArgs("test1", 5000, "test1", Protocol.netmq);
            var path = Assembly.GetExecutingAssembly().Location.Replace(".Tests", "").Replace("Konsole.Remote.dll", "Goblinfactory.Konsole.Remote.exe");
            using (var win = new RemoteWindow(args, path))
            {
                var con = win.Open();
                Thread.Sleep(9000);
            }
        }

        [Test]
        public void when_we_open_a_remote_window_then_we_can_write_to_the_window()
        {
            Assert.Inconclusive("not yet implemented");
        }
    }
}