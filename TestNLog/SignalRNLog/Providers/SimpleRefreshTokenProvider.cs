using Microsoft.Owin.Security.Infrastructure;
using SignalRNLog.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace SignalRNLog.Providers
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public static List<RefreshToken> tmpRefreshTokens = new List<RefreshToken>();
        /// <summary>
        /// Last (After TokenEndpoint) Generating the Refresh Token and Persisting it
        /// </summary>
        /// <param name="context">OWIN-Authen</param>
        /// <returns></returns>
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {

            //var clientid = context.Ticket.Properties.Dictionary[AuthKey.AppId];

            //if (string.IsNullOrEmpty(clientid))
            //{
            //    return;
            //}

            var refreshTokenId = Guid.NewGuid().ToString("n");
            var refreshTokenLifeTime = context.OwinContext.Get<string>(AuthKey.RefreshTokenLifeTime);
            var token = new RefreshToken()
            {
                Id = GetHash(refreshTokenId),
                //ClientId = clientid,
                //Subject = context.Ticket.Identity.Name,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddDays(Convert.ToDouble(refreshTokenLifeTime))
            };

            context.Ticket.Properties.IssuedUtc = token.IssuedUtc;
            context.Ticket.Properties.ExpiresUtc = token.ExpiresUtc;
            //Protected Ticket is the next token (can use this token in future)
            //but this string cannot use directly becuase SerializeTicket result is encrypted token 
            token.ProtectedTicket = context.SerializeTicket();

            tmpRefreshTokens.Add(token);
            context.SetToken(refreshTokenId);
        }

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        /// <summary>
        /// 2.Authentication Check Here (grand type = refreshtoken)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(AuthKey.AllowOrigin);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            //context.Token is refresh_token (in client form)
            string hashedTokenId = GetHash(context.Token);

            var refreshToken = tmpRefreshTokens.FirstOrDefault(x => x.Id == hashedTokenId);

            if (refreshToken != null)
            {
                //Get protectedTicket from refreshToken class
                context.DeserializeTicket(refreshToken.ProtectedTicket);
                //after this method if protected key valid 
                //you are authented!! and your identity are new token 
                tmpRefreshTokens.Remove(refreshToken);
            }
        } 

        #region Not Implement
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}