using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClientTest.Cls
{
    public class AppConfigurations
    {
        public const string baseAPIURL = "http://localhost:1707/api/";
        public const string baseURL = "http://localhost:1707/";
    }

    public static class UrlExtension
    {
        public static string FullResource(this string urlResource)
        {
            return string.Format("{0}{1}", AppConfigurations.baseAPIURL, urlResource);
        }
    }
}
