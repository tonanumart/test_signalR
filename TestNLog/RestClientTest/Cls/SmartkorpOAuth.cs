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
    public class SmartkorpOAuth
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static Logger log1 = LogManager.GetLogger("log1");
        private ISmartkorpAuthRequest _authenChecker;

        public SmartkorpOAuth(ISmartkorpAuthRequest authenChecker)
        {
            _authenChecker = authenChecker;
        }

        public async Task<BearerToken> Auto_AuthenAsync()
        {
            logger.Info("AuthenCheck");
            if (_authenChecker.getModeAuthen() == AuthMode.PASSWORD)
            {
                var form = _authenChecker.GetUserNamePassword();
                return await LoginToServerAsync(form.UserName, form.Password);
            }
            else
            {
                var rfToken = _authenChecker.GetRefreshToken();
                return await RefreshTokenAsync(rfToken);
            }
        }

        private async Task<BearerToken> LoginToServerAsync(string userName, string password)
        {
            logger.Info("Login To server");
            var client = new RestClient(AppConfigurations.baseURL);
            var request = new RestRequest("token", Method.POST);
            request.AddParameter("grant_type", "password", ParameterType.GetOrPost);
            request.AddParameter("username", userName, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.OnBeforeDeserialization = (resp) =>
            {
                resp.ContentType = "application/json";
            };
            var result = await client.ExecuteTaskAsync<BearerToken>(request);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                logger.Info("Login Complete");
                return result.Data;
            }
            else
            {
                throw new Exception();
            }

        }

        public async Task<BearerToken> RefreshTokenAsync(string refreshToken)
        {   
            logger.Info("Refresh Token To server");
            var client = new RestClient(AppConfigurations.baseURL);
            var request = new RestRequest("token", Method.POST);
            request.AddParameter("grant_type", "refresh_token", ParameterType.GetOrPost);
            request.AddParameter("refresh_token", refreshToken, ParameterType.GetOrPost);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.OnBeforeDeserialization = (resp) =>
            {
                resp.ContentType = "application/json";
            };
            var result = await client.ExecuteTaskAsync<BearerToken>(request);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                logger.Info("Refresh Token Complete");
                return result.Data;
            }
            else
            {
                throw new Exception();
            }
        }


        #region Synchonouse
        public BearerToken Auto_Authen()
        {
            logger.Debug("AuthenCheck");
            if (_authenChecker.getModeAuthen() == AuthMode.PASSWORD)
            {
                var form = _authenChecker.GetUserNamePassword();
                var result = LoginToServer(form.UserName,form.Password);
                return result;
            }
            else
            {
                var rfToken = _authenChecker.GetRefreshToken();
                var result = RefreshToken(rfToken);
                return result;
            }
        }



        private BearerToken LoginToServer(string userName, string password)
        {
            logger.Info("Login To server");
            var client = new RestClient(AppConfigurations.baseURL);
            var request = new RestRequest("token", Method.POST);
            request.AddParameter("grant_type", "password", ParameterType.GetOrPost);
            request.AddParameter("username", userName, ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.OnBeforeDeserialization = (resp) =>
            {
                resp.ContentType = "application/json";
            };
            var result = client.Execute<BearerToken>(request);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                logger.Info("Login Complete");
            }
            else
            {
                throw new Exception();
            }
            return result.Data;
        }

        public BearerToken RefreshToken(string refreshToken)
        {   
            logger.Info("Refresh Token To server");
            var client = new RestClient(AppConfigurations.baseURL);
            var request = new RestRequest("token", Method.POST);
            request.AddParameter("grant_type", "refresh_token", ParameterType.GetOrPost);
            request.AddParameter("refresh_token", refreshToken, ParameterType.GetOrPost);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.OnBeforeDeserialization = (resp) =>
            {
                resp.ContentType = "application/json";
            };
            var result = client.Execute<BearerToken>(request);
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                logger.Info("Refresh Token Complete");
            }
            else
            {
                throw new Exception();
            }
            return result.Data;
        }
        #endregion



    }
}
