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
            client.Authenticator = new CustomAuthenticator(new TokenManager());
            _client = client;
        }

        public Action<string> GenReqId(IRestRequest request)
        {
            var reqId = GenMethod();
            logger.Debug("========== Req {0} : Start ==========", reqId);
            logger.Info("Req to {0}", request.Resource.FullResource());
            return (status) =>
            {
                logger.Debug("========== Req {0} : " + status + " ==========", reqId);
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
        }

        #region Request
        public T Execute<T>(IRestRequest request) where T : new()
        {
            request.SetPreRequest();
            var logComplete = GenReqId(request);
            var response = _client.Execute<T>(request);
            logComplete(response.StatusCode.ToString());
            var result = ResponseValid<T>(response);
            return result;
        }

        public async Task<T> ExecuteAsync<T>(IRestRequest request) where T : new()
        {
            request.SetPreRequest();
            var logComplete = GenReqId(request);
            var response = await _client.ExecuteTaskAsync<T>(request);
            logComplete(response.StatusCode.ToString());
            var result = await ResponseValidAsync<T>(response);
            return result;
        }

        public async Task<T> ExecuteAsync<T>(IRestRequest request, IProgress<string> progressIndicator) where T : new()
        {
            try
            {
                request.SetPreRequest();
                var logComplete = GenReqId(request);
                var response = await _client.ExecuteTaskAsync<T>(request);
                logComplete(response.StatusCode.ToString());
                var result = await ResponseValidAsync<T>(response); 
                progressIndicator.Report(string.Empty);
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

        private static List<RestRequest> wait_auth = new List<RestRequest>();

        #region Response
        private async Task<T> ResponseValidAsync<T>(IRestResponse<T> response) where T : new()
        {
            var myAuth = _client.Authenticator as CustomAuthenticator;
            var count = myAuth.DecreseRequest();
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var newReq = CopyRequest<T>(response);
                if (count > 0)
                {
                    logger.Info("waiting last request");
                    wait_auth.Add(newReq);
                    await Task.Run(async () =>
                    {
                        while (wait_auth.Count > 0)
                        {
                            await Task.Delay(100);
                        }
                    });
                    var result = await this.ExecuteAsync<T>(newReq);
                    return result;
                }
                else
                {
                    logger.Info("last request arrive");
                    var taskAuth = myAuth.AuthenCheckAsycn<T>((newToken) =>
                    {
                        var result = this.Execute<T>(newReq);
                        logger.Info("release waiting start ...");
                        Task.Run(async () =>
                        { 
                            await Task.Delay(5000);
                            logger.Info("release waiting end ...");
                            wait_auth.Clear();
                        });
                        return result;
                    });
                    return await taskAuth;
                }
            }

            return await Task.FromResult<T>(response.Data);
        }


        private T ResponseValid<T>(IRestResponse<T> response) where T : new()
        {
            var myAuth = _client.Authenticator as CustomAuthenticator;
            var count = myAuth.DecreseRequest();
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                logger.Info("{0} {1}", response.StatusCode.ToString(), response.Content);
                var newReq = CopyRequest<T>(response);
                return myAuth.AuthenCheck<T>((newToken) =>
                {
                    return this.Execute<T>(newReq);
                });
            }
            int numericStatusCode = (int)response.StatusCode;
            if (numericStatusCode > 200 && numericStatusCode < 301)
            {
                logger.Info("{0} {1}", response.StatusCode.ToString(), response.Content);
            }
            return response.Data;
        }

        private static RestRequest CopyRequest<T>(IRestResponse<T> response) where T : new()
        {
            var oldReq = response.Request;
            var newReq = new RestRequest(oldReq.Resource, oldReq.Method);
            newReq.Parameters.AddRange(oldReq.Parameters.Where(x => x.Name != "Authorization"));
            return newReq;
        }

        #endregion

    }


    public static class RequestExtension
    {
        public static IRestRequest SetPreRequest(this IRestRequest request)
        {
            request.AddHeader("Content-Type", "application/json");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.RequestFormat = DataFormat.Json;
            return request;
        }
    }

}
