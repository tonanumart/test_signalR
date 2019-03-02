using System.Web;
using System.Web.Mvc;
using SignalRNLog.App_Start;

namespace SignalRNLog
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}