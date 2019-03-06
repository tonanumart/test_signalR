using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace SignalRNLog.Auth
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AuthorizeClaimsAttribute : AuthorizeAttribute
    {
        protected override bool UserAuthorized(System.Security.Principal.IPrincipal user)
        {
            NLog.ILogger logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("Client Auth");
            if (user == null)
            {
                logger.Info("Anonymous User (null user)");
                return false;
                //throw new ArgumentNullException("user");
            }

            var principal = user as ClaimsPrincipal;

            if (principal != null)
            {

                var userName = principal.FindFirst(ClaimTypes.Name).Value;
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    logger.Info("Found Info Name"+userName);
                    //logger.Info("Found Info AuthType" + principal.FindFirst(ClaimTypes.AuthenticationMethod).Value);
                    //logger.Info("Found Info IsAuth" + principal.FindFirst(ClaimTypes.Authentication).Value);
                    return true;
                }
                else
                {
                    logger.Info("Invalid Claim");
                    return false;
                }
            }
            else
            {
                logger.Info("Anonymous principal");
                return false;
            }
        }
    }
}