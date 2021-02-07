using Konsole.Platform;
using System;
using System.IO;
using System.Threading;
using static System.ConsoleColor;

namespace Konsole.Remote
{
    class Program
    {
        static void Main(string[] _args)
        {
            try
            {
                _Main(_args);
            }catch(Exception ex)
            {
                File.AppendAllText("remote.log", ex.Message);
                File.AppendAllText("remote.log", ex.ToString());
            }
        }

        static void _Main(string[] _args)
        {
            File.AppendAllLines("remote.log", new[]
            {
                $"starting: {DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()}"
            }); 
            var con = new Window();
            con.WriteLine("starting...");
            con.WriteLine("Goblinfactory remote Konsole version 1.0 (c) Goblinfactory Ltd, 2021, all rights reserved.");
            
            // is the windowwidht height not set correctly if I dont first write something?
            //con.WriteLine($"width:{con.WindowWidth}, height:{con.WindowHeight}");
            new PlatformStuff().LockResizing(con.WindowWidth, con.WindowHeight, false, false);

            try
            {
                var args = RunArgs.Parse(_args);
                Run(args);    
            }
            catch(ArgsException ae)
            {
                Console.CursorTop = con.CursorTop + 1;
                Console.WriteLine("");
                Console.ForegroundColor = Red;
                Console.WriteLine($"Arguments error; {ae.Message}");
                Console.ResetColor();
                Console.WriteLine("");
                Console.ForegroundColor = Green;
                Console.WriteLine("Goblinfactory remote Konsole version 1.0 (c) Goblinfactory Ltd, 2021, all rights reserved.");
                Console.WriteLine("");
                Console.ResetColor();
                Console.ForegroundColor = Cyan;
                Console.WriteLine("Usage konsole {name} {port} {key} {protocol} {<-- example --> remote my-tests 8088 my-test-key gRPC");
                Console.ResetColor();
                Console.WriteLine("");
                Console.WriteLine("  Name     : The name of your remote window. Remote windows are clients that servers talk to. ");
                Console.WriteLine("               E.g. A unit test is the server, and the spawned remote window is the client with a name, e.g. TestFooFoo.");
                Console.WriteLine("  Port     : The port.");
                Console.WriteLine("  Key      : The key that the server will pass in to verify it is a valid server. Required for encrypting end to end.");
                Console.WriteLine("  Protocol : What protocol is the server using to the communicate with the remote client. [netmq]");
                Console.WriteLine("               (default protocol is netmq, currently the only protocol currently implemented, planned for future are webapi, akka.net and protoactor.)");
                                                // grpc ruled out for now since it requires to run as a dedicated service.
                                                // No sample code for hosting the server as a console app?
                                                // have not looked too hard, few minutes checking as of 7 Feb 2021.
                                                // starting with easier netmq.
                Console.WriteLine("");
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            finally
            {
                Console.WriteLine("Press enter to quit.");
                Console.ReadLine();
            }
        }

        static void Run(RunArgs args)
        {
            switch (args.Proto)
            {
                case Protocol.Undefined:
                    throw new Exception("not implemented yet");
                case Protocol.netmq:
                    Console.WriteLine("hello from remote world netmq land...waiting 10 seconds");
                    Thread.Sleep(5000);
                    Console.WriteLine("quitting...");
                    Console.ReadLine();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
