//begin-snippet: RealtimeStockPriceMonitorWithHighSpeedWriter
using System;
using System.Threading;
using System.Threading.Tasks;
using Konsole;
using Konsole.Internal;
using static System.ConsoleColor;

namespace Konsole.Samples
{
    public static class RealtimeStockPriceMonitorWithHighSpeedWriter
    {
        //TODO: get a feed of stock symbols, and allow user to pick stock prices from column B, and add to monitor
        //      on left. use fake (and real) stock service
        static bool finished = false;
        static bool crazyFast = false;
        static Func<bool> rand = () => new Random().Next(100) > 49;
        public static void Main(string[] args)
        {


            using var writer = new HighSpeedWriter();
            var window = new Window(writer);

            window.CursorVisible = false;
            
            var left = window.SplitLeft();
            var leftConsoles = left.SplitRows(
                new Split(0),
                new Split(9, "status"),
                new Split(10)
                );

            var status = leftConsoles[1];           
            status.BackgroundColor = Yellow;
            status.ForegroundColor = Red;
            status.Clear();

            var stocksCon = leftConsoles[0];            
            var menuCon = leftConsoles[2];
            var namesCon = window.SplitRight("account audit log");

            var r = new Random();
            int speed = 200;
            int i = 0;

            // print random names in random colors 
            // and demonstrate scrolling and wrapping at high speed
            var t1 = Task.Run(() => {
                var names = TestData.MakeNames(500);
                while (!finished)
                {
                    if (crazyFast)
                    {
                        while (crazyFast && !finished)
                        {
                            // fill a screen full before flushing
                            // this is super quick because writer 
                            // simply writes to a buffer and no actual
                            // slow IO has happened yet
                            for(int x = 0; x < 100; x++)
                            {
                                var color = (ConsoleColor)(r.Next(100) % 16);
                                namesCon.Write(color, $" {names[i++ % names.Length]}");
                            }
                            // now lets flush this massive block of updates
                            writer.Flush();
                        }
                    }
                    else
                    {
                        var color = (ConsoleColor)(r.Next(100) % 16);
                        namesCon.Write(color, $" {names[i++ % names.Length]}");
                        if (finished) break;
                        Thread.Sleep(r.Next(speed));
                        writer.Flush();
                    }
                }
            });

            var t3 = Task.Run(() =>
            {
                // stock ticker simulation
                var stocksNYSE = new[] { "BRK.A", "MSFT", "AMZN", "BTG.L", "AAPL", "LWRF.L", "GBLN1", "GBLN2" };
                var stocksFTSE = new[] { "SDR", "WPP", "ABF", "BP", "AVV", "AAL", "KGF", "MNDI", "NG","RM", "NXT","PSON" };
                var FTSE100Con = stocksCon.SplitLeft("FTSE 100");
                var NYSECon = stocksCon.SplitRight("NYSE");

                while (!finished)
                {
                    decimal move = (decimal)r.Next(100) / 10;
                    decimal newPrice = (50 + r.Next(100) + move);
                    decimal perc = ((decimal)r.Next(2000) / 100);
                    var increase = rand() ? true : false;
                    var sign = increase ? '+' : '-';
                    var changeColor = perc < 10 ? Cyan : increase ? Green : Red;
                    IConsole con;
                    string stock;
                    if (rand())
                    {
                        con = FTSE100Con;
                        stock = stocksFTSE[r.Next(stocksFTSE.Length)];
                    }
                    else
                    {
                        con = NYSECon;
                        stock = stocksNYSE[r.Next(stocksNYSE.Length)];
                    }
                    con.Write(White, $"   {stock,-10}");
                    con.WriteLine(changeColor, $"{newPrice:0.00}");
                    con.WriteLine(changeColor, $"  ({sign}{newPrice}, {perc}%)");
                    con.WriteLine("");
                    Thread.Sleep(r.Next(2000));
                }
            });


            // create a menu inside the menu console window
            // the menu will write updates to the status console window

            var menu = new Menu(menuCon, "Menu", ConsoleKey.Escape, 30,
                new MenuItem('s', "slow", () =>
                {
                    speed = 200;
                    status.Write(White, $" : {DateTime.Now.ToString("HH:mm:ss - ")}");
                    status.WriteLine(Green, $" SLOW ");
                    crazyFast = false;
                }),
                new MenuItem('f', "fast", () =>
                {
                    speed = 10;
                    status.Write(White, $" : {DateTime.Now.ToString("HH:mm:ss - ")}");
                    status.WriteLine(White, $" FAST ");
                    crazyFast = false;
                }),
                new MenuItem('c', "crazy fast", () =>
                {
                    speed = 1;
                    crazyFast = true;
                    status.Write(White, $" : {DateTime.Now.ToString("HH:mm:SS - ")}");
                    status.WriteLine(Red, $" CRAZY FAST ");
                })

            );

            status.WriteLine("press up and down to select a menu item, and enter or highlighted letter to select. Press escape to quit.");

            // menu writes to the console automatically,
            // but because we're using a buffered screen writer
            // we need to flush the UI after any menu action.
            menu.OnAfterMenuItem = _ => writer.Flush();

            menu.Run();
            // menu will block until user presses the exit key.

            finished = true;
            Task.WaitAll(t1, t3);

            window.Clear();
            window.WriteLine("thank you for flying Konsole air.");
            writer.Flush();
        }
    }
}
//end-snippet