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
            GlobalHost.HubPipeline.AddModule(new SignalrErrorHandler());
            app.MapSignalR();
        }
    }
}
