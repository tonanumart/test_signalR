using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRNLog.SignalR.Sample;

namespace Microsoft.AspNet.SignalR.StockTicker
{
    public class StockTicker
    {
        // Singleton instance
        private readonly static Lazy<StockTicker> _instance = new Lazy<StockTicker>(
            () => new StockTicker(GlobalHost.ConnectionManager.GetHubContext<StockTickerHub>().Clients));

        private readonly object _marketStateLock = new object();
        private readonly object _updateStockPricesLock = new object();

        private readonly ConcurrentDictionary<string, Stock> _stocks = new ConcurrentDictionary<string, Stock>();

        private readonly List<Example> _list = new List<Example>();

        // Stock can go up or down by a percentage of this factor on each change
        private readonly double _rangePercent = 0.002;

        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(5000);
        private readonly Random _updateOrNotRandom = new Random();

        private Timer _timer;
        private volatile bool _updatingStockPrices;
        private volatile MarketState _marketState;

        private int timeCount = 0;

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

        public IEnumerable<Stock> GetAllStocks()
        {
            return _stocks.Values;
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
            _stocks.Clear();

            var stocks = new List<Stock>
            {
                new Stock { Symbol = "MSFT", Price = 41.68m },
                new Stock { Symbol = "AAPL", Price = 92.08m },
                new Stock { Symbol = "GOOG", Price = 543.01m }
            };

            stocks.ForEach(stock => _stocks.TryAdd(stock.Symbol, stock));
           //_list.AddRange(MessageFactory.MockOver_4Kb());
           MockXItems(6); 
        }

        private void MockXItems(int x)
        {//27kb  size
            for (int i = 0; i < x; i++)
            {
                _list.Add(new Example()
                {
                    TopicName = "[โปรโมชั่น] แจ้งคู่มือและรายละเอียดการใช้คูปองส่วนลด 10 บาท วันที่ 22 ก.พ.-31 มี.ค. 62 เฉพาะ 6 สาขา",
                    LandingURL = "http://localhost:44045/Client/Questionaire?transId=16374",
                    UserToken = "bc143239-b2ef-4595-aadd-7c2630255da7",
                    SenderName = "Anumart Chaichana",
                    OSName = "Microsoft Windows 7 Professional",
                    DeviceName = "connex-ton-a",
                    DeviceId = "BFEBFBFF000206A76C29CE53",
                    Index = 1,
                    TransId = 16374,
                    UserId = 25,
                    ThemeId = 17,
                    FontColor = "#009951",
                    AlertTime = "30:00.0",
                    AutoRedirect = 1,
                    BGColor = "#39ad53",
                    DetailColor = "#39ad53",
                    CompanyId = 157,
                    DeviceType = "D",
                    FontNotificationColor = "#39ad53",
                    DispSender = 0,
                    DisplayPersent = 60,
                    DisplayTime = "100",
                    FCMFlag = true,
                    FCMToken = null,
                    IPAddress = "192.168.0.77",
                    URLIcon = null,
                    MessageType = 1,
                    NewsLoop = 50,
                    ReadFlag = true,
                    RemoveRead = 0,
                    ShowCloseBtn = 0,
                    Speed = 100,
                    TopRangeNotification = 1
                });
            }
        }

        private void UpdateStockPrices(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            lock (_updateStockPricesLock)
            {
                if (!_updatingStockPrices)
                {
                    _updatingStockPrices = true;
                     
                    foreach (var stock in _stocks.Values)
                    {
                        if (TryUpdateStockPrice(stock))
                        {
                            BroadcastStockPrice(stock); 
                        }
                    }
                    BroadcastNews();
                    _updatingStockPrices = false;
                }
            }
        }

        private bool TryUpdateStockPrice(Stock stock)
        {
            // Randomly choose whether to udpate this stock or not
            var r = _updateOrNotRandom.NextDouble();
            if (r > 0.1)
            {
                return false;
            }

            // Update the stock price by a random factor of the range percent
            var random = new Random((int)Math.Floor(stock.Price));
            var percentChange = random.NextDouble() * _rangePercent;
            var pos = random.NextDouble() > 0.51;
            var change = Math.Round(stock.Price * (decimal)percentChange, 2);
            change = pos ? change : -change;

            stock.Price += change;
            return true;
        }

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

        private void BroadcastNews()
        {
            timeCount++;
            Clients.All.testMessageCall(new
            {
                list = testMessage(),
                count = timeCount
            });
        }

        private List<Example> testMessage()
        {
            return _list;
        }
    }

    public enum MarketState
    {
        Closed,
        Open
    }
}