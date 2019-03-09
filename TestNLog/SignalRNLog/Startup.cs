using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using SignalRNLog.App_Start;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using SignalRNLog.Providers;

[assembly: OwinStartup(typeof(SignalRNLog.Startup))]

namespace SignalRNLog
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        { 
            HttpConfiguration config = GlobalConfiguration.Configuration;
            //HttpConfiguration config = new HttpConfiguration();
            ConfigureOAuth(app);
            //WebApiConfig.Register(config);
            //WebApiApplication.AfterWebAPIRegister();
            Microsoft.AspNet.SignalR.StockTicker.Startup.ConfigureSignalR(app);

            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
