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
        public const string baseAPIURL = "http://192.168.0.98/API/api";
        public Random x = new Random();
        readonly IRestClient _client;


        private static readonly Lazy<SmartkorpApi> _instance = new Lazy<SmartkorpApi>(() => new SmartkorpApi());
        public static SmartkorpApi Instance
        {
            get { return _instance.Value; }
        }
        private SmartkorpApi()
        {
            var client = new RestClient(baseAPIURL);
            // _client.Authenticator = new CustomAuthenticator();
            _client = client;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            request.AddHeader("Content-Type", "application/json");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = _client.Execute<T>(request);
            int numericStatusCode = (int)response.StatusCode;
            if (numericStatusCode == 401)
            {
                Console.WriteLine("{0} {1}", response.ResponseStatus, response.Content);
            }
            return response.Data;
        }
        public async Task<T> ExecuteAsync<T>(RestRequest request, int x) where T : new()
        {
            request.AddHeader("Content-Type", "application/json");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            request.RequestFormat = DataFormat.Json;

            await Task.Delay(x * 1000);

            var response = await _client.ExecuteTaskAsync<T>(request);
            int numericStatusCode = (int)response.StatusCode;
            if (numericStatusCode == 401)
            {
                Console.WriteLine("{0} {1}", response.ResponseStatus, response.Content);
            }


            return response.Data;
        }

        public readonly object onlyOne = new object();

        public async Task<T> ExecuteAsync<T>(RestRequest request, IProgress<string> progressIndicator) where T : new()
        {
            request.AddHeader("Content-Type", "application/json");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.RequestFormat = DataFormat.Json;


            try
            {
                var response = await _client.ExecuteTaskAsync<T>(request);
                int numericStatusCode = (int)response.StatusCode;
                string content = string.Empty;
                if (numericStatusCode == 401)
                {
                    progressIndicator.Report("401");
                    content = response.Content;
                    Console.WriteLine("{0} {1}", response.ResponseStatus, response.Content);
                }
                return response.Data;
            }
            catch (Exception ex)
            {
                progressIndicator.Report(ex.Message);
                T x = new T();
                return x;
            }

        }
    }

}
