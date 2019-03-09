using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClientTest.Cls
{
    public class SmartkorpApi
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public Random x = new Random();
        readonly IRestClient _client;


        private static readonly Lazy<SmartkorpApi> _instance = new Lazy<SmartkorpApi>(() => new SmartkorpApi());
        public static SmartkorpApi Instance
        {
            get { return _instance.Value; }
        }
        private SmartkorpApi()
        {
            var client = new RestClient(AppConfigurations.baseAPIURL);
            // _client.Authenticator = new CustomAuthenticator();
            _client = client;
        }

        public Action GenReqId(RestRequest request)
        {
            var reqId = GenMethod();
            logger.Debug("========== Req {0} : Start ==========", reqId);
            logger.Info("Req to {0}", request.Resource.FullResource());
            return () =>
            {
                logger.Debug("========== Req {0} : Complete ==========", reqId);
            };
        }
        public static int counter = 1;
        public static object ct_lock = new object();
        private static string GenMethod()
        {
            lock (ct_lock)
            {
                return Convert.ToString(counter++);
            } 
            //return Guid.NewGuid().ToString("N");
        }

        #region Request
        public T Execute<T>(RestRequest request) where T : new()
        {
            request.SetPreRequest();
            var logComplete = GenReqId(request);
            var response = _client.Execute<T>(request);
            logComplete();
            var result = ResponseValid<T>(response);
            return result;
        }

        public async Task<T> ExecuteAsync<T>(RestRequest request) where T : new()
        {
            request.SetPreRequest();
            var logComplete = GenReqId(request);
            var response = await _client.ExecuteTaskAsync<T>(request);
            logComplete();
            var result = ResponseValid<T>(response);
            return result;
        }

        public async Task<T> ExecuteAsync<T>(RestRequest request, IProgress<string> progressIndicator) where T : new()
        {
            try
            {
                request.SetPreRequest();
                var logComplete = GenReqId(request);
                var response = await _client.ExecuteTaskAsync<T>(request);
                progressIndicator.Report(string.Empty);
                logComplete();
                var result = ResponseValid<T>(response);
                return result;
            }
            catch (Exception ex)
            {
                progressIndicator.Report(ex.Message);
                T x = new T();
                return x;
            }
        }
        #endregion


        #region Response
        private T ResponseValid<T>(IRestResponse<T> response) where T : new()
        {
            int numericStatusCode = (int)response.StatusCode;
            if (numericStatusCode == 401)
            {
                logger.Info("{0} {1}", response.ResponseStatus, response.Content);
            }
            else if (numericStatusCode > 200 && numericStatusCode < 301)
            {
                logger.Info("request complete");
            }
            return response.Data;
        }

        #endregion

    }


    public static class RequestExtension
    {
        public static RestRequest SetPreRequest(this RestRequest request)
        {
            request.AddHeader("Content-Type", "application/json");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.RequestFormat = DataFormat.Json;
            return request;
        }
    }

}
