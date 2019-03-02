using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRNLog.SignalR.Sample;
using LinqToExcel;
using System.IO;

namespace Microsoft.AspNet.SignalR.StockTicker
{
    public class StockTicker
    {
        public static string filePath = @"D:\Projects\Research\test_signalR\TestNLog\SignalRNLog\Excel";
        // Singleton instance
        private readonly static Lazy<StockTicker> _instance = new Lazy<StockTicker>(
            () => new StockTicker(GlobalHost.ConnectionManager.GetHubContext<StockTickerHub>().Clients));

        private readonly object _marketStateLock = new object();
        private readonly object _updateStockPricesLock = new object();
        private readonly object _addUserLock = new object();

        //private readonly ConcurrentDictionary<string, Stock> _stocks = new ConcurrentDictionary<string, Stock>();

        //private readonly List<Example> _list = new List<Example>();
        private List<Transaction_UserOnline_Result> _list = new List<Transaction_UserOnline_Result>();

        // Stock can go up or down by a percentage of this factor on each change
        //private readonly double _rangePercent = 0.002;

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(10000);
        private readonly Random _updateOrNotRandom = new Random();

        private Timer _timer;
        private volatile bool _updatingStockPrices;
        private volatile MarketState _marketState;

        private int timeCount = 0;

        //connection Id User
        public readonly ConcurrentDictionary<int, string> users = new ConcurrentDictionary<int, string>();


        public void addUser(string connectionId)
        {
            lock (_addUserLock)
            {
                var userId = users.Count;
                users.TryAdd(userId, connectionId);
            }
        }

        private StockTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            LoadDefaultStocks();
        }

        public static StockTicker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        public MarketState MarketState
        {
            get { return _marketState; }
            private set { _marketState = value; }
        }

        public void OpenMarket()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Open)
                {
                    _timer = new Timer(UpdateStockPrices, null, _updateInterval, _updateInterval);

                    MarketState = MarketState.Open;

                    BroadcastMarketStateChange(MarketState.Open);
                }
            }
        }

        public void CloseMarket()
        {
            lock (_marketStateLock)
            {
                if (MarketState == MarketState.Open)
                {
                    if (_timer != null)
                    {
                        _timer.Dispose();
                    }

                    MarketState = MarketState.Closed;

                    BroadcastMarketStateChange(MarketState.Closed);
                }
            }
        }

        public void Reset()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Closed)
                {
                    throw new InvalidOperationException("Market must be closed before it can be reset.");
                }

                LoadDefaultStocks();
                BroadcastMarketReset();
            }
        }

        private void LoadDefaultStocks()
        {
            var excel = new ExcelQueryFactory(Path.Combine(filePath,"Book1"));
            var list = from c in excel.Worksheet<Transaction_UserOnline_Result>()
                       select c;
            _list = list.ToList();
        }

        private void UpdateStockPrices(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            lock (_updateStockPricesLock)
            {
                if (!_updatingStockPrices)
                {
                    _updatingStockPrices = true;
                    BroadcastTransUserOnline();
                    _updatingStockPrices = false;
                }
            }
        }

        //private bool TryUpdateStockPrice(Stock stock)
        //{
        //    // Randomly choose whether to udpate this stock or not
        //    var r = _updateOrNotRandom.NextDouble();
        //    if (r > 0.1)
        //    {
        //        return false;
        //    }

        //    // Update the stock price by a random factor of the range percent
        //    var random = new Random((int)Math.Floor(stock.Price));
        //    var percentChange = random.NextDouble() * _rangePercent;
        //    var pos = random.NextDouble() > 0.51;
        //    var change = Math.Round(stock.Price * (decimal)percentChange, 2);
        //    change = pos ? change : -change;

        //    stock.Price += change;
        //    return true;
        //}

        private void BroadcastMarketStateChange(MarketState marketState)
        {
            switch (marketState)
            {
                case MarketState.Open:
                    Clients.All.marketOpened();
                    break;
                case MarketState.Closed:
                    Clients.All.marketClosed();
                    break;
                default:
                    break;
            }
        }

        private void BroadcastMarketReset()
        {
            Clients.All.marketReset();
        }

        private void BroadcastStockPrice(Stock stock)
        {
            Clients.All.updateStockPrice(stock);
        }

        private void BroadcastTransUserOnline()
        {
            timeCount++;
            var groupByClient = _list.GroupBy(x => x.UserId).Select(x => new
            {
                UserId = x.Key,
                Items = x.ToList()
            }).OrderByDescending(x => x.Items.Count).ToList();

            int nextId = 0;
            foreach (var client in groupByClient)
            {
                string clientId = this.users[nextId];
                Clients.Client(clientId).testMessageCall(new
                {
                    list = client.Items,
                    count = timeCount
                });
                nextId++;
            }
        }

        //private List<Example> testMessage()
        //{
        //    return _list;
        //}
    }

    public enum MarketState
    {
        Closed,
        Open
    }
}