using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using SignalRNLog.App_Start;

[assembly: OwinStartup(typeof(SignalRNLog.Startup))]

namespace SignalRNLog
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           // 
            Microsoft.AspNet.SignalR.StockTicker.Startup.ConfigureSignalR(app);
            //app.MapSignalR();
        }
    }
}
