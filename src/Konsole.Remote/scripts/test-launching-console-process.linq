<Query Kind="Statements" />

var process = new Process();

var si = process.StartInfo;
si.CreateNoWindow = false;
si.UseShellExecute = false;

var dir = @"C:\src\git-alan-public\konsole\src\Konsole.Remote\bin\Debug\net5.0";
var file = dir + @"\Goblinfactory.Konsole.Remote.exe";
si.FileName = file;
//si.WorkingDirectory = dir;
si.Arguments = "test1 5001 my-key netmq";
si.WindowStyle = ProcessWindowStyle.Normal;

process.Start();
process.WaitForExit();



process.CloseMainWindow();
process.Kill();
process.Dispose();
