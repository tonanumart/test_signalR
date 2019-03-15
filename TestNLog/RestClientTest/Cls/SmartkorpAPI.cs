using NLog;
using RestClientTest.Cls.Authen;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestClientTest.Cls
{
    public class SmartkorpApi : ISmartkorpAuthRequest
    {
        public static Logger logger = LogManager.GetLogger("log1");
        public static SemaphoreSlim authOne = new SemaphoreSlim(1);
        private static readonly List<RestRequest> wait_auth = new List<RestRequest>();
        private static readonly Lazy<SmartkorpApi> _instance = new Lazy<SmartkorpApi>(() => new SmartkorpApi());


        public Random gen = new Random();
        private readonly IRestClient _client;
        private readonly ISmartkorpAuthRequest _authRequest;
        private readonly ITokenManager _tokenManager;

        private readonly SmartkorpOAuth _smk_oauth;



        public static bool isRefreshToken = false;


        public static SmartkorpApi Instance
        {
            get { return _instance.Value; }
        }
        private SmartkorpApi()
        {
            var client = new RestClient(AppConfigurations.baseAPIURL);
            _tokenManager = new TokenManager();
            client.Authenticator = new CustomAuthenticator(_tokenManager);
            _authRequest = this;
            _smk_oauth = new SmartkorpOAuth(_authRequest);
            _client = client;
        }

        private SmartkorpApi(ITokenManager tokenManager, ISmartkorpAuthRequest authRequest)
        {
            var client = new RestClient(AppConfigurations.baseAPIURL);
            _tokenManager = tokenManager;
            client.Authenticator = new CustomAuthenticator(_tokenManager);
            _authRequest = authRequest;
            _smk_oauth = new SmartkorpOAuth(authRequest);
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

        public async Task<T> ExecuteAsync<T>(IRestRequest request, bool isRefreshToken = false) where T : new()
        {
            request.SetPreRequest();
            var logComplete = GenReqId(request);
            if (!isRefreshToken)
                await ((CustomAuthenticator)_client.Authenticator).AuthenticateAsync(_client, request);
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
                await ((CustomAuthenticator)_client.Authenticator).AuthenticateAsync(_client, request);
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



        #region Response
        private async Task<T> ResponseValidAsync<T>(IRestResponse<T> response) where T : new()
        {
            var myAuth = _client.Authenticator as CustomAuthenticator;
            logger.Debug("Response Valid Async {0}", response.StatusCode.ToString());
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var newReq = CopyRequest<T>(response);
                var waitCheck = await authOne.WaitAsync(0);
                if (waitCheck)
                {//only one make auth
                    isRefreshToken = true;
                    logger.Info("only one make auth");
                    var newToken = await _smk_oauth.Auto_AuthenAsync();
                    _tokenManager.NotifyChangeToken(newToken);
                    await Task.Delay(1000);
                    authOne.Release();
                    var result = await this.ExecuteAsync<T>(newReq, true);
                    isRefreshToken = false;
                    return result;
                }
                else
                { //wait until authorize send signal 
                    return await WaitAuthComplete<T>(newReq);
                }
            }
            return response.Data;
        }

        private async Task<T> WaitAuthComplete<T>(RestRequest newReq) where T : new()
        {
            logger.Info("waiting .......");
            while (isRefreshToken)
                await Task.Delay(100);

            var result = await this.ExecuteAsync<T>(newReq);
            return result;
        }


        private T ResponseValid<T>(IRestResponse<T> response) where T : new()
        {
            var myAuth = _client.Authenticator as CustomAuthenticator;
            logger.Debug("Response Valid {0}", response.StatusCode.ToString());
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                logger.Info("{0} {1}", response.StatusCode.ToString(), response.Content);
                var newReq = CopyRequest<T>(response);
                var token = _smk_oauth.Auto_Authen();
                _tokenManager.NotifyChangeToken(token);
                return this.Execute<T>(newReq); ;
            }
            int numericStatusCode = (int)response.StatusCode;
            if (numericStatusCode > 200 && numericStatusCode < 301)
            {
                Application.DoEvents();
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


        public AuthMode getModeAuthen()
        {
            if (string.IsNullOrWhiteSpace(_tokenManager.Access_token))
            {
                return AuthMode.PASSWORD;
            }
            return AuthMode.REFRESHTOKEN;
        }

        public string GetRefreshToken()
        {
            return _tokenManager.Refresh_token;
        }

        public SmartkorpAuthenForm GetUserNamePassword()
        {
            return new SmartkorpAuthenForm()
            {
                UserName = "root",
                Password = "1234"
            };
        }
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
