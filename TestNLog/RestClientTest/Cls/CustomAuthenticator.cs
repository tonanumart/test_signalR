﻿using NLog;
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

        private TokenManager tokenManager;

        private static int request_count = 0;
        private static object lock_count = new object();

        public CustomAuthenticator(TokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            lock (lock_count)
            {
                request_count++;
            }
            AddHeader(request);
        }

        private void AddHeader(IRestRequest request)
        {
            logger.Debug("Start Authenticate Request");
            request.AddHeader("Authorization", string.Format("{0} {1}", tokenManager.bearerToken.token_type, tokenManager.bearerToken.access_token));
            logger.Debug("End Authenticate Request");
        }



        private void NotifyOtherRequest(IRestResponse<BearerToken> result)
        {
            this.tokenManager.bearerToken = result.Data;
            // this.wait_loging_requests.Clear();
        }

        private bool NoTokenMustLogin()
        {
            return string.IsNullOrWhiteSpace(this.tokenManager.bearerToken.access_token);
        }

        public T AuthenCheck<T>(Func<string, T> callback)
        {
            lock (TokenManager.refreshToken)
            {
                if (NoTokenMustLogin())
                {
                    return LoginToServer<T>(callback);
                }
                else
                {
                    return RefreshToken<T>(callback);
                }
            }
        }

        public async Task<T> AuthenCheckAsycn<T>(Func<string, T> callback)
        {
            return await Task.Run<T>(() =>
            {
                return AuthenCheck<T>(callback);
            });
        }


        private T LoginToServer<T>(Func<string, T> callback)
        {
            logger.Info("Login To server");
            var client = new RestClient(AppConfigurations.baseURL);
            var request = new RestRequest("token", Method.POST);
            request.AddParameter("grant_type", "password", ParameterType.GetOrPost);
            request.AddParameter("username", "root", ParameterType.GetOrPost);
            request.AddParameter("password", "1234", ParameterType.GetOrPost);
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
            NotifyOtherRequest(result);
            return callback(this.tokenManager.bearerToken.access_token);
        }

        public T RefreshToken<T>(Func<string, T> callback)
        {   //this method will auto lock new request cannot request to server

            logger.Info("Refresh Token To server");
            var client = new RestClient(AppConfigurations.baseURL);
            var request = new RestRequest("token", Method.POST);
            request.AddParameter("grant_type", "refresh_token", ParameterType.GetOrPost);
            request.AddParameter("refresh_token", this.tokenManager.bearerToken.refresh_token, ParameterType.GetOrPost);
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
            NotifyOtherRequest(result);
            return callback(this.tokenManager.bearerToken.access_token);
        }

        public int DecreseRequest()
        {
            lock (lock_count)
            {
                request_count--;
            }
            return request_count;
        }

        public int getRequestCount()
        {
            return request_count;
        }
    }
}
