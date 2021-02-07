using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Remote
{
    /// <summary>
    /// responsible for spawning a window exe that we can talk to via netmq.
    /// </summary>
    public class RemoteWindow : IDisposable
    {
        private bool _started = false;
        private Process _process;
        private IConsole _remoteWindow;
        private RunArgs _args;
        private readonly string _remoteKonsoleExePath;

        public RemoteWindow(RunArgs args, string remoteKonsoleExePath)
        {
            _args = args;
            _remoteKonsoleExePath = remoteKonsoleExePath;
        }

        public void Dispose()
        {
            _process.CloseMainWindow();
            _process.Kill();
            _process.Dispose();
            _started = false;
        }

        public void WaitForExit()
        {
            if(!_started)
            {
                return;
            }
            _process.WaitForExit();

        }

        public IConsole Open()
        {
            // launch the server
            _process = new Process();
            var si = _process.StartInfo;
            si.WindowStyle = ProcessWindowStyle.Normal;
            si.FileName = _remoteKonsoleExePath;
            si.CreateNoWindow = false;
            si.UseShellExecute = true;
            //si.RedirectStandardInput = false;
            //si.RedirectStandardError = false;
            //si.RedirectStandardOutput = false;
            si.WorkingDirectory = new FileInfo(_remoteKonsoleExePath).DirectoryName;
            if (!File.Exists(si.FileName)) throw new FileNotFoundException($"Could not find Goblinfactory.Konsole.Remote.exe at '{si.FileName}'");
            si.Arguments= $"{_args.Name} {_args.Port} {_args.Key} {_args.Proto}";
            _process.Start();
            _started = true;

            // now I need to start a local client that can talk to the server and handle IConsole commands
            // need a command to remotely close the window when the test finishes or simulate a wait?
            _remoteWindow = new RemoteWindowClient();
            return _remoteWindow;
        }
    }
}