using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Konsole.Samples
{
    public static class GettingStartedDemos
    {
        public class FakeService
        {
            public bool Stop { get; set; } = false;
            private Task _task;
            private int _transaction = 0;
            private int _balance = 1000000;
            public Action<string> OnTransaction = (_) => { };
            public FakeService()
            { 
                _task = new Task(() =>{ 
                    while(!Stop) 
                    {
                        var amount = new Random().Next(3000) - 1500;
                        Thread.Sleep(amount + 2000);
                        _transaction++;
                        _balance += amount;
                        this?.OnTransaction($"TRAN {_transaction:000} Amount:{amount} Balance:{_balance}");
                    }
                });
            }
            public void StartService()
            {
                _task.Start();
            }
        }
        //public static void TailLogging()
        //{
        //    void Wait() => Console.ReadKey(true);

        //    // open a 60 by 20 window in the console
        //    var window = Window.OpenBox("logs", 40, 10);

        //    // split window in two
            
        //    var tail = window.SplitLeft("log tail");
        //    var transactions = window.SplitRight("transactions", );

        //    // stream new transactions to transaction window
        //    var transactionService = new FakeService();
        //    transactionService.OnTransaction = logText => transactions.WriteLine(logText);
        //    transactionService.StartService();

        //    Wait();

        //    // stream just the tail of the logs 
        //    //MyFileWatcher.OnChange(newLines => foreach (var line in newLines) tail.WriteLine(line));

        //}
    }
}
