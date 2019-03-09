using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SignalRNLog.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public static List<MockUser> userTemp = new List<MockUser>()
        {
            new MockUser(){ UserName = "root" , Password = "1234" }
        };

        /// <summary>
        /// 1. /Token Must Validate Infomation Here
        /// </summary>
        /// <param name="context">Owin-Request</param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            context.OwinContext.Set<string>(AuthKey.AllowOrigin, "*");
            context.OwinContext.Set<string>(AuthKey.RefreshTokenLifeTime, "7");
            context.Validated();
            return Task.FromResult<object>(null);
        }


        /// <summary>
        /// 2.Authentication Check Here (grand type = password)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>(AuthKey.AllowOrigin);
            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });


            var user = userTemp.Where(x => x.UserName == context.UserName && x.Password == context.Password).FirstOrDefault();
            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            //identity.AddClaim(new Claim("sub", user.UserName));
            identity.AddClaim(new Claim("role", "user"));

            //var ticket = new AuthenticationTicket(identity, AuthProp(user.UserName));
            //context.Validated(ticket);
            context.Validated(identity);
        }

        /// <summary>
        /// Pre-Last , Issue Your Token Here (response)
        /// If turn off refresh token this is the last section
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            bool caseSpecial = true;
            if (caseSpecial)
            {   //this is the dynamic token expire time (can change here)
                context.Properties.ExpiresUtc = DateTime.UtcNow.AddMinutes(1);//TimeSpan.FromMinutes(1);
            }

            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key.Replace(".", string.Empty), property.Value);
            }

            return Task.FromResult<object>(null);
        }



        private AuthenticationProperties AuthProp(string userName, string clientId = "testApp")
        {
            var prop = new Dictionary<string, string>
            {
                { 
                    AuthKey.AuthProp.AppId , clientId
                },
                { 
                    AuthKey.AuthProp.SessionUserName, userName
                }
            };
            return new AuthenticationProperties(prop);
        }
    }

    public class MockUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}