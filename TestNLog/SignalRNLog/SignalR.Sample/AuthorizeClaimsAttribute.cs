using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace SignalRNLog.SignalR.Sample
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AuthorizeClaimsAttribute : AuthorizeAttribute
    {
        protected override bool UserAuthorized(System.Security.Principal.IPrincipal user)
        {
            NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger(); 

            if (user == null)
            {
                logger.Info("Anonymous User (null user)");
                return true;
                //throw new ArgumentNullException("user");
            }

            var principal = user as ClaimsPrincipal;

            if (principal != null)
            {
                Claim authenticated = principal.FindFirst(ClaimTypes.Authentication);
                if (authenticated != null && authenticated.Value == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                logger.Info("Anonymous principal");
                return true;
            }
        }
    }
}