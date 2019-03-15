using NLog;
using RestClientTest.Cls.Authen;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestClientTest.Cls
{
    public class CustomAuthenticator : IAuthenticator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Logger log1 = LogManager.GetLogger("log1");

        private ITokenManager _tokenManager; 

        public CustomAuthenticator(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            AddHeader(request);
        }

        public async Task AuthenticateAsync(IRestClient client, IRestRequest request)
        {
            if (SmartkorpApi.isRefreshToken)
            {
                log1.Info("request wating.....");
            }
            while (SmartkorpApi.isRefreshToken)
                await Task.Delay(100);
        }

        private void AddHeader(IRestRequest request)
        {
            var accessToken = _tokenManager.Access_token;
            if (!string.IsNullOrWhiteSpace(accessToken))
                logger.Debug("Add Bearer {0}", accessToken.Substring(0, 10));
            request.AddHeader("Authorization", string.Format("{0} {1}", _tokenManager.Token_type, _tokenManager.Access_token));
        }
    }
}
