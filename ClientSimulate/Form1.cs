using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSimulate
{



    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly object labelLock = new object();
        private static Counter _counter = new Counter();

        private int AdjustValue(int adjust, Microsoft.AspNet.SignalR.Client.ConnectionState state)
        {
            lock (labelLock)
            {
                if (state == Microsoft.AspNet.SignalR.Client.ConnectionState.Connecting)
                {
                    _counter.connecting = _counter.connecting + adjust;
                    return _counter.connecting;
                }
                if (state == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    _counter.connected = _counter.connected + adjust;
                    return _counter.connected;
                }
                if (state == Microsoft.AspNet.SignalR.Client.ConnectionState.Reconnecting)
                {
                    _counter.reconnecting = _counter.reconnecting + adjust;
                    return _counter.reconnecting;
                }
                if (state == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
                {
                    _counter.disconnected = _counter.disconnected + adjust;
                    return _counter.disconnected;

                }
            }

            return 0;
        }


        private int AdjustValue(int adjust, string state, int id)
        {
            lock (labelLock)
            {
                if (state == "MKO")
                {
                    Console.WriteLine("========================");
                    Console.WriteLine("Before " + _counter.marketOpen + " (Client : " + id + ")");
                    _counter.marketOpen = _counter.marketOpen + adjust;
                    return _counter.marketOpen;
                }
                if (state == "MKC")
                {
                    Console.WriteLine("========================");
                    Console.WriteLine("Before " + _counter.marketClose + " (Client : " + id + ")");
                    _counter.marketClose = _counter.marketClose + adjust;
                    return _counter.marketClose;
                }
                if (state == "Close")
                {
                    _counter.closedConnection = _counter.closedConnection + adjust;
                    return _counter.closedConnection;
                }
                if (state == "Call")
                {
                    _counter.fnCall = _counter.fnCall + adjust;
                    return _counter.fnCall;
                }
            }

            return 0;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            List<Task> start = new List<Task>();
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                try
                {
                    await StartHub(i + 1);
                }
                catch (Exception ex)
                {
                    lock (labelLock)
                    {
                        var text = logTextBox.Text;
                        logTextBox.LabelAssign(string.Format("{0}{1}{2}", text, Environment.NewLine, "Loop Err : " + ex.Message));
                    }
                }

            }
        }

        private async Task StartHub(int id)
        {
            var hubConnection = new HubConnection("http://localhost:1707/");
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("StockTicker");
            stockTickerHubProxy.On<Stock>("UpdateStockPrice", stock =>
            {
                this.logTextBox.Text = stock.PercentChange+"";
                // Console.WriteLine("Stock update for {0} new price {1}", stock.Symbol, stock.Price);
            });

            stockTickerHubProxy.On<ClientResult>("TestMessageCall", (result) =>
            {
                //var num = this.AdjustValue(1, "Call", id);
                //labelCall.LabelAssign(num + "");
            });

            stockTickerHubProxy.On("MarketOpened", () =>
            {
                var num = this.AdjustValue(1, "MKO", id);
                Console.WriteLine("After " + num + " (Client : " + id + ")");
                labelOpen.LabelAssign(num + "");
            });

            stockTickerHubProxy.On("MarketClosed", () =>
            {
                var num = this.AdjustValue(1, "MKC", id);
                Console.WriteLine("After " + num + " (Client : " + id + ")");
                labelClose.LabelAssign(num + "");
            });


            hubConnection.Error += (e) =>
            {
                lock (labelLock)
                {
                    var text = logTextBox.Text;
                    logTextBox.LabelAssign(string.Format("{0}{1}{2}", text, Environment.NewLine, "Client : " + id + "  " + e.Message));
                }
            };

            hubConnection.StateChanged += (e) =>
            {
                if (e.NewState == Microsoft.AspNet.SignalR.Client.ConnectionState.Connecting
                    && e.OldState == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected)
                {
                    var new_label = swichLabeL(e.NewState);
                    int num = AdjustValue(1, e.NewState);
                    new_label.LabelAssign(num + "");
                }
                else
                {
                    var old_label = swichLabeL(e.OldState);
                    int old_state = AdjustValue(-1, e.OldState);
                    old_label.LabelAssign(old_state + "");

                    var new_label = swichLabeL(e.NewState);
                    int new_state = AdjustValue(1, e.NewState);
                    new_label.LabelAssign(new_state + "");
                }
            };

            hubConnection.Closed += () =>
            {
                int num = AdjustValue(1, "Close", id);
                closeLabel.LabelAssign(num + "");
            };

            await hubConnection.Start();
        }

        private Label swichLabeL(Microsoft.AspNet.SignalR.Client.ConnectionState connectionState)
        {
            switch (connectionState)
            {
                case Microsoft.AspNet.SignalR.Client.ConnectionState.Connecting: return connectingLabel;
                case Microsoft.AspNet.SignalR.Client.ConnectionState.Connected: return connectedLabel;
                case Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected: return disconnectedLabel;
                case Microsoft.AspNet.SignalR.Client.ConnectionState.Reconnecting: return ReconnectingLabel;
            }
            return null;
        }

    }

    public static class ExtensionLabel
    {
        public static void LabelAssign(this Control label, string num)
        {
            if (label.InvokeRequired)
            {
                label.BeginInvoke((MethodInvoker)delegate()
                {
                    label.Text = num;
                });
            }
            else
            {
                label.Text = num;
            }
        }
    }


    public class Counter
    {
        public int connected { get; set; }
        public int connecting { get; set; }
        public int reconnecting { get; set; }
        public int disconnected { get; set; }
        public int marketOpen { get; set; }
        public int marketClose { get; set; }
        public int closedConnection { get; set; }
        public int fnCall { get; set; }
    }


}
