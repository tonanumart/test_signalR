using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace TestNLog.Background
{
    public class SignalRController
    {
        public static int clientTime = 0;
        public static void DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            int arg = (int)e.Argument;
            if (arg == 1000) Thread.Sleep(arg);
            clientTime++;
            e.Result = CreateSignalR(bw, clientTime);
            //Very long time until client die
            if (bw.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private static int CreateSignalR(BackgroundWorker bw, int clientTime)
        {
            var hubConnection = new HubConnection("http://localhost:1707/");
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("StockTicker");
            stockTickerHubProxy.On<Stock>("UpdateStockPrice", stock =>
            {

            });

            var closeFlag = false; 
            hubConnection.Closed += () =>
            {
                ReportWating(bw, ref closeFlag);
            };

            hubConnection.StateChanged += (state) =>
            {
                if (state.NewState == ConnectionState.Connected)
                {
                    bw.ReportProgress(50, new SignalRSResult()
                    {
                        EnumValue = SignalRStatus.Normal
                    });
                }
                else if (state.NewState == ConnectionState.Disconnected)
                {
                    ReportWating(bw, ref closeFlag); 
                }
            };

            hubConnection.Start().Wait();
            while (!bw.CancellationPending)
            {
                Thread.Sleep(5000);
            }
            throw new Exception("Create New Hub");
        }

        private static void ReportWating(BackgroundWorker bw, ref bool closeFlag)
        {
            if (closeFlag) return;
            bw.ReportProgress(50, new SignalRSResult()
            {
                EnumValue = SignalRStatus.Waiting
            }); 
            closeFlag = true;
            bw.CancelAsync(); 
        }

        public static void Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker signalRWorker = sender as BackgroundWorker;
            if (e.Error != null)
            {
                var logger = NLog.LogManager.GetLogger("SignalR");
                logger.Error(e.Error);
            }
            signalRWorker.RunWorkerAsync(1000);
        }
    }


    public class Stock
    {

        public string Symbol { get; set; }

        public decimal DayOpen { get; set; }

        public decimal DayLow { get; set; }

        public decimal DayHigh { get; set; }

        public decimal LastChange { get; set; }

        public decimal Change { get; set; }

        public double PercentChange { get; set; }

        public decimal Price { get; set; }
    }
}
