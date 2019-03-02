using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Filters;

namespace SignalRNLog.App_Start
{
    public class ExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Error(context.Exception);
            //Debug.WriteLine(context.Exception);
        }
    }
}
