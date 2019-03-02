using Microsoft.AspNet.SignalR.Hubs;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRNLog.App_Start
{
    public class SignalrErrorHandler : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            // Replace this with your chosen logger
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Error(exceptionContext.Error);
            //Trace.WriteLine(exceptionContext.Error.Message);
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}